using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XO.Local.Spike.Infrastructure.GES.Interfaces;
using XO.Local.Spike.Infrastructure.Mongo;
using XO.Local.Spike.Infrastructure.SharedModels;

namespace XO.Local.Spike.Infrastructure
{
    public interface IDispatcher
    {
        void StartDispatching();
        void StopDispatching();
    }

    public class DispatcherBase : IDispatcher
    {
        protected readonly IMongoRepository _mongoRepository;
        private readonly List<IHandler> _eventHandlers;
        protected IEventStoreConnection _gesConnection;
        private bool _stopRequested;
        private EventStoreAllCatchUpSubscription _subscription;
        private string _handlerType;
        protected IGESEvent _event;
        protected ResolvedEvent _rawEvent;
        private BroadcastBlock<IGESEvent> _broadcastBlock;
        protected string _targetClrTypeName;
        protected Func<ResolvedEvent, bool> _eventFilter;

        public DispatcherBase(IMongoRepository mongoRepository, IGESConnection gesConnection, List<IHandler> eventHandlers)
        {
            _mongoRepository = mongoRepository;
            _gesConnection = gesConnection.BuildConnection();
            _gesConnection.ConnectAsync();
            _eventHandlers = eventHandlers;
            _handlerType = this.GetType().Name;
            RegisterHandlers();
            GetLastEventProcessedForHandlers();
        }

        private void GetLastEventProcessedForHandlers()
        {
            var actionBlock = new ActionBlock<IHandler>(x => x.GetLastPositionProcessed(), new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 8 });
            _eventHandlers.ForEach(async x=> await actionBlock.SendAsync(x));
        }

        public void RegisterHandlers()
        {
            _broadcastBlock = new BroadcastBlock<IGESEvent>(x => x);
            _eventHandlers.ForEach(x => _broadcastBlock.LinkTo(x.ReturnActionBlock(), x.HandlesEvent));
            _broadcastBlock.LinkTo(DataflowBlock.NullTarget<IGESEvent>());
        }

        public void StartDispatching()
        {
            _stopRequested = false;
            _subscription = _gesConnection.SubscribeToAllFrom(Position.Start, false, HandleNewEvent,null,null,new UserCredentials("admin","changeit"));
        }

        public void StopDispatching()
        {
            _stopRequested = true;
            if (_subscription != null)
                _subscription.Stop(TimeSpan.FromSeconds(2));
        }

        private void HandleNewEvent(EventStoreCatchUpSubscription subscription, ResolvedEvent @event)
        {
            if (!_eventFilter(@event)) { return; }
            _rawEvent = @event;
            _event = ProcessRawEvent();
            if (_event == null) { return; }
            HandleEvent(_rawEvent.Event.EventType);
        }

        public void HandleEvent(string eventType)
        {
            Console.WriteLine("Handing Event to broadcast block: {0}",_event.EventType);
            _broadcastBlock.Post(_event);
        }

        protected IGESEvent ProcessRawEvent()
        {
            if (_rawEvent.OriginalEvent.Metadata.Length <= 0 || _rawEvent.OriginalEvent.Data.Length <= 0)
            { return null; }

            var gesEvent = DeserializeEvent(_rawEvent.OriginalEvent.Metadata, _rawEvent.OriginalEvent.Data);
            gesEvent.OriginalPosition = _rawEvent.OriginalPosition;
            return gesEvent;
        }

        private IGESEvent DeserializeEvent(byte[] metadata, byte[] data)
        {
            // tried to get this out of here and into one call but couldn't do it
            var actualClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(_targetClrTypeName);
            return (IGESEvent)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)actualClrTypeName));
        }
    }
}