using System;

namespace SportPlanner.Models
{
    public class User
    {
        public User(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
        public string Name { get; set; }
    }
}
