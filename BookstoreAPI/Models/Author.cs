namespace BookstoreAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Biography { get; set; }
        public DateTime? BirthYear { get; set; }
        public DateTime? DeathYear { get; set;}

        public Author()
        {
        }
    }
}
