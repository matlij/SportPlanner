using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportPlanner.Models;

namespace SportPlanner.Services
{
    public class MockEventDataStore : IDataStore<Event>
    {
        readonly List<Event> _events;

        public MockEventDataStore()
        {
            _events = new List<Event>()
            {
                new Event(Guid.NewGuid().ToString(), EventType.Traning){ Date = new DateTime(2020, 09, 22, 20, 0, 0) },
                new Event(Guid.NewGuid().ToString(), EventType.Traning){ Date = new DateTime(2020, 09, 29, 20, 0, 0) },
                new Event(Guid.NewGuid().ToString(), EventType.Game)
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
            var oldItem = _events.Where((Event arg) => arg.Id == @event.Id).FirstOrDefault();
            _events.Remove(oldItem);
            _events.Add(@event);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var oldItem = _events.Where((Event arg) => arg.Id == id).FirstOrDefault();
            _events.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Event> GetAsync(string id)
        {
            return await Task.FromResult(_events.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Event>> GetFromUserAsync(string userId, bool forceRefresh = false)
        {
            return await Task.FromResult(_events.Where(e => e.Users.Any(u => u.UserId == userId)));
        }

        public async Task<IEnumerable<Event>> GetAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_events);
        }
    }
}