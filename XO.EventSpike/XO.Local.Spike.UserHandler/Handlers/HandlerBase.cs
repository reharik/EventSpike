using System;
using XO.Local.Spike.Messages.Events;
using XO.Local.Spike.ReadModel;
using XO.Local.Spike.ReadModel.Model;

namespace XO.Local.Spike.UserHandler.Handlers
{
    public class HandlerBase
    {
        protected IMongoRepository _mongoRepository;
        protected string _handlerType;
        protected LastProcessedPosition _lastProcessedPosition;

        protected bool ExpectEventPositionIsGreaterThanLastRecorded(IGESEvent x)
        {
            return x.OriginalPosition == null ||
                   _lastProcessedPosition.CommitPosition >= x.OriginalPosition.Value.CommitPosition;
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

        public void GetLastPositionProcessed()
        {
            _lastProcessedPosition = _mongoRepository.Get<LastProcessedPosition>(x => x.HandlerType == _handlerType);
        }

        protected void HandleEvent(IGESEvent @event, Func<IGESEvent, IReadModel> handleBy)
        {
            if (ExpectEventPositionIsGreaterThanLastRecorded(@event)) { return; };
            var view = handleBy(@event);
            _mongoRepository.Save(view);
            SetEventAsRecorded(@event);
        }
    }
}