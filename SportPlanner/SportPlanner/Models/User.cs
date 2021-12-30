using SQLite;
using System;

namespace SportPlanner.Models
{
    public class User
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
