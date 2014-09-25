using EventSpike.Infrastructure;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace EventSpike.Workflows
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