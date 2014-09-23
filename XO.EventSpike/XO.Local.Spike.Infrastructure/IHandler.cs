using System.Threading.Tasks.Dataflow;
using XO.Local.Spike.Infrastructure.SharedModels;

namespace XO.Local.Spike.Infrastructure
{
    public interface IHandler
    {
        bool HandlesEvent(IGESEvent @event);
        ActionBlock<IGESEvent> ReturnActionBlock();
        void GetLastPositionProcessed();
    }
}