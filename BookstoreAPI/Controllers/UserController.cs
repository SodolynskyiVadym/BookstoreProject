using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;

        public UserController(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _dapper = new DataContextDapper(_config);
        }



        [HttpGet("getAllUsers")]
        public IEnumerable<User> GetAllUsers()
        {
            return _dapper.LoadData<User>("SELECT * FROM book_schema.Users");
        }


        [HttpGet("getUser")]
        public User? GetUser(int id)
        {
            return _dapper.LoadDataSingle<User>($"SELECT * FROM book_schema.Users WHERE id={id}");
        }
    }
}
