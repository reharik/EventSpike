using System;

namespace XO.Local.Spike.Infrastructure.Interfaces
{
    public interface IRouteEvents
	{
		void Register<T>(Action<T> handler);
		void Register(IAggregate aggregate);

		void Dispatch(object eventMessage);
	}
}