using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BookstoreAPI.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize]
public class BookController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DataContextDapper _dapper;


    public BookController(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _dapper = new DataContextDapper(_config);
    }



    [AllowAnonymous]
    [HttpGet("getBook/{id}")]
    public BookDTO? GetBookInfo(int id)
    {
        string sqlGetInfoBook = $@"SELECT
                BookGenerallyInfo.*,
                BookDetailInfo.booklanguage,
                BookDetailInfo.numberpages,
                BookDetailInfo.publisherid,
                BookDetailInfo.yearpublication,
                BookDetailInfo.description,
                Authors.Name AS authorName 
            FROM book_schema.BookGenerallyInfo
            INNER JOIN book_schema.BookDetailInfo ON book_schema.BookGenerallyInfo.id = book_schema.BookDetailInfo.BookId
            INNER JOIN book_schema.Authors ON book_schema.BookGenerallyInfo.authorId = book_schema.Authors.Id
            WHERE book_schema.BookGenerallyInfo.id =@Id;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Id", id, System.Data.DbType.Int64);

        BookDTO? book = _dapper.LoadDataSingleWithParameters<BookDTO>(sqlGetInfoBook, parameters);
        return book;
    }



    [AllowAnonymous]
    [HttpPost("getSomeBooks")]
    public IEnumerable<BookOrderDTO> getOrderedBooks([FromBody] Dictionary<int, int> idAndQuantity)
    {
        string sqlGetOrderedBooks = @"SELECT BookGenerallyInfo.*, Authors.Name AS authorName
            FROM book_schema.BookGenerallyInfo 
            LEFT JOIN book_schema.Authors ON Authors.id = BookGenerallyInfo.authorId
            WHERE BookGenerallyInfo.id = ANY (@BooksId)";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@BooksId", idAndQuantity.Keys.ToArray(), System.Data.DbType.Object);

        return _dapper.LoadDataWithParameters<BookOrderDTO>(sqlGetOrderedBooks, parameters);


    }




    [AllowAnonymous]
    [HttpGet("getAllBooks")]
    public IEnumerable<BookDTO> GetAllBooks()
    {
        return _dapper.LoadData<BookDTO>(@"SELECT BookGenerallyInfo.*, Authors.Name AS authorName 
            FROM book_schema.BookGenerallyInfo 
            LEFT JOIN book_schema.Authors ON Authors.id = BookGenerallyInfo.authorId;");
    }



    [HttpPost("createBook")]
    public IActionResult CreateBook([FromBody] BookDTO book)
    {
        string sqlCreateBook = @"CALL book_schema.spBook_Upsert(
            @NumberPages::INTEGER,                        
            @Language::VARCHAR,    
            @YearPublication::DATE,
            @Description::TEXT,    
            @PublisherId::INTEGER,            
            @Name::VARCHAR,        
            @AuthorId::INTEGER,               
            @AvailableQuantity::INTEGER,      
            @Price::INTEGER,                  
            @Discount::INTEGER                                        
            );";


        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@NumberPages", book.NumberPages, System.Data.DbType.Int64);
        parameters.Add("@Language", book.BookLanguage, System.Data.DbType.String);
        parameters.Add("@YearPublication", book.YearPublication, System.Data.DbType.Date);
        parameters.Add("@Description", book.Description, System.Data.DbType.String);
        parameters.Add("@PublisherId", book.PublisherId, System.Data.DbType.Int64);
        parameters.Add("@Name", book.Name, System.Data.DbType.String);
        parameters.Add("@AuthorId", book.AuthorId, System.Data.DbType.Int64);
        parameters.Add("@AvailableQuantity", book.AvailableQuantity, System.Data.DbType.Int64);
        parameters.Add("@Price", book.Price, System.Data.DbType.Int64);
        parameters.Add("@Discount", book.Discount, System.Data.DbType.Int64);

        _dapper.ExecuteSqlWithParameters(sqlCreateBook, parameters);
        return Ok();
    }


    [HttpPatch("updateBook")]
    public IActionResult UpdateBook([FromBody] BookDTO book)
    {
        string sqlUpdateBook = @"CALL book_schema.spBook_upsert(
            @NumberPages::INTEGER,                        
            @Language::VARCHAR,    
            @YearPublication::DATE,
            @Description::TEXT,    
            @PublisherId::INTEGER,            
            @Name::VARCHAR,        
            @AuthorId::INTEGER,               
            @AvailableQuantity::INTEGER,      
            @Price::INTEGER,                  
            @Discount::INTEGER,               
            @Id::INTEGER
            );";

        BookDTO bookDTO = book;

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@NumberPages", book.NumberPages, System.Data.DbType.Int64);
        parameters.Add("@Language", book.BookLanguage, System.Data.DbType.String);
        parameters.Add("@YearPublication", book.YearPublication, System.Data.DbType.Date);
        parameters.Add("@Description", book.Description, System.Data.DbType.String);
        parameters.Add("@PublisherId", book.PublisherId, System.Data.DbType.Int64);
        parameters.Add("@Name", book.Name, System.Data.DbType.String);
        parameters.Add("@AuthorId", book.AuthorId, System.Data.DbType.Int64);
        parameters.Add("@AvailableQuantity", book.AvailableQuantity, System.Data.DbType.Int64);
        parameters.Add("@Price", book.Price, System.Data.DbType.Int64);
        parameters.Add("@Discount", book.Discount, System.Data.DbType.Int64);
        parameters.Add("@Id", book.Id, System.Data.DbType.Int64);

        _dapper.ExecuteSqlWithParameters(sqlUpdateBook, parameters);
        return Ok();
    }


    [HttpDelete("deleteBook/{id}")]
    public IActionResult DeleteBook(int id)
    {
        string sqlDeleteBook = $@"DELETE FROM book_schema.BookDetailInfo WHERE bookId={id};
            DELETE FROM book_schema.BookGenerallyInfo WHERE id={id};";
        if (_dapper.ExecuteSql(sqlDeleteBook)) return Ok();
        else return StatusCode(400, "Book not fount for deleting");
    }



    [AllowAnonymous]
    [HttpGet("getAllAuthors")]
    public IEnumerable<Author> getAllAuthors()
    {
        return _dapper.LoadData<Author>("SELECT * FROM book_schema.Authors");
    }



    [AllowAnonymous]
    [HttpGet("getAuthor/{id}")]
    public Author? GetAuthor(int id)
    {
        return _dapper.LoadDataSingle<Author>($"SELECT * FROM book_schema.Authors WHERE Authors.Id={id}");
    }



    [HttpPost("createAuthor")]
    public IActionResult CreateAuthor([FromBody] Author author)
    {
        string sqlCreateAuthor = @"
            INSERT INTO book_schema.Authors(Name, Biography, BirthYear, DeathYear) 
            VALUES (@Name, @Biography, @BirthYear, @DeathYear)";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Name", author.Name, System.Data.DbType.String);
        parameters.Add("@Biography", author.Biography, System.Data.DbType.String);
        parameters.Add("@BirthYear", author.BirthYear, System.Data.DbType.Date);
        parameters.Add("@DeathYear", author.DeathYear, System.Data.DbType.Date);

   
        if (_dapper.ExecuteSqlWithParameters(sqlCreateAuthor, parameters)) return Ok();
        else throw new Exception("Failed to insert author into DB");
    }


    [HttpPatch("updateAuthor")]
    public IActionResult UpdateAuthor([FromBody] Author author)
    {
        string sqlCreateAuthor = @"UPDATE book_schema.Authors SET Name=@Name, Biography=@Biography, BirthYear=@BirthYear, 
                DeathYear=@DeathYear WHERE Id=@Id";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Name", author.Name, System.Data.DbType.String);
        parameters.Add("@Biography", author.Biography, System.Data.DbType.String);
        parameters.Add("@BirthYear", author.BirthYear, System.Data.DbType.Date);
        parameters.Add("@DeathYear", author.DeathYear, System.Data.DbType.Date);
        parameters.Add("@Id", author.Id, System.Data.DbType.Int64);


        if (_dapper.ExecuteSqlWithParameters(sqlCreateAuthor, parameters)) return Ok();
        else throw new Exception("Failed to insert author into DB");
    }


    [HttpDelete("deleteAuthor/{id}")]
    public IActionResult DeleteAuthor(int id)
    {
        string sqlDeleteAuthor = $@"DELETE FROM book_schema.Authors WHERE id={id}";
        if (_dapper.ExecuteSql(sqlDeleteAuthor)) return Ok();
        else return StatusCode(400, "Author not fount for deleting");
    }



    [AllowAnonymous]
    [HttpGet("getAllPublishers")]
    public IEnumerable<Publisher> GetAllPublishers()
    {
        return _dapper.LoadData<Publisher>("SELECT * FROM book_schema.Publishers");
    }




    [AllowAnonymous]
    [HttpGet("getPublisher/{id}")]
    public Publisher? GetPublisher(int id)
    {
        return _dapper.LoadDataSingle<Publisher>($@"SELECT * FROM book_schema.Publishers WHERE Publishers.Id={id}");
    }


    [HttpPost("createPublisher")]
    public IActionResult CreatePublisher([FromBody] Publisher publisher) 
    {
        string sqlCreatePublisher = @"INSERT INTO book_schema.Publishers(Name) VALUES (@Name)";

        DynamicParameters dynamic = new DynamicParameters();
        dynamic.Add("@Name", publisher.Name, System.Data.DbType.String);

       if (_dapper.ExecuteSqlWithParameters(sqlCreatePublisher, dynamic)) return Ok();
       else throw new Exception("Failed to insert publisher into DB");
    }


    [HttpPatch("updatePublisher")]
    public IActionResult UpdatePublisher([FromBody] Publisher publisher)
    {
        string sqlUpdatePublisher = $@"UPDATE book_schema.Publishers SET Name=@Name WHERE Publishers.Id=@Id";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Name", publisher.Name, System.Data.DbType.String);
        parameters.Add("@Id", publisher.Id, System.Data.DbType.Int64);

        if (_dapper.ExecuteSqlWithParameters(sqlUpdatePublisher, parameters)) return Ok();
        else throw new Exception("Failed to update publisher");
    }


    [HttpDelete("deletePublisher/{id}")]
    public IActionResult DeletePublisher(int id)
    {
        string sqlDeletePublisher = $@"DELETE FROM book_schema.Publishers WHERE id={id}";
        if (_dapper.ExecuteSql(sqlDeletePublisher)) return Ok();
        else return StatusCode(400, "Publisher not fount for deleting");
    }
}