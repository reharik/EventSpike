using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using StructureMap;
using XO.Local.Spike.MessageBinders;
using XO.Local.Spike.MessageBinders.MessageBinders;

namespace XO.Local.Spike.ConsoleApp
{
    class Program
    {
        private static Fixture _fixture;

        static void Main(string[] args)
        {
            Bootstrapper.Bootstrap();
            CreateUsers();
        }

        private static void CreateUsers()
        {
            var unpw = new Dictionary<string, string>();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Enter userName to create new User");
                Console.WriteLine("Enter 'login [userName]' to login User");
                Console.ForegroundColor = ConsoleColor.White;
            

                var input = Console.ReadLine();
                if (input.StartsWith("login "))
                {
                    var lu = ObjectFactory.Container.GetInstance<LoginUserMessageBinder>();
                    var userName = input.Substring(input.LastIndexOf(" ", StringComparison.Ordinal)+1);
                    try
                    {
                        lu.AcceptRequest(userName, unpw[userName]);
                        Console.WriteLine(Environment.NewLine);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("User {0} Logged in successfully", userName);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("userName: {0}", userName);
                    }
                }
                if (!string.IsNullOrEmpty(input) && !input.StartsWith("login "))
                {
                    _fixture = new Fixture();
                    var mb = ObjectFactory.Container.GetInstance<RegisterUserMessageBinder>();
                    var smr = _fixture.Create<SomeMessyRequest>();
                    try
                    {
                        var userName = input.Trim();
                        var dob = DateTime.Now.AddYears(-(new Random().Next(0, 50)));
                        mb.AcceptRequest(userName, smr.Email, smr.LastName, smr.FirstName, smr.Password, dob);
                        unpw.Add(userName, smr.Password);
                        Console.WriteLine(Environment.NewLine);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("User {0} Created successfully", userName);
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine(ex.Message);
                    }
                }
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("You must enter a value fool!");
                }
                Console.WriteLine(Environment.NewLine);

            }
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
