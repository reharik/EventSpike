using StructureMap.Configuration.DSL;
using StructureMap.Graph;

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
            For<IGetEventStoreRepository>().Singleton()
                .Use(x => new GetEventStoreRepository(x.GetInstance<IGESConnection>().BuildConnection()));
        }
    }
}