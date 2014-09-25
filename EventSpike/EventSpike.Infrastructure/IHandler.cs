using System.Threading.Tasks.Dataflow;
using EventSpike.Infrastructure.SharedModels;

namespace EventSpike.Infrastructure
{
    public interface IHandler
    {
        bool HandlesEvent(IGESEvent @event);
        ActionBlock<IGESEvent> ReturnActionBlock();
        void GetLastPositionProcessed();
    }
}