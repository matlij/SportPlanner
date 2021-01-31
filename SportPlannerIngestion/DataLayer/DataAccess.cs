using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModelsCore;
using ModelsCore.Exceptions;
using ModelsCore.Interfaces;
using ModelsCore.TaskModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportPlannerIngestion.DataLayer.Data;
using SportPlannerIngestion.DataLayer.Models.Translations;
using SportPlannerIngestion.DataLayer.Models;

namespace SportPlannerIngestion.DataLayer
{
    public class DataAccess : IDataAccess
    {
        private readonly string _connectionString;

        public DataAccess(string connectionString)
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

            return context.Events
                .Include(e => e.EventUsers)
                .ThenInclude(eu => eu.User)
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

        public async Task<EventDto> Store(TaskCreateEvent input)
        {
            using var context = new SportPlannerContext(_connectionString);

            var @event = input.AsEvent();
            var newUsers = GetEventUsers(context, @event, input.Users);
            @event.EventUsers = newUsers;

            context.Add(@event);
            await context.SaveChangesAsync();

            return @event.AsEventDto();
        }

        public async Task<EventDto> Update(TaskUpdateEvent input)
        {
            using var context = new SportPlannerContext(_connectionString);

            var existing = context.Events
                .Include(e => e.EventUsers)
                .Single(e => e.Identifier == input.Identifier);

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

        private static ICollection<EventUser> GetEventUsers(SportPlannerContext context, Event @event, IEnumerable<EventUserDto> inputUsers)
        {
            var newUsers = new List<EventUser>();
            if (inputUsers == null)
            {
                return newUsers;
            }

            foreach (var inputEventUser in inputUsers)
            {
                User existingUser = GetUserFromSource(context, inputEventUser);
                var newEventUser = new EventUser
                {
                    User = existingUser,
                    Event = @event,
                    IsAttending = inputEventUser.IsAttending
                };
                newUsers.Add(newEventUser);
            }

            return newUsers;
        }

        private static User GetUserFromSource(SportPlannerContext context, EventUserDto inputEventUser)
        {
            var user = context.Users.FirstOrDefault(u => u.Identifier == inputEventUser.UserId);
            if (user == null)
            {
                throw new BadInputException("Couldn't find user with id: " + inputEventUser.UserId);
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
