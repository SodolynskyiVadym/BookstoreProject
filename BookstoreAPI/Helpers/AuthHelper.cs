using BookstoreAPI.DTO;
using BookstoreAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookstoreAPI.DapperRequests;

namespace BookstoreAPI.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        private readonly string possibleValuePassword = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public AuthHelper(IConfiguration config, DataContextDapper dapper)
        {
            _dapper = dapper;
            _config = config;
        }
        public byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                Convert.ToBase64String(passwordSalt);

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000000,
                numBytesRequested: 256 / 8
            );
        }


        public string CreateToken(int userId, string role)
        {
            Claim[] claims = new Claim[] {
                new Claim("userId", userId.ToString()),
                new Claim("role", role)
            };

            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString != null ? tokenKeyString : ""));

            SigningCredentials credentials = new SigningCredentials(
                    tokenKey,
                    SecurityAlgorithms.HmacSha512Signature
                );

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);

        }



        public bool RegisterUser(UserRegisterDto userRegistration)
        {
            if(userRegistration.Password == null) return false;
            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) rng.GetNonZeroBytes(passwordSalt);

            byte[] passwordHash = GetPasswordHash(userRegistration.Password, passwordSalt);

            string sqlAddAuth = UserRequest.CreateUser;

            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("@Role", userRegistration.Role, DbType.String);
            parameters.Add("@Email", userRegistration.Email, DbType.String);
            parameters.Add("@PasswordHash", passwordHash, DbType.Binary);
            parameters.Add("@PasswordSalt", passwordSalt, DbType.Binary);

            return _dapper.ExecuteSqlWithParameters(sqlAddAuth, parameters);
        }


        public bool UpdatePassword(int id, string password)
        {
            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte[] passwordHash = GetPasswordHash(password, passwordSalt);

            string sqlAddAuth = UserRequest.UpdateUser(id);

            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("@PasswordHash", passwordHash, DbType.Binary);
            parameters.Add("@PasswordSalt", passwordSalt, DbType.Binary);

            return _dapper.ExecuteSqlWithParameters(sqlAddAuth, parameters);
        }



        public string GenerateRandomPassword()
        {
            int length = 12;
            var randomBytes = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            var result = new StringBuilder(length);

            foreach (var b in randomBytes)
            {
                result.Append(possibleValuePassword[b % (possibleValuePassword.Length)]);
            }

            return result.ToString();
        }
    }
}
