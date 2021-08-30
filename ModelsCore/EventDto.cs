using ModelsCore.Enums;
using System;
using System.Collections.Generic;

namespace ModelsCore
{
    public class EventDto
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public EventType EventType { get; set; }
        public string Address { get; set; }
        public IEnumerable<EventUserDto> Users { get; set; }
    }
}
