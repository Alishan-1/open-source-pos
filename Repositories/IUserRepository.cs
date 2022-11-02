using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models;
namespace Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user if email not already exists. Also creates a new company id if not supplied
        /// </summary>
        /// <param name="model"></param>
        /// <returns>-1 if user email already exists else new user id</returns>
        Task<int> AddUserAsync(UserCred model);
        Task<UserCred> GetUserByEmailAsync(string email);
        Task<UserCred> GetUserByIDAsync(int userId);
        Task<int> UpdUserAsync(UserCred model);
        /// <summary>
        /// This Function is used to update User log table with successful and unsuccessful attempts
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AuthenticationResult> UserAuthenticationAndUpdatesAfterLoginAsync(UserCred model);

        Task<int> LogOutUserAsync(UserCred userParam);
        Task<UserSessionLog> GetUserLogBySessionTokenAsync(UserCred userParam);
        /// <summary>
        /// Get fiscal year of company based on current db server date
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        Task<FiscalYearST> GetCurrentFiscalYear(int CompanyID);
        Task<int> CreateCurrentFiscalYear(int companyID);
        /// <summary>
        /// Get Users of a particular company for listing
        /// </summary>
        /// <param name="query"></param>
        /// <param name="companyId"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<List<UserCred>> GetUsersAsync(string query, int companyId, int limit, int offset);
    }
}
