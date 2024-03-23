using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace BookstoreAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;


        public AuthController(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _dapper = new DataContextDapper(_config);
            _authHelper = new AuthHelper(_config, _dapper);
        }



        [AllowAnonymous]
        [HttpGet("getAllUsers")]
        public IEnumerable<User> GetAllUsers()
        {
            return _dapper.LoadData<User>("SELECT * FROM book_schema.Users");
        }


        [AllowAnonymous]
        [HttpGet("getUser")]
        public User? GetUser(int id)
        {
            return _dapper.LoadDataSingle<User>($"SELECT * FROM book_schema.Users WHERE id={id}");
        }


       
        [HttpGet("getUserByToken")]
        public IActionResult getUserByToken()
        {
            int userId;
            if (Int32.TryParse(this.User.FindFirst("userId")?.Value, out userId))
            {
                string sqlGetUser = @"SELECT * FROM book_schema.users where users.id = @Id";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", userId, DbType.Int32);

                return Ok(_dapper.LoadDataSingleWithParameters<User>(sqlGetUser, parameters));
            }
            else return StatusCode(401, "Incorrect token");
        }


        [AllowAnonymous]
        [HttpPost("registerUser")]
        public IActionResult RegisterUser([FromBody]UserRegisterDTO userRegister)
        {
            string? isUserNameExist = _dapper.LoadDataSingle<string>($"SELECT name FROM book_schema.Users WHERE email='{userRegister.Email}'");

            if (userRegister.Password.Equals(userRegister.ConfirmPassword) && isUserNameExist.IsNullOrEmpty())
            {
                _authHelper.RegisterUser(userRegister, "USER");
                return Ok();
            }
            else return StatusCode(400, "User already exist or incorrect password");            
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO userLogin)
        {
            User? user = _dapper.LoadDataSingle<User>(@$"SELECT * FROM book_schema.Users WHERE email='{userLogin.Email}'");

            if (user != null)
            {
                byte[] passwordHash = _authHelper.GetPasswordHash(userLogin.Password, user.PasswordSalt);

                for (int index = 0; index < passwordHash.Length; index++)
                {
                    if (passwordHash[index] != user.PasswordHash[index])
                    {
                        return StatusCode(401, "Incorrect password!");
                    }
                }
                return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user.Id, user.Role) } });
            }
            else return StatusCode(400, "User donesn't exist");
        }



        [HttpPatch("updateUser")]
        public IActionResult UpdateUser(UserUpdateDTO userUpdate)
        {
            int userId;
            if (Int32.TryParse(this.User.FindFirst("userId")?.Value, out userId))
            {
                if (_authHelper.UpdateUser(userId, userUpdate)) return Ok();
                else return StatusCode(500, "User was not updated");
            }
            else return StatusCode(400, "User was not updated");
        }
    }
}
