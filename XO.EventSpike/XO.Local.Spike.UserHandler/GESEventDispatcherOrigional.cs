//namespace XO.Local.Spike.UserHandler
//{
//    public class GetEventStoreEventDispatcherOrigional
//    {
//        private readonly IEventBus _eventBus;
//        private readonly EventStoreConnection _connection;
//
//        private bool _stopRequested;
//        private EventStoreAllCatchUpSubscription _subscription;
//        private readonly IPersistGetEventStorePosition _positionRepository;
//
//        public GetEventStoreEventDispatcher(EventStoreConnection connection, IEventBus eventBus, IPersistGetEventStorePosition positionRepository)
//        {
//            if (connection == null) throw new ArgumentNullException("connection");
//            if (eventBus == null) throw new ArgumentNullException("eventBus");
//            if (positionRepository == null) throw new ArgumentNullException("positionRepository");
//
//            _connection = connection;
//            _eventBus = eventBus;
//            _positionRepository = positionRepository;
//        }
//
//        public void StartDispatching()
//        {
//            _stopRequested = false;
//            RecoverSubscription();
//        }
//
//        public void StopDispatching()
//        {
//            _stopRequested = true;
//            if (_subscription != null)
//                _subscription.Stop(TimeSpan.FromSeconds(2));
//        }
//
//        private void HandleSubscriptionDropped(EventStoreCatchUpSubscription subscription, string reason, Exception error)
//        {
//            if (_stopRequested)
//                return;
//
//            RecoverSubscription();
//        }
//
//        private void RecoverSubscription()
//        {
//            _subscription = _connection.SubscribeToAllFrom(_positionRepository.GetLastProcessedPosition(), false, HandleNewEvent, HandleSubscriptionDropped);
//        }
//
//        private void HandleNewEvent(EventStoreCatchUpSubscription subscription, ResolvedEvent @event)
//        {
//            _eventBus.Publish(ProcessRawEvent(@event));
//
//            if (!@event.OriginalPosition.HasValue)
//                throw new ArgumentException("ResolvedEvent didn't come off a subscription to all (has no position).");
//
//            _positionRepository.PersistLastPositionProcessed(@event.OriginalPosition.Value);
//        }
//
//        private static object ProcessRawEvent(ResolvedEvent rawEvent)
//        {
//            //NOTE: Normally you'd deserialize here... however, in the interests of not wanting to write
//            // a load of events etc, we're just going to return the ResolvedEvent. Original is below.
//
//            return rawEvent;
//            //if (rawEvent.OriginalEvent.Metadata.Length > 0 && rawEvent.OriginalEvent.Data.Length > 0)
//            //    return DeserializeEvent(rawEvent.OriginalEvent.Metadata, rawEvent.OriginalEvent.Data);
//            //return null;
//        }
//
//        /// <summary>
//        /// Deserializes the event from the raw GetEventStore event to my event.
//        /// Took this from a gist that James Nugent posted on the GetEventStore forumns.
//        /// </summary>
//        /// <param name="metadata"></param>
//        /// <param name="data"></param>
//        /// <returns></returns>
//        private static object DeserializeEvent(byte[] metadata, byte[] data)
//        {
//            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property("EventClrTypeName").Value;
//            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
//        }
//    }
//}