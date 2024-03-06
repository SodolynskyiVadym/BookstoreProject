namespace BookstoreAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhotoUrl { get; set; }

        public User(int id, string name, string role, string email, string password, string? photoUrl)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Role = role ?? throw new ArgumentNullException(nameof(role));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            PhotoUrl = photoUrl;
        }

        public User()
        {
        }
    }
}
