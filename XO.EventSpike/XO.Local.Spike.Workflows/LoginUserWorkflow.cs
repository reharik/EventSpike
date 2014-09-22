using System;
using XO.Local.Spike.Domain.AggregateRoots;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Messages.Command;

namespace XO.Local.Spike.Workflows
{
    public interface ILoginUserWorkflow
    {
        void LoginUser(LoginUser loginUser);
    }

    public class LoginUserWorkflow : ILoginUserWorkflow
    {
        private readonly IGetEventStoreRepository _getEventStoreRepository;

        public LoginUserWorkflow(IGetEventStoreRepository getEventStoreRepository)
        {
            _getEventStoreRepository = getEventStoreRepository;
        }

        public async void LoginUser(LoginUser loginUser)
        {
            //            var byId = _getEventStoreRepository.GetById<User>(new Guid("f036aadb-d427-4478-ac8c-c168e2d55d9f"));
            var user = await _getEventStoreRepository.GetById<User>(loginUser.Id);
            user.Handle(loginUser);
            _getEventStoreRepository.Save(user, Guid.NewGuid(), a => { });
        }
    }
}