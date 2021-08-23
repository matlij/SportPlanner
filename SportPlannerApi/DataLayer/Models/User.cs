using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportPlannerIngestion.DataLayer.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Identifier { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
        public ICollection<EventUser> EventUsers { get; set; }
    }
}
