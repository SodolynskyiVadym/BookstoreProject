using BookstoreAPI.DapperRequests;
using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
    public BookDto? GetBookInfo(int id)
    {
        string sqlGetInfoBook = BookRequest.GetInfoBook(id);

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Id", id, System.Data.DbType.Int64);

        BookDto? book = _dapper.LoadDataSingle<BookDto>(sqlGetInfoBook);
        return book;
    }

    
    [AllowAnonymous]
    [HttpPost("getBooksByGenre")]
    public IEnumerable<BookMainInfoDto> GetBookByGenre(IEnumerable<string> genres)
    {
        genres = genres.Select(g => g.ToUpper());
        
        string sqlGetBookByGenre = BookRequest.GetBookByGenre;
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Genres", genres.ToArray(), System.Data.DbType.Object);
        return _dapper.LoadDataWithParameters<BookMainInfoDto>(sqlGetBookByGenre, parameters);
    }

    
    [AllowAnonymous]
    [HttpGet("getSomeBooks")]
    public IEnumerable<BookMainInfoDto> GetOrderedBooks()
    {
        string sqlGetOrderedBooks = BookRequest.GetOrderedBooks;

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
        return _dapper.LoadDataWithParameters<BookMainInfoDto>(sqlGetOrderedBooks, parameters);
    }



    [AllowAnonymous]
    [HttpGet("getAllBooks")]
    public IEnumerable<BookMainInfoDto> GetAllBooks()
    {
        return _dapper.LoadData<BookMainInfoDto>(BookRequest.GetAllBooks);
    }


    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("createBook")]
    public IActionResult CreateBook([FromBody] BookCreateUpdateDto book)
    {
        string sqlCreateBook = BookRequest.CreateBook;

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@NumberPages", book.NumberPages, System.Data.DbType.Int64);
        parameters.Add("@Language", book.BookLanguage, System.Data.DbType.String);
        parameters.Add("@YearPublication", book.YearPublication, System.Data.DbType.Date);
        parameters.Add("@Description", book.Description, System.Data.DbType.String);
        parameters.Add("@PublisherId", book.PublisherId, System.Data.DbType.Int64);
        parameters.Add("@Name", book.Name, System.Data.DbType.String);
        parameters.Add("@AuthorId", book.AuthorId, System.Data.DbType.Int64);
        parameters.Add("@AvailableQuantity", book.AvailableQuantity, System.Data.DbType.Int64);
        parameters.Add("@ImageUrl", book.ImageUrl, System.Data.DbType.String);
        parameters.Add("@Price", book.Price, System.Data.DbType.Int64);
        parameters.Add("@Discount", book.Discount, System.Data.DbType.Int64);
        parameters.Add("@Genres", book.Genres, System.Data.DbType.Object);

        _dapper.ExecuteSqlWithParameters(sqlCreateBook, parameters);
        return Ok();
    }



    [Authorize(Roles = "EDITOR, ADMIN")]
    [AllowAnonymous]
    [HttpPatch("updateBook")]
    public IActionResult UpdateBook([FromBody] BookCreateUpdateDto book)
    {
        string sqlUpdateBook = BookRequest.UpdateBook;

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@NumberPages", book.NumberPages, System.Data.DbType.Int64);
        parameters.Add("@Language", book.BookLanguage, System.Data.DbType.String);
        parameters.Add("@YearPublication", book.YearPublication, System.Data.DbType.Date);
        parameters.Add("@Description", book.Description, System.Data.DbType.String);
        parameters.Add("@PublisherId", book.PublisherId, System.Data.DbType.Int64);
        parameters.Add("@Name", book.Name, System.Data.DbType.String);
        parameters.Add("@ImageUrl", book.ImageUrl, System.Data.DbType.String);
        parameters.Add("@AuthorId", book.AuthorId, System.Data.DbType.Int64);
        parameters.Add("@AvailableQuantity", book.AvailableQuantity, System.Data.DbType.Int64);
        parameters.Add("@Price", book.Price, System.Data.DbType.Int64);
        parameters.Add("@Discount", book.Discount, System.Data.DbType.Int64);
        parameters.Add("@InputGenres", book.Genres, System.Data.DbType.Object);
        parameters.Add("@Id", book.Id, System.Data.DbType.Int64);

        _dapper.ExecuteSqlWithParameters(sqlUpdateBook, parameters);
        return Ok();
    }


    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpDelete("deleteBook/{id}")]
    public IActionResult DeleteBook(int id)
    {
        string sqlDeleteBook = BookRequest.DeleteBook(id);
        if (_dapper.ExecuteSql(sqlDeleteBook)) return Ok();
        return StatusCode(400, "Book wasn't deleted");
    }
}