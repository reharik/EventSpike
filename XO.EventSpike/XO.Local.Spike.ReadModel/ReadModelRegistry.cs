using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using XO.Local.Spike.ReadModel;

namespace XO.Local.Domain
{
     public class ReadModelRegistry : Registry
    {
        public ReadModelRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
            });
            For<IMongoDB>().Use(x => new Spike.ReadModel.MongoDB("mongodb://localhost"));
            For<IMongoRepository>().Use(x => new MongoRepository(x.GetInstance<IMongoDB>().GetDatabase()));
        }
    }
}