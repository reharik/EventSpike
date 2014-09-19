using System;
using XO.Local.Spike.Domain.AggregateRoots;
using XO.Local.Spike.ReadModel;
using XO.Local.Spike.ReadModel.Model;
using XO.Local.Spike.Workflows;

namespace XO.Local.Spike.MessageBinders
{
    public class LoginUserMessageBinder
    {
        private readonly IMongoRepository _mongoRepository;
        private readonly ILoginUserWorkflow _loginUserWorkflow;

        public LoginUserMessageBinder(IMongoRepository mongoRepository, ILoginUserWorkflow loginUserWorkflow)
        {
            _mongoRepository = mongoRepository;
            _loginUserWorkflow = loginUserWorkflow;
        }

        public void AcceptRequest(string userName, string password)
        {
            var user = _mongoRepository.Get<User>(x => x.UserName == userName);
            if (user == null)
            {
                throw new Exception("Username not found");
            }

            // validate email address.
            var registerUser = new LoginUser {Id = user.Id, UserName = user.UserName, Password = password};
            _loginUserWorkflow.LoginUser(registerUser);
        }
    }
}