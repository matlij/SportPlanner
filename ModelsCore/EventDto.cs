using ModelsCore.Enums;
using System;
using System.Collections.Generic;

namespace ModelsCore
{
    public class EventUserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAttending { get; set; }
    }

    public class EventDto
    {
        public string Identifier { get; set; }
        public DateTime Date { get; set; }
        public EventType EventType { get; set; }
        public string Address { get; set; }
        public ICollection<EventUserDto> Users { get; set; }
    }
}
