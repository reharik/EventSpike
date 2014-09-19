using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace XO.Local.Domain
{
     public class MessageBinderRegistry : Registry
    {
        public MessageBinderRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
            });

        }
    }
}