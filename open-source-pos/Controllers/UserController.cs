using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using Models;
using Services;
using System.Net;
using Newtonsoft.Json.Linq;

namespace open_source_pos.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        public UserController(IUserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
        }
        /// <summary>
        /// used to login user
        /// </summary>
        /// <param name="userParam"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserCred userParam)
        {
            try
            {
                //System.Diagnostics.Debugger.Break();
                var user = await _userService.Authenticate(userParam.UserEmail, userParam.UserPassword, userParam);

                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                if (user.authenticationResult.IsLockoutEnabled.GetValueOrDefault(false))
                {
                    return BadRequest(new
                    {
                        message = "Your account is locked for some time because of too many unsuccessful attempts",
                        data = new { IsLockoutEnabled = user.LockoutEnabled, user.LockoutEnd }
                    });
                }

                if (!user.authenticationResult.IsAuthorisedCurrently.GetValueOrDefault(true))
                    return BadRequest(new { message = "Username or password is incorrect" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpPost("IsUserLogedInAndRemembered")]
        public async Task<IActionResult> IsUserLogedInAndRemembered([FromBody]UserCred userParam)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).First();
                var userID = int.Parse(userIdClaim.Value);
                var serviceResponse = await _userService.IsUserLogedInAndRemembered(userParam, userID);

                if (serviceResponse == null)
                    return BadRequest(new { message = "An error occoured!" });
                return StatusCode((int)(serviceResponse.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), serviceResponse);



            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }

        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromBody]UserCred userParam)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).First();
                var userID = int.Parse(userIdClaim.Value);
                var serviceResponse = await _userService.LogOut(userParam, userID);

                if (serviceResponse == null)
                    return BadRequest(new { message = "An error occoured!" });
                return StatusCode((int)(serviceResponse.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), serviceResponse);



            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }

        }

        /// <summary>
        /// Get user details from token.
        /// </summary>
        /// <param name="userParam"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetCurrentUser")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                string sUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var x = User.Identities.First().Name;
                int userId = int.Parse(x);
                var user = _userService.GetById(userId);
                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                user.PasswordHash = null;
                user.PasswordSalt = null;
                //var user = await _userService.Authenticate(userParam.UserEmail, userParam.UserPassword, userParam);

                return Ok(user);

                //if (user.authenticationResult.IsLockoutEnabled.GetValueOrDefault(false))
                //{
                //    return BadRequest(new { message = "Your account is locked for some time because of too many unsuccessful attempts" });
                //}

                //if (!user.authenticationResult.IsAuthorisedCurrently.GetValueOrDefault(true))
                //    return BadRequest(new { message = "Username or password is incorrect" });


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCred userCred)
        {
            // map dto to entity
            //var user = _mapper.Map<User>(userDto);
            //userCred.UserPassword = "2";
            try
            {
                var a = Request.Host;
                // The first user will be an Admin user with Users.IsAdmin flag set to true. Further users will not be Admin (Users.IsAdmin flag will be false)
                if (!userCred.IsAdmin.HasValue)
                {
                    userCred.IsAdmin = true;
                }
                // save 
                var x = await _userService.Create(userCred);
                if (x != null)
                {
                    string loginURL = GetLoginUrl(a);

                    string subject = "Email Conformation for open-source-pos";
                    userCred.LinkOrCode = @"<p> Use the following Password To Login to Your open-source-pos Account <br/>";
                    userCred.LinkOrCode += userCred.UserPassword + @"</p><p>login  <a href=""";
                    userCred.LinkOrCode += loginURL;
                    userCred.LinkOrCode += @"""> here </a></p> ";
                    userCred.LinkOrCode += "<p>Or paste the following link in your browser address bar </p> ";
                    userCred.LinkOrCode += "<p>" + loginURL + " </p> ";
                    await _emailSender.SendEmailAsync(userCred.UserEmail, subject, userCred.LinkOrCode);
                    userCred.UserPassword = null;
                }
                return Ok(x);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpGet("GetHostName")]
        public IActionResult GetHostName()
        {
            //to get host at server
            try
            {
                var a = Request.Host;
                return Ok(a.Host);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody]UserCred userParam)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).First();
                var userID = int.Parse(userIdClaim.Value);
                var serviceResponse = await _userService.ChangePassword(userParam, userID);

                if (serviceResponse == null)
                    return BadRequest(new { message = "An error occoured!" });
                return StatusCode((int)(serviceResponse.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), serviceResponse);



            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }

        }

        [AllowAnonymous]
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgerPassword([FromBody]UserCred userParam)
        {
            try
            {
                var serviceResponse = await _userService.ForgerPassword(userParam);

                if (serviceResponse == null)
                    return BadRequest(new { message = "An error occoured!" });

                if (serviceResponse.Data != null && serviceResponse.Data is string)
                {
                    var a = Request.Host;
                    string loginURL = GetLoginUrl(a);

                    string subject = "Forget Password Request for open-source-pos Account";
                    string LinkOrCode = @"<p> Use the following Password To Login to Your open-source-pos Account <br/>";
                    LinkOrCode += ((string)serviceResponse.Data) + @"</p><p>login  <a href=""";
                    LinkOrCode += loginURL;
                    LinkOrCode += @"""> here </a></p> ";
                    LinkOrCode += "<p>Or paste the following link in your browser address bar </p> ";
                    LinkOrCode += "<p>" + loginURL + " </p> ";
                    await _emailSender.SendEmailAsync(userParam.UserEmail, subject, LinkOrCode);
                    //this statement is very important it removes user password from response
                    serviceResponse.Data = null;
                }

                return StatusCode((int)(serviceResponse.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), serviceResponse);



            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }

        }

        //[Authorize]
        //[HttpPost("add/employee")]
        //public async Task<IActionResult> RegisterEmployeeUser([FromBody]NotificationModel model)
        //{

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // we do not have employee details at the time of sending invitation, so we get them here
        //            var sr = await _employeeService.GetDataByIdAsync(model.LocationId.Value, model.EmployeeId.Value);
        //            var employee = sr.Data as Employees;

        //            var userCred = TransaposeToUserCred(model, employee);

        //            var a = Request.Host;

        //            var userCheck = await _userService.GetByEmailAsync(model.SendToEmail);
        //            string loginURL = GetLoginUrl(a);
        //            // if the user does not exists already than create new user and than send email
        //            if (userCheck == null)
        //            {

        //                userCheck = await _userService.Create(userCred);
        //                if (userCheck != null)
        //                {

        //                    string subject = "Job Invitation from Chow Choice";
        //                    userCred.LinkOrCode = $"<p> You have been invited for a job through Chow Choice  <br/>";
        //                    userCred.LinkOrCode += @"<p> Use the following Password To Login to Your ChowChoice Account <br/>";
        //                    userCred.LinkOrCode += userCred.UserPassword + @"</p><p>login  <a href=""";
        //                    userCred.LinkOrCode += loginURL;
        //                    userCred.LinkOrCode += @"""> here </a></p> ";
        //                    userCred.LinkOrCode += "<p>Or paste the following link in your browser address bar </p> ";
        //                    userCred.LinkOrCode += "<p>" + loginURL + " </p> ";
        //                    await _emailSender.SendEmailAsync(userCred.UserEmail, subject, userCred.LinkOrCode);
        //                    userCred.UserPassword = null;
        //                }
        //                else
        //                {
        //                    throw new Exception("Unable to create a new user");
        //                }
        //            }
        //            // if the user already exists  than only send email
        //            else if (userCheck.UserID > 0)
        //            {
        //                string subject = "Job Invitation from Chow Choice";
        //                userCred.LinkOrCode = $"<p> You have been invited for a job through Chow Choice  <br/>";
        //                userCred.LinkOrCode += @"<p> Use your existing  account with this email to login to Chow Choice </p>";
        //                userCred.LinkOrCode += @"<p>login  <a href=""";
        //                userCred.LinkOrCode += loginURL;
        //                userCred.LinkOrCode += @"""> here </a></p> ";
        //                userCred.LinkOrCode += "<p>Or paste the following link in your browser address bar </p> ";
        //                userCred.LinkOrCode += "<p>" + loginURL + " </p> ";
        //                userCred.LinkOrCode += "<p> If you have forgotten your password than you can regenerate you password from login form by clicking on the <b>forget password</b> link </p> ";
        //                await _emailSender.SendEmailAsync(userCred.UserEmail, subject, userCred.LinkOrCode);
        //                userCred.UserPassword = null;

        //            }

        //            model.ConfirmByUserID = userCheck.UserID;
        //            model.AppRoleID = userCheck.AppRoleID;

        //            //user created Now add data in EmployeeInvitationReferences and EmployeeJobs
        //            var result = await _employeeInvitationService.AddDataEmployeeInvitation_EmpJobsAsync(model);
        //            result.Data = model;
        //            return StatusCode((int)(result.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), result);
        //        }
        //        return BadRequest(new ServiceResponse { Data = model, IsValid = false, Message = "Unable To create user " });
        //    }
        //    catch (Exception ex)
        //    {
        //        // return error message if there was an exception
        //        return BadRequest(new ServiceResponse { Data = model, IsValid = false, Message = "Unable To create user <br/>" + ex.Message });
        //    }



        //}

        private static string GetLoginUrl(Microsoft.AspNetCore.Http.HostString hostString)
        {
            string loginURL = "";
            if (hostString.Host == "localhost")
            {
                loginURL = @"http://localhost:4200/#/login";
            }
            else if (hostString.Host == "open-source-pos.alishah.pro")
            {
                loginURL = @"http://open-source-pos.alishah.pro/#/login";
            }
            else if (hostString.Host == "vss-server")
            {
                loginURL = @"http://vss-server:9211/#/home/login";
            }

            return loginURL;
        }

        //private UserCred TransaposeToUserCred(NotificationModel model, Employees employee)
        //{
        //    var user = new UserCred
        //    {
        //        AppID = 1,
        //        UserEmail = model.SendToEmail,
        //        IsAdmin = model.role == "ADMIN",

        //        FirstName = employee.FirstName,
        //        MiddleName = employee.MiddleName,
        //        LastName = employee.LastName,
        //        IsActive = true,
        //        IsCustomer = false,
        //        AccessFailedCount = 0,
        //        EmailConfirmed = false,
        //        LockoutEnabled = false,
        //        IsTemp = true,

        //        IsDeleted = false,

        //        CreateDate = DateTime.UtcNow,

        //        AppRoleID = GetAppRoleId(model.role),




        //    };
        //    return user;
        //}

        private int GetAppRoleId(string role)
        {
            switch (role)
            {
                case "ADMIN": return 2;
                case "EMPLOYEE": return 3;
                case "MANAGER": return 4;
                default:
                    return 0;
                    //  break;
            }
        }

        /// <summary>
        /// Dummy method just for testing
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("get")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        /// <summary>
        /// Get Users of a particular company for listing
        /// </summary>
        /// <param name="jSearchUsers"></param>
        /// <returns></returns>
        [Authorize]
        [Route("getCompanyUsers")]
        [HttpPost]
        public async Task<IActionResult> GetCompanyUsers([FromBody] JObject jSearchUsers)
        {
            try
            {
                dynamic SearchUsers = jSearchUsers;
                string query = SearchUsers.query;
                int companyId = SearchUsers.companyId;
                int limit = SearchUsers.limit;
                int offset = SearchUsers.offset;

                ServiceResponse response = await _userService.GetUsersAsync(query, companyId, limit, offset);


                return StatusCode((int)(response.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Authorize]
        [HttpPost("CreateCompanyUser")]        
        public async Task<IActionResult> CreateCompanyUser([FromBody] UserCred userCred)
        {
            try
            {
                var a = Request.Host;
                // The Company users are not admin.
                userCred.IsAdmin = false;
                // save 
                var x = await _userService.Create(userCred);
                if (x != null)
                {
                    string loginURL = GetLoginUrl(a);

                    string subject = "Email Conformation for open-source-pos";
                    userCred.LinkOrCode = @"<p> Use the following Password To Login to Your open-source-pos Account <br/>";
                    userCred.LinkOrCode += userCred.UserPassword + @"</p><p>login  <a href=""";
                    userCred.LinkOrCode += loginURL;
                    userCred.LinkOrCode += @"""> here </a></p> ";
                    userCred.LinkOrCode += "<p>Or paste the following link in your browser address bar </p> ";
                    userCred.LinkOrCode += "<p>" + loginURL + " </p> ";
                    await _emailSender.SendEmailAsync(userCred.UserEmail, subject, userCred.LinkOrCode);
                    userCred.UserPassword = null;
                }
                ServiceResponse response = new ServiceResponse();
                response.Data = x;
                response.Title = ServiceMessages.TitleSuccess;
                response.Message = ServiceMessages.DataSaved;
                response.Flag = true;
                response.IsValid = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                ServiceResponse response = new ServiceResponse();
                response.Data = null;
                response.Title = ServiceMessages.TitleFailure;
                response.Message = ex.Message;
                response.Flag = false;
                response.IsValid = false;
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [Authorize]
        [HttpPost("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserCred userCred)
        {
            try
            {
                
                // update
                var response = await _userService.UpdateUserProfile(userCred);

                return StatusCode((int)(response.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), response);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

    }
    
}