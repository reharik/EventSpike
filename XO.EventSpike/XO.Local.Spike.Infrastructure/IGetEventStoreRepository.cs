using System;
using System.Collections.Generic;
using XO.Local.Spike.Infrastructure.Interfaces;

namespace XO.Local.Spike.Infrastructure
{
    public interface IGetEventStoreRepository
	{
	    TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate;
		TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate;
		void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
	}
}