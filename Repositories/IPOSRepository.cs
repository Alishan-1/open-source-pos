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
        Task<int> SaveInvMaster(IDbConnection c, InvoiceMaster mst);
        Task<int> SaveInvDetail(IDbConnection c, InvoiceDetailItems dtl);
        Task<int> UpdDataAsync(POS model);
        Task<int> UpdateInvDetail(IDbConnection c, InvoiceDetailItems dtl);        
        Task<List<FNN_ITEM_ST>> GetSearchItemsAsync(string query, int companyId);
        /// <summary>
        /// Returns the Invoice master records for Listing.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="companyId"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<List<InvoiceMasterListing>> GetInvoicesAsync(string query, int companyId, int limit, int offset);
        /// <summary>
        /// Get all details (items) of a particular Invoice
        /// </summary>
        /// <param name="InvoiceNo"></param>
        /// <param name="InvoiceType"></param>
        /// <param name="FiscalYearID"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        Task<List<InvoiceDetailItemEditModel>> GetInvoiceDetailsAsync(string InvoiceNo, string InvoiceType, int FiscalYearID, int CompanyID);
    }
}
