using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XO.Local.Spike.Messages.Events;
using XO.Local.Spike.ReadModel;
using XO.Local.Spike.ReadModel.Model;

namespace XO.Local.Spike.UserHandler
{
    public interface IReadStoreDispatcher
    {
        void StartDispatching();
        void StopDispatching();
    }

    public class ReadStoreDispatcher : IReadStoreDispatcher
    {
        protected readonly IMongoRepository _mongoRepository;
        private readonly List<IEventHandler> _eventHandlers;
        protected IEventStoreConnection _gesConnection;
        private bool _stopRequested;
        private EventStoreAllCatchUpSubscription _subscription;
        private string _handlerType;
       protected IGESEvent _event;
        protected ResolvedEvent _rawEvent;
        private BroadcastBlock<IGESEvent> _broadcastBlock;

        public ReadStoreDispatcher(IMongoRepository mongoRepository, List<IEventHandler> eventHandlers)
        {
            _mongoRepository = mongoRepository;
            _eventHandlers = eventHandlers;
            _handlerType = this.GetType().Name;
            _gesConnection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            _gesConnection.Connect();
            RegisterHandlers();
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
            var lastProcessedPosition = _mongoRepository.Get<LastProcessedPosition>(x => x.HandlerType == _handlerType)
                ?? new LastProcessedPosition();
            _subscription = _gesConnection.SubscribeToAllFrom(lastProcessedPosition.Position, false, HandleNewEvent, null, HandleSubscriptionDropped, new UserCredentials("admin", "changeit"));
        }

        public void StopDispatching()
        {
            _stopRequested = true;
            if (_subscription != null)
                _subscription.Stop(TimeSpan.FromSeconds(2));
        }

        private void HandleSubscriptionDropped(EventStoreCatchUpSubscription subscription, SubscriptionDropReason reason, Exception error)
        {
            if (_stopRequested)
                return;
            _eventHandlers.ForEach(x => x.GetLastPositionProcessed());
            _subscription.Start();
        }

        private void HandleNewEvent(EventStoreCatchUpSubscription subscription, ResolvedEvent @event)
        {
            if (@event.Event.EventType.StartsWith("$")) { return; }
            _rawEvent = @event;
            _event = ProcessRawEvent();
            if (_event == null) { return; }
            HandleEvent(_rawEvent.Event.EventType);
        }

        public void HandleEvent(string eventType)
        {
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

        private static IGESEvent DeserializeEvent(byte[] metadata, byte[] data)
        {
            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property("EventClrTypeName").Value;
            return (IGESEvent)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
        }
    }
}