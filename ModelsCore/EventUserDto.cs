using System;

namespace ModelsCore
{
    public class EventUserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int UserReply { get; set; }
        public bool IsOwner { get; set; }
    }
}
