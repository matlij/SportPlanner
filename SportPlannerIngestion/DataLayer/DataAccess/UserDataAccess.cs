using ModelsCore;
using ModelsCore.Interfaces;
using SportPlannerIngestion.DataLayer.Data;
using SportPlannerIngestion.DataLayer.Models;
using SportPlannerIngestion.DataLayer.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportPlannerIngestion.DataLayer.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        private readonly string _connectionString;

        public UserDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<UserDto> GetAll()
        {
            using var context = new SportPlannerContext(_connectionString);

            return context.Users
                .Select(u => u.AsUserDto())
                .ToList();
        }
    }
}
