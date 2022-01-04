using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SportPlanner.Models
{
    public class Event
    {
        public Event(Guid id, EventType eventType)
        {
            Id = id;
            EventType = eventType;
            Users = new ObservableCollection<EventUser>();
        }

        public Guid Id { get; }
        public DateTime Date { get; set; }
        public string DateString => $"Tid: {Date.TimeOfDay:hh\\:mm} - {Date.ToLongDateString()}";
        public string DaysLeft { get => $"Days left: {(Date - DateTime.Now).Days}"; }
        public EventType EventType { get; }
        public Address Address { get; set; }
        public ObservableCollection<EventUser> Users { get; set; }
        public bool CurrentUserIsAttending { get; set; }
        public string UsersAttending { get => $"Närvaro: {Users.Count(u => u.IsAttending)} / {Users.Count}"; }
    }

    public class Address
    {
        public Guid Id { get; set; }
        public string FullAddress { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
