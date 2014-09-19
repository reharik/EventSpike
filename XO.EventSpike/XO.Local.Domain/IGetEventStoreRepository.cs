using System;
using System.Collections.Generic;
using XO.Local.Domain.Interfaces;

namespace XO.Local.Domain
{
    public interface IGetEventStoreRepository
	{
	    TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate;
		TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate;
		void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
	}
}