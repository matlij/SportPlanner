using DataLayer.Data;
using DataLayer.Models;
using DataLayer.Models.Translations;
using Microsoft.EntityFrameworkCore;
using ModelsCore;
using ModelsCore.Interfaces;
using ModelsCore.TaskModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DataLayer : IDataLayer
    {
        public EventDto Get(string identifier)
        {
            using var context = new SportPlannerContext();

            return context.Events
                .FirstOrDefault(e => e.Identifier == identifier)
                .AsEventDto();
        }

        public ICollection<EventDto> GetByUser(string userIdentifier)
        {
            using var context = new SportPlannerContext();

            var events = context.Events
                .Include(e => e.EventUsers)
                .ThenInclude(e => e.User)
                .Where(e => e.EventUsers.Any(eu => eu.User.Identifier == userIdentifier));

            return events.Select(e => e.AsEventDto()).ToList();
        }

        public async Task<EventDto> Store(TaskCreateEvent input)
        {
            using var context = new SportPlannerContext();

            var @event = input.AsEvent();
            context.Add(@event);
            await context.SaveChangesAsync();

            return @event.AsEventDto();
        }

        public async Task<EventDto> Update(TaskUpdateEvent input)
        {
            using var context = new SportPlannerContext();

            var existing = context.Events
                .Include(e => e.EventUsers)
                .Single(e => e.Identifier == input.Identifier);

            var newUsers = GetEventUsers(input.Users, context, existing);
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
            using var context = new SportPlannerContext();

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

        private static ICollection<EventUser> GetEventUsers(IEnumerable<EventUserDto> inputUsers, SportPlannerContext context, Event existing)
        {
            var newUsers = new List<EventUser>();
            if (inputUsers == null)
            {
                return newUsers;
            }

            foreach (var inputEventUser in inputUsers)
            {
                var existingUser = context.Users
                    .Single(u => u.Identifier == inputEventUser.Identifier);
                var newEventUser = new EventUser
                {
                    User = existingUser,
                    Event = existing,
                    IsAttending = inputEventUser.IsAttending
                };
                newUsers.Add(newEventUser);
            }

            return newUsers;
        }
    }
}
