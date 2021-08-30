using System.Collections.Generic;

namespace SportPlannerIngestion.DataLayer.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<EventUser> Events { get; set; }
    }
}
