﻿using EventSpike.Infrastructure;
using EventSpike.ReadModel;
using StructureMap;
using StructureMap.Graph;

namespace EventSpike.EventHandler
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
                x.Scan(z=>
                {
                    z.TheCallingAssembly();
                    z.AddAllTypesOf<IHandler>();
                    z.WithDefaultConventions();
                });
                x.For<IDispatcher>().Use<ReadStoreDispatcher>();
                x.AddRegistry(new InfrastructureRegistry());
                x.AddRegistry(new ReadModelRegistry());
            });
        }
    }
}