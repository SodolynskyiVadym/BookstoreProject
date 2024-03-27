namespace BookstoreAPI.DTO
{
    public class UserUpdateDTO
    {
        public string? Name { get; set; }
        public string? Password { get; set; }

        public UserUpdateDTO()
        {
        }

        public override string? ToString()
        {
            return $"Password - {this.Password}, Name - {this.Name}";
        }
    }
}
