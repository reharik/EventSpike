using System.Threading;
using System.Threading.Tasks.Dataflow;
using EventStore.ClientAPI;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Infrastructure.Mongo;
using XO.Local.Spike.Infrastructure.SharedModels;
using XO.Local.Spike.Messages.Events;
using XO.Local.Spike.ReadModel;
using XO.Local.Spike.ReadModel.Model;

namespace XO.Local.Spike.UserHandler.Handlers
{
    public class UserHandler : HandlerBase, IHandler
    {
        public UserHandler(IMongoRepository mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
            _handlerType = "UserHandler";
            _lastProcessedPosition = new LastProcessedPosition();
        }

        public bool HandlesEvent(IGESEvent @event)
        {
            if (@event.EventType == "UserCreated") { return true; }
            if (@event.EventType == "UserRegistered") { return true; }
            if (@event.EventType == "UserLoggedIn") { return true; }
            return false;
        } 
       
        public ActionBlock<IGESEvent> ReturnActionBlock()
        {
            return new ActionBlock<IGESEvent>(x =>
                {
                    switch (x.EventType)
                    {
                        case "UserCreated":
                            HandleEvent(x,userCreated);
                            break;
                        case "UserRegistered":
                            HandleEvent(x,userRegistered);
                            break;
                        case "UserLoggedIn":
                            HandleEvent(x,userLoggedIn);
                            break;
                    }
                }, new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = 4
                });
        }

        private IReadModel userLoggedIn(IGESEvent x)
        {
            var userLoggedIn = (UserLoggedIn) x;
            return new UserLogins
                {
                    UserName = userLoggedIn.UserName,
                    Id = userLoggedIn.Id,
                    Token = userLoggedIn.Token,
                    Date = userLoggedIn.Now
                };
        }

        private IReadModel userRegistered(IGESEvent x)
        {
            Thread.Sleep(1000);
            var userRegistered = (UserRegistered)x;
            var user = _mongoRepository.Get<User>(u => u.Id == userRegistered.Id);
            user.UserName = userRegistered.UserName;
            user.FirstName = userRegistered.FirstName;
            user.LastName = userRegistered.LastName;
            user.Email = userRegistered.EmailAddress;
            return user;
        }

        private IReadModel userCreated(IGESEvent x)
        {
            var userCreated = (UserCreated)x;
            return new User {Id = userCreated.Id};
        }
    }
}