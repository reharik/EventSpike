using System;
using EventSpike.Infrastructure.SharedModels;
using EventStore.ClientAPI;

namespace EventSpike.Messages.Command
{
    public class LoginUser : IGESEvent
    {
        public LoginUser(Guid id, string password, string userName)
        {
            Id = id;
            Password = password;
            UserName = userName;
            EventType = "LoginUser";
        }

        public Guid Id { get; set; }
        public string Password { get;  set; }
        public string UserName { get; set; }
        public string EventType { get; private set; }
        public Position? OriginalPosition { get; set; }
    }
}