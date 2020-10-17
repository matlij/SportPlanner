using SportPlanner.Models.Models;
using System;
using System.Collections.Generic;

namespace SportPlanner.Models.TaskModels
{
    public class TaskCreateEvent
    {
        public DateTime Date { get; set; }
        public EventType EventType { get; set; }
        public IEnumerable<string> UsersInvited { get; set; }
    }
}
