using XO.Local.Spike.Domain.AggregateRoots;
using XO.Local.Spike.ReadModel;
using XO.Local.Spike.ReadModel.Model;

namespace XO.Local.Spike.UserHandler
{
    public interface IUserHandler
    {
        void StartDispatching();
        void StopDispatching();
    }

    public class UserHandler : GESEventDispatcherBase, IUserHandler
    {
        public UserHandler(IMongoRepository mongoRepository) : base(mongoRepository)
        {
        }

        protected override void HandleEvent(string eventType)
        {
            User user = null;
            switch (eventType)
            {
                case "UserCreated":
                    var userCreated = (UserCreated) _event;
                    user = new User {Id = userCreated.Id};
                    _mongoRepository.Save(user);
                    SetEventAsRecorded();
                    break;
                case "UserRegistered":
                    var userRegistered = (UserRegistered) _event;
                    user = _mongoRepository.Get<User>(x => x.Id == userRegistered.Id);
                    user.UserName = userRegistered.UserName;
                    user.FirstName = userRegistered.FirstName;
                    user.LastName = userRegistered.LastName;
                    user.Email = userRegistered.EmailAddress;
                    _mongoRepository.Save(user);
                    SetEventAsRecorded();
                    break;
                case "UserLoggedIn":
                    var userLoggedIn = (UserLoggedIn) _event;
                    var userLogins = new UserLogins();
                    userLogins.UserName = userLoggedIn.UserName;
                    userLogins.Id = userLoggedIn.Id;
                    userLogins.Token = userLoggedIn.Token;
                    userLogins.Date = userLoggedIn.Now;
                    _mongoRepository.Save(userLogins);
                    SetEventAsRecorded();
                    break;
            }
        }
    }
}