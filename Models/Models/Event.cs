using System;
using System.Collections.Generic;

namespace SportPlanner.Models.Models
{

    public class Event
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; }
        public EventType EventType { get; set; }
        public string Address { get; set; }
        public ICollection<User> UsersAttending { get; set; }
    }

    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
    }

    public enum EventType
    {
        Undefined,
        Traning,
        Game,
        Social,
        Other
    }
}
