using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models;
namespace Repositories
{
    public interface IUserRepository
    {
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
    }
}
