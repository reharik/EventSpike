using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Infrastructure.GES.Interfaces;
using XO.Local.Spike.Infrastructure.Mongo;

namespace XO.Local.Spike.Workflows
{
    public class CommandDispatcher : DispatcherBase
    {
        public CommandDispatcher(IMongoRepository mongoRepository, IGESConnection gesConnection, List<IHandler> eventHandlers) 
            : base(mongoRepository, gesConnection, eventHandlers)
        {
            _targetClrTypeName = "CommandClrTypeName";
            _eventFilter = x => !x.Event.EventType.StartsWith("$") && JObject.Parse(Encoding.UTF8.GetString(x.Event.Metadata)).Property(_targetClrTypeName).HasValues;
        }
    }
}