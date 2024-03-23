namespace BookstoreAPI.DTO
{
    public class UserRegisterDTO
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public UserRegisterDTO()
        {
        }
    }
}
