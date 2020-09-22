using System;
using System.Collections.ObjectModel;

namespace SportPlanner.Models
{
    public class Event
    {
        public Event(string id, EventType eventType)
        {
            Id = id;
            EventType = eventType;
            UsersInvited = new ObservableCollection<string>();
            UsersAttending = new ObservableCollection<string>();

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
        public string DateAndDatesLeft { get => $"{Date.Day}/{Date.Month} - Days left: {(Date - DateTime.Now).Days}"; }
        public EventType EventType { get; }
        public string IconImage { get; private set; }
        public ObservableCollection<string> UsersInvited { get; set; }
        public ObservableCollection<string> UsersAttending { get; set; }
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
