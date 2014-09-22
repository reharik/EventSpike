using System;
using EventStore.ClientAPI;

namespace XO.Local.Spike.Messages.Events
{
    public class UserCreated :IGESEvent
    {
        public Guid Id { get; private set; }
        public string EventType { get; private set; }
        public Position? OriginalPosition { get; set; }

        public UserCreated(Guid id)
        {
            Id = id;
            EventType = "UserCreated";
        }
    }
}