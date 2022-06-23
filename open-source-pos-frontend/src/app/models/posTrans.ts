export interface posTrans {
    
    InvoiceNo: number,
    InvoiceDate: Date,
    Cashier: string,
    POSCode: string
    
}

export interface posItemRow {
    SrNo: number;
    id: string;
    customCode:string;
    ItemId?:string;
    Description:string;
    Quantity:number;
    SalePrice:number;
    Amount:number;
    
}

export interface  posItem {
    
    id: string;
    customCode:string;
    ItemId?:string;
    Description:string;
    SalePrice:number;
    
    
}


export interface POS {
    objFNN_INV_MST_TR?: FNN_INV_MST_TR;
    objFNN_INV_DTL_ITEM_TR?: FNN_INV_DTL_ITEM_TR;
    listFNN_INV_DTL_ITEM_TR?: FNN_INV_DTL_ITEM_TR[];
    Task?: string;
}

export interface FNN_INV_MST_TR {
    INV_NO: number;
    INV_DATE: Date;
    BUYER_ID?: any;
    CREATE_USER?: any;
    CREATE_DATE?: any;
    UPDATE_USER?: any;
    UPDATE_DATE?: any;
    COMPANY_ID: number;
    SUB_COMPANY_ID: number;
    RT_ID?: any;
    REMARKS?: any;
    INV_AMT?: any;
    DISCOUNT_PER?: any;
    DISCOUNT_AMT?: any;
    STAX_PER?: any;
    STAX_AMT?: any;
    TOTAL_AMT?: any;
    FREIGHT?: any;
    OTHER_CHGS?: any;
    NET_AMOUNT?: any;
    SIZE_DESC?: any;
    SIZE_UNIT?: any;
    THICKNESS?: any;
    INV_TYPE?: any;
    fiscal_year_id?: any;
    TOT_AMT?: any;
    linked?: any;
    SALE_TYPE?: any;
    CALC_STYLE?: any;
    OTHER_TAX_PER?: any;
    OTHER_TAX_AMT?: any;
    INV_DUE_DAYS?: any;
    Shipto_ID?: any;
    CreditLimit?: any;
    ConsumedCredit?: any;
    BalanceCredit?: any;
    Status?: any;
    BRANCH_ID?: any;
    Opt?: any;
    VHR_ENTRY_ID?: any;
    VOUCHER_NO?: any;
    VOUCHER_TYPE?: any;
    VOUCHER_DATE?: any;
    REVZDOC_NO?: any;
    ISPOST?: any;
    BAL_AMOUNT?: any;
    TAXWH?: any;
    SAL_ACC?: any;
    TAX_ACC?: any;
    DISC_ACC?: any;
    TAXEXT_ACC?: any;
}
export interface FNN_INV_DTL_ITEM_TR {
    INV_NO: number;
    S_NO?: any;
    ARTICLE_NO?: any;
    GRADE?: any;
    PCS?: any;
    QTY?: any;
    UNIT?: any;
    DC_RATE?: any;
    INV_RATE?: any;
    INV_AMOUNT?: any;
    CREATE_USER?: any;
    CREATE_DATE?: any;
    UPDATE_USER?: any;
    UPDATE_DATE?: any;
    COMPANY_ID?: any;
    SUB_COMPANY_ID?: any;
    RT_ID?: any;
    INV_DTL_ITM_SNO?: any;
    CARTON_NO?: any;
    COLOUR_DESC?: any;
    DC_NO?: any;
    CONTRACT_NO?: any;
    fiscal_year_id?: any;
    RET_PCS?: any;
    RET_QTY?: any;
    RET_RATE?: any;
    RET_AMOUNT?: any;
    SIDES?: any;
    RET_INV_NO?: any;
    INV_TYPE?: any;
    SALE_TYPE?: any;
    DISCOUNT_PER?: any;
    DISCOUNT_AMT?: any;
    SCH_CODE?: any;
    PO_NUMBER?: any;
    BRANCH_ID?: any;
    Opt?: any;
    SALE_ACCOUNT_ID?: any;
    EMPLOYEE_ID?: any;
    TAX_PER?: any;
    TAX_AMT?: any;
    EMPLOYEE?: any;
    SALE_ACCOUNT?: any;
    DISC_PER?: any;
    DISC_AMT?: any;
}