using System;
using XO.Local.Domain;

namespace XO.Local.Spike.Domain.AggregateRoots
{
    public class User : AggregateBase
    {
        private string _password;
        private bool _loggedIn;

        public User()
        {
            Register<UserCreated>(e => Id = e.Id);
            Register<UserLoggedIn>(e => { });
            RaiseEvent(new UserCreated(Guid.NewGuid()));
        }

        #region Handle
        public void Handle(RegisterUser cmd)
        {
            ExpectPasswordSecure(cmd.Password);
            ExpectEmailAddressValid(cmd.EmailAddress);
            RaiseEvent(new UserRegistered(Id, cmd.UserName,cmd.Password,cmd.FirstName,cmd.LastName,cmd.EmailAddress));
        }

        public void Handle(LoginUser cmd)
        {
            ExpectNotLoggedOn();
            ExpectCorrectPassword(cmd.Password);
            var token = CreateToken();
            RaiseEvent(new UserLoggedIn(Id, cmd.UserName, token, DateTime.Now));
        }

        private Guid CreateToken()
        {
            return Guid.NewGuid();
        }

        #endregion 
        #region Apply
        public void Apply(UserCreated vent)
        {
            Id = vent.Id;
        }

        public void Apply(UserRegistered vent)
        {
            _password = vent.Password;
        }
        #endregion 
        #region Expect
        private void ExpectCorrectPassword(string password)
        {
            if (password != _password)
            {
                throw new Exception("Incorrect password dog");
            }
        }

        private void ExpectNotLoggedOn()
        {
            if (_loggedIn)
            {
                throw new Exception("User already logged in!");
            }
        }
        
        private void ExpectEmailAddressValid(string emailAddress)
        {
            if (emailAddress.Length <= 2)
            {
                throw new Exception("invalid email yo.");
            }
        }

        private void ExpectPasswordSecure(string password)
        {
            if (password.Length <= 2)
            {
                throw new Exception("Password needs to be longer than two characters");
            }
        }
        #endregion

    }
}