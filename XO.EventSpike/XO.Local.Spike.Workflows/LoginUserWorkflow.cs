using System;
using System.Threading.Tasks.Dataflow;
using XO.Local.Spike.Domain.AggregateRoots;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Infrastructure.GES.Interfaces;
using XO.Local.Spike.Infrastructure.Mongo;
using XO.Local.Spike.Infrastructure.SharedModels;
using XO.Local.Spike.Messages.Command;

namespace XO.Local.Spike.Workflows
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
                    var loginUser = (LoginUser)x;
                    User user = await _getEventStoreRepository.GetById<User>(loginUser.Id);
                    user.Handle(loginUser);
                    _getEventStoreRepository.Save(user, Guid.NewGuid(), a => { });
                    SetEventAsRecorded(x);
                });
        }
    }
}