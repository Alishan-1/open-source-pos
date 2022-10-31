using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Repositories;
using Repositories.Log;
using Services.Validations;

namespace Services
{

    public class POSService : IPOSService
    {
        private readonly IPOSRepository _repo;
        private readonly ILogIt _log;



        public POSService(IPOSRepository repo, ILogIt log)
        {
            _repo = repo;
            _log = log;
        }
        

        

        public async Task<ServiceResponse> GetSearchItemsAsync(string query, int companyId)
        {
            try
            {
                ServiceResponse response = new ServiceResponse();

                if (companyId <= 0)

                {
                    response.IsValid = false;
                    response.Title = "Error!";
                    response.Message = "Invalid company request with paramenters.";
                }
                else
                {
                    var items = await _repo.GetSearchItemsAsync(query, companyId);

                    var posItems = new List<PosItem>();

                    foreach (var item in items)
                    {
                        var posItem = new PosItem { CustomCode =item.CustomCode, ItemId = item.ItemId, Description = item.Description, Id = item.ItemId, SalePrice=item.SalePrice };
                        posItems.Add(posItem);

                    }
                    response.Data = posItems;
                    response.IsValid = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<ServiceResponse>(ex).Result;
            }

        }

        public async Task<ServiceResponse> AddDataAsync(POS model)
        {
            try
            {
                ServiceResponse vmServiceResponse = ServiceValidation.Validate(model, new PosValidator());

                int result = 0;

                if (vmServiceResponse.IsValid)
                {
                    if (model.Task == "SAVE_MASTER")
                    {
                        var mstSr = ServiceValidation.Validate(model.objInvoiceMaster, new InvoiceMasterValidator());
                        if (!mstSr.IsValid)
                        {
                            mstSr.Title = ServiceMessages.TitleFailure;
                            mstSr.Message = ServiceErrorsMessages.DataInvalid;
                            mstSr.Flag = false;
                            return mstSr;
                        }
                    }
                    if (model.Task == "SAVE_DETAIL")
                    {
                        var dtlSr = ServiceValidation.Validate(model.objInvoiceDetailItems, new InvoiceDetailItemsValidator());
                        if (!dtlSr.IsValid)
                        {
                            dtlSr.Title = ServiceMessages.TitleFailure;
                            dtlSr.Message = ServiceErrorsMessages.DataInvalid;
                            dtlSr.Flag = false;
                            return dtlSr;
                        }
                    }
                    if (model.Task == "SAVE_MASSTER_WITH_DETAIL")
                    {
                        var mstSr = ServiceValidation.Validate(model.objInvoiceMaster, new InvoiceMasterValidator());
                        if (!mstSr.IsValid)
                        {
                            mstSr.Title = ServiceMessages.TitleFailure;
                            mstSr.Message = ServiceErrorsMessages.DataInvalid;
                            mstSr.Flag = false;
                            return mstSr;
                        }
                        var dtlSr = ServiceValidation.Validate(model.objInvoiceDetailItems, new InvoiceDetailItemsValidator());
                        if (!dtlSr.IsValid)
                        {
                            dtlSr.Title = ServiceMessages.TitleFailure;
                            dtlSr.Message = ServiceErrorsMessages.DataInvalid;
                            dtlSr.Flag = false;
                            return dtlSr;
                        }
                    }

                    result = await _repo.AddDataAsync(model);
                    vmServiceResponse.Data = result;

                    if (result <= 0)
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleFailure;
                        vmServiceResponse.Message = ServiceMessages.DataNotSaved;
                        vmServiceResponse.Flag = false;
                        vmServiceResponse.IsValid = false;
                    }
                    else
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleSuccess;
                        vmServiceResponse.Message = ServiceMessages.DataSaved;
                        vmServiceResponse.Flag = true;
                        vmServiceResponse.IsValid = true;
                    }
                }
                else
                {
                    vmServiceResponse.Title = ServiceMessages.TitleFailure;
                    vmServiceResponse.Message = ServiceErrorsMessages.DataInvalid;
                    vmServiceResponse.Flag = false;
                }

                return vmServiceResponse;


            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<ServiceResponse>(ex).Result;
            }

        }
        public async Task<ServiceResponse> UpdDataAsync(POS model)
        {
            try
            {
                ServiceResponse vmServiceResponse = ServiceValidation.Validate(model, new PosValidator());

                int result = 0;

                if (vmServiceResponse.IsValid)
                {
                    if (model.Task == "UPDATE_DETAIL")
                    {
                        var dtlSr = ServiceValidation.Validate(model.objInvoiceDetailItems, new InvoiceDetailItemsValidator());
                        if (!dtlSr.IsValid)
                        {
                            dtlSr.Title = ServiceMessages.TitleFailure;
                            dtlSr.Message = ServiceErrorsMessages.DataInvalid;
                            dtlSr.Flag = false;
                            return dtlSr;
                        }
                    }
                    if (model.Task == "UPDATE_MASTER")
                    {
                        var mstSr = ServiceValidation.Validate(model.objInvoiceMaster, new InvoiceMasterValidator());
                        if (!mstSr.IsValid)
                        {
                            mstSr.Title = ServiceMessages.TitleFailure;
                            mstSr.Message = ServiceErrorsMessages.DataInvalid;
                            mstSr.Flag = false;
                            return mstSr;
                        }
                    }
                    result = await _repo.UpdDataAsync(model);
                    vmServiceResponse.Data = result;

                    if (result <= 0)
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleFailure;
                        vmServiceResponse.Message = ServiceMessages.DataNotFound;
                        vmServiceResponse.Flag = false;
                        vmServiceResponse.IsValid = false;
                    }
                    else if (result > 1)
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleFailure;
                        vmServiceResponse.Message = "More Rows Updated Than Expected.";
                        vmServiceResponse.Flag = false;
                        vmServiceResponse.IsValid = false;
                    }
                    else
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleSuccess;
                        vmServiceResponse.Message = ServiceMessages.DataSaved;
                        vmServiceResponse.Flag = true;
                        vmServiceResponse.IsValid = true;
                    }
                }
                else
                {
                    vmServiceResponse.Title = ServiceErrorsMessages.Title;
                    vmServiceResponse.Message = ServiceErrorsMessages.DataInvalid;
                    vmServiceResponse.Flag = false;
                }

                return vmServiceResponse;


            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<ServiceResponse>(ex).Result;
            }

        }
        public async Task<ServiceResponse> GetInvoicesAsync(string query, int companyId, int limit, int offset)
        {
            try
            {
                ServiceResponse response = new ServiceResponse();

                if (companyId <= 0)
                {
                    response.Flag = false;
                    response.IsValid = false;
                    response.Title = "Error!";
                    response.Message = "Invalid Value for company.";
                }
                else
                {
                    var invoices = await _repo.GetInvoicesAsync(query, companyId, limit, offset);

                    GetInvoicesModel model = new GetInvoicesModel();
                    model.Invoices = invoices;
                    model.Count = invoices.Count;

                    response.Flag = true;
                    response.Data = model;
                    response.IsValid = true;
                }
                return response;
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<ServiceResponse>(ex).Result;
            }

        }

        public async Task<ServiceResponse> GetInvoiceDetailsAsync(string InvoiceNo, string InvoiceType, int FiscalYearID, int CompanyID)
        {
            try
            {
                ServiceResponse response = new ServiceResponse();

                if ( string.IsNullOrWhiteSpace( InvoiceNo) || string.IsNullOrWhiteSpace(InvoiceType) || CompanyID <= 0 || FiscalYearID <= 0)
                {
                    response.Flag = false;
                    response.IsValid = false;
                    response.Title = "Error!";
                    response.Message = "Invalid Parameters.";
                }
                else
                {
                    response.Data = await _repo.GetInvoiceDetailsAsync(InvoiceNo, InvoiceType, FiscalYearID, CompanyID);
                    response.Flag = true;                    
                    response.IsValid = true;
                }
                return response;
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<ServiceResponse>(ex).Result;
            }

        }

        public async Task<ServiceResponse> DeleteInvDetail(InvoiceDetailItems dtl)
        {
            try
            {
                ServiceResponse vmServiceResponse = new ServiceResponse();
                vmServiceResponse.IsValid = false;

                if (dtl.InvoiceNo > 0 && dtl.SrNo > 0 && dtl.CompanyID > 0 && dtl.FiscalYearID > 0 &&
                        !string.IsNullOrWhiteSpace( dtl.InvoiceType)     )
                {
                    vmServiceResponse.IsValid = true;
                }
                
                
                int result = 0;

                if (vmServiceResponse.IsValid)
                {
                    result = await _repo.DeleteInvDetail(dtl);
                    vmServiceResponse.Data = result;

                    if (result <= 0)
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleFailure;
                        vmServiceResponse.Message = ServiceMessages.DataNotFound;
                        vmServiceResponse.Flag = false;
                        vmServiceResponse.IsValid = false;
                    }
                    else if (result > 1)
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleFailure;
                        vmServiceResponse.Message = "More Rows Deleted Than Expected.";
                        vmServiceResponse.Flag = false;
                        vmServiceResponse.IsValid = false;
                    }
                    else
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleSuccess;
                        vmServiceResponse.Message = ServiceMessages.DataSaved;
                        vmServiceResponse.Flag = true;
                        vmServiceResponse.IsValid = true;
                    }
                }
                else
                {
                    vmServiceResponse.Title = ServiceErrorsMessages.Title;
                    vmServiceResponse.Message = ServiceErrorsMessages.DataInvalid;
                    vmServiceResponse.Flag = false;
                }

                return vmServiceResponse;


            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<ServiceResponse>(ex).Result;
            }

        }
        public async Task<ServiceResponse> DeleteInvoice(InvoiceMaster mst)
        {
            try
            {
                ServiceResponse vmServiceResponse = new ServiceResponse();
                vmServiceResponse.IsValid = false;

                if (mst.InvoiceNo > 0 && mst.CompanyID > 0 && mst.FiscalYearID > 0 &&
                        !string.IsNullOrWhiteSpace(mst.InvoiceType))
                {
                    vmServiceResponse.IsValid = true;
                }


                int result = 0;

                if (vmServiceResponse.IsValid)
                {
                    result = await _repo.DeleteInvoice(mst);
                    vmServiceResponse.Data = result;

                    if (result <= 0)
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleFailure;
                        vmServiceResponse.Message = ServiceMessages.DataNotFound;
                        vmServiceResponse.Flag = false;
                        vmServiceResponse.IsValid = false;
                    }
                    else if (result > 1)
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleFailure;
                        vmServiceResponse.Message = "More Rows Deleted Than Expected.";
                        vmServiceResponse.Flag = false;
                        vmServiceResponse.IsValid = false;
                    }
                    else
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleSuccess;
                        vmServiceResponse.Message = ServiceMessages.DataSaved;
                        vmServiceResponse.Flag = true;
                        vmServiceResponse.IsValid = true;
                    }
                }
                else
                {
                    vmServiceResponse.Title = ServiceErrorsMessages.Title;
                    vmServiceResponse.Message = ServiceErrorsMessages.DataInvalid;
                    vmServiceResponse.Flag = false;
                }

                return vmServiceResponse;


            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<ServiceResponse>(ex).Result;
            }

        }


        public async Task<ServiceResponse> DeleteInvoicesList(InvoiceMaster[] invoicesMst)
        {
            try
            {
                ServiceResponse vmServiceResponse = new ServiceResponse();
                vmServiceResponse.IsValid = false;

                foreach (InvoiceMaster mst in invoicesMst)
                {
                    if (mst.InvoiceNo > 0 && mst.CompanyID > 0 && mst.FiscalYearID > 0 &&
                        !string.IsNullOrWhiteSpace(mst.InvoiceType))
                    {
                        vmServiceResponse.IsValid = true;
                    }
                    else
                    {
                        vmServiceResponse.IsValid = false;
                    }
                }
                


                int result = 0;

                if (vmServiceResponse.IsValid)
                {
                    result = await _repo.DeleteInvoicesList(invoicesMst);
                    vmServiceResponse.Data = result;

                    if (result <= 0)
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleFailure;
                        vmServiceResponse.Message = ServiceMessages.DataNotFound;
                        vmServiceResponse.Flag = false;
                        vmServiceResponse.IsValid = false;
                    }
                    else if (result > invoicesMst.Length)
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleFailure;
                        vmServiceResponse.Message = "More Rows Deleted Than Expected.";
                        vmServiceResponse.Flag = false;
                        vmServiceResponse.IsValid = false;
                    }
                    else
                    {
                        vmServiceResponse.Title = ServiceMessages.TitleSuccess;
                        vmServiceResponse.Message = $"{result} invoice(s) deleted successfully";
                        vmServiceResponse.Flag = true;
                        vmServiceResponse.IsValid = true;
                    }
                }
                else
                {
                    vmServiceResponse.Title = ServiceErrorsMessages.Title;
                    vmServiceResponse.Message = ServiceErrorsMessages.DataInvalid;
                    vmServiceResponse.Flag = false;
                }

                return vmServiceResponse;


            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<ServiceResponse>(ex).Result;
            }

        }
    }
}
