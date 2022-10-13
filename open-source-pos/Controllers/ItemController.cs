using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Models;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Api.Controllers
{
    [EnableCors("corsGlobalPolicy")]
    [Authorize]
    
    [Produces("application/json")]
    [Route("api/item")]
    public class ItemController : Controller
    {
        private readonly IItemService _ItemService;

        public ItemController(IItemService ItemService)
        {
            _ItemService = ItemService;
        }

        /// <summary>
        /// used to get items.
        /// </summary>
        /// <param name="jSearchItems"></param>
        /// <returns></returns>
        [Route("getitems")]
        [HttpPost]
        public async Task<IActionResult> GetSearchItems([FromBody] JObject jSearchItems)
        {
            try
            {
                dynamic SearchItems = jSearchItems;
                string query = SearchItems.query;
                int companyId = SearchItems.companyId;
                int limit = SearchItems.limit;
                int offset = SearchItems.offset;

                ServiceResponse response = await _ItemService.GetItemsAsync(query, companyId, limit, offset);


                return StatusCode((int)(response.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }
        /// <summary>
        /// insert pos Product / Item record.
        /// </summary>
        /// <param name="posItem"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PosItem posItem)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).First();
                var userID = int.Parse(userIdClaim.Value);

                if (posItem != null)
                {
                    posItem.CreateUser = userID;
                }
                ServiceResponse response = await _ItemService.AddDataAsync(posItem);
                return StatusCode((int)(response.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), response);
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }
        /// <summary>
        /// Update pos Product / Item record.
        /// </summary>
        /// <param name="posItem"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]PosItem posItem)
        {
            var userIdClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).First();
            var userID = int.Parse(userIdClaim.Value);

            if (posItem != null)
            {
                posItem.UpdateUser = userID;
            }
            ServiceResponse response = await _ItemService.UpdDataAsync(posItem);
            return StatusCode((int)(response.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), response);
        }
    }
}