using System;
using System.Collections;

namespace XO.Local.Spike.Infrastructure.Interfaces
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