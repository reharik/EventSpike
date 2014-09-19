using System;

namespace XO.Local.Spike.Domain.AggregateRoots
{
    public class UserCreated
    {
        public Guid Id { get; set; }

        public UserCreated(Guid id)
        {
            Id = id;
        }
    }
}