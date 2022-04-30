using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IPOSService 
    {

        Task<ServiceResponse> GetSearchItemsAsync(string query, int companyId);
        Task<ServiceResponse> AddDataAsync(POS model);
        Task<ServiceResponse> UpdDataAsync(POS model);
        
    }
}
