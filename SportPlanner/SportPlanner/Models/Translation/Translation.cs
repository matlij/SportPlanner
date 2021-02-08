using ModelsCore;
using ModelsCore.TaskModels;
using SportPlanner.Extensions;
using System.Linq;

namespace SportPlanner.Models.Translation
{
    internal static class Translation
    {
        internal static User AsUser(this UserDto userDto)
        {
            return new User(userDto.UserId)
            {
                Name = userDto.UserName
            };
        }

        internal static Event AsEvent(this EventDto eventDto)
        {
            return new Event(eventDto.Identifier, (EventType)eventDto.EventType)
            {
                Address = eventDto.Address,
                Date = eventDto.Date,
                Users = eventDto.Users.AsObservableCollection(CreateEventUser)
            };
        }

        internal static EventDto AsEventDto(this Event @event) => new EventDto
        {
            Identifier = @event.Id,
            Address = @event.Address,
            Date = @event.Date,
            EventType = (ModelsCore.Enums.EventType)@event.EventType,
            Users = @event.Users.Select(CreateEventUserDto).ToList()
        };

        internal static TaskCreateEvent AsTaskCreateEvent(this Event @event) => new TaskCreateEvent
        {
            Address = @event.Address,
            Date = @event.Date,
            EventType = (ModelsCore.Enums.EventType)@event.EventType,
            Users = @event.Users.Select(CreateEventUserDto).ToList()
        };

        internal static TaskUpdateEvent AsTaskUpdateEvent(this Event @event) => new TaskUpdateEvent
        {
            Identifier = @event.Id,
            Address = @event.Address,
            Date = @event.Date,
            EventType = (ModelsCore.Enums.EventType)@event.EventType,
            Users = @event.Users.Select(CreateEventUserDto).ToList()
        };

        private static EventUser CreateEventUser(EventUserDto eventUser) => new EventUser(eventUser.UserId)
        {
            IsAttending = eventUser.IsAttending,
            UserName = eventUser.UserName
        };

        private static EventUserDto CreateEventUserDto(EventUser eventUser) => new EventUserDto
        {
            UserId = eventUser.UserId,
            IsAttending = eventUser.IsAttending
        };
    }
}
