using BookstoreAPI.DapperRequests;
using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using BookstoreAPI.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookstoreAPI.Controllers;

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
        _mailHelper = new MailHelper(mailSettings);
    }

    [AllowAnonymous]
    [HttpGet("getAllUsers")]
    public IEnumerable<User> GeAllUsers()
    {
        return _dapper.LoadData<User>(UserRequest.GetAllUsers);
    }


    [AllowAnonymous]
    [HttpGet("getUser/{id}")]
    public User? GetUser(int id)
    {
        return _dapper.LoadDataSingle<User>(UserRequest.GetUserById(id));
    }


    [Authorize]
    [HttpGet("getUserByToken")]
    public IActionResult GetUserByToken()
    {
        if (int.TryParse(User.FindFirst("userId")?.Value, out var userId)) return Ok(_dapper.LoadDataSingle<User>(UserRequest.GetUserById(userId)));
        return StatusCode(401, "Incorrect token");
    }


    [AllowAnonymous]
    [HttpPost("registerUser")]
    public IActionResult RegisterUser([FromBody] UserRegisterDto userRegister)
    {
        var isUserNameExist = _dapper.LoadDataSingle<User>(UserRequest.GetUserByEmail(userRegister.Email));

        if (isUserNameExist == null)
        {
            userRegister.Role = "USER";
            _authHelper.RegisterUser(userRegister);
            return Ok();
        }
        return StatusCode(400, "User already exist or incorrect password");
    }


    [Authorize(Roles = "ADMIN")]
    [HttpPost("registerEditorAdmin")]
    public IActionResult RegisterWorker([FromBody] UserRegisterDto userRegister)
    {
        if (userRegister.Role != "ADMIN" && userRegister.Role != "EDITOR") return StatusCode(400, "Incorrect role");
        var userEmail = _dapper.LoadDataSingle<User>(UserRequest.GetUserByEmail(userRegister.Email));
        if (userEmail == null)
        {
            userRegister.Password = _authHelper.GenerateRandomPassword();
            _authHelper.RegisterUser(userRegister);
            _mailHelper.SendPassword(userRegister.Email, userRegister.Password, userRegister.Role);
            return Ok();
        }
        return StatusCode(400, "User already exist");
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDto userLogin)
    {
        var user = _dapper.LoadDataSingle<User>(UserRequest.GetUserByEmail(userLogin.Email));

        if (user != null)
        {
            var passwordHash = _authHelper.GetPasswordHash(userLogin.Password, user.PasswordSalt);

            for (var index = 0; index < passwordHash.Length; index++) 
                if (passwordHash[index] != user.PasswordHash[index]) return StatusCode(400, "Incorrect password!");
            return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user.Id, user.Role) } });
        }

        return StatusCode(400, "User doesn't exist");
    }


    [Authorize]
    [HttpPatch("updatePassword")]
    public IActionResult UpdateUser(UserUpdateDto userUpdate)
    {
        if (userUpdate.Password.Length < 8) return StatusCode(400, "Password must be greater than 8 symbols");
        if (int.TryParse(User.FindFirst("userId")?.Value, out var userId))
        {
            if (_authHelper.UpdatePassword(userId, userUpdate.Password)) return Ok();
        }
        return StatusCode(400, "User was not found");
    }


    [Authorize(Roles = "ADMIN")]
    [HttpDelete("deleteUser/{id}")]
    public IActionResult DeleteUser(int id)
    {
        _dapper.ExecuteSql(UserRequest.DeleteUser(id));
        return Ok();
    }
}