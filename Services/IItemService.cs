using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IItemService
    {

        
        Task<ServiceResponse> AddDataAsync(PosItem posItem);
        Task<ServiceResponse> UpdDataAsync(PosItem posItem);
        Task<ServiceResponse> GetItemsAsync(string query, int companyId, int limit, int offset);
    }
}
