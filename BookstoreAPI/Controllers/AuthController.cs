using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("registerUser")]
        public IActionResult RegisterUser([FromBody]UserRegisterDTO userRegister)
        {
            User? userExist = _dapper.LoadDataSingle<User>($"SELECT * FROM book_schema.Users WHERE email='{userRegister.Email}'");

            if (userRegister.Password.Equals(userRegister.ConfirmPassword) || userExist == null)
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
