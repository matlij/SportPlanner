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
                new Event(Guid.NewGuid().ToString()){ Date = new DateTime(2020, 09, 22), EventType = EventType.Traning  },
                new Event(Guid.NewGuid().ToString()){ Date = new DateTime(2020, 09, 29), EventType = EventType.Traning  }
            };
        }

        public async Task<bool> AddItemAsync(Event @event)
        {
            _events.Add(@event);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Event @event)
        {
            var oldItem = _events.Where((Event arg) => arg.Id == @event.Id).FirstOrDefault();
            _events.Remove(oldItem);
            _events.Add(@event);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = _events.Where((Event arg) => arg.Id == id).FirstOrDefault();
            _events.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Event> GetItemAsync(string id)
        {
            return await Task.FromResult(_events.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Event>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_events);
        }
    }
}