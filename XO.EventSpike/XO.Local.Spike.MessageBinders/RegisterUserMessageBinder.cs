using System;
using XO.Local.Spike.Messages.Command;
using XO.Local.Spike.ReadModel;
using XO.Local.Spike.ReadModel.Model;
using XO.Local.Spike.Workflows;

namespace XO.Local.Spike.MessageBinders
{
    public class RegisterUserMessageBinder
    {
        private readonly IMongoRepository _mongoRepository;
        private readonly IRegisterUserWorkflow _registerUserWorkflow;

        public RegisterUserMessageBinder(IMongoRepository mongoRepository, IRegisterUserWorkflow registerUserWorkflow)
        {
            _mongoRepository = mongoRepository;
            _registerUserWorkflow = registerUserWorkflow;
        }

        public void AcceptRequest(string userName, string emailAddress, string lastName, string firstName,
                                  string password)
        {
            var user = _mongoRepository.Get<User>(x => x.UserName == userName);
            if (user != null)
            {
                throw new Exception("User with that username already exists");
            }

            // validate email address.
            var registerUser = new RegisterUser(userName, emailAddress, lastName, firstName, password);
            _registerUserWorkflow.RegisterUser(registerUser);
        }
    }
}