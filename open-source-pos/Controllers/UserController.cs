using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Models;
using Services;

namespace open_source_pos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

                var user = await _userService.Authenticate(userParam.UserEmail, userParam.UserPassword, userParam);

                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                if (user.authenticationResult.IsLockoutEnabled.GetValueOrDefault(false))
                {
                    return BadRequest(new { message = "Your account is locked for some time because of too many unsuccessful attempts" });
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


    }
}