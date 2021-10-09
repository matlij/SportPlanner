using SportPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportPlanner.Services
{
    public class MockEventDataStore : IDataStore<Event>
    {
        private readonly List<Event> _events;

        public MockEventDataStore()
        {
            _events = new List<Event>()
            {
                new Event(Guid.NewGuid(), EventType.Traning){ Date = new DateTime(2020, 09, 22, 20, 0, 0) },
                new Event(Guid.NewGuid(), EventType.Traning){ Date = new DateTime(2020, 09, 29, 20, 0, 0) },
                new Event(Guid.NewGuid(), EventType.Game)
                {
                    Date = new DateTime(2020, 10, 02, 17, 0, 0),
                    Users = new System.Collections.ObjectModel.ObservableCollection<EventUser>()
                },
            };
        }

        public async Task<bool> AddAsync(Event @event)
        {
            _events.Add(@event);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(Event @event)
        {
            var oldItem = _events.FirstOrDefault((Event arg) => arg.Id == @event.Id);
            _events.Remove(oldItem);
            _events.Add(@event);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var oldItem = _events.FirstOrDefault((Event arg) => arg.Id == Guid.Parse(id));
            _events.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Event> GetAsync(string id)
        {
            return await Task.FromResult(_events.FirstOrDefault(s => s.Id == Guid.Parse(id)));
        }

        public async Task<IEnumerable<Event>> GetFromUserAsync(string userId, bool forceRefresh = false)
        {
            return await Task.FromResult(_events.Where(e => e.Users.Any(u => u.UserId == new Guid(userId))));
        }

        public async Task<IEnumerable<Event>> GetAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_events);
        }
    }
}