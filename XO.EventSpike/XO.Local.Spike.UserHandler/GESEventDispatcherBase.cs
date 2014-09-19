using System;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XO.Local.Spike.ReadModel;
using XO.Local.Spike.ReadModel.Model;

namespace XO.Local.Spike.UserHandler
{
    public class GESEventDispatcherBase
    {
        protected readonly IMongoRepository _mongoRepository;
        protected IEventStoreConnection _gesConnection;
        private bool _stopRequested;
        private EventStoreAllCatchUpSubscription _subscription;
        private LastProcessedPosition _lastProcessedPosition;
        private string _handlerType;
       protected object _event;
        protected ResolvedEvent _rawEvent;

        public GESEventDispatcherBase(IMongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
            _handlerType = this.GetType().Name;
            _gesConnection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            _gesConnection.Connect();
        }

        public void StartDispatching()
        {
            _stopRequested = false;
            _lastProcessedPosition = _mongoRepository.Get<LastProcessedPosition>(x => x.HandlerType == _handlerType)
                ?? new LastProcessedPosition { HandlerType = _handlerType, CommitPosition = 0, PreparePosition = 0 };
            var position = new Position(_lastProcessedPosition.CommitPosition, _lastProcessedPosition.PreparePosition);
            _subscription = _gesConnection.SubscribeToAllFrom(position, false, HandleNewEvent, null, HandleSubscriptionDropped, new UserCredentials("admin", "changeit"));
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

        protected virtual void HandleEvent(string eventType)
        {
            throw new NotImplementedException();
        }

        protected void SetEventAsRecorded()
        {
            if (!_rawEvent.OriginalPosition.HasValue)
                throw new ArgumentException("ResolvedEvent didn't come off a subscription to all (has no position).");

            _lastProcessedPosition.CommitPosition = _rawEvent.OriginalPosition.Value.CommitPosition;
            _lastProcessedPosition.PreparePosition = _rawEvent.OriginalPosition.Value.PreparePosition;
            _mongoRepository.Save(_lastProcessedPosition);
        }

        protected object ProcessRawEvent()
        {
            if (_rawEvent.OriginalEvent.Metadata.Length > 0 && _rawEvent.OriginalEvent.Data.Length > 0)
                return DeserializeEvent(_rawEvent.OriginalEvent.Metadata, _rawEvent.OriginalEvent.Data);
            return null;
        }

        private static object DeserializeEvent(byte[] metadata, byte[] data)
        {
            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property("EventClrTypeName").Value;
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
        }
    }
}