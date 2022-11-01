using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models;

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
        Task<ServiceResponse> IsUserLogedInAndRemembered(UserCred userParam, int userID);
        Task<ServiceResponse> LogOut(UserCred userParam, int userID);
        /// <summary>
        /// Get Users of a particular company for listing
        /// </summary>
        /// <param name="query"></param>
        /// <param name="companyId"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<ServiceResponse> GetUsersAsync(string query, int companyId, int limit, int offset);

    }
}
