using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SportPlanner.Models
{
    public class TaskAddEventUser
    {
        public TaskAddEventUser(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string Name { get; set; }
        public bool Invited { get; set; }
    }

    public class EventUser : IEquatable<EventUser>
    {
        public EventUser(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
        public string UserName { get; set; }
        public bool IsAttending { get; set; }

        public bool Equals(EventUser other)
        {
            if (other == null)
                return false;

            return other.UserId == UserId;
        }
    }

    public class Event
    {
        public Event(string id, EventType eventType)
        {
            Id = id;
            EventType = eventType;
            Users = new ObservableCollection<EventUser>();
            IconImage = GetIconImage(eventType);
        }

        private static string GetIconImage(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Traning:
                    return "icon_traning.png";
                case EventType.Game:
                    return "icon_game.png";
                default:
                    return null;
            };
        }

        public string Id { get; }
        public DateTime Date { get; set; }
        public string DateString { get => Date.ToShortDateString(); }
        public string DaysLeft { get => $"Days left: {(Date - DateTime.Now).Days}"; }
        public EventType EventType { get; }
        public string IconImage { get; private set; }
        public string Address { get; set; }
        public ObservableCollection<EventUser> Users { get; set; }
        public bool CurrentUserIsAttending { get; set; }
        public string UsersAttending { get => $"Attending: {Users.Count(u => u.IsAttending)} / {Users.Count}"; }
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
