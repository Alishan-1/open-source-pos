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
using Services.ViewModels;
using Utilities;

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

        public async Task<UserCred> Create(UserCred user)
        {
            // validation
            if (string.IsNullOrWhiteSpace(user.UserEmail))
                throw new Exception("Email is required");

            user.UserEmail = user.UserEmail.ToLower();

            if (await _repo.GetUserByEmailAsync(user.UserEmail) != null)
                throw new Exception("User With Email \"" + user.UserEmail + "\" already exists");

            string passwordHash, passwordSalt;
            user.UserPassword = Util.GenerateRandomPassword();
            CreatePasswordHash(user.UserPassword, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.CreateDate = DateTime.Now;
            user.ExpirePassword = DateTime.Now.AddHours(4);
            user.IsTemp = true;
            user.IsDeleted = false;
            //user.IsAdmin = true;
            user.IsCustomer = false;
            user.EmailConfirmed = false;
            user.LockoutEnabled = false;
            user.AccessFailedCount = 0;
            user.IsActive = true;


            var newUserID = await _repo.AddUserAsync(user);
            if (newUserID <= 0)
                throw new Exception("Cant Register user");
            //dont send password related info to frontend
            user.UserID = newUserID;
            user.PasswordHash = null;
            user.PasswordSalt = null;



            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            //client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential("onebitsoft31@gmail.com", "@neBit$oft31");
            //client.EnableSsl = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = new MailAddress("onebitsoft31@gmail.com");
            //mailMessage.To.Add(receiver);
            //mailMessage.Body = "body";
            //mailMessage.Subject = "subject";
            //client.Send(mailMessage);

            //return 200;

            return user;
        }
        /*
        public void Update(User userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (_context.Users.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }
        */
        public IEnumerable<UserCred> GetAll()
        {
            // return users without passwords
            var x = new List<UserCred>();
            x.Add(new UserCred { UserID = 1, FirstName = "Alishan" });
            return x;
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt, string oldPasswordSalt = null)
        {

            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            System.Security.Cryptography.HMACSHA512 hmac;
            if (oldPasswordSalt == null)
            {
                hmac = new System.Security.Cryptography.HMACSHA512();
            }
            else
            {
                byte[] storedSaltba = Convert.FromBase64String(oldPasswordSalt);
                hmac = new System.Security.Cryptography.HMACSHA512(storedSaltba);
            }
            using (hmac)
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
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

        public UserCred GetById(int userId)
        {
            var user = _repo.GetUserByIDAsync(userId).Result;
            return user;


        }



        public async Task<ServiceResponse> ChangePassword(UserCred userParam, int userId)
        {
            //try
            //{
            //    ServiceResponse vmServiceResponse = ServiceValidation.Validate(userParam, new ChangePasswordValidator(userId));

            //    int result = 0;
            //    if (vmServiceResponse.IsValid)
            //    {
            //        var user = await _repo.GetUserByEmailAsync(userParam.UserEmail);
            //        if (userParam.UserID != user.UserID)
            //        {
            //            vmServiceResponse.Title = ServiceErrorsMessages.Title;
            //            vmServiceResponse.Message = "Invalid Password Change Request. Login again and than try";
            //            vmServiceResponse.Flag = false;
            //            vmServiceResponse.IsValid = false;
            //            vmServiceResponse.StatusCode = StatusCodes.Status400BadRequest;
            //            return vmServiceResponse;
            //        }
            //        // check if the user has enterd his current password correctly
            //        if (!VerifyPasswordHash(userParam.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            //        {
            //            vmServiceResponse.Title = ServiceErrorsMessages.Title;
            //            vmServiceResponse.Message = "You entered an invalid Current password";
            //            vmServiceResponse.Flag = false;
            //            vmServiceResponse.IsValid = false;
            //            vmServiceResponse.StatusCode = StatusCodes.Status400BadRequest;
            //            return vmServiceResponse;
            //        }


            //        // check if the password is same as current password
            //        if (VerifyPasswordHash(userParam.UserPassword, user.PasswordHash, user.PasswordSalt))
            //        {
            //            vmServiceResponse.Title = ServiceErrorsMessages.Title;
            //            vmServiceResponse.Message = "You entered the password which is same as you current password, please try another password";
            //            vmServiceResponse.Flag = false;
            //            vmServiceResponse.IsValid = false;
            //            vmServiceResponse.StatusCode = StatusCodes.Status400BadRequest;
            //            return vmServiceResponse;
            //        }
            //        // check if the password is same as Previous password
            //        if (!string.IsNullOrWhiteSpace(user.PreviousPassword))
            //        {
            //            if (VerifyPasswordHash(userParam.UserPassword, user.PreviousPassword, user.PasswordSalt))
            //            {
            //                vmServiceResponse.Title = ServiceErrorsMessages.Title;
            //                vmServiceResponse.Message = "You entered the password which is same as one of your old passwords, please try another password";
            //                vmServiceResponse.Flag = false;
            //                vmServiceResponse.IsValid = false;
            //                vmServiceResponse.StatusCode = StatusCodes.Status400BadRequest;
            //                return vmServiceResponse;
            //            }
            //        }


            //        string newPasswordHash, passwordSalt;
            //        CreatePasswordHash(userParam.UserPassword, out newPasswordHash, out passwordSalt, user.PasswordSalt);
            //        userParam.PreviousPassword = user.PasswordHash;
            //        userParam.PasswordHash = newPasswordHash;
            //        userParam.ExpirePassword = DateTime.Now.AddDays(30);
            //        userParam.IsTemp = false;
            //        userParam.IsDeleted = null;
            //        userParam.IsActive = null;
            //        userParam.IsAdmin = null;
            //        userParam.IsCustomer = null;
            //        userParam.EmailConfirmed = true;
            //        userParam.LockoutEnabled = false;

            //        result = await _repo.UpdUserAsync(userParam);
            //        vmServiceResponse.Data = result;

            //        if (result <= 0)
            //        {
            //            vmServiceResponse.Title = ServiceErrorsMessages.Title;
            //            vmServiceResponse.Message = ServiceMessages.DataNotSaved;
            //            vmServiceResponse.Flag = false;
            //            vmServiceResponse.IsValid = false;
            //        }
            //        else if (result == 1)
            //        {
            //            vmServiceResponse.Title = ServiceMessages.Title;
            //            vmServiceResponse.Message = ServiceMessages.DataSaved;
            //            vmServiceResponse.Flag = true;
            //            vmServiceResponse.IsValid = true;
            //        }
            //        else
            //        {
            //            vmServiceResponse.Title = ServiceErrorsMessages.Title;
            //            vmServiceResponse.Message = "One row was expected to be updated but More rows "
            //                + "has been updated against userid " + userParam.UserID + " and user Email " + userParam.UserEmail + " ";
            //            vmServiceResponse.Flag = false;
            //            vmServiceResponse.IsValid = false;

            //        }
            //    }

            //    else
            //    {
            //        vmServiceResponse.Title = ServiceErrorsMessages.Title;
            //        vmServiceResponse.Message = ServiceErrorsMessages.DataInvalid;
            //        vmServiceResponse.Flag = false;
            //    }

            //    return vmServiceResponse;

            //}
            //catch (Exception ex)
            //{
            //    _log.ExceptionLogFunc(ex);
            //    return Task.FromException<ServiceResponse>(ex).Result;
            //}

            return new ServiceResponse();
        }


        /// <summary>
        /// This function will be used to process forget password request
        /// </summary>
        /// <param name="userParam">Parameter contaning user email to change password</param>
        /// <returns></returns>
        public async Task<ServiceResponse> ForgerPassword(UserCred userParam)
        {
            //try
            //{
            //    ServiceResponse vmServiceResponse = ServiceValidation.Validate(userParam, new ForgetPasswordValidator());
            //    vmServiceResponse.Data = null;
            //    int result = 0;
            //    if (vmServiceResponse.IsValid)
            //    {
            //        var user = await _repo.GetUserByEmailAsync(userParam.UserEmail);
            //        //check if the user exists against provided email
            //        if (user == null || user.UserID <= 0)
            //        {
            //            //if the user does not exists against the provided email. we will not tell this to user for security reasons.
            //            vmServiceResponse.Title = ServiceMessages.Title;
            //            vmServiceResponse.Message = "password reset link was sent to your email If you entered correct email";
            //            vmServiceResponse.Flag = true;
            //            vmServiceResponse.IsValid = true;
            //            vmServiceResponse.StatusCode = StatusCodes.Status200OK;
            //            return vmServiceResponse;
            //        }


            //        string newPasswordHash, passwordSalt;
            //        userParam.UserPassword = Util.GenerateRandomPassword();
            //        CreatePasswordHash(userParam.UserPassword, out newPasswordHash, out passwordSalt, user.PasswordSalt);
            //        userParam.PreviousPassword = user.PasswordHash;
            //        userParam.PasswordHash = newPasswordHash;
            //        userParam.ExpirePassword = DateTime.Now.AddDays(30);
            //        userParam.IsTemp = false;
            //        userParam.IsDeleted = null;
            //        userParam.IsActive = null;
            //        userParam.IsAdmin = null;
            //        userParam.IsCustomer = null;
            //        userParam.EmailConfirmed = true;
            //        userParam.LockoutEnabled = false;
            //        userParam.UserID = user.UserID;

            //        //set the IsForgetPassword to true
            //        userParam.IsForgetPassword = true;

            //        result = await _repo.UpdUserAsync(userParam);
            //        vmServiceResponse.Data = result;

            //        if (result <= 0)
            //        {
            //            vmServiceResponse.Title = ServiceErrorsMessages.Title;
            //            vmServiceResponse.Message = ServiceMessages.DataNotSaved;
            //            vmServiceResponse.Flag = false;
            //            vmServiceResponse.IsValid = false;
            //        }
            //        else if (result == 1)
            //        {
            //            vmServiceResponse.Title = ServiceMessages.Title;
            //            vmServiceResponse.Message = ServiceMessages.DataSaved;
            //            vmServiceResponse.Flag = true;
            //            vmServiceResponse.IsValid = true;
            //            // send this password in email from controller
            //            vmServiceResponse.Data = userParam.UserPassword;
            //        }
            //        else
            //        {
            //            vmServiceResponse.Title = ServiceErrorsMessages.Title;
            //            vmServiceResponse.Message = "One row was expected to be updated but More rows "
            //                + "has been updated against userid " + userParam.UserID + " and user Email " + userParam.UserEmail + " ";
            //            vmServiceResponse.Flag = false;
            //            vmServiceResponse.IsValid = false;

            //        }
            //    }

            //    else
            //    {
            //        vmServiceResponse.Title = ServiceErrorsMessages.Title;
            //        vmServiceResponse.Message = ServiceErrorsMessages.DataInvalid;
            //        vmServiceResponse.Flag = false;
            //    }

            //    return vmServiceResponse;

            //}
            //catch (Exception ex)
            //{
            //    _log.ExceptionLogFunc(ex);
            //    return Task.FromException<ServiceResponse>(ex).Result;
            //}

            return new ServiceResponse();
        }

        public async Task<UserCred> GetByEmailAsync(string userEmail)
        {
            return await _repo.GetUserByEmailAsync(userEmail);

        }


    }
}
