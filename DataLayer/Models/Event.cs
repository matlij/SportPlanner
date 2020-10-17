using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Identifier { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; }
        public int EventType { get; set; }
        public string Address { get; set; }
        public ICollection<EventUser> EventUsers { get; set; }
    }

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Identifier { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public ICollection<EventUser> EventUsers { get; set; }
    }

    public class EventUser
    {
        public int EventId { get; set; }
        public int UserId { get; set; }

        public Event Event { get; set; }
        public User User { get; set; }
        public bool IsAttending { get; set; }
    }
}
