using StructureMap;
using StructureMap.Graph;
using XO.Local.Domain;

namespace XO.Local.Spike.ConsoleApp
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
                x.Scan(z =>
                {
                    z.TheCallingAssembly();
                    z.WithDefaultConventions();
                });
                x.AddRegistry(new InfrastructureRegistry());
                x.AddRegistry(new MessageBinderRegistry());
                x.AddRegistry(new ReadModelRegistry());
                x.AddRegistry(new WorkflowRegistry());
            });
        }
    }
}