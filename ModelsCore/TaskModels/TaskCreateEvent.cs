using ModelsCore.Enums;
using System;
using System.Collections.Generic;

namespace ModelsCore.TaskModels
{
    public class TaskCreateEvent
    {
        public DateTime Date { get; set; }
        public EventType EventType { get; set; }
        public string Address { get; set; }
        public IEnumerable<EventUserDto> Users { get; set; }
    }
}
