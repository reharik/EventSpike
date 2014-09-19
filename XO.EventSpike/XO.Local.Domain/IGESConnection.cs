using System.Net;
using EventStore.ClientAPI;

namespace XO.Local.Domain
{
    public interface IGESConnection
    {
        IEventStoreConnection BuildConnection();
    }

    public class GESConnection : IGESConnection
    {
        public IEventStoreConnection BuildConnection()
        {
            return EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
        }
    }
}