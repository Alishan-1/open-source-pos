using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class POS
    {
        public InvoiceMaster objInvoiceMaster { get; set; }
        public InvoiceDetailItems objInvoiceDetailItems { get; set; }

        public List<InvoiceDetailItems> listInvoiceDetailItems { get; set; }

        public string Task { get; set; }

    }
    /// <summary>
    /// USED FOR SAVING MASTER FIELDS OF POS
    /// </summary>
    public class InvoiceMaster
    {
        public int InvoiceNo { get; set; }

        public DateTime InvoiceDate { get; set; }

        public int? CustomerID { get; set; }

        public int? CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int CompanyID { get; set; }
        public string ModuleID { get; set; }
        public decimal? TotalAmount { get; set; }

        public decimal? DiscountPercent { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? SaleTaxPercent { get; set; }

        public decimal? SaleTaxAmount { get; set; }

        public decimal? NetAmount { get; set; }

        public decimal? ReceivedAmount { get; set; }

        public decimal? BalanceAmount { get; set; }
        public string InvoiceType { get; set; }

        public int? FiscalYearID { get; set; }

        public decimal? OtherTaxPercent { get; set; }
        public decimal? OtherTaxAmount { get; set; }
        public decimal? CreditLimit { get; set; }

        public decimal? ConsumedCredit { get; set; }

        public decimal? BalanceCredit { get; set; }

        public string Status { get; set; }

        public int? BranchID { get; set; }

        public string Remarks { get; set; }

    }
    /// <summary>
    /// USED FOR SAVING DETAIL FIELDS OF POS
    /// </summary>
    public class InvoiceDetailItems
    {
        public int InvoiceNo { get; set; }

        public int? SrNo { get; set; }
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public decimal? Quantity { get; set; }

        public string Unit { get; set; }
        public decimal? InvoiceRate { get; set; }

        public decimal? TaxPercent { get; set; }

        public decimal? TaxAmount { get; set; }
        public decimal? DiscountPercent { get; set; }

        public decimal? DiscountAmount { get; set; }
        public decimal? InvoiceValue { get; set; }

        public int? CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? CompanyID { get; set; }

        public int? BranchID { get; set; }

        public string ModuleID { get; set; }

        public int? FiscalYearID { get; set; }

        public string InvoiceType { get; set; }
    }

    public class FNN_ITEM_ST
    {
        public string CompanyID { get; set; }

        public string ItemId { get; set; }

        public string CustomCode { get; set; }

        public string Description { get; set; }

        public string ShortDesc { get; set; }

        public string GenericDesc { get; set; }

        public string ClassID { get; set; }

        public string ITEM_CATEGORY { get; set; }

        public string PackCode { get; set; }

        public string Manucode { get; set; }

        public int ItemType { get; set; }

        public int ValuationMethod { get; set; }

        public string MCode { get; set; }

        public int SaleTaxOption { get; set; }

        public string SaleTaxCode { get; set; }

        public int PurchaseTaxOption { get; set; }

        public string PurchaseTaxCode { get; set; }

        public string SiteId { get; set; }

        public string BinID { get; set; }

        public decimal PurchaseCost { get; set; }

        public decimal SalePrice { get; set; }

        public decimal Sed { get; set; }

        public decimal MaxSaleQty { get; set; }

        public int Itype { get; set; }

        public int ReorderQty { get; set; }

        public decimal AvgRate { get; set; }

        public decimal AvgRate1 { get; set; }

        public decimal SaleDiscPerc { get; set; }

        public int PriceDescCStatus { get; set; }

        public int QtyCStatus { get; set; }

        public decimal DAvgRate { get; set; }

        public decimal? discper { get; set; }

        public decimal? discamt { get; set; }

        public decimal? StockS { get; set; }

        public decimal? StockG { get; set; }

        public decimal? packqty { get; set; }

        public decimal? packdisc { get; set; }

        public int? QtyAnAllow { get; set; }

        public string TmpFld1 { get; set; }

        public string TmpFld2 { get; set; }

        public decimal? FxdPer { get; set; }

        public decimal? PCost { get; set; }

        public decimal? RoundCost { get; set; }

        public DateTime? AddDateTime { get; set; }

        public DateTime? EditDateTime { get; set; }

        public decimal? LastDiscAmt { get; set; }

        public string STATUS { get; set; }

        public string REMARKS { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        public decimal? SUB_COMPANY_ID { get; set; }

        public string RT_ID { get; set; }

        public string type { get; set; }

        public string CatCode { get; set; }

        public int? Branch_ID { get; set; }

    }

    public class PosItem
    {
        public string Id { get; set; }
        public string CustomCode { get; set; }
        public string Description { get; set; }
        public decimal SalePrice { get; set; }
        public string ItemId { get; set; }
        public int CompanyID { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }


    }

    public class GetPosItemsModel
    {        
        public int Count { get; set; }
        public List<PosItem> Items { get; set; }
    }
    public class GetInvoicesModel
    {
        public int Count { get; set; }
        public List<InvoiceMaster> Items { get; set; }
    }
}
