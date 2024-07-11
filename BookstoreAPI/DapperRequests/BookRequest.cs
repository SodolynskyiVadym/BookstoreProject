namespace BookstoreAPI.DapperRequests;

public class BookRequest
{ 
    public static readonly string GetBooksByNameAndAuthor = "SELECT id FROM book_schema.bookgenerallyinfo where authorId=@AuthorId and name=@Name";
    
    public static string GetInfoBook(int id) => $@"SELECT
                BookGenerallyInfo.*,
                BookDetailInfo.booklanguage,
                BookDetailInfo.numberpages,
                BookDetailInfo.publisherid,
                BookDetailInfo.yearpublication,
                BookDetailInfo.description,
                Authors.Name AS authorName,
                Publishers.Name AS PublisherName
            FROM book_schema.BookGenerallyInfo
            INNER JOIN book_schema.BookDetailInfo ON book_schema.BookGenerallyInfo.id = book_schema.BookDetailInfo.BookId
            INNER JOIN book_schema.Authors ON book_schema.BookGenerallyInfo.authorId = book_schema.Authors.Id
            INNER JOIN book_schema.Publishers ON BookDetailInfo.publisherId = book_schema.Publishers.Id
            WHERE book_schema.BookGenerallyInfo.id ={id};";
    
    public static string GetBookGenres(int id) => $@"SELECT Genre from book_schema.Genres WHERE book_schema.Genres.bookId = {id};";

    public static readonly string GetSomeBooks = "SELECT * FROM book_schema.bookgenerallyInfo WHERE BookGenerallyInfo.id = ANY (@BooksId)";
    
    
    public static readonly string GetOrderedBooks = @"SELECT BookGenerallyInfo.*, Authors.Name AS authorName
            FROM book_schema.BookGenerallyInfo 
            LEFT JOIN book_schema.Authors ON Authors.id = BookGenerallyInfo.authorId
            WHERE BookGenerallyInfo.id = ANY (@BooksId)";
    
    
    public static string GetBooksByPublisherId(int id) => $@"SELECT
                BookGenerallyInfo.*
            FROM book_schema.BookGenerallyInfo
            JOIN book_schema.BookDetailInfo ON BookDetailInfo.bookId = BookGenerallyInfo.Id
            WHERE book_schema.BookDetailInfo.publisherId = {id};"; 
    

    public static readonly string GetAllBooks = @"SELECT BookGenerallyInfo.*, Authors.Name AS authorName 
            FROM book_schema.BookGenerallyInfo 
            LEFT JOIN book_schema.Authors ON Authors.id = BookGenerallyInfo.authorId;";
    
    
    public static readonly string CreateBook = @"CALL book_schema.spBook_Upsert(
            @NumberPages::INTEGER,                        
            @Language::VARCHAR,    
            @YearPublication::DATE,
            @Description::TEXT,    
            @PublisherId::INTEGER,            
            @Name::VARCHAR,       
            @ImageUrl::VARCHAR,     
            @AuthorId::INTEGER,               
            @AvailableQuantity::INTEGER,  
            @Price::INTEGER,                  
            @Discount::INTEGER,
            @Genres::VARCHAR[] 
            );";
    
    
    public static readonly string UpdateBook = @"CALL book_schema.spBook_upsert(
            @NumberPages::INTEGER,                        
            @Language::VARCHAR,    
            @YearPublication::DATE,
            @Description::TEXT,    
            @PublisherId::INTEGER,            
            @Name::VARCHAR,    
            @ImageUrl::VARCHAR,    
            @AuthorId::INTEGER,               
            @AvailableQuantity::INTEGER,      
            @Price::INTEGER,                  
            @Discount::INTEGER,
            @InputGenres::VARCHAR[],               
            @Id::INTEGER
            );";


    public static string DeleteBook(int id)
    {
            return $@"DELETE FROM book_schema.BookDetailInfo WHERE bookId={id};
            DELETE FROM book_schema.BookGenerallyInfo WHERE id={id};";
    }
}