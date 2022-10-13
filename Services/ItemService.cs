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

    public class ItemService : IItemService
    {
        private readonly IItemRepository _repo;
        private readonly ILogIt _log;



        public ItemService(IItemRepository repo, ILogIt log)
        {
            _repo = repo;
            _log = log;
        }
        

        

        public async Task<ServiceResponse> GetItemsAsync(string query, int companyId, int limit, int offset)
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
                    var items = await _repo.GetItemsAsync(query, companyId,limit,offset);

                    GetPosItemsModel model = new GetPosItemsModel();
                    model.Items = items;
                    model.Count = items.Count;

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

        public async Task<ServiceResponse> AddDataAsync(PosItem posItem)
        {
            try
            {
                ServiceResponse vmServiceResponse = ServiceValidation.Validate(posItem, new PosItemValidator());

                int result = 0;

                if (vmServiceResponse.IsValid)
                {
                    
                    result = await _repo.AddDataAsync(posItem);
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
                        vmServiceResponse.Data = result;
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
        public async Task<ServiceResponse> UpdDataAsync(PosItem posItem)
        {
            try
            {
                ServiceResponse vmServiceResponse = ServiceValidation.Validate(posItem, new PosItemValidator());

                int result = 0;

                if (vmServiceResponse.IsValid)
                {
                    result = await _repo.UpdDataAsync(posItem);
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
