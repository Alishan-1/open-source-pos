using Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IItemRepository 
    {
        Task<int> AddDataAsync(PosItem posItem);        
        Task<int> UpdDataAsync(PosItem posItem);        
        Task<List<PosItem>> GetItemsAsync(string query, int companyId, int limit, int offset);
    }
}
