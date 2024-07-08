namespace BookstoreAPI.DapperRequests;

public class UserRequest
{
    public static readonly string GetAllUsers = "SELECT * FROM book_schema.Users";

    public static string GetUserById(int id) => $"SELECT * FROM book_schema.Users WHERE id={id}";

    public static string GetUserByEmail(string email) => $"SELECT name FROM book_schema.Users WHERE email='{email}'";

    public static readonly string CreateUser = @"
        INSERT INTO book_schema.Users(Name, Email, PasswordHash, PasswordSalt, Role)
        VALUES (@Name, @Email, @PasswordHash, @PasswordSalt, @Role)";

    public static readonly string UpdateUser = @"UPDATE book_schema.Users SET Name=@Name, Email=@Email, PasswordHash=@PasswordHash, PasswordSalt=@PasswordSalt, Role=@Role WHERE Id=@Id";

    public static string DeleteUser(int id)
    {
        return $@"DELETE FROM book_schema.Users WHERE id={id}";
    }
}