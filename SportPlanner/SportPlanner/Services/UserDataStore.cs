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
    internal class UserDataStore : IDataStore<User>
    {
        private readonly IGenericRepository _repository;

        public UserDataStore(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<CrudResult> AddAsync(User item)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"{UriConstants.UserUri}"
            };

            var response = await _repository.PostAsync(uri.ToString(), item);
            return response.result;
        }

        public async Task<bool> UpdateAsync(User item)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"{UriConstants.UserUri}/{item.Id}"
            };

            return await _repository.PutAsync(uri.ToString(), item);
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAsync(bool forceRefresh = false)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"{UriConstants.UserUri}"
            };

            var result = await _repository.GetAsync<IEnumerable<UserDto>>(uri.ToString());
            return result
                .Select(u => u.AsUser())
                .ToList();
        }

        public async Task<User> GetAsync(string id)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"{UriConstants.UserUri}/{id}"
            };

            var result = await _repository.GetAsync<UserDto>(uri.ToString());
            if (result is null)
            {
                return null;
            }

            return result.AsUser();
        }
    }
}
