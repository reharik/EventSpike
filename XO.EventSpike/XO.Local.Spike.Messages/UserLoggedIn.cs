using System;

namespace XO.Local.Spike.Domain.AggregateRoots
{
    public class UserLoggedIn
    {
        public Guid Id { get; private set; }
        public string UserName { get; set; }
        public Guid Token { get; private set; }
        public DateTime Now { get; private set; }

        public UserLoggedIn(Guid id, string userName, Guid token, DateTime now)
        {
            Id = id;
            UserName = userName;
            Token = token;
            Now = now;
        }
    }
}