using EventSpike.Infrastructure;
using EventSpike.MessageBinders;
using EventSpike.ReadModel;
using StructureMap;
using StructureMap.Graph;

namespace EventSpike.MessageBinderConsole
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
            });
        }
    }
}
