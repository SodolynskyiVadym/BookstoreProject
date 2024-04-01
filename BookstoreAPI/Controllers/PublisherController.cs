using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PublisherController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        private readonly FileHelper _fileHelper;


        public PublisherController(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _dapper = new DataContextDapper(_config);
            _fileHelper = new FileHelper();
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


            _fileHelper.RenamePhoto(oldName, publisher.Name, publisher.Id, "publisherPhoto");

            return Ok();
        }

        [Authorize(Roles = "EDITOR, ADMIN")]
        [HttpPost("createImagePublisher")]
        public async Task<IActionResult> UploadImagePublisher()
        {
            string? name = Request.Form["name"];

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Name", name, System.Data.DbType.String);

            int? id = _dapper.LoadDataSingleWithParameters<int>($"SELECT id FROM book_schema.publishers where name=@Name", parameters);
            if (id == null) return StatusCode(400, "Publisher doesn't exist");

            var file = Request.Form.Files[0];
            var filePath = Path.Combine(@"../book_store_front/src/assets/publisherPhoto/", $"{name.ToLower().Replace(" ", "")}{id}.jpg");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok();
        }


        [Authorize(Roles = "EDITOR, ADMIN")]
        [HttpDelete("deletePublisher/{id}")]
        public IActionResult DeletePublisher(int id)
        {
            string? name = _dapper.LoadDataSingle<string>($"SELECT name FROM book_schema.publishers WHERE id={id}");
            string sqlDeletePublisher = $@"DELETE FROM book_schema.Publishers WHERE id={id}";

            if (_dapper.ExecuteSql(sqlDeletePublisher))
            {
                _fileHelper.DeletePhoto(name, id, "publisherPhoto");
                return Ok();
            }
            else return StatusCode(400, "Publisher not fount for deleting");
        }
    }
}
