using System;
using EventStore.ClientAPI;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Infrastructure.SharedModels;

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