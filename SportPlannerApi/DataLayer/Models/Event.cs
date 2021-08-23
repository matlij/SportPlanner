﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportPlannerIngestion.DataLayer.Models
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
}
