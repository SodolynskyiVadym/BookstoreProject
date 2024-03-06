namespace BookstoreAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Biography { get; set; }
        public int? BirthYear { get; set; }
        public int? DeathYear { get; set;}
        public string? PhotoUrl { get; set; }

        public Author(int id, string firstName, string lastName, string? biography, int? birthYear, int? deathYear,
            string? photoUrl)
        {
            Id = id;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Biography = biography;
            BirthYear = birthYear;
            DeathYear = deathYear;
            PhotoUrl = photoUrl;
        }

        public Author()
        {
        }
    }
}
