using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Workflows;

namespace XO.Local.Domain
{
     public class WorkflowRegistry : Registry
    {
        public WorkflowRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
                x.AddAllTypesOf<IHandler>();
            });
        }
    }
}