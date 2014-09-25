using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventSpike.Infrastructure.SharedModels;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace EventSpike.Infrastructure
{
    public class MessageBinderBase
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private const string CommandClrTypeHeader = "CommandClrTypeName";
        private const string CommitIdHeader = "CommitId";
        private const string EventStreamName = "CommandDispatch";

        public MessageBinderBase(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
            _eventStoreConnection.ConnectAsync();
        }

        protected async void PostEvent(IGESEvent @event, Guid commitId, Dictionary<string, object> updateHeaders = null)
        {
            // standard data for metadata portion of persisted event
            var commitHeaders = new Dictionary<string, object>
            {
                // handy tracking id
                {CommitIdHeader, commitId},
                {CommandClrTypeHeader, @event.GetType().AssemblyQualifiedName}
            };
            // add extra data to metadata portion of presisted event
            commitHeaders = (updateHeaders ?? new Dictionary<string, object>())
                .Concat(commitHeaders)
                .GroupBy(d => d.Key)
                .ToDictionary(d => d.Key, d => d.First().Value);
            
            // process command so they fit the expectations of GES
            var commandToSave = new[] {ToEventData(Guid.NewGuid(), @event, commitHeaders)};
            // post to command stream
            await _eventStoreConnection.AppendToStreamAsync(EventStreamName, ExpectedVersion.Any, commandToSave);
        }

        private static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, object> headers)
        {
            var serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, serializerSettings));

            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(headers, serializerSettings));
            var typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }
    }
}