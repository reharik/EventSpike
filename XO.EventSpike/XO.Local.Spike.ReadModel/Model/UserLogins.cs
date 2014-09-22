using System;

namespace XO.Local.Spike.ReadModel.Model
{
    public class UserLogins : IReadModel
    {
        public string UserName { get; set; }
        public Guid Id { get; set; }
        public Guid Token { get; set; }
        public DateTime Date { get; set; }
    }
}