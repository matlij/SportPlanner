using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlippinTen.Core.Repository;
using ModelsCore;
using ModelsCore.Constants;
using SportPlanner.Models;
using SportPlanner.Models.Translation;

namespace SportPlanner.Services
{
    public class EventDataStore : IEventDataStore
    {
        private readonly IGenericRepository _repository;

        public EventDataStore(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddAsync(Event @event)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $@"{UriConstants.EventUri}"
            };

            var eventDto = @event.AsEventDto();
            await _repository.PostAsync(uri.ToString(), eventDto);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(Event @event)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"{UriConstants.EventUri}/{@event.Id}"
            };

            var eventDto = @event.AsEventDto();
            await _repository.PutAsync(uri.ToString(), eventDto);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await Task.FromResult(false);
        }

        public async Task<Event> GetAsync(string id)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"{UriConstants.EventUri}/{id}"
            };

            var result = await _repository.GetAsync<EventDto>(uri.ToString());

            return result.AsEvent();
        }

        public async Task<IEnumerable<Event>> GetAsync(bool forceRefresh = false)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"{UriConstants.EventUri}"
            };

            return await GetEvents(uri);
        }

        public async Task<IEnumerable<Event>> GetFromUserAsync(string userId, bool forceRefresh = false)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"api/user/{userId}/events"
            };

            return await GetEvents(uri);
        }

        private async Task<IEnumerable<Event>> GetEvents(UriBuilder uri)
        {
            var result = await _repository.GetAsync<IEnumerable<EventDto>>(uri.ToString());

            return result
                .Select(e => e.AsEvent())
                .ToList();
        }
    }
}