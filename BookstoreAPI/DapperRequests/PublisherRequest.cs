namespace BookstoreAPI.DapperRequests;

public class PublisherRequest
{
    public static readonly string GetAllPublishers = "SELECT * FROM book_schema.Publishers";

    public static string GetPublisherById(int id) => $"SELECT * FROM book_schema.Publishers WHERE id={id}";

    public static readonly string CreatePublisher = @"INSERT INTO book_schema.Publishers(Name, ImageUrl) VALUES (@Name, @ImageUrl)";

    public static readonly string UpdatePublisher = @"UPDATE book_schema.Publishers SET Name=@Name, ImageUrl=@ImageUrl WHERE Id=@Id";

    public static string DeletePublisher(int id)
    {
        return $@"DELETE FROM book_schema.Publishers WHERE id={id}";
    }
}