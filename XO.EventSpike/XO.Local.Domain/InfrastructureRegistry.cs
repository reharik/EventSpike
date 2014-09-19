using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace XO.Local.Domain
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
            For<IGetEventStoreRepository>()
                .Use(x => new GetEventStoreRepository(x.GetInstance<IGESConnection>().BuildConnection()));
        }
    }
}