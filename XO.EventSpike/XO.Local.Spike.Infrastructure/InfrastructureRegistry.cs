using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using XO.Local.Spike.Infrastructure.Mongo;

namespace XO.Local.Spike.Infrastructure
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
          
        }
    }
}