namespace BookstoreAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordForgotHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public User()
        {
        }
    }
}
