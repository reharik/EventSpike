using System;

namespace XO.Local.Spike.Domain.AggregateRoots
{
    public class UserRegistered
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }

        public UserRegistered(Guid id, string userName, string password, string firstName, string lastName, string emailAddress)
        {
            Id = id;
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }
    }
}