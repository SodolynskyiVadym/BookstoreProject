using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text;
using System.IO;
using Microsoft.IdentityModel.Tokens;

namespace BookstoreAPI.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize]
public class BookController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DataContextDapper _dapper;
    private readonly FileHelper _fileHelper;


    public BookController(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _dapper = new DataContextDapper(_config);
        _fileHelper = new FileHelper();
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
                Authors.Name AS authorName,
                Publishers.Name AS PublisherName
            FROM book_schema.BookGenerallyInfo
            INNER JOIN book_schema.BookDetailInfo ON book_schema.BookGenerallyInfo.id = book_schema.BookDetailInfo.BookId
            INNER JOIN book_schema.Authors ON book_schema.BookGenerallyInfo.authorId = book_schema.Authors.Id
            INNER JOIN book_schema.Publishers ON BookDetailInfo.publisherId = book_schema.Publishers.Id
            WHERE book_schema.BookGenerallyInfo.id =@Id;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Id", id, System.Data.DbType.Int64);

        BookDTO? book = _dapper.LoadDataSingleWithParameters<BookDTO>(sqlGetInfoBook, parameters);
        return book;
    }




    [AllowAnonymous]
    [HttpGet("getSomeBooks")]
    public IEnumerable<BookOrderDTO> getOrderedBooks()
    {
        string sqlGetOrderedBooks = @"SELECT BookGenerallyInfo.*, Authors.Name AS authorName
            FROM book_schema.BookGenerallyInfo 
            LEFT JOIN book_schema.Authors ON Authors.id = BookGenerallyInfo.authorId
            WHERE BookGenerallyInfo.id = ANY (@BooksId)";

        string? strBooksId = Request.Query["ids"];
        int[] booksId = new int[] { 0 };

        if (!string.IsNullOrEmpty(strBooksId))
        {
            string[] idStrings = strBooksId.Split(",");
            booksId = new int[idStrings.Length];

            for (int i = 0; i < idStrings.Length; i++) if (int.TryParse(idStrings[i], out int id)) booksId[i] = id;
        }


        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@BooksId", booksId.ToArray(), System.Data.DbType.Object);

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


    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("createBook")]
    public IActionResult CreateBook([FromBody] BookCreateUpdateDTO book)
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



    [Authorize(Roles = "EDITOR, ADMIN")]
    [AllowAnonymous]
    [HttpPatch("updateBook")]
    public IActionResult UpdateBook([FromBody] BookCreateUpdateDTO book)
    {
        string sqlGetOldName = $@"SELECT name FROM book_schema.bookGenerallyInfo WHERE id={book.Id}";
        string? oldName = _dapper.LoadDataSingle<string>(sqlGetOldName);


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


        _fileHelper.RenameBookPhoto(oldName ?? "", book.Name, book.Id);

        return Ok();
    }


    [Authorize(Roles = "EDITOR, ADMIN")]
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
        string sqlGetAuthor = @"SELECT * FROM book_schema.Authors WHERE Authors.Id=@Id";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("id", id, System.Data.DbType.Int64);
        return _dapper.LoadDataSingleWithParameters<Author>(sqlGetAuthor, parameters);
    }




    [AllowAnonymous]
    [HttpGet("getAuthorBooks/{id}")]
    public IActionResult GetAuthorBooks(int id)
    {
        string sqlGetAuthor = @"SELECT * FROM book_schema.Authors WHERE id=@Id";
        string sqlGetBooks = @"SELECT
                BookGenerallyInfo.*
            FROM book_schema.BookGenerallyInfo
            WHERE book_schema.BookGenerallyInfo.authorId =@Id;";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Id", id, System.Data.DbType.Int64);

        IEnumerable<BookGenerallyInfo> books = _dapper.LoadDataWithParameters<BookGenerallyInfo>(sqlGetBooks, parameters);
        Author? author = _dapper.LoadDataSingleWithParameters<Author>(sqlGetAuthor, parameters);

        return Ok(new { Author = author, Books = books });
    }



    [Authorize(Roles = "EDITOR, ADMIN")]
    [AllowAnonymous]
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



    [Authorize(Roles = "EDITOR, ADMIN")]
    [AllowAnonymous]
    [HttpPatch("updateAuthor")]
    public IActionResult UpdateAuthor([FromBody] Author author)
    {
        string sqlGetOldNameAuthor = $@"SELECT name FROM book_schema.authors WHERE id={author.Id}";

        string? oldName = _dapper.LoadDataSingle<string>(sqlGetOldNameAuthor);

        string sqlCreateAuthor = @"UPDATE book_schema.Authors SET Name=@Name, Biography=@Biography, BirthYear=@BirthYear, 
                DeathYear=@DeathYear WHERE Id=@Id";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Name", author.Name, System.Data.DbType.String);
        parameters.Add("@Biography", author.Biography, System.Data.DbType.String);
        parameters.Add("@BirthYear", author.BirthYear, System.Data.DbType.Date);
        parameters.Add("@DeathYear", author.DeathYear, System.Data.DbType.Date);
        parameters.Add("@Id", author.Id, System.Data.DbType.Int64);


        _dapper.ExecuteSqlWithParameters(sqlCreateAuthor, parameters);

        _fileHelper.RenameAuthorPhoto(oldName, author.Name, author.Id);
        return Ok();
    }



    [Authorize(Roles = "EDITOR, ADMIN")]
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




    [AllowAnonymous]
    [HttpGet("getPublisherBooks/{id}")]
    public IActionResult GetPublisherBooks(int id)
    {

        string sqlGetAuthor = @"SELECT * FROM book_schema.Publishers WHERE Publishers.Id=@Id";

        string sqlGetBooks = @"SELECT
                BookGenerallyInfo.*
            FROM book_schema.BookGenerallyInfo
            JOIN book_schema.BookDetailInfo ON BookDetailInfo.bookId = BookGenerallyInfo.Id
            WHERE book_schema.BookDetailInfo.publisherId = @Id;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Id", id, System.Data.DbType.Int64);

        Publisher? publisher = _dapper.LoadDataSingleWithParameters<Publisher>(sqlGetAuthor, parameters);

        IEnumerable<BookGenerallyInfo> books = _dapper.LoadDataWithParameters<BookGenerallyInfo>(sqlGetBooks, parameters);

        return Ok(new { Publisher = publisher, Books = books });
    }



    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("createPublisher")]
    public IActionResult CreatePublisher([FromBody] Publisher publisher) 
    {
        string sqlCreatePublisher = @"INSERT INTO book_schema.Publishers(Name) VALUES (@Name)";

        DynamicParameters dynamic = new DynamicParameters();
        dynamic.Add("@Name", publisher.Name, System.Data.DbType.String);

       if (_dapper.ExecuteSqlWithParameters(sqlCreatePublisher, dynamic)) return Ok();
       else throw new Exception("Failed to insert publisher into DB");
    }



    [Authorize(Roles = "EDITOR, ADMIN")]
    [AllowAnonymous]
    [HttpPatch("updatePublisher")]
    public IActionResult UpdatePublisher([FromBody] Publisher publisher)
    {
        string sqlGetNamePublisher = @"SELECT name FROM book_schema.publishers WHERE publishers.Id = @Id";


        string sqlUpdatePublisher = $@"UPDATE book_schema.Publishers SET Name=@Name WHERE Publishers.Id=@Id";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Name", publisher.Name, System.Data.DbType.String);
        parameters.Add("@Id", publisher.Id, System.Data.DbType.Int64);


        string? oldName = _dapper.LoadDataSingleWithParameters<string>(sqlGetNamePublisher, parameters);
        _dapper.ExecuteSqlWithParameters(sqlUpdatePublisher, parameters);
  

        _fileHelper.RenamePublisherPhoto(oldName, publisher.Name, publisher.Id);

        return Ok();
    }


    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpDelete("deletePublisher/{id}")]
    public IActionResult DeletePublisher(int id)
    {
        string sqlDeletePublisher = $@"DELETE FROM book_schema.Publishers WHERE id={id}";
        if (_dapper.ExecuteSql(sqlDeletePublisher)) return Ok();
        else return StatusCode(400, "Publisher not fount for deleting");
    }
}