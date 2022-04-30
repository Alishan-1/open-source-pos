using Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IPOSRepository 
    {
        Task<int> AddDataAsync(POS model);
        Task<int> SaveInvMaster(IDbConnection c, FNN_INV_MST_TR mst);
        Task<int> SaveInvDetail(IDbConnection c, FNN_INV_DTL_ITEM_TR dtl);
        Task<int> UpdDataAsync(POS model);
        Task<int> UpdateInvDetail(IDbConnection c, FNN_INV_DTL_ITEM_TR dtl);        
        Task<List<FNN_ITEM_ST>> GetSearchItemsAsync(string query, int companyId);
    }
}
