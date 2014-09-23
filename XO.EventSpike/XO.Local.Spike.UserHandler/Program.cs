using System;
using StructureMap;
using XO.Local.Spike.Infrastructure;

namespace XO.Local.Spike.UserHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Bootstrap();
            var dispatcher = ObjectFactory.Container.GetInstance<IDispatcher>();
            dispatcher.StartDispatching();    
            Console.Read();
        }
    }
}   
