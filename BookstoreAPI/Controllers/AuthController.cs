using System.Data;
using BookstoreAPI.DapperRequests;
using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using BookstoreAPI.Settings;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
    public IActionResult RegisterUser([FromBody] UserRegisterDTO userRegister)
    {
        var isUserNameExist = _dapper.LoadDataSingle<string>(UserRequest.GetUserByEmail(userRegister.Email));

        if (isUserNameExist.IsNullOrEmpty())
        {
            _authHelper.RegisterUser(userRegister, "USER");
            return Ok();
        }

        return StatusCode(400, "User already exist or incorrect password");
    }


    [Authorize(Roles = "ADMIN")]
    [HttpPost("registerWorker")]
    public IActionResult RegisterWorker([FromBody] UserRegisterByEmailRoleDTO userEmailRole)
    {
        var isUserNameExist = _dapper.LoadDataSingle<string>(UserRequest.GetUserByEmail(userEmailRole.Email));
        if (isUserNameExist.IsNullOrEmpty())
        {
            var password = _authHelper.GenerateRandomPassword();

            var userRegister = new UserRegisterDTO(userEmailRole.Role, userEmailRole.Email, password);
            _authHelper.RegisterUser(userRegister, userEmailRole.Role);
            _mailHelper.SendPassword(userEmailRole.Email, password, userEmailRole.Role);

            return Ok();
        }

        return StatusCode(400, "User already exist");
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDTO userLogin)
    {
        var user = _dapper.LoadDataSingle<User>(UserRequest.GetUserByEmail(userLogin.Email));

        if (user != null)
        {
            var passwordHash = _authHelper.GetPasswordHash(userLogin.Password, user.PasswordSalt);

            for (var index = 0; index < passwordHash.Length; index++)
                if (passwordHash[index] != user.PasswordHash[index])
                    return StatusCode(401, "Incorrect password!");
            return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user.Id, user.Role) } });
        }

        return StatusCode(400, "User doesn't exist");
    }


    [Authorize]
    [HttpPatch("updateUser")]
    public IActionResult UpdateUser(UserUpdateDTO userUpdate)
    {
        if (!userUpdate.Password.IsNullOrEmpty() && userUpdate.Password.Length < 8)
            return StatusCode(400, "Password must be greater than 8 symbols");

        int userId;
        if (int.TryParse(User.FindFirst("userId")?.Value, out userId))
        {
            if (_authHelper.UpdateUser(userId, userUpdate)) return Ok();
            return StatusCode(500, "User was not updated");
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