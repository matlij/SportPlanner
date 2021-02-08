using SportPlanner.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportPlanner.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(string id);
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetAsync(bool forceRefresh = false);
    }

    public interface IEventDataStore : IDataStore<Event>
    {
        Task<IEnumerable<Event>> GetFromUserAsync(string userId, bool forceRefresh = false);
    }
}
