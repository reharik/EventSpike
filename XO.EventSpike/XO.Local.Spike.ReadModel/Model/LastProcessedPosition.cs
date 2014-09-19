using System;

namespace XO.Local.Spike.ReadModel.Model
{
    public class LastProcessedPosition : IReadModel
    {
        public Guid Id { get; set; }
        public string HandlerType { get; set; }
        public long CommitPosition { get; set; }
        public long PreparePosition { get; set; }
    }
}