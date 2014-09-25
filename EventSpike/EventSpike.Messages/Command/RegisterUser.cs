using System;
using EventSpike.Infrastructure.SharedModels;
using EventStore.ClientAPI;

namespace EventSpike.Messages.Command
{
    public class RegisterUser : IGESEvent
    {
        public RegisterUser(string userName, string emailAddress, string lastName, string firstName, string password, DateTime dob)
        {
            UserName = userName;
            EmailAddress = emailAddress;
            LastName = lastName;
            FirstName = firstName;
            Password = password;
            Dob = dob;
            EventType = "RegisterUser";
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }
        public DateTime Dob { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string EventType { get; private set; }
        public Position? OriginalPosition { get; set; }
    }
}