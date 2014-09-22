using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using StructureMap;
using XO.Local.Spike.MessageBinders;

namespace XO.Local.Spike.ConsoleApp
{
    class Program
    {
        private static Fixture _fixture;

        static void Main(string[] args)
        {
            Bootstrapper.Bootstrap();
            CreateUsers(1);
        }

        private static void CreateUsers(int numberOfUsersToCreate)
        {
            _fixture = new Fixture();
            var mb = ObjectFactory.Container.GetInstance<RegisterUserMessageBinder>();
            var lu = ObjectFactory.Container.GetInstance<LoginUserMessageBinder>();
            var userNames = new List<Tuple<string,string>>();
            for (int i = 0; i < numberOfUsersToCreate; i++)
            {
                var smr = _fixture.Create<SomeMessyRequest>();
                mb.AcceptRequest(smr.UserName, smr.Email, smr.LastName, smr.FirstName, smr.Password);
                userNames.Add(new Tuple<string, string>(smr.UserName, smr.Password));
            }
            Thread.Sleep(10000);
            userNames.ForEach(x=> lu.AcceptRequest(x.Item1,x.Item2));
//            Console.Read();

        }
    }
                
    public class SomeMessyRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
