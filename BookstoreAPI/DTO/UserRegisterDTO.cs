namespace BookstoreAPI.DTO
{
    public class UserRegisterDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserRegisterDTO()
        {
        }

        public UserRegisterDTO(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
