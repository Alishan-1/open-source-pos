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
        Task<ServiceResponse> GetInvoicesAsync(string query, int companyId, int limit, int offset);
        /// <summary>
        /// Get all details (items) of a particular Invoice
        /// </summary>
        /// <param name="InvoiceNo"></param>
        /// <param name="InvoiceType"></param>
        /// <param name="FiscalYearID"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        Task<ServiceResponse> GetInvoiceDetailsAsync(string InvoiceNo, string InvoiceType, int FiscalYearID, int CompanyID);
        /// <summary>
        /// Deleates a single, particular item row from the details of invoice.
        /// </summary>
        /// <param name="dtl"></param>
        /// <returns></returns>
        Task<ServiceResponse> DeleteInvDetail(InvoiceDetailItems dtl);
        /// <summary>
        /// Delete whole invoice including all details if it is not posted.
        /// </summary>
        /// <param name="mst"></param>
        /// <returns></returns>
        Task<ServiceResponse> DeleteInvoice(InvoiceMaster mst);
        /// <summary>
        /// Deletes a list of whole invoices including all details if it is not posted.
        /// </summary>
        /// <param name="invoicesMst">list of invoices to delete</param>
        /// <returns></returns>
        Task<ServiceResponse> DeleteInvoicesList(InvoiceMaster[] invoicesMst);

    }
}
