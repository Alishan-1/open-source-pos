export interface posTrans {
    
    InvoiceNo: number,
    InvoiceDate: Date,
    Cashier: string,
    POSCode: string,
    ModuleID: string,
    CompanyID: number,
    InvoiceType: string,
    FiscalYearID: string
    BranchID: number
    
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
    /**
     * is the row has already been inserted in the database
     */
    IsInserted:boolean;
}

export interface  posItem {
    
    Id: string;
    CustomCode:string;
    /**main primary kay of item */
    ItemId?:string;
    Description:string;
    SalePrice?:number;
    CompanyID?:number;
    CreateUser?:number;
    UpdateUser?:number;
    
}


export interface POS {
    objInvoiceMaster?: InvoiceMaster;
    objInvoiceDetailItems?: InvoiceDetailItems;
    listInvoiceDetailItems?: InvoiceDetailItems[];
    Task?: string;
}

export interface InvoiceMaster {
    InvoiceNo: number;
    InvoiceDate: Date;
    CustomerID?: any;
    CreateUser?: any;
    CreateDate?: any;
    UpdateUser?: any;
    UpdateDate?: any;
    CompanyID: number;
    ModuleID?: any;
    TotalAmount?: any;
    DiscountPercent?: any;
    DiscountAmount?: any;
    SaleTaxPercent?: any;
    SaleTaxAmount?: any;
    NetAmount?: any;
    ReceivedAmount?: any;
    BalanceAmount?:any;
    InvoiceType?: any;
    FiscalYearID?: any;
    OtherTaxPercent?: any;
    OtherTaxAmount?: any;
    CreditLimit?: any;
    ConsumedCredit?: any;
    BalanceCredit?: any;
    Status?: any;
    BranchID?: any;
    Remarks?: any;
}
export interface InvoiceDetailItems {
    InvoiceNo: number;
    SrNo?: number;
    ItemCode?: string;
    ItemDescription?: string;
    Quantity?: number;
    Unit?: string;
    InvoiceRate?: number;
    TaxPercent?: number;
    TaxAmount?: number;
    DiscountPercent?: number;
    DiscountAmount?: number;
    InvoiceValue?: number;
    CreateUser?: number;
    CreateDate?: Date;
    UpdateUser?: number;
    UpdateDate?: Date;
    CompanyID?: number;
    BranchID?: number;
    ModuleID?: string;
    FiscalYearID?: string;
    InvoiceType?: string;
}


export interface InvoiceMasterListing  extends InvoiceMaster{
    UserName?: string;
    NoOfItems?: number;
}

export interface InvoiceDetailItemEditModel  extends InvoiceDetailItems{
    CustomCode?: string;
}