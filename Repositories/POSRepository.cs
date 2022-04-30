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
    public class POSRepository : IPOSRepository
    {
        private readonly IRepository _repo;
        private readonly ILogIt _log;

        public POSRepository(IRepository repo, ILogIt logIt)
        {
            _repo = repo;
            _log = logIt;
        }
        
        public async Task<int> AddDataAsync(POS model)
        {
            try
            {
                return await _repo.WithFNNConnection(async c =>
                {
                    var mst = model.objFNN_INV_MST_TR;
                    var dtl = model.objFNN_INV_DTL_ITEM_TR;
                    int invNo = 0, dtlID = 0;
                    switch (model.Task)
                    {
                        case "SAVE_MASTER":
                            invNo = await SaveInvMaster(c, mst);
                            return invNo;
                        case "SAVE_DETAIL":
                            dtlID = await SaveInvDetail(c, dtl);
                            return dtlID;
                        case "SAVE_MASSTER_WITH_DETAIL":
                            invNo = await SaveInvMaster(c, mst);
                            dtl.INV_NO = invNo;
                            dtlID = await SaveInvDetail(c, dtl);
                            return invNo;
                        default:
                            return -1;
                    }

                    
                });
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return -2;//Task.FromException<List<FNN_ITEM_ST>>(ex).Result;
            }
        }

        public async Task<int> SaveInvMaster(IDbConnection c, FNN_INV_MST_TR mst)
        {
            string sqlInsrtInvMst = @"
                        DECLARE @MMAX_ID INT
                        DECLARE @INV_NO VARCHAR(20)                                      

                        SET @MMAX_ID = ISNULL(
                        (SELECT max( CAST( SUBSTRING((CONVERT(VARCHAR, INV_NO)),5,5) as int)) FROM FNN_INV_MST_TR 
                        WHERE COMPANY_ID= @COMPANY_ID AND SUB_COMPANY_ID= @SUB_COMPANY_ID and fiscal_year_id = @fiscal_year_id AND SALE_TYPE = @SALE_TYPE) ,0)     

                        SET @MMAX_ID = @MMAX_ID + 1                                  
                        SET @INV_NO = dbo.GetINVNo(@MMAX_ID, @INV_DATE,@COMPANY_ID)


                        insert into FNN_INV_MST_TR
                        (INV_NO,	INV_DATE,	BUYER_ID,	CREATE_USER,	CREATE_DATE,	COMPANY_ID,		SUB_COMPANY_ID,		RT_ID,	INV_AMT,	DISCOUNT_AMT,	STAX_PER,	STAX_AMT,	TOTAL_AMT,	NET_AMOUNT,		INV_TYPE,	fiscal_year_id,		TOT_AMT,	linked,		SALE_TYPE,	CALC_STYLE,		OTHER_TAX_PER,	OTHER_TAX_AMT,	INV_DUE_DAYS,	Shipto_ID,	BRANCH_ID,	SAL_ACC,	DISC_ACC) VALUES
                        (@INV_NO,	@INV_DATE,	@BUYER_ID,	@CREATE_USER,	@CREATE_DATE,	@COMPANY_ID,	@SUB_COMPANY_ID,	@RT_ID,	@INV_AMT,	@DISCOUNT_AMT,	@STAX_PER,	@STAX_AMT,	@TOTAL_AMT,	@NET_AMOUNT,	@INV_TYPE,	@fiscal_year_id,	@TOT_AMT,	@linked,	@SALE_TYPE,	@CALC_STYLE,	@OTHER_TAX_PER,	@OTHER_TAX_AMT,	@INV_DUE_DAYS,	@Shipto_ID,	@BRANCH_ID,	@SAL_ACC,	@DISC_ACC)
                        SELECT @INV_NO
                        ; ";

            var res = await c.QueryAsync<int>(sqlInsrtInvMst,
                new
                {
                    
                    INV_DATE = mst.INV_DATE,
                    mst.BUYER_ID,
                    mst.CREATE_USER,
                    mst.CREATE_DATE,
                    mst.COMPANY_ID,
                    mst.SUB_COMPANY_ID,
                    mst.RT_ID,
                    mst.INV_AMT,
                    mst.DISCOUNT_AMT,
                    mst.STAX_PER,
                    mst.STAX_AMT,
                    mst.TOTAL_AMT,
                    mst.NET_AMOUNT,
                    mst.INV_TYPE,
                    mst.fiscal_year_id,
                    mst.TOT_AMT,
                    mst.linked,
                    mst.SALE_TYPE,
                    mst.CALC_STYLE,
                    mst.OTHER_TAX_PER,
                    mst.OTHER_TAX_AMT,
                    mst.INV_DUE_DAYS,
                    mst.Shipto_ID,
                    mst.BRANCH_ID,
                    mst.SAL_ACC,
                    mst.DISC_ACC
                });
            return res.FirstOrDefault();
        }
        public async Task<int> SaveInvDetail(IDbConnection c, FNN_INV_DTL_ITEM_TR dtl)
        {
            string sqlInsrtInvDtl = @"
                DECLARE @S_NO INT;
                SET @S_NO = (SELECT ISNULL(MAX(S_NO),0)+1 FROM FNN_INV_DTL_ITEM_TR WHERE COMPANY_ID=@COMPANY_ID AND SUB_COMPANY_ID=@SUB_COMPANY_ID AND INV_NO=@INV_NO AND SALE_TYPE = @SALE_TYPE AND fiscal_year_id=@fiscal_year_id)
                IF @S_NO IS NULL
	                SET @S_NO = 1
                INSERT INTO FNN_INV_DTL_ITEM_TR
                (INV_NO,	S_NO,	ARTICLE_NO,		GRADE,	PCS,	QTY,	UNIT,	DC_RATE,	INV_RATE,	INV_AMOUNT,		CREATE_USER,	CREATE_DATE,	COMPANY_ID,		SUB_COMPANY_ID,		RT_ID,	CARTON_NO,	COLOUR_DESC,	DC_NO,	CONTRACT_NO,	fiscal_year_id,		SIDES,	INV_TYPE,	SALE_TYPE,	DISCOUNT_PER,	DISCOUNT_AMT,	SCH_CODE,	BRANCH_ID,	EMPLOYEE_ID,	TAX_PER,	TAX_AMT,	EMPLOYEE) VALUES
                (@INV_NO,	@S_NO,	@ARTICLE_NO,	@GRADE,	@PCS,	@QTY,	@UNIT,	@DC_RATE,	@INV_RATE,	@INV_AMOUNT,	@CREATE_USER,	@CREATE_DATE,	@COMPANY_ID,	@SUB_COMPANY_ID,	@RT_ID,	@CARTON_NO,	@COLOUR_DESC,	@DC_NO,	@CONTRACT_NO,	@fiscal_year_id,	@SIDES,	@INV_TYPE,	@SALE_TYPE,	@DISCOUNT_PER,	@DISCOUNT_AMT,	@SCH_CODE,	@BRANCH_ID,	@EMPLOYEE_ID,	@TAX_PER,	@TAX_AMT,	@EMPLOYEE) 

                SELECT @S_NO
                        ; ";

            var res = await c.QueryAsync<int>(sqlInsrtInvDtl,
                new
                {
                    dtl.INV_NO,
                    dtl.ARTICLE_NO,
                    dtl.GRADE,
                    dtl.PCS,
                    dtl.QTY,
                    dtl.UNIT,
                    dtl.DC_RATE,
                    dtl.INV_RATE,
                    dtl.INV_AMOUNT,
                    dtl.CREATE_USER,
                    dtl.CREATE_DATE,
                    dtl.COMPANY_ID,
                    dtl.SUB_COMPANY_ID,
                    dtl.RT_ID,
                    dtl.CARTON_NO,
                    dtl.COLOUR_DESC,
                    dtl.DC_NO,
                    dtl.CONTRACT_NO,
                    dtl.fiscal_year_id,
                    dtl.SIDES,
                    dtl.INV_TYPE,
                    dtl.SALE_TYPE,
                    dtl.DISCOUNT_PER,
                    dtl.DISCOUNT_AMT,
                    dtl.SCH_CODE,
                    dtl.BRANCH_ID,
                    dtl.EMPLOYEE_ID,
                    dtl.TAX_PER,
                    dtl.TAX_AMT,
                    dtl.EMPLOYEE
                });
            return res.FirstOrDefault();
        }

        public async Task<int> UpdDataAsync(POS model)
        {
            try
            {
                return await _repo.WithFNNConnection(async c =>
                {
                    //var mst = model.objFNN_INV_MST_TR;
                    var dtl = model.objFNN_INV_DTL_ITEM_TR;
                    //int invNo = 0, rows = 0;
                    switch (model.Task)
                    {
                        case "UPDATE_MASTER":
                            throw new NotImplementedException();
                        case "UPDATE_DETAIL":
                            return await UpdateInvDetail(c, dtl);                            
                        case "UPDATE_MASSTER_WITH_DETAIL":
                            throw new NotImplementedException();
                        default:
                            return -1;
                    }


                });
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return -2;//Task.FromException<List<FNN_ITEM_ST>>(ex).Result;
            }

        }

        public async Task<int> UpdateInvDetail(IDbConnection c, FNN_INV_DTL_ITEM_TR dtl)
        {
            string sqlUpdateInvDtl = @"
                UPDATE FNN_INV_DTL_ITEM_TR               
                SET                
                ARTICLE_NO = @ARTICLE_NO,		QTY = @QTY,						UNIT = @UNIT,	
                DC_RATE = @DC_RATE,				INV_RATE = @INV_RATE,			INV_AMOUNT = @INV_AMOUNT,
                DISCOUNT_PER = @DISCOUNT_PER,	DISCOUNT_AMT = @DISCOUNT_AMT,	SCH_CODE = @SCH_CODE,	
                EMPLOYEE_ID = @EMPLOYEE_ID,		TAX_PER = @TAX_PER,				TAX_AMT = @TAX_AMT,	
                EMPLOYEE = @EMPLOYEE
                WHERE                 
                INV_NO = @INV_NO            
                AND S_NO = @S_NO                
                AND COMPANY_ID = @COMPANY_ID                
                AND SUB_COMPANY_ID = @SUB_COMPANY_ID                
                AND fiscal_year_id = @fiscal_year_id  
                AND INV_TYPE = @INV_TYPE       
                AND SALE_TYPE = @SALE_TYPE
                SELECT @@ROWCOUNT
                        ; ";

            var res = await c.QueryAsync<int>(sqlUpdateInvDtl,
                new
                {
                    dtl.ARTICLE_NO,
                    dtl.QTY,
                    dtl.UNIT,
                    dtl.DC_RATE,
                    dtl.INV_RATE,
                    dtl.INV_AMOUNT,
                    dtl.DISCOUNT_PER,
                    dtl.DISCOUNT_AMT,
                    dtl.SCH_CODE,
                    dtl.EMPLOYEE_ID,
                    dtl.TAX_PER,
                    dtl.TAX_AMT,
                    dtl.EMPLOYEE,
                    dtl.INV_NO,
                    dtl.S_NO,
                    dtl.COMPANY_ID,
                    dtl.SUB_COMPANY_ID,
                    dtl.fiscal_year_id,
                    dtl.INV_TYPE,
                    dtl.SALE_TYPE
                });
            return res.FirstOrDefault();
        }

        public async Task<List<FNN_ITEM_ST>> GetSearchItemsAsync(string query, int companyId)
        {
            try
            {
                return await _repo.WithFNNConnection(async c =>
                {
                    string sqlSearchItems = @"SELECT top 100 * FROM FNN_ITEM_ST
                        WHERE isnull(CustomCode, '') + isnull(ITEM_DESC, '') like '%' + @QUERY + '%'
                        AND COMPANY_ID = @COMPANY_ID  ORDER BY ITEM_DESC; ";

                    var searchItems = await c.QueryAsync<FNN_ITEM_ST>(sqlSearchItems, new { QUERY = query, COMPANY_ID = companyId });
                    return searchItems.ToList();
                });
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<List<FNN_ITEM_ST>>(ex).Result;
            }
        }
        
    }
}
