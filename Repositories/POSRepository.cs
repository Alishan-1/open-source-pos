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
                    var mst = model.objInvoiceMaster;
                    var dtl = model.objInvoiceDetailItems;
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
                            dtl.InvoiceNo = invNo;
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

        public async Task<int> SaveInvMaster(IDbConnection c, InvoiceMaster mst)
        {

            // TODO: Check why the number generation does not work within five hours of entering into new fiscal year.
            string sqlInsrtInvMst = @"
                        DECLARE @MMAX_ID INT                                      

                        SET @MMAX_ID = ISNULL(
                        (SELECT max( CAST( SUBSTRING((CONVERT(VARCHAR, InvoiceNo)),5,5) as int)) FROM InvoiceMaster 
                        WHERE CompanyID = @CompanyID AND FiscalYearID = @FiscalYearID AND InvoiceType = @InvoiceType) ,0)     

                        
                        SET @MMAX_ID = @MMAX_ID + 1                                  
                        SET @InvoiceNo = dbo.GetINVNo(@MMAX_ID, @InvoiceDate, @CompanyID)
                        

                        INSERT INTO InvoiceMaster(
                        InvoiceNo,
                        InvoiceDate,
                        CustomerID,
                        CreateUser,
                        CreateDate,
                        UpdateUser,
                        UpdateDate,
                        CompanyID,
                        ModuleID,
                        TotalAmount,
                        DiscountPercent,
                        DiscountAmount,
                        SaleTaxPercent,
                        SaleTaxAmount,
                        NetAmount,
                        ReceivedAmount,
                        BalanceAmount,
                        InvoiceType,
                        FiscalYearID,
                        OtherTaxPercent,
                        OtherTaxAmount,
                        CreditLimit,
                        ConsumedCredit,
                        BalanceCredit,
                        Status,
                        BranchID,
                        Remarks)
                        VALUES (
                        @InvoiceNo,
                        @InvoiceDate,
                        @CustomerID,
                        @CreateUser,
                        @CreateDate,
                        @UpdateUser,
                        @UpdateDate,
                        @CompanyID,
                        @ModuleID,
                        @TotalAmount,
                        @DiscountPercent,
                        @DiscountAmount,
                        @SaleTaxPercent,
                        @SaleTaxAmount,
                        @NetAmount,
                        @ReceivedAmount,
                        @BalanceAmount,
                        @InvoiceType,
                        @FiscalYearID,
                        @OtherTaxPercent,
                        @OtherTaxAmount,
                        @CreditLimit,
                        @ConsumedCredit,
                        @BalanceCredit,
                        @Status,
                        @BranchID,
                        @Remarks)
                        SELECT @InvoiceNo
                        ; ";

            var res = await c.QueryAsync<int>(sqlInsrtInvMst,
                new
                {
                    
                    mst.InvoiceNo,
                    mst.InvoiceDate,
                    mst.CustomerID,
                    mst.CreateUser,
                    mst.CreateDate,
                    mst.UpdateUser,
                    mst.UpdateDate,
                    mst.CompanyID,
                    mst.ModuleID,
                    mst.TotalAmount,
                    mst.DiscountPercent,
                    mst.DiscountAmount,
                    mst.SaleTaxPercent,
                    mst.SaleTaxAmount,
                    mst.NetAmount,
                    mst.ReceivedAmount,
                    mst.BalanceAmount,
                    mst.InvoiceType,
                    mst.FiscalYearID,
                    mst.OtherTaxPercent,
                    mst.OtherTaxAmount,
                    mst.CreditLimit,
                    mst.ConsumedCredit,
                    mst.BalanceCredit,
                    mst.Status,
                    mst.BranchID,
                    mst.Remarks
                });
            return res.FirstOrDefault();
        }
        public async Task<int> SaveInvDetail(IDbConnection c, InvoiceDetailItems dtl)
        {
            string sqlInsrtInvDtl = @"
                DECLARE @SrNo INT;
                SET @SrNo = (SELECT ISNULL(MAX(SrNo),0)+1 FROM InvoiceDetailItems WHERE CompanyID=@CompanyID AND InvoiceNo=@InvoiceNo AND InvoiceType = @InvoiceType AND FiscalYearID=@FiscalYearID)
                IF @SrNo IS NULL
	                SET @SrNo = 1
                insert into InvoiceDetailItems(
                    InvoiceNo,
                    SrNo,
                    ItemCode,
                    ItemDescription,
                    Quantity,
                    Unit,
                    InvoiceRate,
                    TaxPercent,
                    TaxAmount,
                    DiscountPercent,
                    DiscountAmount,
                    InvoiceValue,
                    CreateUser,
                    CreateDate,
                    UpdateUser,
                    UpdateDate,
                    CompanyID,
                    BranchID,
                    ModuleID,
                    FiscalYearID,
                    InvoiceType)
                    VALUES(
                    @InvoiceNo,
                    @SrNo,
                    @ItemCode,
                    @ItemDescription,
                    @Quantity,
                    @Unit,
                    @InvoiceRate,
                    @TaxPercent,
                    @TaxAmount,
                    @DiscountPercent,
                    @DiscountAmount,
                    @InvoiceValue,
                    @CreateUser,
                    @CreateDate,
                    @UpdateUser,
                    @UpdateDate,
                    @CompanyID,
                    @BranchID,
                    @ModuleID,
                    @FiscalYearID,
                    @InvoiceType) 

                SELECT @SrNo
                        ; ";

            var res = await c.QueryAsync<int>(sqlInsrtInvDtl,
                new
                {
                    dtl.InvoiceNo,                    
                    dtl.ItemCode,
                    dtl.ItemDescription,
                    dtl.Quantity,
                    dtl.Unit,
                    dtl.InvoiceRate,
                    dtl.TaxPercent,
                    dtl.TaxAmount,
                    dtl.DiscountPercent,
                    dtl.DiscountAmount,
                    dtl.InvoiceValue,
                    dtl.CreateUser,
                    dtl.CreateDate,
                    dtl.UpdateUser,
                    dtl.UpdateDate,
                    dtl.CompanyID,
                    dtl.BranchID,
                    dtl.ModuleID,
                    dtl.FiscalYearID,
                    dtl.InvoiceType
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
                    var dtl = model.objInvoiceDetailItems;
                    //int invNo = 0, rows = 0;
                    switch (model.Task)
                    {
                        case "UPDATE_MASTER":
                            return await UpdateInvMaster(c, model.objInvoiceMaster);
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

        public async Task<int> UpdateInvMaster(IDbConnection c, InvoiceMaster mst)
        {
            string sqlUpdateInvDtl = @"
                UPDATE InvoiceMaster               
                SET                
                
                InvoiceDate = ISNULL(@InvoiceDate , InvoiceDate),
                CustomerID = ISNULL(@CustomerID ,CustomerID ),
                CreateUser = ISNULL(@CreateUser , CreateUser),
                CreateDate = ISNULL(@CreateDate , CreateDate),
                UpdateUser = ISNULL(@UpdateUser , UpdateUser),
                UpdateDate = ISNULL(@UpdateDate , UpdateDate),
                ModuleID = ISNULL(@ModuleID , ModuleID),
                TotalAmount = ISNULL(@TotalAmount , TotalAmount),
                DiscountPercent = ISNULL(@DiscountPercent , DiscountPercent),
                DiscountAmount = ISNULL(@DiscountAmount , DiscountAmount),
                SaleTaxPercent = ISNULL(@SaleTaxPercent , SaleTaxPercent),
                SaleTaxAmount = ISNULL(@SaleTaxAmount , SaleTaxAmount),
                NetAmount = ISNULL(@NetAmount , NetAmount),
                ReceivedAmount = ISNULL(@ReceivedAmount , ReceivedAmount),
                BalanceAmount = ISNULL(@BalanceAmount , BalanceAmount),
                OtherTaxPercent = ISNULL(@OtherTaxPercent , OtherTaxPercent),
                OtherTaxAmount = ISNULL(@OtherTaxAmount , OtherTaxAmount),
                CreditLimit = ISNULL(@CreditLimit , CreditLimit),
                ConsumedCredit = ISNULL(@ConsumedCredit , ConsumedCredit),
                BalanceCredit = ISNULL(@BalanceCredit , BalanceCredit),
                Status = ISNULL(@Status , Status),
                BranchID = ISNULL(@BranchID , BranchID),
                Remarks = ISNULL(@Remarks , Remarks)
                WHERE                 
                InvoiceNo = @InvoiceNo
                AND FiscalYearID = @FiscalYearID
                AND InvoiceType = @InvoiceType
                AND CompanyID = @CompanyID

                SELECT @@ROWCOUNT
                        ; ";

            var res = await c.QueryAsync<int>(sqlUpdateInvDtl,
                new
                {
                    mst.InvoiceNo,
                    mst.InvoiceDate,
                    mst.CustomerID,
                    mst.CreateUser,
                    mst.CreateDate,
                    mst.UpdateUser,
                    mst.UpdateDate,
                    mst.CompanyID,
                    mst.ModuleID,
                    mst.TotalAmount,
                    mst.DiscountPercent,
                    mst.DiscountAmount,
                    mst.SaleTaxPercent,
                    mst.SaleTaxAmount,
                    mst.NetAmount,
                    mst.ReceivedAmount,
                    mst.BalanceAmount,
                    mst.InvoiceType,
                    mst.FiscalYearID,
                    mst.OtherTaxPercent,
                    mst.OtherTaxAmount,
                    mst.CreditLimit,
                    mst.ConsumedCredit,
                    mst.BalanceCredit,
                    mst.Status,
                    mst.BranchID,
                    mst.Remarks
                });
            return res.FirstOrDefault();
        }

        public async Task<int> UpdateInvDetail(IDbConnection c, InvoiceDetailItems dtl)
        {
            string sqlUpdateInvDtl = @"
                UPDATE InvoiceDetailItems               
                SET                
                ItemCode = @ItemCode, ItemDescription = @ItemDescription,		Quantity = @Quantity,   Unit = @Unit,	
                InvoiceRate = @InvoiceRate,			InvoiceValue = @InvoiceValue,
                DiscountPercent = @DiscountPercent,	DiscountAmount = @DiscountAmount,		
                TaxPercent = @TaxPercent,   TaxAmount = @TaxAmount
                WHERE                 
                InvoiceNo = @InvoiceNo            
                AND SrNo = @SrNo                
                AND CompanyID = @CompanyID                
                AND FiscalYearID = @FiscalYearID  
                AND InvoiceType = @InvoiceType

                SELECT @@ROWCOUNT
                        ; ";

            var res = await c.QueryAsync<int>(sqlUpdateInvDtl,
                new
                {
                    dtl.ItemCode,
                    dtl.ItemDescription,
                    dtl.Quantity,
                    dtl.Unit,
                    dtl.InvoiceRate,
                    dtl.InvoiceValue,
                    dtl.DiscountPercent,
                    dtl.DiscountAmount,
                    dtl.TaxPercent,
                    dtl.TaxAmount,
                    dtl.SrNo,
                    dtl.InvoiceNo,
                    dtl.CompanyID,
                    dtl.FiscalYearID,
                    dtl.InvoiceType,
                });
            return res.FirstOrDefault();
        }

        public async Task<List<FNN_ITEM_ST>> GetSearchItemsAsync(string query, int companyId)
        {
            try
            {
                return await _repo.WithFNNConnection(async c =>
                {
                    string sqlSearchItems = @"SELECT top 100 * FROM PosItem
                        WHERE isnull(CustomCode, '') + isnull(Description, '') like '%' + @QUERY + '%'
                        AND CompanyID = @COMPANY_ID  ORDER BY Description; ";

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

        public async Task<List<InvoiceMasterListing>> GetInvoicesAsync(string query, int companyId, int limit, int offset)
        {
            try
            {
                return await _repo.WithFNNConnection(async c =>
                {
                    string sql = @"SELECT INV.InvoiceNo, INV.InvoiceDate, INV.CreateUser, USR.FirstName+' '+USR.LastName AS UserName, INV.CompanyID, INV.NetAmount, INV.InvoiceType, INV.FiscalYearID, INV.Status, INV.Remarks, COUNT(ITEM.InvoiceNo) AS NoOfItems
                        FROM InvoiceMaster INV
                        LEFT JOIN Users USR ON INV.CreateUser = USR.UserID
                        LEFT JOIN InvoiceDetailItems ITEM ON ITEM.InvoiceNo = INV.InvoiceNo AND ITEM.CompanyID = INV.CompanyID AND ITEM.FiscalYearID = INV.FiscalYearID AND ITEM.InvoiceType = INV.InvoiceType
                        WHERE INV.CompanyID = @CompanyID
                        GROUP BY ITEM.InvoiceNo, INV.InvoiceNo, INV.InvoiceDate, INV.CreateUser, USR.FirstName+' '+USR.LastName, INV.CompanyID, INV.NetAmount, INV.InvoiceType, INV.FiscalYearID, INV.Status, INV.Remarks
                        ORDER BY INV.InvoiceNo";

                    var searchinvoices = await c.QueryAsync<InvoiceMasterListing>(sql, new { Query = query, CompanyID = companyId });
                    return searchinvoices.ToList();
                });
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<List<InvoiceMasterListing>>(ex).Result;
            }
        }

        public async Task<List<InvoiceDetailItemEditModel>> GetInvoiceDetailsAsync(string InvoiceNo, string InvoiceType , int FiscalYearID, int CompanyID)
        {
            try
            {
                return await _repo.WithFNNConnection(async c =>
                {
                    string sql = @"SELECT InvoiceNo,	SrNo,	ItemCode, itm.Description as	ItemDescription, itm.CustomCode,	Quantity,	Unit,	InvoiceRate,	TaxPercent,	TaxAmount,	DiscountPercent,
                        DiscountAmount,	InvoiceValue,	dtl.CreateUser,	dtl.CreateDate,	dtl.UpdateUser, dtl.UpdateDate,	dtl.CompanyID,	BranchID,	ModuleID,	FiscalYearID,	InvoiceType FROM InvoiceDetailItems dtl
                        left join PosItem itm on dtl.ItemCode = itm.ItemId and dtl.CompanyID = itm.CompanyID
                        WHERE InvoiceNo = @InvoiceNo
                        AND InvoiceType = @InvoiceType
                        AND FiscalYearID = @FiscalYearID
                        AND dtl.CompanyID = @CompanyID
                        ORDER BY SrNo";

                    var searchinvoices = await c.QueryAsync<InvoiceDetailItemEditModel>(sql, new { InvoiceNo, InvoiceType, FiscalYearID, CompanyID });
                    return searchinvoices.ToList();
                });
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<List<InvoiceDetailItemEditModel>>(ex).Result;
            }
        }

        public async Task<int> DeleteInvDetail(InvoiceDetailItems dtl)
        {

            try
            {
                return await _repo.WithFNNConnection(async c =>
                {
                    string sqlDeleteInvDtl = @"
                        DELETE FROM InvoiceDetailItems               
                        WHERE                 
                        InvoiceNo = @InvoiceNo            
                        AND SrNo = @SrNo                
                        AND CompanyID = @CompanyID                
                        AND FiscalYearID = @FiscalYearID  
                        AND InvoiceType = @InvoiceType

                        SELECT @@ROWCOUNT
                        ; ";

                    var res = await c.QueryAsync<int>(sqlDeleteInvDtl,
                        new
                        {
                            dtl.SrNo,
                            dtl.InvoiceNo,
                            dtl.CompanyID,
                            dtl.FiscalYearID,
                            dtl.InvoiceType,
                        });
                    return res.FirstOrDefault();
                });
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<int>(ex).Result;
            }

            
        }

        public async Task<int> DeleteInvoice(InvoiceMaster mst)
        {
            try
            {
                return await _repo.WithFNNConnection(async c =>
                {
                    string sqlDeleteInvDtl = @"
                       BEGIN
	                        DECLARE @STATUS VARCHAR(10) = '';
	                        SELECT @STATUS = Status FROM InvoiceMaster 
	                        WHERE                 
		                        InvoiceNo = @InvoiceNo            	
		                        AND CompanyID = @CompanyID                
		                        AND FiscalYearID = @FiscalYearID  
		                        AND InvoiceType = @InvoiceType	
	
	                        IF @STATUS != 'P'
	                        BEGIN
		                        DELETE FROM InvoiceDetailItems               
		                        WHERE                 
		                        InvoiceNo = @InvoiceNo            
		                        AND CompanyID = @CompanyID                
		                        AND FiscalYearID = @FiscalYearID  
		                        AND InvoiceType = @InvoiceType

		                        DELETE FROM InvoiceMaster
		                        WHERE                 
		                        InvoiceNo = @InvoiceNo            		
		                        AND CompanyID = @CompanyID                
		                        AND FiscalYearID = @FiscalYearID  
		                        AND InvoiceType = @InvoiceType

		                        SELECT @@ROWCOUNT
	                        END
	                        ELSE	
		                        THROW 51000, 'This record is posted. It cannot be deleted', 1;
                        END
                        ; ";

                    var res = await c.QueryAsync<int>(sqlDeleteInvDtl,
                        new
                        {
                            mst.InvoiceNo,
                            mst.CompanyID,
                            mst.FiscalYearID,
                            mst.InvoiceType,
                        });
                    return res.FirstOrDefault();
                });
            }
            catch (Exception ex)
            {
                _log.ExceptionLogFunc(ex);
                return Task.FromException<int>(ex).Result;
            }
        }
        public async Task<int> DeleteInvoicesList(InvoiceMaster[] invoices)
        {
            try
            {
                return await _repo.WithFNNConnection(async c =>
                {
                    string sqlDeleteInvDtl = @"
                       BEGIN
	                        DECLARE @STATUS VARCHAR(10) = '';
	                        SELECT @STATUS = Status FROM InvoiceMaster 
	                        WHERE                 
		                        InvoiceNo = @InvoiceNo            	
		                        AND CompanyID = @CompanyID                
		                        AND FiscalYearID = @FiscalYearID  
		                        AND InvoiceType = @InvoiceType	
	
	                        IF @STATUS != 'P'
	                        BEGIN
		                        DELETE FROM InvoiceDetailItems               
		                        WHERE                 
		                        InvoiceNo = @InvoiceNo            
		                        AND CompanyID = @CompanyID                
		                        AND FiscalYearID = @FiscalYearID  
		                        AND InvoiceType = @InvoiceType

		                        DELETE FROM InvoiceMaster
		                        WHERE                 
		                        InvoiceNo = @InvoiceNo            		
		                        AND CompanyID = @CompanyID                
		                        AND FiscalYearID = @FiscalYearID  
		                        AND InvoiceType = @InvoiceType

		                        SELECT @@ROWCOUNT
	                        END
	                        ELSE	
		                        THROW 51000, 'This record is posted. It cannot be deleted', 1;
                        END
                        ; ";

                    int invDeleted = 0;
                    foreach (InvoiceMaster inv in invoices)
                    {
                        var res = await c.QueryAsync<int>(sqlDeleteInvDtl,
                        new
                        {
                            inv.InvoiceNo,
                            inv.CompanyID,
                            inv.FiscalYearID,
                            inv.InvoiceType,
                        });

                        invDeleted += res.FirstOrDefault();
                    }                    
                    return invDeleted;
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
