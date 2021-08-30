using System.ComponentModel.DataAnnotations;

namespace SportPlannerIngestion.DataLayer.Models
{
    public class EventUser
    {
        public string EventId { get; set; }
        public Event Event { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public bool IsOwner { get; set; }

        public int UserReply { get; set; }
    }
}
