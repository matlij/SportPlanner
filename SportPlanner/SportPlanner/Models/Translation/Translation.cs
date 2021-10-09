using ModelsCore;
using SportPlanner.Extensions;
using System.Linq;

namespace SportPlanner.Models.Translation
{
    internal static class Translation
    {
        internal static User AsUser(this UserDto userDto)
        {
            return new User(userDto.Id)
            {
                Name = userDto.Name
            };
        }

        internal static Address AsAddress(this AddressDto address)
        {
            return new Address
            {
                Id = address.Id,
                FullAddress = address.FullAddress,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
            };
        }

        internal static AddressDto AsAddressDto(this Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                FullAddress = address.FullAddress,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
            };
        }

        internal static Event AsEvent(this EventDto eventDto)
        {
            return new Event(eventDto.Id, (EventType)eventDto.EventType)
            {
                Address = eventDto.Address.AsAddress(),
                Date = eventDto.Date,
                Users = eventDto.Users.AsObservableCollection(CreateEventUser)
            };
        }

        internal static EventDto AsEventDto(this Event @event)
        {
            return new EventDto
            {
                Id = @event.Id,
                Address = @event.Address.AsAddressDto(),
                Date = @event.Date,
                EventType = (ModelsCore.Enums.EventType)@event.EventType,
                Users = @event.Users.Select(CreateEventUserDto).ToList(),
            };
        }

        private static EventUser CreateEventUser(EventUserDto eventUser)
        {
            return new EventUser(eventUser.UserId)
            {
                UserName = eventUser.UserName,
                IsOwner = eventUser.IsOwner,
                UserReply = (EventReply)eventUser.UserReply
            };
        }

        private static EventUserDto CreateEventUserDto(EventUser eventUser)
        {
            return new EventUserDto
            {
                UserId = eventUser.UserId,
                UserReply = (int)eventUser.UserReply,
                IsOwner = eventUser.IsOwner
            };
        }
    }
}
