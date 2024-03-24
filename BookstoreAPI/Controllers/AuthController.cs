using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using BookstoreAPI.Settings;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace BookstoreAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        private readonly MailHelper _mailHelper;


        public AuthController(IConfiguration config, IOptions<MailSettings> mailSettings)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config, _dapper);
            _mailHelper = new Helpers.MailHelper(mailSettings);
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


        [Authorize]
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

            if (isUserNameExist.IsNullOrEmpty())
            {
                _authHelper.RegisterUser(userRegister, "USER");
                return Ok();
            }
            else return StatusCode(400, "User already exist or incorrect password");            
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPost("registerWorker")]
        public IActionResult RegisterWorker([FromBody] UserRegisterByEmailRoleDTO userEmailRole)
        {
            string? isUserNameExist = _dapper.LoadDataSingle<string>($"SELECT name FROM book_schema.Users WHERE email='{userEmailRole.Email}'");
            if (isUserNameExist.IsNullOrEmpty())
            {
                string password = _authHelper.GenerateRandomPassword();

                UserRegisterDTO userRegister = new UserRegisterDTO(userEmailRole.Role, userEmailRole.Email, password);
                _authHelper.RegisterUser(userRegister, userEmailRole.Role);
                _mailHelper.SendPassword(userEmailRole.Email, password, userEmailRole.Role);

                return Ok();
            }
            else return StatusCode(400, "User already exist");
            
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


        [Authorize]
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

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("deleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            _dapper.ExecuteSql(@$"DELETE FROM book_schema.users WHERE id={id}");
            return Ok();
        }
    }
}
