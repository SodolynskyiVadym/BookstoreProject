namespace BookstoreAPI.DapperRequests;

public class BookRequest
{
    public static string GetInfoBook(int id) => $@"SELECT
            BookGenerallyInfo.*,
            BookDetailInfo.booklanguage,
            BookDetailInfo.numberpages,
            BookDetailInfo.publisherid,
            BookDetailInfo.yearpublication,
            BookDetailInfo.description,
            Authors.Name AS authorName,
            Publishers.Name AS PublisherName,
            array_agg(g.Genre) as Genres
        FROM book_schema.BookGenerallyInfo
                 JOIN book_schema.genres g ON book_schema.BookGenerallyInfo.id = g.bookid
                 INNER JOIN book_schema.BookDetailInfo ON book_schema.BookGenerallyInfo.id = book_schema.BookDetailInfo.BookId
                 INNER JOIN book_schema.Authors ON book_schema.BookGenerallyInfo.authorId = book_schema.Authors.Id
                 INNER JOIN book_schema.Publishers ON BookDetailInfo.publisherId = book_schema.Publishers.Id
        WHERE book_schema.BookGenerallyInfo.id =1
        GROUP BY book_schema.BookGenerallyInfo.id, BookDetailInfo.booklanguage, BookDetailInfo.numberpages, BookDetailInfo.publisherid, 
                 BookDetailInfo.yearpublication, BookDetailInfo.description, Authors.Name, Publishers.Name;
";

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
    
    
    public static readonly string GetBookByGenre = @"SELECT b.*, book_schema.Authors.Name AS authorName
            FROM book_schema.bookgenerallyinfo b
                LEFT JOIN book_schema.Authors ON Authors.id = b.authorId
            WHERE b.id IN (
                SELECT g.bookid
                FROM book_schema.genres g
                WHERE g.genre = ANY(@Genres)
                GROUP BY g.bookid
            );";
    

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