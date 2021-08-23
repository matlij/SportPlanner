namespace SportPlanner.Models
{
    public class TaskAddEventUser
    {
        public TaskAddEventUser(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string Name { get; set; }
        public bool Invited { get; set; }
    }
}
