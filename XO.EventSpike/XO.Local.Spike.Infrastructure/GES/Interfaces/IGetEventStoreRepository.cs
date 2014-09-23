using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XO.Local.Spike.Infrastructure.GES.Interfaces
{
    public interface IGetEventStoreRepository
	{
        Task<TAggregate> GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate;
        Task<TAggregate> GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate;
		void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
	}
}