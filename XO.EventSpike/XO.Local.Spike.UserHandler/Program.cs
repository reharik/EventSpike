using System;
using StructureMap;

namespace XO.Local.Spike.UserHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Bootstrap();
            var userHandler = ObjectFactory.Container.GetInstance<IUserHandler>();
            userHandler.StartDispatching();    
            Console.Read();
        }
    }
}   
