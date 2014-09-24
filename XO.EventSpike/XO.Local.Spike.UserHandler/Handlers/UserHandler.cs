using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using Newtonsoft.Json;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Infrastructure.Mongo;
using XO.Local.Spike.Infrastructure.SharedModels;
using XO.Local.Spike.Messages.Events;
using XO.Local.Spike.ReadModel.Model;

namespace XO.Local.Spike.EventHandler.Handlers
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
                    Console.WriteLine("Handling User Event: {0}",x.EventType);

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
            var userLogins = new UserLogins
                {
                    UserName = userLoggedIn.UserName,
                    Id = userLoggedIn.Id,
                    Token = userLoggedIn.Token,
                    Date = userLoggedIn.Now
                };
            var input = JsonConvert.SerializeObject(userLoggedIn);
            var output = JsonConvert.SerializeObject(userLogins);
            Console.WriteLine("input: {0}", input);
            Console.WriteLine("output: {0}", output);
            return userLogins;
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
            var input = JsonConvert.SerializeObject(userRegistered);
            var output = JsonConvert.SerializeObject(user);
            Console.WriteLine("input: {0}", input);
            Console.WriteLine("output: {0}", output); 
            return user;
        }

        private IReadModel userCreated(IGESEvent x)
        {
            var userCreated = (UserCreated)x;
            var user = new User {Id = userCreated.Id};
            var input = JsonConvert.SerializeObject(userCreated);
            var output = JsonConvert.SerializeObject(user);
            Console.WriteLine("input: {0}", input);
            Console.WriteLine("output: {0}", output); 
            return user;
        }
    }
}