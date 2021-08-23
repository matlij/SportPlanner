namespace SportPlanner.Models
{
    public class User
    {
        public User(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string Name { get; set; }
    }
}
