using EventSpike.Infrastructure.GES;
using EventSpike.Infrastructure.GES.Interfaces;
using EventSpike.Infrastructure.Mongo;
using EventStore.ClientAPI;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace EventSpike.Infrastructure
{
     public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
            });
            For<IMongoDB>().Use(x => new Mongo.MongoDB("mongodb://localhost"));
            For<IMongoRepository>().Use(x => new MongoRepository(x.GetInstance<IMongoDB>().GetDatabase()));
            For<IEventStoreConnection>().Use(x => x.GetInstance<IGESConnection>().BuildConnection());
            For<IGetEventStoreRepository>().Use(x => new GetEventStoreRepository(x.GetInstance<IEventStoreConnection>()));
        }
    }
}