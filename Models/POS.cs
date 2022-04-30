using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class POS
    {
        public FNN_INV_MST_TR objFNN_INV_MST_TR { get; set; }
        public FNN_INV_DTL_ITEM_TR objFNN_INV_DTL_ITEM_TR { get; set; }

        public List<FNN_INV_DTL_ITEM_TR> listFNN_INV_DTL_ITEM_TR { get; set; }

        public string Task { get; set; }

    }
    /// <summary>
    /// USED FOR SAVING MASTER FIELDS OF POS
    /// </summary>
    public class FNN_INV_MST_TR
    {
        public int INV_NO { get; set; }

        public DateTime INV_DATE { get; set; }

        public int? BUYER_ID { get; set; }

        public int? CREATE_USER { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public int? UPDATE_USER { get; set; }

        public DateTime? UPDATE_DATE { get; set; }

        public int COMPANY_ID { get; set; }

        public int SUB_COMPANY_ID { get; set; }

        public string RT_ID { get; set; }

        public string REMARKS { get; set; }

        public decimal? INV_AMT { get; set; }

        public decimal? DISCOUNT_PER { get; set; }

        public decimal? DISCOUNT_AMT { get; set; }

        public decimal? STAX_PER { get; set; }

        public decimal? STAX_AMT { get; set; }

        public decimal? TOTAL_AMT { get; set; }

        public decimal? FREIGHT { get; set; }

        public decimal? OTHER_CHGS { get; set; }

        public decimal? NET_AMOUNT { get; set; }

        public string SIZE_DESC { get; set; }

        public string SIZE_UNIT { get; set; }

        public string THICKNESS { get; set; }

        public string INV_TYPE { get; set; }

        public int? fiscal_year_id { get; set; }

        public decimal? TOT_AMT { get; set; }

        public string linked { get; set; }

        public string SALE_TYPE { get; set; }

        public string CALC_STYLE { get; set; }

        public decimal? OTHER_TAX_PER { get; set; }

        public decimal? OTHER_TAX_AMT { get; set; }

        public int? INV_DUE_DAYS { get; set; }

        public int? Shipto_ID { get; set; }

        public decimal? CreditLimit { get; set; }

        public decimal? ConsumedCredit { get; set; }

        public decimal? BalanceCredit { get; set; }

        public string Status { get; set; }

        public int? BRANCH_ID { get; set; }

        public string Opt { get; set; }

        public int? VHR_ENTRY_ID { get; set; }

        public string VOUCHER_NO { get; set; }

        public string VOUCHER_TYPE { get; set; }

        public DateTime? VOUCHER_DATE { get; set; }

        public int? REVZDOC_NO { get; set; }

        public int? ISPOST { get; set; }

        public decimal? BAL_AMOUNT { get; set; }

        public decimal? TAXWH { get; set; }

        public int? SAL_ACC { get; set; }

        public int? TAX_ACC { get; set; }

        public int? DISC_ACC { get; set; }

        public int? TAXEXT_ACC { get; set; }

    }
    /// <summary>
    /// USED FOR SAVING DETAIL FIELDS OF POS
    /// </summary>
    public class FNN_INV_DTL_ITEM_TR
    {
        public int INV_NO { get; set; }

        public int? S_NO { get; set; }

        public string ARTICLE_NO { get; set; }

        public string GRADE { get; set; }

        public int? PCS { get; set; }

        public decimal? QTY { get; set; }

        public string UNIT { get; set; }

        public decimal? DC_RATE { get; set; }

        public decimal? INV_RATE { get; set; }

        public decimal? INV_AMOUNT { get; set; }

        public int? CREATE_USER { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public int? UPDATE_USER { get; set; }

        public DateTime? UPDATE_DATE { get; set; }

        public int? COMPANY_ID { get; set; }

        public int? SUB_COMPANY_ID { get; set; }

        public string RT_ID { get; set; }

        public int? INV_DTL_ITM_SNO { get; set; }

        public string CARTON_NO { get; set; }

        public string COLOUR_DESC { get; set; }

        public string DC_NO { get; set; }

        public string CONTRACT_NO { get; set; }

        public int? fiscal_year_id { get; set; }

        public decimal? RET_PCS { get; set; }

        public decimal? RET_QTY { get; set; }

        public decimal? RET_RATE { get; set; }

        public decimal? RET_AMOUNT { get; set; }

        public decimal? SIDES { get; set; }

        public string RET_INV_NO { get; set; }

        public string INV_TYPE { get; set; }

        public string SALE_TYPE { get; set; }

        public decimal? DISCOUNT_PER { get; set; }

        public decimal? DISCOUNT_AMT { get; set; }

        public string SCH_CODE { get; set; }

        public string PO_NUMBER { get; set; }

        public int? BRANCH_ID { get; set; }

        public string Opt { get; set; }

        public string SALE_ACCOUNT_ID { get; set; }

        public string EMPLOYEE_ID { get; set; }

        public decimal? TAX_PER { get; set; }

        public decimal? TAX_AMT { get; set; }

        public string EMPLOYEE { get; set; }

        public string SALE_ACCOUNT { get; set; }

        public decimal? DISC_PER { get; set; }

        public decimal? DISC_AMT { get; set; }

    }

    public class FNN_ITEM_ST
    {
        public string COMPANY_ID { get; set; }

        public string ITEM_ID { get; set; }

        public string CustomCode { get; set; }

        public string ITEM_DESC { get; set; }

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

        public decimal SaleCost { get; set; }

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

        public string CREATE_USER { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public string UPDATE_USER { get; set; }

        public DateTime? UPDATE_DATE { get; set; }

        public decimal? SUB_COMPANY_ID { get; set; }

        public string RT_ID { get; set; }

        public string type { get; set; }

        public string CatCode { get; set; }

        public int? Branch_ID { get; set; }

    }

    public class posItem
    {
        public string id { get; set; }
        public string customCode { get; set; }
        public string Description { get; set; }
        public decimal SalePrice { get; set; }
        public string ItemId { get; set; }

    }
}
