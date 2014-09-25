using System;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using XO.Local.Spike.Infrastructure.Mongo;
using XO.Local.Spike.Messages.Command;
using XO.Local.Spike.ReadModel.Model;

namespace XO.Local.Spike.MessageBinders.MessageBinders
{
    public class LoginUserMessageBinder : MessageBinderBase
    {
        private readonly IMongoRepository _mongoRepository;

        public LoginUserMessageBinder(IMongoRepository mongoRepository, IEventStoreConnection eventStoreConnection)
            : base(eventStoreConnection)
        {
            _mongoRepository = mongoRepository;
        }

        public void AcceptRequest(string userName, string password)
        {
            var user = _mongoRepository.Get<User>(x => x.UserName == userName);
            if (user == null)
            {
                throw new Exception("Username not found");
            }

            // validate email address.
            var loginUser = new LoginUser(user.Id, password, user.UserName);
            // noise
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Command Created: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(JsonConvert.SerializeObject(loginUser));
            // noise

            PostEvent(loginUser,Guid.NewGuid());
        }
    }
}