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
    }
}
