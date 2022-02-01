using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models;
using Repositories.Log;
using Repositories.SqlServer;

using Dapper;
using System.Data;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepository _repo;
        private readonly ILogIt _logIt;

        public UserRepository(IRepository repo, ILogIt logIt)
        {
            _repo = repo;
            _logIt = logIt;
        }

        public async Task<UserCred> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _repo.WithConnection(async cmd =>
                {
                    var p = new DynamicParameters();
                    p.Add("UserEmail", email, DbType.String);


                    var result = await cmd.QueryMultipleAsync("usp_Users_GetDataBy_Email", param: p, commandType: CommandType.StoredProcedure);
                    UserCred user = result.ReadAsync<UserCred>().Result.FirstOrDefault();
                    return user;
                });
            }
            catch (Exception ex)
            {
                _logIt.ExceptionLogFunc(ex);
                return Task.FromException<UserCred>(ex).Result;
            }
        }

        /// <summary>
        /// This Function is used to update User log table with successful and unsuccessful attempts
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> UserAuthenticationAndUpdatesAfterLoginAsync(UserCred model)
        {
            try
            {
                return await _repo.WithConnection(async cmd =>
                {
                    var p = new DynamicParameters();

                    if (model.UsersGeoLocation == null)
                    {
                        model.UsersGeoLocation = new UserGeoLocation();
                    }

                    p.Add("UserEmail", model.UserEmail, DbType.String);
                    p.Add("IsPasswordValid", model.IsPasswordCorrect, DbType.Boolean);
                    p.Add("UserLocalDate", model.StartDate, DbType.DateTime);


                    p.Add("ip", model.UsersGeoLocation.ip, DbType.String);
                    p.Add("city", model.UsersGeoLocation.city, DbType.String);
                    p.Add("region", model.UsersGeoLocation.region, DbType.String);

                    p.Add("region_code", model.UsersGeoLocation.region_code, DbType.String);
                    p.Add("country", model.UsersGeoLocation.country, DbType.String);
                    p.Add("country_name", model.UsersGeoLocation.country_name, DbType.String);
                    p.Add("continent_code", model.UsersGeoLocation.continent_code, DbType.String);
                    p.Add("in_eu", model.UsersGeoLocation.in_eu, DbType.Boolean);
                    p.Add("postal", model.UsersGeoLocation.postal, DbType.String);
                    p.Add("latitude", model.UsersGeoLocation.latitude, DbType.Decimal);
                    p.Add("longitude", model.UsersGeoLocation.longitude, DbType.Decimal);
                    p.Add("timezone", model.UsersGeoLocation.timezone, DbType.String);
                    p.Add("utc_offset", model.UsersGeoLocation.utc_offset, DbType.String);
                    p.Add("country_calling_code", model.UsersGeoLocation.country_calling_code, DbType.String);
                    p.Add("currency", model.UsersGeoLocation.currency, DbType.String);
                    p.Add("languages", model.UsersGeoLocation.languages, DbType.String);
                    p.Add("asn", model.UsersGeoLocation.asn, DbType.String);
                    p.Add("org", model.UsersGeoLocation.org, DbType.String);
                    //p.Add("PreviousPassword", model.PreviousPassword, DbType.String);

                    var result = await cmd.QueryFirstAsync<AuthenticationResult>
                        ("usp_User_Authentication_Procedure", p, commandType: CommandType.StoredProcedure);

                    return result;
                });

            }
            catch (Exception ex)
            {
                _logIt.ExceptionLogFunc(ex);
                return Task.FromException<AuthenticationResult>(ex).Result;
            }
        }

    }
}
