using System;
using StructureMap;

namespace XO.Local.Spike.UserHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Bootstrap();
            var dispatcher = ObjectFactory.Container.GetInstance<IReadStoreDispatcher>();
            dispatcher.StartDispatching();    
            Console.Read();
        }
    }
}   
