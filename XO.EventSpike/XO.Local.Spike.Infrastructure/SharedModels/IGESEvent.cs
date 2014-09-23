using EventStore.ClientAPI;

namespace XO.Local.Spike.Infrastructure.SharedModels
{
    public interface IGESEvent
    {
        string EventType { get; }
        Position? OriginalPosition { get; set; }
    }
}