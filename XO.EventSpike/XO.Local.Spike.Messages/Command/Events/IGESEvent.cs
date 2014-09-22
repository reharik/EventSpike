using EventStore.ClientAPI;

namespace XO.Local.Spike.Messages.Events
{
    public interface IGESEvent
    {
        string EventType { get; }
        Position? OriginalPosition { get; set; }
    }
}