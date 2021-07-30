using ModelsCore;
using ModelsCore.Interfaces;
using SportPlannerIngestion.DataLayer.Data;
using SportPlannerIngestion.DataLayer.Translations;
using System.Collections.Generic;
using System.Linq;

namespace SportPlannerIngestion.DataLayer.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        private readonly string _connectionString;
        private readonly SportPlannerContext _context;

        public UserDataAccess(SportPlannerContext context)
        {
            _context = context;
        }

        public IEnumerable<UserDto> GetAll()
        {
            return _context.Users
                .Select(u => u.AsUserDto())
                .ToList();
        }
    }
}
