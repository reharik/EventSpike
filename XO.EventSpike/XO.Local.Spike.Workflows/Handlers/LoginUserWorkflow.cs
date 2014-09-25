using System;
using System.Threading.Tasks.Dataflow;
using Newtonsoft.Json;
using XO.Local.Spike.Domain.AggregateRoots;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Infrastructure.GES.Interfaces;
using XO.Local.Spike.Infrastructure.Mongo;
using XO.Local.Spike.Infrastructure.SharedModels;
using XO.Local.Spike.Messages.Command;

namespace XO.Local.Spike.Workflows.Handlers
{
    public class LoginUserWorkflow : HandlerBase, IHandler
    {
        private readonly IGetEventStoreRepository _getEventStoreRepository;

        public LoginUserWorkflow(IGetEventStoreRepository getEventStoreRepository, IMongoRepository mongoRepository)
            : base(mongoRepository)
        {
            _getEventStoreRepository = getEventStoreRepository;
            _handlerType = "LoginUserWorkflow";
        }

        public bool HandlesEvent(IGESEvent @event)
        {
            return @event.EventType == "LoginUser";
        }

        public ActionBlock<IGESEvent> ReturnActionBlock()
        {
            return new ActionBlock<IGESEvent>(async x =>
                {
                    if (ExpectEventPositionIsGreaterThanLastRecorded(x)) { return; }

                    var loginUser = (LoginUser)x;
                    User user = await _getEventStoreRepository.GetById<User>(loginUser.Id);
                    user.Handle(loginUser);
                    _getEventStoreRepository.Save(user, Guid.NewGuid());
                    // noise
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Command Saved: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(JsonConvert.SerializeObject(user));
                    Console.Write(Environment.NewLine);
                    // noise
                    SetEventAsRecorded(x);
                });
        }
    }
}