using System;
using System.Collections.Generic;

namespace SportPlannerIngestion.DataLayer.Models
{
    public class Event : BaseEntity
    {
        public DateTime Date { get; set; }
        public int EventType { get; set; }
        public IEnumerable<EventUser> Users { get; set; }

        //[ForeignKey("Addresses_Id")]
        public Address Address { get; set; }

        public Guid? AddressId { get; set; }
    }
}
