using ModelsCore.Enums;
using SportPlanner.Models;
using System;
using System.Threading.Tasks;

namespace SportPlanner.Services
{
    public interface IUserLoginService
    {
        Task<CrudResult> AddUser(User user);
        Task DeleteUser(Guid id);
        Task<User> GetUserFromLocalDb();
        Task<(bool succeeded, User user)> LoginUser();

        Task<bool> UpsertUser(User user);
    }
}