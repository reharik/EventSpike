using System;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using XO.Local.Spike.Infrastructure.SharedModels;

namespace XO.Local.Spike.MessageBinders
{
    public class MessageBinderBase
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private const string CommandClrTypeHeader = "CommandClrTypeName";
        private const string CommitIdHeader = "CommitId";
        private const string EventStreamName = "CommandDispatch";

        public MessageBinderBase(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        protected void PostEvent(IGESEvent @event, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            var commitHeaders = new Dictionary<string, object>
                {
                    {CommitIdHeader, commitId},
                    {CommandClrTypeHeader, this.GetType().AssemblyQualifiedName}
                };
            updateHeaders(commitHeaders);
            var commandToSave = new[] {ToEventData(Guid.NewGuid(), @event, commitHeaders)};
           
            _eventStoreConnection.AppendToStreamAsync(EventStreamName, ExpectedVersion.Any, commandToSave);
        }

        private static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, object> headers)
        {
            var serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, serializerSettings));

            var eventHeaders = new Dictionary<string, object>(headers)
                {
                    {
                        CommandClrTypeHeader, evnt.GetType().AssemblyQualifiedName
                    }
                };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, serializerSettings));
            var typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }
    }
}