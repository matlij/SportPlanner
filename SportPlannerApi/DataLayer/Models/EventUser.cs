using System.ComponentModel.DataAnnotations;

namespace SportPlannerIngestion.DataLayer.Models
{
    public class EventUser
    {
        public int EventId { get; set; }
        public int UserId { get; set; }

        public Event Event { get; set; }
        public User User { get; set; }
        public int UserReply { get; set; }

        [Required]
        public bool IsOwner { get; set; }
    }
}
