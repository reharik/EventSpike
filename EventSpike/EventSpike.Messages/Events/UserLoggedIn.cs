using System;
using EventSpike.Infrastructure.SharedModels;
using EventStore.ClientAPI;

namespace EventSpike.Messages.Events
{
    public class UserLoggedIn : IGESEvent
    {
        public Guid Id { get; private set; }
        public string UserName { get; set; }
        public Guid Token { get; private set; }
        public DateTime Now { get; private set; }
        public string EventType { get; private set; }
        public Position? OriginalPosition { get; set; }

        public UserLoggedIn(Guid id, string userName, Guid token, DateTime now)
        {
            Id = id;
            UserName = userName;
            Token = token;
            Now = now;
            EventType = "UserLoggedIn";
        }

    }
}