using System;
using XO.Local.Domain;
using XO.Local.Spike.Domain.AggregateRoots;

namespace XO.Local.Spike.Workflows
{
    public interface IRegisterUserWorkflow
    {
        void RegisterUser(RegisterUser registerUser);
    }

    public class RegisterUserWorkflow : IRegisterUserWorkflow
    {
        private readonly IGetEventStoreRepository _getEventStoreRepository;

        public RegisterUserWorkflow(IGetEventStoreRepository getEventStoreRepository)
        {
            _getEventStoreRepository = getEventStoreRepository;
        }

        public void RegisterUser(RegisterUser registerUser)
        {
            var user = new User();
            user.Handle(registerUser);
//            var byId = repository.GetById<User>(new Guid("f036aadb-d427-4478-ac8c-c168e2d55d9f"));
            _getEventStoreRepository.Save(user, Guid.NewGuid(), a => { });
        }
    }
}