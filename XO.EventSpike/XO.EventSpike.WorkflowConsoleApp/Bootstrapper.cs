using StructureMap;
using StructureMap.Graph;
using XO.Local.Domain;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Workflows;

namespace XO.EventSpike.WorkflowConsoleApp
{
    public class Bootstrapper
    {
        public static void Bootstrap()
        {
            new Bootstrapper().Start();
        }

        private void Start()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(z =>
                {
                    z.TheCallingAssembly();
                    z.AddAllTypesOf<IHandler>();
                    z.WithDefaultConventions();
                });
                x.For<IDispatcher>().Use<CommandDispatcher>();

                x.AddRegistry(new WorkflowRegistry());
                x.AddRegistry(new InfrastructureRegistry());
//                x.AddRegistry(new ReadModelRegistry());
            });
            ObjectFactory.Container.AssertConfigurationIsValid();
            ObjectFactory.Container.WhatDoIHave();

        }
    }
}