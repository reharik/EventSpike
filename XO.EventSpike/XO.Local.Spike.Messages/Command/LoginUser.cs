using System;
using EventStore.ClientAPI;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Infrastructure.SharedModels;
using XO.Local.Spike.Messages.Events;

namespace XO.Local.Spike.Messages.Command
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