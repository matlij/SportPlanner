using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModelsCore;
using ModelsCore.Exceptions;
using ModelsCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportPlannerIngestion.DataLayer.Data;
using SportPlannerIngestion.DataLayer.Models.Translations;
using SportPlannerIngestion.DataLayer.Models;
using System;

namespace SportPlannerIngestion.DataLayer.DataAccess
{
    public class EventDataAccess : IEventDataAccess
    {
        private readonly string _connectionString;

        public EventDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ICollection<EventDto> Get()
        {
            using var context = new SportPlannerContext(_connectionString);
            var events = GetEvents(context);

            return events
                .Select(e => e.AsEventDto())
                .ToList();
        }

        public EventDto Get(string identifier)
        {
            using var context = new SportPlannerContext(_connectionString);

            return GetEvents(context)
                .FirstOrDefault(e => e.Identifier == identifier)
                .AsEventDto();
        }

        public ICollection<EventDto> GetByUser(string userIdentifier)
        {
            using var context = new SportPlannerContext(_connectionString);

            var events = GetEvents(context)
                .Where(e => e.EventUsers
                    .Any(eu => eu.User.Identifier == userIdentifier));

            return events.Select(e => e.AsEventDto()).ToList();
        }

        public async Task<EventDto> Store(EventDto input)
        {
            using var context = new SportPlannerContext(_connectionString);

            return await StoreEvent(context, input);
        }

        public async Task<EventDto> Update(EventDto input)
        {
            using var context = new SportPlannerContext(_connectionString);

            var existing = context.Events
                .Include(e => e.EventUsers)
                .SingleOrDefault(e => e.Identifier == input.Identifier);
            if (existing == null)
            {
                return await StoreEvent(context, input);
            }

            var newUsers = GetEventUsers(context, existing, input.Users);
            if (existing.EventUsers != null)
                existing.EventUsers.Clear();
            existing.EventUsers = newUsers;

            existing.Date = input.Date;
            existing.Address = input.Address;
            existing.EventType = (int)input.EventType;

            await context.SaveChangesAsync();
            return existing.AsEventDto();
        }

        public async Task<bool> Delete(string identifier)
        {
            using var context = new SportPlannerContext(_connectionString);

            var eventToRemove = context.Events.Single(e => e.Identifier == identifier);
            if (eventToRemove == null)
            {
                return false;
            }

            var eventUsersToRemove = context.EventUsers.Where(eu => eu.Event.Identifier == identifier);
            foreach (var eventUser in eventUsersToRemove)
            {
                context.EventUsers.Remove(eventUser);
            }

            context.Remove(eventToRemove);

            await context.SaveChangesAsync();
            return true;
        }

        private static async Task<EventDto> StoreEvent(SportPlannerContext context, EventDto input)
        {
            if (string.IsNullOrEmpty(input.Identifier))
            {
                input.Identifier = Guid.NewGuid().ToString();
            }

            var @event = input.AsEvent();

            var eventUsers = GetEventUsers(context, @event, input.Users);
            @event.EventUsers = eventUsers;

            context.Add(@event);
            await context.SaveChangesAsync();

            return @event.AsEventDto();
        }

        private static ICollection<EventUser> GetEventUsers(SportPlannerContext context, Event @event, IEnumerable<EventUserDto> inputUsers)
        {
            var eventUsers = new List<EventUser>();
            if (inputUsers == null)
            {
                return eventUsers;
            }

            foreach (var inputEventUser in inputUsers)
            {
                var existingUser = GetUserFromSource(context, inputEventUser.UserId);
                var newEventUser = new EventUser
                {
                    UserId = existingUser.Id,
                    IsAttending = inputEventUser.IsAttending,
                    IsOwner = inputEventUser.IsOwner
                };
                if (@event.Id != default)
                {
                    newEventUser.EventId = @event.Id;
                }
                else
                {
                    newEventUser.Event = @event;
                }

                eventUsers.Add(newEventUser);
            }

            return eventUsers;
        }

        private static User GetUserFromSource(SportPlannerContext context, string userId)
        {
            var user = context.Users.FirstOrDefault(u => u.Identifier == userId);
            if (user == null)
            {
                throw new BadInputException("Couldn't find user with id: " + userId);
            }

            return user;
        }

        private static IIncludableQueryable<Event, User> GetEvents(SportPlannerContext context)
        {
            return context.Events
                .Include(e => e.EventUsers)
                .ThenInclude(e => e.User);
        }
    }
}
