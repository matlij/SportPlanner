using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SportPlanner.Models
{
    public class Event
    {
        public Event(string id)
        {
            Id = id;
            UsersInvited = new List<string>();
            UsersAttending = new List<string>();
        }

        public string Id { get; }
        public DateTime Date { get; set; }
        public string DayAndDate { get => $"{Date.DayOfWeek} {Date.Day}/{Date.Month}"; }
        public EventType EventType { get; set; }
        public IEnumerable<string> UsersInvited { get; set; }
        public IEnumerable<string> UsersAttending { get; set; }
    }

    public class User
    {
        public User(string id)
        {
            Id = id;
        }

        public string Id { get; }
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
