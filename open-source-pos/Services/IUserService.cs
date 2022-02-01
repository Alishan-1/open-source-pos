using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models;
using Services.ViewModels;

namespace Services
{
    public interface IUserService 
    {
        Task<UserCred> Authenticate(string username, string password, UserCred userParam);
        Task<UserCred> Create(UserCred userCred);
        IEnumerable<UserCred> GetAll();
        UserCred GetById(int userId);
        Task<UserCred> GetByEmailAsync(string userEmail);
        Task<ServiceResponse> ChangePassword(UserCred userParam, int userId);
        /// <summary>
        /// This function will be used to process forget password request
        /// </summary>
        /// <param name="userParam">Parameter contaning user email to change password</param>
        /// <returns></returns>
        Task<ServiceResponse> ForgerPassword(UserCred userParam);
        /// <summary>
        /// Returns validation messages for login screen
        /// </summary>
        /// <returns></returns>

    }
}
