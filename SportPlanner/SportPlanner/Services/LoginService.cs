using ModelsCore.Enums;
using ModelsCore.Extensions;
using SportPlanner.Models;
using SportPlanner.Repository;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SportPlanner.Services
{
    public class UserLoginService : IUserLoginService
    {
        private readonly IDataStore<User> _userDataStore;

        public UserLoginService(IDataStore<User> userDataStore)
        {
            _userDataStore = userDataStore ?? throw new ArgumentNullException(nameof(userDataStore));
        }

        public async Task<(bool succeeded, User user)> LoginUser()
        {
            var user = await GetUserFromLocalDb();
            if (user is null)
            {
                return (false, null);
            }

            var userInServer = await _userDataStore.GetAsync(user.Id.ToString());
            if (userInServer is null)
            {
                await _userDataStore.AddAsync(user);
            }
            else if (user.Name != userInServer.Name || user.Id != userInServer.Id)
            {
                return (false, null);
            }

            return (true, user);
        }

        public async Task<User> GetUserFromLocalDb()
        {
            var db = await UserDatabase.Instance;

            return (await db.GetAll())?.SingleOrDefault();
        }

        public async Task<CrudResult> AddUser(User newUser)
        {
            var result = await _userDataStore.AddAsync(newUser);
            if (!result.IsPositiveResult())
            {
                return result;
            }
            else if (result == CrudResult.AlreadyExists)
            {
                var existingUser = (await _userDataStore.GetAsync(forceRefresh: true)).Single(u => string.Equals(u.Name, newUser.Name, StringComparison.OrdinalIgnoreCase));
                return await UpdateInLocalDb(existingUser);
            }
            else
            {
                return await UpdateInLocalDb(newUser);
            }
        }

        private static async Task<CrudResult> UpdateInLocalDb(User entity)
        {
            var db = await UserDatabase.Instance;
            return await db.Upsert(entity);
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
                    var db = await UserDatabase.Instance;
                    var result = await db.Upsert(user);
                    return result.IsPositiveResult();
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

            var db = await UserDatabase.Instance;
            await db.Delete(id);
        }
    }
}
