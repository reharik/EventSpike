using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSpike.Infrastructure.GES.Interfaces
{
    public interface IGetEventStoreRepository
	{
        Task<TAggregate> GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate;
        Task<TAggregate> GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate;
		void Save(IAggregate aggregate, Guid commitId, IDictionary<string, object> updateHeaders = null);
	}
}