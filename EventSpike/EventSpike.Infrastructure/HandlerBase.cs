using System;
using EventSpike.Infrastructure.Mongo;
using EventSpike.Infrastructure.SharedModels;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace EventSpike.Infrastructure
{
    public class HandlerBase
    {
        protected IMongoRepository _mongoRepository;
        protected string _handlerType;
        protected LastProcessedPosition _lastProcessedPosition;

        public HandlerBase(IMongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        protected void SetEventAsRecorded(IGESEvent @event)
        {
            // can probably find way to check if we have already got it  
            // and then just update and save rather than retreive for every event
            if (!@event.OriginalPosition.HasValue)
                throw new ArgumentException("ResolvedEvent didn't come off a subscription to all (has no position).");
            var lastProcessedPosition = _mongoRepository.Get<LastProcessedPosition>(x => x.HandlerType == _handlerType)
                                        ?? new LastProcessedPosition { HandlerType = _handlerType, CommitPosition = 0, PreparePosition = 0 };

            lastProcessedPosition.CommitPosition = @event.OriginalPosition.Value.CommitPosition;
            lastProcessedPosition.PreparePosition = @event.OriginalPosition.Value.PreparePosition;
            _mongoRepository.Save(lastProcessedPosition);
        }

        // this is used by dispatchers on restart
        public void GetLastPositionProcessed()
        {
            _lastProcessedPosition = _mongoRepository.Get<LastProcessedPosition>(x => x.HandlerType == _handlerType)
                ??new LastProcessedPosition{CommitPosition = Position.Start.CommitPosition,PreparePosition = Position.Start.PreparePosition,HandlerType = _handlerType};
        }

        // this is only used by readmodeleventhandler not commandmodel
        //TODO figure out how to abstract this so it can be use by both.  
        //TODO problem is event is saved to mongo and command is saved to GES
        protected void HandleEvent(IGESEvent @event, Func<IGESEvent, IReadModel> handleBy)
        {
            if (ExpectEventPositionIsGreaterThanLastRecorded(@event)) { return; };
            var view = handleBy(@event);
            _mongoRepository.Save(view);
            // noise
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("ReadModel Saved: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(JsonConvert.SerializeObject(view));
            // noise
            SetEventAsRecorded(@event);
        }
        // this is used by all handlers
        protected bool ExpectEventPositionIsGreaterThanLastRecorded(IGESEvent x)
        {
            return x.OriginalPosition == null || _lastProcessedPosition.CommitPosition >= x.OriginalPosition.Value.CommitPosition;
        }
    }
}