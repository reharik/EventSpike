using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using XO.Local.Spike.Infrastructure.SharedModels;

namespace XO.Local.Spike.Infrastructure.Mongo
{
    public interface IMongoRepository
    {
        T Get<T>(Guid id) where T : IReadModel;
        T Get<T>(Expression<Func<T, bool>> filter) where T : IReadModel;
        IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> filter = null) where T : IReadModel;
        void Save<T>(T value) where T : IReadModel;
        void Save<T>(IEnumerable<T> values) where T : IReadModel;
    }

    public class MongoRepository : IMongoRepository
    {
        private MongoDatabase _mongoDatabase { get; set; }

        public MongoRepository(MongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public virtual T Get<T>(Guid id) where T:IReadModel
        {
            var collection = _mongoDatabase.GetCollection<T>(typeof(T).Name.ToLower());
            return collection.FindOneByIdAs<T>(id);
        }

        public virtual IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> filter = null) where T : IReadModel
        {
            var collection = _mongoDatabase.GetCollection<T>(typeof(T).Name.ToLower());
            var query = filter == null ? null : Query<T>.Where(filter);
            return (null == query ? collection.FindAllAs<T>() : collection.FindAs<T>(query)).ToList();
        }

        public virtual T Get<T>(Expression<Func<T, bool>> filter) where T : IReadModel
        {
            return GetAll(filter).FirstOrDefault();
        }

        public virtual void Save<T>(T value) where T : IReadModel
        {
            var collection = _mongoDatabase.GetCollection<T>(value.GetType().Name.ToLower());
            var result = collection.Save(value);

            if (!result.Ok)
                throw new MongoQueryException(result.ErrorMessage);
        }

        public virtual void Save<T>(IEnumerable<T> values) where T : IReadModel
        {
            var collection = _mongoDatabase.GetCollection<T>(typeof(T).Name.ToLower());
            var results = collection.InsertBatch<T>(values);

            if (results.Any(r => !r.Ok))
                throw new MongoQueryException(string.Format("Error ocurred on insert batch for {0} documents",
                    results.Count(r => !r.Ok)));
        }


        protected IMongoQuery QueryBuilder(params IMongoQuery[] filters)
        {
            var query = new List<IMongoQuery>(filters);
            return query.Any() ? Query.And(query) : null;
        }
    }
}
