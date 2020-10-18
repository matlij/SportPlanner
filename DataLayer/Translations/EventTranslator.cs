using ModelsCore;
using ModelsCore.Enums;
using ModelsCore.TaskModels;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Models.Translations
{
    public static class EventTranslator
    {
        public static EventDto AsEventDto(this Event input)
        {
            return new EventDto
            {
                Identifier = input.Identifier,
                Address = input.Address,
                Date = input.Date,
                EventType = (EventType)input.EventType,
                Users = input.EventUsers != null
                    ? input.EventUsers.Select(AsEventUserDto).ToList()
                    : new List<EventUserDto>()
            };
        }

        public static Event AsEvent(this TaskUpdateEvent input)
        {
            return new Event
            {
                Identifier = input.Identifier,
                Address = input.Address,
                Date = input.Date,
                EventType = (int)input.EventType,
                EventUsers = input.Users != null
                    ? input.Users.Select(u => AsEventUser(input.Identifier, u)).ToList()
                    : new List<EventUser>()
            };
        }

        public static Event AsEvent(this TaskCreateEvent input)
        {
            return new Event
            {
                Address = input.Address,
                Date = input.Date,
                EventType = (int)input.EventType
            };
        }

        private static EventUserDto AsEventUserDto(EventUser input)
        {
            return new EventUserDto
            {
                UserId = input.User.Identifier,
                IsAttending = input.IsAttending
            };
        }

        private static EventUser AsEventUser(string eventIdentifier, EventUserDto input)
        {
            return new EventUser
            {
                User = new User { Identifier = input.UserId },
                Event = new Event { Identifier = eventIdentifier },
                IsAttending = input.IsAttending
            };
        }
    }
}
