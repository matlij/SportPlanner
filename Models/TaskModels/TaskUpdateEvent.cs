using SportPlanner.Models.Models;
using System;
using System.Collections.Generic;

namespace SportPlanner.Models.TaskModels
{
    public class TaskUpdateEvent
    {
        public DateTime Date { get; set; }
        public EventType EventType { get; set; }
        public ICollection<string> UsersInvited { get; set; }
        public ICollection<string> UsersAttending { get; set; }
    }
}
