using StructureMap;
using StructureMap.Graph;
using XO.Local.Domain;
using XO.Local.Spike.Infrastructure;

namespace XO.Local.Spike.UserHandler
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
                x.Scan(z=>
                {
                    z.TheCallingAssembly();
                    z.AddAllTypesOf<IEventHandler>();
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