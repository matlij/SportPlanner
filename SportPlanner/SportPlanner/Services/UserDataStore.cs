using ModelsCore;
using ModelsCore.Constants;
using SportPlanner.Models;
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

        public Task<bool> AddAsync(User item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(User item)
        {
            throw new NotImplementedException();
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

        public Task<User> GetAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
