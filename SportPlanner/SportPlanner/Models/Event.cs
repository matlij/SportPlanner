using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SportPlanner.Models
{
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

                case EventType.Social:
                    return "icon_party.png";

                default:
                    return null;
            };
        }

        public string Id { get; }
        public DateTime Date { get; set; }
        public string DateString => $"{Date.TimeOfDay:hh\\:mm} - {Date.ToLongDateString()}";
        public string DaysLeft { get => $"Days left: {(Date - DateTime.Now).Days}"; }
        public EventType EventType { get; }
        public string IconImage { get; private set; }
        public string Address { get; set; }
        public ObservableCollection<EventUser> Users { get; set; }
        public bool CurrentUserIsAttending { get; set; }
        public string UsersAttending { get => $"{Users.Count(u => u.IsAttending)} / {Users.Count} attending"; }
    }
}
