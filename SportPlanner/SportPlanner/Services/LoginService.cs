using SportPlanner.Models;
using SportPlanner.Models.Constants;
using SportPlanner.Repository.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SportPlanner.Services
{
    public class UserLoginService : IUserLoginService
    {
        private readonly IDataStore<User> _userDataStore;
        private readonly ILocalRepository<User> _localUserInfoRepository;

        public UserLoginService(IDataStore<User> userDataStore, ILocalRepository<User> localUserInfoRepository)
        {
            _userDataStore = userDataStore ?? throw new System.ArgumentNullException(nameof(userDataStore));
            _localUserInfoRepository = localUserInfoRepository ?? throw new System.ArgumentNullException(nameof(localUserInfoRepository));
        }

        public async Task<(bool succeeded, User user)> LoginUser()
        {
            var user = _localUserInfoRepository.GetEntity(FileNameConstants.UserInfoJson);
            if (user is null)
            {
                return (false, null);
            }

            var userInServer = await _userDataStore.GetAsync(user.Id.ToString());
            if (userInServer is null)
            {
                await _userDataStore.AddAsync(user);
            }
            else if (user.Name != userInServer.Name)
            {
                return (false, null);
            }

            return (true, user);
        }

        public async Task<bool> UpsertUser(User user)
        {
            try
            {
                var succeeded = await _userDataStore.UpdateAsync(user);
                if (!succeeded)
                {
                    Debug.WriteLine($"Register user '{user.Name}' failed. Server return an error.");
                    return false;
                }
                else
                {
                    _localUserInfoRepository.UpsertEntity(FileNameConstants.UserInfoJson, user);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Upsert user with id '{user.Id}' failed. {e.Message}");
                return false;
            }
        }

        public async Task DeleteUser(Guid id)
        {
            await _userDataStore.DeleteAsync(id.ToString());
            _localUserInfoRepository.DeleteEntity(FileNameConstants.UserInfoJson);
        }
    }
}
