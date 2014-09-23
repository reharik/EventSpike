using System;
using System.Threading.Tasks.Dataflow;
using XO.Local.Domain;
using XO.Local.Spike.Domain.AggregateRoots;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Infrastructure.GES.Interfaces;
using XO.Local.Spike.Infrastructure.Mongo;
using XO.Local.Spike.Infrastructure.SharedModels;
using XO.Local.Spike.Messages.Command;

namespace XO.Local.Spike.Workflows
{
    public class RegisterUserWorkflow : HandlerBase, IHandler
    {
        private readonly IGetEventStoreRepository _getEventStoreRepository;

        public RegisterUserWorkflow(IGetEventStoreRepository getEventStoreRepository, IMongoRepository mongoRepository)
            : base(mongoRepository)
        {
            _getEventStoreRepository = getEventStoreRepository;
        }

        public bool HandlesEvent(IGESEvent @event)
        {
            return @event.EventType == "RegisterUser";
        }

        public ActionBlock<IGESEvent> ReturnActionBlock()
        {
            return new ActionBlock<IGESEvent>(x =>
            {
                var registerUser = (RegisterUser)x;
                var user = new User();
                user.Handle(registerUser);
                _getEventStoreRepository.Save(user, Guid.NewGuid(), a => { });
                SetEventAsRecorded(x);
            });
        }
    }
}