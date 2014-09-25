using System;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using XO.Local.Spike.Infrastructure.Mongo;
using XO.Local.Spike.Messages.Command;
using XO.Local.Spike.ReadModel.Model;

namespace XO.Local.Spike.MessageBinders.MessageBinders
{
    public class RegisterUserMessageBinder : MessageBinderBase
    {
        private readonly IMongoRepository _mongoRepository;

        public RegisterUserMessageBinder(IMongoRepository mongoRepository, IEventStoreConnection eventStoreConnection)
            : base(eventStoreConnection)
        {
            _mongoRepository = mongoRepository;
        }

        public void AcceptRequest(string userName, string emailAddress, string lastName, string firstName,
                                  string password, DateTime dob)
        {
            var user = _mongoRepository.Get<User>(x => x.UserName == userName);
            if (user != null)
            {
                throw new Exception("User with that username already exists");
            }

            // validate email address.
            var registerUser = new RegisterUser(userName, emailAddress, lastName, firstName, password, dob);
            // noise
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Command Saved: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(JsonConvert.SerializeObject(registerUser));
            // noise
            PostEvent(registerUser, Guid.NewGuid());

        }
    }
}