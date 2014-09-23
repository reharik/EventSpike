using System;
using EventStore.ClientAPI;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Infrastructure.Mongo;
using XO.Local.Spike.Messages.Command;
using XO.Local.Spike.ReadModel;
using XO.Local.Spike.ReadModel.Model;
using XO.Local.Spike.Workflows;

namespace XO.Local.Spike.MessageBinders
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
            PostEvent(loginUser,Guid.NewGuid(), a => { });
        }
    }
}