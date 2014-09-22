using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XO.Local.Spike.Infrastructure.Interfaces;

namespace XO.Local.Spike.Infrastructure
{
    public interface IGetEventStoreRepository
	{
        Task<TAggregate> GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate;
        Task<TAggregate> GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate;
		void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
	}
}