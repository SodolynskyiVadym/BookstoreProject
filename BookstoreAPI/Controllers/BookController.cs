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
    [HttpPost("createImageBook")]
    public async Task<IActionResult> UploadImageBook()
    {
        string? authorId = Request.Form["authorId"];
        string? name = Request.Form["name"];


        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@AuthorId", int.Parse(authorId), System.Data.DbType.Int64);
        parameters.Add("@Name", name, System.Data.DbType.String);

        int? id = _dapper.LoadDataSingleWithParameters<int>($"SELECT id FROM book_schema.bookgenerallyinfo where authorId=@AuthorId and name=@Name", parameters);
        if (id == null) return StatusCode(400, "Book doesn't exist");

        var file = Request.Form.Files[0];
        var filePath = Path.Combine(@"../book_store_front/src/assets/bookPhoto/", $"{name.ToLower().Replace(" ", "")}{id}.jpg");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return Ok();
    }



    [Authorize(Roles = "EDITOR, ADMIN")]
    [AllowAnonymous]
    [HttpPatch("updateBook")]
    public IActionResult UpdateBook([FromBody] BookCreateUpdateDTO book)
    {
        string sqlGetOldName = $@"SELECT name FROM book_schema.bookGenerallyInfo WHERE id={book.Id}";
        string? oldName = _dapper.LoadDataSingle<string>(sqlGetOldName);

        Console.WriteLine(book.Description);


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


        _fileHelper.RenamePhoto(oldName ?? "", book.Name, book.Id, "bookPhoto");

        return Ok();
    }


    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpDelete("deleteBook/{id}")]
    public IActionResult DeleteBook(int id)
    {
        string? name = _dapper.LoadDataSingle<string>($"SELECT name FROM book_schema.bookgenerallyinfo WHERE id={id}");
        string sqlDeleteBook = $@"DELETE FROM book_schema.BookDetailInfo WHERE bookId={id};
            DELETE FROM book_schema.BookGenerallyInfo WHERE id={id};";
        if (_dapper.ExecuteSql(sqlDeleteBook))
        {
            _fileHelper.DeletePhoto(name, id, "bookPhoto");
            return Ok();
        }
        else return StatusCode(400, "Book not fount for deleting");
    }
}