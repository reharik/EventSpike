using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XO.Local.Spike.Infrastructure.GES.Exceptions;
using XO.Local.Spike.Infrastructure.GES.Interfaces;

namespace XO.Local.Spike.Infrastructure.GES
{
    public class GetEventStoreRepository : IGetEventStoreRepository
    {
        private const string EventClrTypeHeader = "EventClrTypeName";
        private const string AggregateClrTypeHeader = "AggregateClrTypeName";
        private const string CommitIdHeader = "CommitId";
        private const int WritePageSize = 500;
        private const int ReadPageSize = 500;

        private readonly Func<Type, Guid, string> _aggregateIdToStreamName;

        private readonly IEventStoreConnection _eventStoreConnection;
        private static readonly JsonSerializerSettings SerializerSettings;

        static GetEventStoreRepository()
        {
            SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
        }

        public GetEventStoreRepository(IEventStoreConnection eventStoreConnection)
            : this(eventStoreConnection, (t, g) => string.Format("{0}-{1}", char.ToLower(t.Name[0]) + t.Name.Substring(1), g.ToString("N")))
        {
        }

        public GetEventStoreRepository(IEventStoreConnection eventStoreConnection, Func<Type, Guid, string> aggregateIdToStreamName)
        {
            _eventStoreConnection = eventStoreConnection;
            _aggregateIdToStreamName = aggregateIdToStreamName;
            _eventStoreConnection.ConnectAsync();
        }

        public Task<TAggregate> GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            return GetById<TAggregate>(id, int.MaxValue);
        }

        public async Task<TAggregate> GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
        {
            if (version <= 0)
                throw new InvalidOperationException("Cannot get version <= 0");

            // detirmine stream name based on agg type concat with agg id
            var streamName = _aggregateIdToStreamName(typeof(TAggregate), id);
            // instanciate the agg via reflection 
            var aggregate = ConstructAggregate<TAggregate>();

            var sliceStart = 0;
            StreamEventsSlice currentSlice;
            do
            {
                // specify number of events to pull. if number of events too large for one call use limit
                var sliceCount = sliceStart + ReadPageSize <= version
                                    ? ReadPageSize
                                    : version - sliceStart + 1;
                // get all events, or first batch of events from GES
                currentSlice = await _eventStoreConnection.ReadStreamEventsForwardAsync(streamName, sliceStart, sliceCount, false);
                //validate
                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                    throw new AggregateNotFoundException(id, typeof (TAggregate));
                //validate                
                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                    throw new AggregateDeletedException(id, typeof (TAggregate));
                
                sliceStart = currentSlice.NextEventNumber;

                // apply each event to the aggregate
                currentSlice.Events.ToList().ForEach(x => aggregate.ApplyEvent(DeserializeEvent(x.OriginalEvent.Metadata, x.OriginalEvent.Data)));
            } while (version >= currentSlice.NextEventNumber && !currentSlice.IsEndOfStream);

            //validate
            if (aggregate.Version != version && version < Int32.MaxValue)
                throw new AggregateVersionException(id, typeof (TAggregate), aggregate.Version, version);                

            return aggregate;
        }
        
        private static TAggregate ConstructAggregate<TAggregate>()
        {
            return (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
        }

        private static object DeserializeEvent(byte[] metadata, byte[] data)
        {
            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
        }

        public async void Save(IAggregate aggregate, Guid commitId, IDictionary<string, object> updateHeaders = null )
        { 
            // standard data for metadata portion of persisted event
            var commitHeaders = new Dictionary<string, object>
            {
                // handy tracking id
                {CommitIdHeader, commitId},
                // type of aggregate being persisted 
                {AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName}
            };
            // add extra data to metadata portion of presisted event
            commitHeaders = (updateHeaders ?? new Dictionary<string, object>())
                .Concat(commitHeaders)
                .GroupBy(d => d.Key)
                .ToDictionary(d => d.Key, d => d.First().Value);

            // streamname is created by func, by default agg type concat to agg id
            var streamName = _aggregateIdToStreamName(aggregate.GetType(), aggregate.Id);
            // get all uncommitted events
            var newEvents = aggregate.GetUncommittedEvents().Cast<object>().ToList();
            // process events so they fit the expectations of GES
            var eventsToSave = newEvents.Select(e => ToEventData(Guid.NewGuid(), e, commitHeaders)).ToList();
            // calculate the expected version of the agg root in event store to detirmine if concurrency conflict
            var originalVersion = aggregate.Version - newEvents.Count;
            var expectedVersion = originalVersion == 0 ? ExpectedVersion.NoStream : originalVersion-1;

            // if numberr of events to save is small enough it can happen in one call
            if (eventsToSave.Count < WritePageSize)
            {
                await _eventStoreConnection.AppendToStreamAsync(streamName, expectedVersion, eventsToSave);
            }
            // otherwise batch events and start transaction 
            else
            {
                var transaction = await _eventStoreConnection.StartTransactionAsync(streamName, expectedVersion);

                var position = 0;
                while (position < eventsToSave.Count)
                {
                    var pageEvents = eventsToSave.Skip(position).Take(WritePageSize);
                    await transaction.WriteAsync(pageEvents);
                    position += WritePageSize;
                }

                await transaction.CommitAsync();
            }

            aggregate.ClearUncommittedEvents();
        }

        private static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, object> headers)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, SerializerSettings));

            var eventHeaders = new Dictionary<string, object>(headers)
            {
                {
                    EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName
                }
            };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, SerializerSettings));
            var typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }
    }
}
