namespace XO.Local.Spike.Domain.AggregateRoots
{
    public class RegisterUser
    {
        public RegisterUser(string userName, string emailAddress, string lastName, string firstName, string password)
        {
            UserName = userName;
            EmailAddress = emailAddress;
            LastName = lastName;
            FirstName = firstName;
            Password = password;
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
    }
}