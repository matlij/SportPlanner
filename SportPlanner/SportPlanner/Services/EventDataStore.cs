using ModelsCore;
using ModelsCore.Enums;
using SportPlanner.Models;
using SportPlanner.Models.Constants;
using SportPlanner.Models.Translation;
using SportPlanner.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportPlanner.Services
{
    public class EventDataStore : IEventDataStore
    {
        private readonly IGenericRepository _repository;

        public EventDataStore(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<CrudResult> AddAsync(Event @event)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $@"{UriConstants.EventUri}"
            };

            var eventDto = @event.AsEventDto();
            await _repository.PostAsync(uri.ToString(), eventDto);

            return await Task.FromResult(CrudResult.Ok);
        }

        public Task<bool> UpdateAsync(Event @event)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"{UriConstants.EventUri}/{@event.Id}"
            };

            var eventDto = @event.AsEventDto();
            return _repository.PutAsync(uri.ToString(), eventDto);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"{UriConstants.EventUri}/{id}"
            };

            var result = await _repository.DeleteAsync(uri.ToString());

            return await Task.FromResult(result);
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
                Path = UriConstants.EventUri,
            };

            return await GetEvents(uri);
        }

        public async Task<IEnumerable<Event>> GetFromUserAsync(Guid userId, bool forceRefresh = false)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = UriConstants.EventUri,
                Query = $"userId={userId}"
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