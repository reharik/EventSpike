using System.Threading.Tasks.Dataflow;
using XO.Local.Spike.Messages.Events;
using XO.Local.Spike.ReadModel;
using XO.Local.Spike.ReadModel.Model;

namespace XO.Local.Spike.UserHandler
{
    public interface IEventHandler
    {
        bool HandlesEvent(IGESEvent @event);
        ActionBlock<IGESEvent> ReturnActionBlock();
        void GetLastPositionProcessed();
    }
}