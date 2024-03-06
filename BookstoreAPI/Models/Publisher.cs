namespace BookstoreAPI.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? PhotoUrl { get; set; }

        public Publisher(int id, string name, string? photoUrl)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            PhotoUrl = photoUrl;
        }

        public Publisher()
        {
        }
    }
}
