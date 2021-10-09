using System.Collections.Generic;

namespace SportPlannerIngestion.DataLayer.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<EventUser> Events { get; set; }
    }
}
