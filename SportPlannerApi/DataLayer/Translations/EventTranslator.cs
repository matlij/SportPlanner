using ModelsCore;
using ModelsCore.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SportPlannerIngestion.DataLayer.Models.Translations
{
    public static class EventTranslator
    {
        public static Event AsEvent(this EventDto input)
        {
            return new Event
            {
                Identifier = input.Identifier,
                Address = input.Address,
                Date = input.Date,
                EventType = (int)input.EventType,
                //Owner = new User { Identifier = input.Owner },
                EventUsers = input.Users != null
                    ? input.Users.Select(u => AsEventUser(input.Identifier, u)).ToList()
                    : new List<EventUser>()
            };
        }

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
                    : new List<EventUserDto>(),
                //Owner = input.Owner.Identifier
            };
        }

        private static EventUserDto AsEventUserDto(EventUser input)
        {
            return new EventUserDto
            {
                UserId = input.User.Identifier,
                UserReply = input.UserReply,
                UserName = input.User.Name,
                IsOwner = input.IsOwner
            };
        }

        private static EventUser AsEventUser(string eventIdentifier, EventUserDto input)
        {
            return new EventUser
            {
                User = new User { Identifier = input.UserId },
                Event = new Event { Identifier = eventIdentifier },
                UserReply = input.UserReply,
                IsOwner = input.IsOwner
            };
        }
    }
}
