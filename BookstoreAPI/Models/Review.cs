namespace BookstoreAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }
        public int Mark { get; set; }

        public Review(int id, int userId, string? description, int mark)
        {
            Id = id;
            UserId = userId;
            Description = description;
            Mark = mark;
        }

        public Review()
        {
        }
    }
}
