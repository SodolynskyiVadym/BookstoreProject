using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
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
        return _dapper.LoadDataSingle<BookGenerallyInfo>($"SELECT * FROM book_schema.GenerallyBookInfo WHERE GenerallyBookInfo.Id={id}");
    }

    [HttpPost("createBook")]
    public IActionResult CreateBook([FromBody] BookDTO book)
    {
        return Ok();
    }

    [HttpGet("getAuthor/{id}")]
    public Author GetAuthor(int id)
    {
        return _dapper.LoadDataSingle<Author>($"SELECT * FROM book_schema.Authors WHERE Authors.Id={id}");
    }
}