using System;

namespace SportPlanner.Models
{
    public class EventUser : IEquatable<EventUser>
    {
        public EventUser(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
        public string UserName { get; set; }

        public bool IsAttending => UserReply == EventReply.Attending;

        public bool IsOwner { get; set; }
        public EventReply UserReply { get; set; }

        public bool Equals(EventUser other)
        {
            if (other == null)
                return false;

            return other.UserId == UserId;
        }
    }
}
