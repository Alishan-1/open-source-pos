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

                    var posItems = new List<posItem>();

                    foreach (var item in items)
                    {
                        var posItem = new posItem { customCode =item.CustomCode, ItemId = item.ITEM_ID, Description = item.ITEM_DESC, id = item.ITEM_ID, SalePrice=item.SaleCost };
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
                        var mstSr = ServiceValidation.Validate(model.objFNN_INV_MST_TR, new FNN_INV_MST_TRValidator());
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
                        var dtlSr = ServiceValidation.Validate(model.objFNN_INV_DTL_ITEM_TR, new FNN_INV_DTL_ITEM_TRValidator());
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
                        var mstSr = ServiceValidation.Validate(model.objFNN_INV_MST_TR, new FNN_INV_MST_TRValidator());
                        if (!mstSr.IsValid)
                        {
                            mstSr.Title = ServiceMessages.TitleFailure;
                            mstSr.Message = ServiceErrorsMessages.DataInvalid;
                            mstSr.Flag = false;
                            return mstSr;
                        }
                        var dtlSr = ServiceValidation.Validate(model.objFNN_INV_DTL_ITEM_TR, new FNN_INV_DTL_ITEM_TRValidator());
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
                        var dtlSr = ServiceValidation.Validate(model.objFNN_INV_DTL_ITEM_TR, new FNN_INV_DTL_ITEM_TRValidator());
                        if (!dtlSr.IsValid)
                        {
                            dtlSr.Title = ServiceMessages.TitleFailure;
                            dtlSr.Message = ServiceErrorsMessages.DataInvalid;
                            dtlSr.Flag = false;
                            return dtlSr;
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
    }
}
