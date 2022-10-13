using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Repositories.SqlServer;
using Repositories.Log;
using Dapper;
using System.Data;
using System.Linq;

namespace Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IRepository _repo;
        private readonly ILogIt _log;

        public ItemRepository(IRepository repo, ILogIt logIt)
        {
            _repo = repo;
            _log = logIt;
        }
        
        public async Task<int> AddDataAsync(PosItem posItem)
        {
            try
            {
                return await _repo.WithConnection(async cmd =>
                {
                    string sql = @"
                    DECLARE @MMAX_ID INT                                      

                    SET @MMAX_ID = ISNULL( (SELECT max( isnull( ItemId, 0)) FROM PosItem) ,0)
                        
                    SET @MMAX_ID = @MMAX_ID + 1                                                          

                    INSERT INTO PosItem(ItemId, CustomCode, Description, SalePrice, CreateUser, CreateDate, CompanyID)
                    VALUES(@MMAX_ID, @CustomCode, @Description, @SalePrice, @CreateUser, GETDATE(), @CompanyID)
                    SELECT @MMAX_ID
                        ; ";

                    var res = await cmd.QueryAsync<int>(sql, posItem);
                    var result = res.FirstOrDefault();
                    return result;

                });

            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<int>(ex).Result;
            }
        }

        public async Task<List<PosItem>> GetItemsAsync(string query, int companyId, int limit, int offset)
        {
            try
            {
                return await _repo.WithFNNConnection(async c =>
                {
                    string sqlSearchItems = @"SELECT  ItemId, CustomCode, Description, SalePrice, CreateUser, CreateDate, UpdateUser, UpdateDate, CompanyID FROM PosItem
                        WHERE isnull(CustomCode, '') + isnull(Description, '') like '%' + @QUERY + '%'
                        AND CompanyID = @COMPANY_ID  ORDER BY Description; ";

                    var searchItems = await c.QueryAsync<PosItem>(sqlSearchItems, new { QUERY = query, COMPANY_ID = companyId });
                    return searchItems.ToList();
                });
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<List<PosItem>>(ex).Result;
            }
        }

        public async Task<int> UpdDataAsync(PosItem posItem)
        {
            try
            {
                return await _repo.WithConnection(async cmd =>
                {
                    string sql = @"
                    UPDATE PosItem SET
                    CustomCode = @CustomCode, 
                    Description = @Description, 
                    SalePrice = @SalePrice,
                    UpdateUser = @UpdateUser,
                    UpdateDate = GETDATE()
                    WHERE ItemId = @ItemId AND CompanyID = @CompanyID
                    SELECT @@ROWCOUNT
                    ; ";

                    var res = await cmd.QueryAsync<int>(sql,posItem);
                    var result = res.FirstOrDefault();
                    return result;

                });

            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<int>(ex).Result;
            }

        }
    }
}
