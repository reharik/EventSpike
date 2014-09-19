using StructureMap.Configuration.DSL;
using StructureMap.Graph;

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
            });

        }
    }
}