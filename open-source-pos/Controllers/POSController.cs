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

namespace ChowChoice.Api.Controllers.BSS_ERP
{
    [EnableCors("corsGlobalPolicy")]
    [Authorize]
    
    [Produces("application/json")]
    [Route("api/POS")]
    public class POSController : Controller
    {
        private readonly IPOSService _POSService;

        public POSController(IPOSService POSService)
        {
            _POSService = POSService;
        }
        

        //[Route("add")]
        //[HttpPost]
        //public async Task<IActionResult> AddData([FromBody]Models.EmployeeTimeLog employeeTimeLogAdd)
        //{
        //    ServiceResponse response = await _POSService.AddDataAsync(employeeTimeLogAdd);
        //    return StatusCode((int)(response.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), response);
        //}

        //[Route("update")]
        //[HttpPost]
        //public async Task<IActionResult> UpdData([FromBody]Models.EmployeeTimeLog employeeTimeLogUpdate)
        //{
        //    ServiceResponse response = await _POSService.UpdDataAsync(employeeTimeLogUpdate);
        //    return StatusCode((int)(response.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), response);
        //}

        

        

        

        ///////// new working ///////

        /// <summary>
        /// used to get items in search.
        /// </summary>
        /// <param name="jEmployeeTimeLog"></param>
        /// <returns></returns>
        [Route("GetSearchItems")]
        [HttpPost]
        public async Task<IActionResult> GetSearchItems([FromBody] JObject jSearchItems)
        {
            dynamic SearchItems = jSearchItems;
            string Query = SearchItems.Query;
            int companyId = SearchItems.companyId;

            ServiceResponse response = await _POSService.GetSearchItemsAsync(Query, companyId);

            
            return StatusCode((int)(response.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), response);
        }
        /// <summary>
        /// insert pos transaction record.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]POS pos)
        {
            ServiceResponse response = await _POSService.AddDataAsync(pos);
            return StatusCode((int)(response.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), response);
        }
        /// <summary>
        /// Update pos transaction records
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]POS pos)
        {
            ServiceResponse response = await _POSService.UpdDataAsync(pos);
            return StatusCode((int)(response.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest), response);
        }
    }
}