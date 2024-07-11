namespace BookstoreAPI.DapperRequests;

public class ReviewRequest
{
    public static string GetBookReviews(int id) => $@"SELECT reviews.*, Users.Email AS userName 
            FROM book_schema.Reviews 
            LEFT JOIN book_schema.Users ON Reviews.userId = Users.id
            WHERE bookId = {id}";
    
    public static readonly string GetUserReview = "SELECT * FROM book_schema.Reviews WHERE bookId = @BookId AND userId = @UserId";

    public static readonly string CreateReview = "INSERT INTO book_schema.Reviews (bookId, userId, description, mark) VALUES (@BookId, @UserId, @Description, @Mark)";

    public static readonly string UpdateReview = @"UPDATE book_schema.Reviews SET description = @Description, mark = @Mark WHERE id = @Id";

    public static string DeleteReview(int id)
    {
        return $@"DELETE FROM book_schema.Reviews WHERE id={id}";
    }
}