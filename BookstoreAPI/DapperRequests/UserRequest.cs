namespace BookstoreAPI.DapperRequests;

public class UserRequest
{
    public static readonly string GetAllUsers = "SELECT * FROM book_schema.Users";

    public static string GetUserById(int id) => $"SELECT * FROM book_schema.Users WHERE id={id}";

    public static string GetUserByEmail(string email) => $"SELECT * FROM book_schema.Users WHERE email='{email}'";

    public static readonly string CreateUser = @"INSERT INTO book_schema.Users(Email, PasswordHash, PasswordSalt, Role)
        VALUES (@Email, @PasswordHash, @PasswordSalt, @Role)";

    public static string UpdateUser(int id) => $"UPDATE book_schema.Users SET PasswordHash=@PasswordHash, PasswordSalt=@PasswordSalt WHERE Id={id}";

    public static string DeleteUser(int id) => $"DELETE FROM book_schema.Users WHERE id={id}";
}