using EventStore.ClientAPI;

namespace EventSpike.Infrastructure.SharedModels
{
    public interface IGESEvent
    {
        string EventType { get; }
        Position? OriginalPosition { get; set; }
    }
}