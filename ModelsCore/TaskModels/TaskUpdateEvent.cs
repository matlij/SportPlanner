using ModelsCore.Enums;
using System;
using System.Collections.Generic;

namespace ModelsCore.TaskModels
{
    public class TaskUpdateEvent
    {
        public string Identifier { get; set; }
        public DateTime Date { get; set; }
        public EventType EventType { get; set; }
        public string Address { get; set; }
        public ICollection<EventUserDto> Users { get; set; }
    }
}
