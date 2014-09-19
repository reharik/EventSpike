using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace XO.Local.Spike.ReadModel
{
    public interface IMongoDB
    {
        MongoDatabase GetDatabase();
    }

    public class MongoDB : IMongoDB, IDisposable
    {
        string _connStr { get; set; }
        MongoDatabase _db { get; set; }
        MongoClient _client { get; set; }

        /// <summary>
        /// Connection string config key
        /// </summary>
        /// <param name="connectionStringName"></param>
        public MongoDB(string connectionStringName)
        {
            _connStr = connectionStringName;
        }

        /// <summary>
        /// Gets MongoDB databse instance
        /// </summary>
        /// <returns></returns>
        public MongoDatabase GetDatabase()
        {
            var connStr = new MongoUrl(_connStr);
            if (null == _client)
                _client = new MongoClient(connStr);

            return (_db ?? (_db = _client.GetServer()
                .GetDatabase("test",
                    new MongoDatabaseSettings
                    {
                        GuidRepresentation = GuidRepresentation.Standard
                    })));
        }

        public void Dispose()
        {
            _db = null;
            _client = null;
        }
    }
}
