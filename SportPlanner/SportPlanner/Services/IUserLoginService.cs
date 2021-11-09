using SportPlanner.Models;
using System;
using System.Threading.Tasks;

namespace SportPlanner.Services
{
    public interface IUserLoginService
    {
        Task DeleteUser(Guid id);

        Task<(bool succeeded, User user)> LoginUser();

        Task<bool> UpsertUser(User user);
    }
}