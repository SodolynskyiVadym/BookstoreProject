namespace BookstoreAPI.DapperRequests;

public class AuthorRequest
{
    public static readonly string GetAllAuthors = "SELECT * FROM book_schema.Authors";

    public static readonly string GetAuthorById = "SELECT * FROM book_schema.Authors WHERE id=@Id";

    public static readonly string GetAuthorBooks = @"SELECT BookGenerallyInfo.*, Authors.Name AS authorName
        FROM book_schema.BookGenerallyInfo 
        LEFT JOIN book_schema.Authors ON Authors.id = BookGenerallyInfo.authorId
        WHERE Authors.id = @Id";

    public static readonly string CreateAuthor = @"
        INSERT INTO book_schema.Authors(Name, Biography, BirthYear, DeathYear, ImageUrl) 
        VALUES (@Name, @Biography, @BirthYear, @DeathYear, @ImageUrl)";

    public static readonly string UpdateAuthor = "UPDATE book_schema.Authors SET name=@Name, biography=@Biography, birthYear=@BirthYear, deathYear=@DeathYear WHERE Id=@Id";

    public static string DeleteAuthor(int id)
    {
        return $@"DELETE FROM book_schema.Authors WHERE id={id}";
    }
}