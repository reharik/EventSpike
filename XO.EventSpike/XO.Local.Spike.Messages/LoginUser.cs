using System;

namespace XO.Local.Spike.Domain.AggregateRoots
{
    public class LoginUser
    {
        public Guid Id { get; set; }
        public string Password { get;  set; }
        public string UserName { get; set; }
    }
}