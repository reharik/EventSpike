using System;
using StructureMap;
using XO.Local.Spike.Infrastructure;

namespace XO.EventSpike.WorkflowConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Bootstrap();
            try
            {
                var dispatcher = ObjectFactory.Container.GetInstance<IDispatcher>();
                dispatcher.StartDispatching();
            }
            catch (Exception ex)
            {
                // do something with exception.  emit event or something
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }   
    }
}
