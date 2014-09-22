using System;

namespace XO.Local.Spike.Messages.Command
{
    public class LoginUser
    {
        public Guid Id { get; set; }
        public string Password { get;  set; }
        public string UserName { get; set; }
    }
}