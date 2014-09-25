using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace EventSpike.ReadModel
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
          
        }
    }
}