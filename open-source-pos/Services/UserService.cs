using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;

using Microsoft.AspNetCore.Http;


using Models;
using Repositories;

namespace Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly AppSettings _appSettings;
        public UserService(IUserRepository repo, IOptions<AppSettings> appSettings)
        {
            _repo = repo;
            _appSettings = appSettings.Value;
        }

        public async Task<UserCred> Authenticate(string userEmail, string password, UserCred userParam)
        {

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(password))
                return null;


            //Get the user here
            var user = await _repo.GetUserByEmailAsync(userEmail);

            // check if username exists
            if (user == null)
                return null;
            // check if password is correct
            var isPasswordCorrect = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
            user.IsPasswordCorrect = isPasswordCorrect;
            //here start date is used as a request start date
            if (!user.StartDate.HasValue)
            {
                user.StartDate = DateTime.Now;
            }
            user.UsersGeoLocation = userParam.UsersGeoLocation;
            user.StartDate = userParam.StartDate;
            var authenticationresult = await _repo.UserAuthenticationAndUpdatesAfterLoginAsync(user);
            user.authenticationResult = authenticationresult;
            if (!isPasswordCorrect)
            {
                // remove password before returning
                user.PasswordHash = null;
                user.PasswordSalt = null;
                user.UserPassword = null;
                return user;
            }



            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_appSettings.TokenExpiresInHours)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            //If the user's password is expired The user will be asked to change it
            user.IsPasswordExpired = (user.ExpirePassword <= DateTime.Now);


            // remove password before returning
            user.PasswordHash = null;
            user.PasswordSalt = null;
            user.UserPassword = null;

            return user;
        }
        private static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            byte[] storedHashba = Convert.FromBase64String(storedHash);
            byte[] storedSaltba = Convert.FromBase64String(storedSalt);
            if (storedHashba.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSaltba.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSaltba))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHashba[i]) return false;
                }
            }

            return true;
        }

    }
}
