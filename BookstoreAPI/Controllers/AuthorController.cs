﻿using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        private readonly FileHelper _fileHelper;
        private readonly DataHelper _dataHelper;


        public AuthorController(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _dapper = new DataContextDapper(_config);
            _fileHelper = new FileHelper();
            _dataHelper = new DataHelper(_dapper);
        }

        [AllowAnonymous]
        [HttpGet("getAllAuthors")]
        public IEnumerable<Author> GetAllAuthors()
        {
            return _dapper.LoadData<Author>("SELECT * FROM book_schema.Authors");
        }




        [AllowAnonymous]
        [HttpGet("getAuthor/{id}")]
        public Author? GetAuthor(int id)
        {
            return _dataHelper.GetById<Author>("authors", id);
        }
        
        

        [AllowAnonymous]
        [HttpGet("getAuthorBooks/{id}")]
        public IActionResult GetAuthorBooks(int id)
        {
            Author? author = _dataHelper.GetById<Author>("authors", id);
            IEnumerable<BookGenerallyInfo> books = _dataHelper.GetByParameter<BookGenerallyInfo>("bookGenerallyInfo", "authorId", id);
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
            return StatusCode(400, "Failed to insert author into DB");
        }


        [Authorize(Roles = "EDITOR, ADMIN")]
        [HttpPost("createImageAuthor")]
        public async Task<IActionResult> UploadImageAuthor()
        {
            string? biography = Request.Form["biography"];
            string? name = Request.Form["name"];


            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Biography", biography, System.Data.DbType.String);
            parameters.Add("@Name", name, System.Data.DbType.String);

            int? id = _dapper.LoadDataSingleWithParameters<int>($"SELECT id FROM book_schema.authors where biography=@Biography and name=@Name", parameters);
            if (id == null) return StatusCode(400, "Author doesn't exist");

            var file = Request.Form.Files[0];
            var filePath = Path.Combine(@"../book_store_front/src/assets/authorPhoto/", $"{name.ToLower().Replace(" ", "")}{id}.jpg");

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok();
        }



        [Authorize(Roles = "EDITOR, ADMIN")]
        [AllowAnonymous]
        [HttpPatch("updateAuthor")]
        public IActionResult UpdateAuthor([FromBody] Author author)
        {
            string sqlGetOldNameAuthor = $@"SELECT name FROM book_schema.authors WHERE id={author.Id}";

            string? oldName = _dapper.LoadDataSingle<string>(sqlGetOldNameAuthor);

            string sqlCreateAuthor = @"UPDATE book_schema.Authors SET name=@Name, biography=@Biography, birthYear=@BirthYear, 
                deathYear=@DeathYear WHERE Id=@Id";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Name", author.Name, System.Data.DbType.String);
            parameters.Add("@Biography", author.Biography, System.Data.DbType.String);
            parameters.Add("@BirthYear", author.BirthYear, System.Data.DbType.Date);
            parameters.Add("@DeathYear", author.DeathYear, System.Data.DbType.Date);
            parameters.Add("@Id", author.Id, System.Data.DbType.Int64);
            
            _dapper.ExecuteSqlWithParameters(sqlCreateAuthor, parameters);

            _fileHelper.RenamePhoto(oldName, author.Name, author.Id, "authorPhoto");
            return Ok();
        }



        [Authorize(Roles = "EDITOR, ADMIN")]
        [HttpDelete("deleteAuthor/{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            string? name = _dapper.LoadDataSingle<string>($"SELECT name FROM book_schema.authors WHERE id={id}");
            string sqlDeleteAuthor = $@"DELETE FROM book_schema.Authors WHERE id={id}";
            if (_dapper.ExecuteSql(sqlDeleteAuthor))
            {
                _fileHelper.DeletePhoto(name, id, "authorPhoto");
                return Ok();
            }
            return StatusCode(400, "Author not fount for deleting");
        }

    }
}
