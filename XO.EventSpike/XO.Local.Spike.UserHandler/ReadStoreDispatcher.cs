using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using XO.Local.Spike.Infrastructure;
using XO.Local.Spike.Infrastructure.GES.Interfaces;
using XO.Local.Spike.Infrastructure.Mongo;
using XO.Local.Spike.ReadModel;

namespace XO.Local.Spike.UserHandler
{
   
    public class ReadStoreDispatcher : DispatcherBase
    {
        public ReadStoreDispatcher(IMongoRepository mongoRepository, IGESConnection gesConnection, List<IHandler> eventHandlers) 
            : base(mongoRepository, gesConnection, eventHandlers)
        {
            _targetClrTypeName = "EventClrTypeName";
            _eventFilter = x => x.Event.EventType.StartsWith("$");
        }
    }
}