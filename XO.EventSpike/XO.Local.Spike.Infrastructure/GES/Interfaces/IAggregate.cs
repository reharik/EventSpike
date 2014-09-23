using System;
using System.Collections;

namespace XO.Local.Spike.Infrastructure.GES.Interfaces
{
    public interface IAggregate
	{
		Guid Id { get; }
		int Version { get; }

		void ApplyEvent(object @event);
		ICollection GetUncommittedEvents();
		void ClearUncommittedEvents();

	}
}