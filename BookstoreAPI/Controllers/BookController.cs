using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DataContextDapper _dapper;


    public BookController(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _dapper = new DataContextDapper(_config);
    }



    [HttpGet("getBook/{id}")]
    public BookGenerallyInfo? GetBook(int id)
    {
        return _dapper.LoadDataSingle<BookGenerallyInfo>($"SELECT * FROM book_schema.BookGenerallyInfo WHERE BookGenerallyInfo.Id={id}");
    }

    [HttpGet("getInfoBook/{id}")]
    public BookDTO? GetBookInfo(int id)
    {
        string sqlGetInfoBook = $@"SELECT
                BookGenerallyInfo.*,
                BookDetailInfo.booklanguage,
                BookDetailInfo.numberpages,
                BookDetailInfo.publisherid,
                BookDetailInfo.yearpublication,
                BookDetailInfo.description
            FROM book_schema.BookGenerallyInfo
            INNER JOIN book_schema.BookDetailInfo ON book_schema.BookGenerallyInfo.id = book_schema.BookDetailInfo.BookId
            WHERE book_schema.BookGenerallyInfo.id = {id.ToString() ?? "NULL"};";

        BookDTO? book = _dapper.LoadDataSingle<BookDTO>(sqlGetInfoBook);
        return book;
    }



    [HttpGet("getAllBooks")]
    public IEnumerable<BookGenerallyInfo> GetAllBooks()
    {
        return _dapper.LoadData<BookGenerallyInfo>("SELECT * FROM book_schema.BookGenerallyInfo");
    }



    [HttpPost("createBook")]
    public IActionResult CreateBook([FromBody] BookDTO book)
    {
        string sqlCreateBook = @"CALL book_schema.spBook_Upsert(
            @NumberPages,                        
            @Language::VARCHAR,    
            @YearPublication::DATE,
            @Description::TEXT,    
            @PublisherId,            
            @Name::VARCHAR,        
            @AuthorId,               
            @AvailableQuantity,      
            @Price,                  
            @Discount,               
            0,                           
            @PhotoUrl
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
        parameters.Add("@PhotoUrl", book.PhotoUrl, System.Data.DbType.String);

        if (_dapper.ExecuteSqlWithParameters(sqlCreateBook, parameters)) return Ok();
        else throw new Exception("Failed to insert book into DB");
    }



    [HttpGet("getAllAuthors")]
    public IEnumerable<Author> getAllAuthors()
    {
        return _dapper.LoadData<Author>("SELECT * FROM book_schema.Authors");
    }


    [HttpGet("getAuthor/{id}")]
    public Author? GetAuthor(int id)
    {
        return _dapper.LoadDataSingle<Author>($"SELECT * FROM book_schema.Authors WHERE Authors.Id={id}");
    }



    [HttpPost("createAuthor")]
    public IActionResult CreateAuthor([FromBody] Author author)
    {
        string sqlCreateAuthor = @"
            INSERT INTO book_schema.Authors(FirstName, LastName, Biography, BirthYear, DeathYear, PhotoUrl) 
            VALUES (@FirstName, @LastName, @Biography, @BirthYear, @DeathYear, @PhotoUrl)";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@FirstName", author.FirstName, System.Data.DbType.String);
        parameters.Add("@LastName", author.LastName, System.Data.DbType.String);
        parameters.Add("@Biography", author.Biography, System.Data.DbType.String);
        parameters.Add("@BirthYear", author.BirthYear, System.Data.DbType.Date);
        parameters.Add("@DeathYear", author.DeathYear, System.Data.DbType.Date);
        parameters.Add("@PhotoUrl", author.PhotoUrl, System.Data.DbType.String);

   
        if (_dapper.ExecuteSqlWithParameters(sqlCreateAuthor, parameters)) return Ok();
        else throw new Exception("Failed to insert author into DB");
    }



    [HttpGet("getAllPublishers")]
    public IEnumerable<Publisher> GetAllPublishers()
    {
        return _dapper.LoadData<Publisher>("SELECT * FROM book_schema.Publishers");
    }


    [HttpGet("getPublisher/{id}")]
    public Publisher? GetPublisher(int id)
    {
        return _dapper.LoadDataSingle<Publisher>($@"SELECT * FROM book_schema.Publishers WHERE Publishers.Id={id}");
    }


    [HttpPost("createPublisher")]
    public IActionResult CreatePublisher([FromBody] Publisher publisher) 
    {
        string sqlCreatePublisher = @"INSERT INTO book_schema.Publishers(Name, PhotoUrl) VALUES (@Name, @PhotoUrl)";

        DynamicParameters dynamic = new DynamicParameters();
        dynamic.Add("@Name", publisher.Name, System.Data.DbType.String);
        dynamic.Add("@PhotoUrl", publisher.PhotoUrl, System.Data.DbType.String);

       if (_dapper.ExecuteSqlWithParameters(sqlCreatePublisher, dynamic)) return Ok();
       else throw new Exception("Failed to insert publisher into DB");
    }
}