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

                case EventType.Social:
                    return "icon_party.png";

                default:
                    return null;
            };
        }

        public Guid Id { get; }
        public DateTime Date { get; set; }
        public string DateString => $"{Date.TimeOfDay:hh\\:mm} - {Date.ToLongDateString()}";
        public string DaysLeft { get => $"Days left: {(Date - DateTime.Now).Days}"; }
        public EventType EventType { get; }
        public string IconImage { get; private set; }
        public Address Address { get; set; }
        public ObservableCollection<EventUser> Users { get; set; }
        public bool CurrentUserIsAttending { get; set; }
        public string UsersAttending { get => $"{Users.Count(u => u.IsAttending)} / {Users.Count} attending"; }
    }

    public class Address
    {
        public Guid Id { get; set; }
        public string FullAddress { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
