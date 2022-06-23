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
        public async Task<int> AddUserAsync(UserCred model)
        {
            try
            {
                return await _repo.WithConnection(async cmd =>
                {
                    var p = new DynamicParameters();

                    p.Add("AppID", model.AppID, DbType.Int64);
                    p.Add("AppRoleID", model.AppRoleID, DbType.Int64);
                    p.Add("UserEmail", model.UserEmail, DbType.String);
                    p.Add("FirstName", model.FirstName, DbType.String);
                    p.Add("MiddleName", model.MiddleName, DbType.String);
                    p.Add("LastName", model.LastName, DbType.String);
                    p.Add("PasswordHash", model.PasswordHash, DbType.String);
                    p.Add("PasswordSalt", model.PasswordSalt, DbType.String);
                    p.Add("CreateDate", model.CreateDate, DbType.DateTime);
                    p.Add("PhoneNumber", model.PhoneNumber, DbType.String);
                    p.Add("ExpirePassword", model.ExpirePassword, DbType.DateTime);
                    p.Add("IsTemp", model.IsTemp, DbType.Boolean);
                    p.Add("IsDeleted", model.IsDeleted, DbType.Boolean);
                    p.Add("IsAdmin", model.IsAdmin, DbType.Boolean);
                    p.Add("IsCustomer", model.IsCustomer, DbType.Boolean);
                    p.Add("EmailConfirmed", model.EmailConfirmed, DbType.Boolean);
                    p.Add("LockoutEnabled", model.LockoutEnabled, DbType.Boolean);
                    p.Add("AccessFailedCount", model.AccessFailedCount, DbType.Int64);
                    p.Add("IsActive", model.IsActive, DbType.Boolean);

                    var result = await cmd.ExecuteScalarAsync<int>("usp_Users_AddData", p, commandType: CommandType.StoredProcedure);
                    return result;
                });

            }
            catch (Exception ex)
            {
                _logIt.ExceptionLogFunc(ex);
                return Task.FromException<int>(ex).Result;
            }
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

        public async Task<UserCred> GetUserByIDAsync(int userId)
        {
            try
            {
                return await _repo.WithConnection(async cmd =>
                {
                    var p = new DynamicParameters();
                    p.Add("UserID", userId, DbType.Int64);


                    var result = await cmd.QueryMultipleAsync("usp_Users_GetDataBy_UserID", param: p, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdUserAsync(UserCred model)
        {
            try
            {
                return await _repo.WithConnection(async cmd =>
                {
                    var p = new DynamicParameters();

                    p.Add("UserID", model.UserID, DbType.Int64);
                    p.Add("AppID", model.AppID, DbType.Int64);
                    p.Add("AppRoleID", model.AppRoleID, DbType.Int64);
                    p.Add("UserEmail", model.UserEmail, DbType.String);
                    p.Add("FirstName", model.FirstName, DbType.String);
                    p.Add("MiddleName", model.MiddleName, DbType.String);
                    p.Add("LastName", model.LastName, DbType.String);
                    p.Add("PasswordHash", model.PasswordHash, DbType.String);
                    p.Add("PasswordSalt", model.PasswordSalt, DbType.String);
                    p.Add("PhoneNumber", model.PhoneNumber, DbType.String);
                    p.Add("ExpirePassword", model.ExpirePassword, DbType.DateTime);
                    p.Add("IsTemp", model.IsTemp, DbType.Boolean);
                    p.Add("IsDeleted", model.IsDeleted, DbType.Boolean);
                    p.Add("IsAdmin", model.IsAdmin, DbType.Boolean);
                    p.Add("IsCustomer", model.IsCustomer, DbType.Boolean);
                    p.Add("EmailConfirmed", model.EmailConfirmed, DbType.Boolean);
                    p.Add("LockoutEnabled", model.LockoutEnabled, DbType.Boolean);
                    p.Add("AccessFailedCount", model.AccessFailedCount, DbType.Int64);
                    p.Add("IsActive", model.IsActive, DbType.Boolean);
                    p.Add("PreviousPassword", model.PreviousPassword, DbType.String);
                    p.Add("IsForgetPassword", model.IsForgetPassword, DbType.Boolean);
                    //in usp_Users_UpdDataBy_Email UserEmail and Userid both are requied 
                    //all other parameters are optional and the column value will not be changed if the parameter value is null
                    var result = await cmd.ExecuteScalarAsync<int>("usp_Users_UpdDataBy_Email", p, commandType: CommandType.StoredProcedure);
                    return result;
                });

            }
            catch (Exception ex)
            {
                _logIt.ExceptionLogFunc(ex);
                return Task.FromException<int>(ex).Result;
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



                    p.Add("browser", model.DeviceInfo.browser, DbType.String);
                    p.Add("os", model.DeviceInfo.os, DbType.String);
                    p.Add("device", model.DeviceInfo.device, DbType.String);
                    p.Add("userAgent", model.DeviceInfo.userAgent, DbType.String);
                    p.Add("os_version", model.DeviceInfo.os_version, DbType.String);
                    p.Add("isMobile", model.DeviceInfo.isMobile, DbType.Boolean);
                    p.Add("isTablet", model.DeviceInfo.isTablet, DbType.Boolean);
                    p.Add("isDesktop", model.DeviceInfo.isDesktop, DbType.Boolean);
                    p.Add("RememberUser", model.RememberUser, DbType.Boolean);
                    p.Add("SessionToken", model.SessionToken, DbType.String);
                    p.Add("TokenExpirationDate", model.TokenExpirationDate, DbType.DateTimeOffset);
                    //used as session start date
                    p.Add("SessionDate", model.SessionDate, DbType.DateTimeOffset);


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

        public async Task<UserSessionLog> GetUserLogBySessionTokenAsync(UserCred userParam)
        {
            try
            {
                return await _repo.WithConnection(async cmd =>
                {
                    var p = new DynamicParameters();
                    p.Add("UserID", userParam.UserID, DbType.Int64);
                    p.Add("SessionToken", userParam.SessionToken, DbType.String);

                    var result = await cmd.QueryMultipleAsync("usp_UserLog_GetData_BySessionToken", param: p, commandType: CommandType.StoredProcedure);
                    UserSessionLog user = result.ReadAsync<UserSessionLog>().Result.FirstOrDefault();
                    return user;
                });
            }
            catch (Exception ex)
            {
                _logIt.ExceptionLogFunc(ex);
                return Task.FromException<UserSessionLog>(ex).Result;
            }
        }

        public async Task<int> LogOutUserAsync(UserCred model)
        {
            try
            {
                return await _repo.WithConnection(async cmd =>
                {
                    var p = new DynamicParameters();

                    p.Add("UserID", model.UserID, DbType.Int64);
                    p.Add("UserEmail", model.UserEmail, DbType.String);
                    p.Add("SessionToken", model.SessionToken, DbType.String);
                    p.Add("SessionEndDateTime", model.SessionDate, DbType.DateTimeOffset);
                    var result = await cmd.ExecuteScalarAsync<int>("usp_Users_LogOutUser", p, commandType: CommandType.StoredProcedure);
                    return result;
                });

            }
            catch (Exception ex)
            {
                _logIt.ExceptionLogFunc(ex);
                return Task.FromException<int>(ex).Result;
            }
        }


    }
}
