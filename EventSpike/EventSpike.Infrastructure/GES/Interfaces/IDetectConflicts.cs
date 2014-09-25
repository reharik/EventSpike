using System.Collections.Generic;

namespace EventSpike.Infrastructure.GES.Interfaces
{
    public interface IDetectConflicts
	{
		void Register<TUncommitted, TCommitted>(ConflictDelegate handler)
			where TUncommitted : class
			where TCommitted : class;

		bool ConflictsWith(IEnumerable<object> uncommittedEvents, IEnumerable<object> committedEvents);
	}

	public delegate bool ConflictDelegate(object uncommitted, object committed);
}