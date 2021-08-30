using System;
using System.Collections.Generic;

namespace SportPlannerIngestion.DataLayer.Models
{
    public class Event
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int EventType { get; set; }
        public string Address { get; set; }
        public IEnumerable<EventUser> Users { get; set; }
    }
}
