import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

//import { Car } from '../../models/car';
import { posTrans, posItemRow, posItem, POS, InvoiceMaster, InvoiceDetailItems } from '../../models/posTrans';
import { PosService } from './pos.service';
import { Configuration } from '../../app.constants';
// import { MessageService } from 'primeng/api/messageservice';
import { AuthService } from '../login/auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-dashboard',
  templateUrl: 'pos.component.html',
})
export class PosComponent  implements OnInit  {
  disabled:any = true;
  //clonedCars: { [s: string]: Car; } = {};
  @ViewChild('hrBeforeDtl') hrBeforeDtlRef: ElementRef | undefined;  

  posItemRows: posItemRow[] =  [];  
  form: posTrans  = {
    InvoiceNo: 0,
    InvoiceDate: new Date(),
    Cashier: "",
    POSCode: "",
    ModuleID: '',
    CompanyID: 0,
    InvoiceType: '',
    FiscalYearID: '',
    BranchID: 0
  };  
  displayItemSearchModal:boolean = false;  
  displayInvoiceTotalModal:boolean = false;  
  posItemSearch:posItem[] = [];  
  selectedItemFromSearch:posItem = {
    id: "",
    customCode: "",
    ItemId: "",
    Description: "",
    SalePrice: 0
  };  
  SelectedIndexOfDtl:number = 0;  
  txtTotalAmount:number = 0;  
  modelTotalAmount: number = 0;  
  discountPercnt: number = 0;  
  netAmount: number = 0;  
  receiveAmount: number = 0 ;  
  balanceAmount: number = 0;  
  posItemSearchKeyUptime:number = 0;  
  posItemSearchKeyUpPrevTime:number = 0;  
  txtSearch: string = "";  
  DateTimeFormate:string = "";
  currentUser:any;  
  constructor(private authService: AuthService,private posService: PosService, private router: Router, private _configuration: Configuration /*private messageService: MessageService*/) { 
    this.DateTimeFormate = Configuration.DateTimeFormate;
    this.currentUser = this.authService.GetlocalStorageUser();
  }

    ngOnInit() {
      this.RefreshForm();    
    }
    
    RefreshForm(){
      let dt = new Date();
     
      this.form = {
        InvoiceNo: 10000,
        InvoiceDate: dt,
        Cashier: `${this.currentUser.FirstName} ${this.currentUser.LastName}` ,
        POSCode: "02",
        ModuleID: 'POS',
        CompanyID: this.currentUser.CompanyID,
        InvoiceType: 'INV',
        FiscalYearID: this.currentUser.FiscalYearID,
        BranchID: 1
      }
      this.posItemRows= [
        {
          SrNo:1,
          id:"1",
          Description: "",
          Amount:0,
          Quantity:1,
          SalePrice:0,
          customCode:""
        },
        
      ]
  
         
        this.displayItemSearchModal = false;
        this.receiveAmount = 0;
        this.discountPercnt = 0;
        this.posItemSearchKeyUptime = new Date().getTime();
        this.posItemSearchKeyUpPrevTime = 0;
        this.txtTotalAmount = 0;
    }
  
    
    onRowEditInit(car: any) {
        //this.clonedCars[car.vin] = {...car};
    }

    onRowEditSave(car: any) {
        if (car.year > 0) {
            //delete this.clonedCars[car.vin];
            //this.messageService.add({severity:'success', summary: 'Success', detail:'Car is updated'});
        }
        else {
            //this.messageService.add({severity:'error', summary: 'Error', detail:'Year is required'});
        }
    }

    // onRowEditCancel(car: Car, index: number) {
    //     this.cars2[index] = this.clonedCars[car.vin];
    //     delete this.clonedCars[car.vin];
    // }


    /**
     *  on enter key press in custom code field
     *  */
    onKeydown($event: any, rowData: posItemRow, rowIndex:number){
      // console.log($event);
      // console.log(rowData);
      // console.log(rowIndex);
      this.SelectedIndexOfDtl = rowIndex;
      if(rowData.customCode.length <= 0)    
      {
        this.GetSearchItems("");
        
        this.txtSearch = "";
        this.displayItemSearchModal = true;
      }
      else{
        //todo:load item against custome code
        let code = rowData.customCode;
      }

    }

    onKeyupQty($event: any, rowData: posItemRow, rowIndex: number){
      
      rowData.Amount = rowData.Quantity * rowData.SalePrice;
      this.CalculateTotalAmount();
    }

    onBlurQtyAndRate($event: any, rowData: posItemRow, rowIndex: number){
      debugger;
      rowData.Amount = rowData.Quantity * rowData.SalePrice;
      this.CalculateTotalAmount();
      this.UpdateDetailRow(this.form?.InvoiceNo || 0, rowData)
    }

    BtnReceiveAmountClick($event: any){
      this.modelTotalAmount = this.txtTotalAmount;
      this.CalculateReceiveAmountValues(null);
      this.displayInvoiceTotalModal = true;
    }
    onPosItemRowSelect($event: any){
      // console.log(this.selectedItemFromSearch);
      // this.SelectedIndexOfDtl
      let oldItem = this.posItemRows![this.SelectedIndexOfDtl!];
      let selItem = this.selectedItemFromSearch;
      this.posItemRows![this.SelectedIndexOfDtl!] = {
        id:oldItem.id,
        SrNo:oldItem.SrNo,
        customCode: selItem?.customCode || "",
        Description: selItem?.Description || "",
        SalePrice: selItem?.SalePrice || 0,
        Quantity: 1,
        Amount: selItem?.SalePrice || 0,
        ItemId:selItem?.ItemId,
      }
      this.displayItemSearchModal = false;
      debugger;
      //save data to db. 
      if(this.posItemRows!.length <= 1){
        //save master and detail
        this.SaveFirstRow(this.form!, this.posItemRows![this.SelectedIndexOfDtl!]);
      }  
      else{
        // save detail only
        this.SaveDetailRow( this.form!.InvoiceNo, this.posItemRows![this.SelectedIndexOfDtl!]);
      }
      this.AddNewRowToDetail();
    }
  
    
    onPosItemSearchKeyUp($event:any){
      // debugger;
      this.posItemSearchKeyUptime = new Date().getTime();
      //get new items after 1.0 sec
      if(this.posItemSearchKeyUptime - this.posItemSearchKeyUpPrevTime! >= 1000){
        this.posItemSearchKeyUpPrevTime = this.posItemSearchKeyUptime;
        this.posItemSearchKeyUptime = new Date().getTime();
        this.GetSearchItems(this.txtSearch!);
      }  
      
    }

    public AddNewRowToDetail(){
      let lastItem = this.posItemRows![this.posItemRows!.length -1]
      
      let newItemRow: posItemRow =  {
        SrNo:lastItem.SrNo + 1,
        id:(lastItem.SrNo + 1).toString(),
        Description: "",
        Amount:0,
        Quantity:0,
        SalePrice:0,
        customCode:""
      }
      this.posItemRows?.push(newItemRow);
      setTimeout(()=>{ // this will make the execution after the above boolean has changed
        
        // hrBeforeDtlRef is the refrence to the hr element above the p-table component 
        //now we are following the path from hr to the required cell to set focus.
        /** this.hrBeforeDtlRef!.nativeElement.nextSibling.nextSibling.firstElementChild
          .firstElementChild.firstElementChild.tBodies[0]
          .rows[this.posItemRows!.length -1].cells[1].click(); */
        
        this.hrBeforeDtlRef!.nativeElement.nextSibling.firstElementChild
          .firstElementChild.firstElementChild.tBodies[0]
          .rows[this.posItemRows!.length -1].cells[1].click();
      },0);  
      this.CalculateTotalAmount();
    }

    CalculateTotalAmount(){
      this.txtTotalAmount = 0;
      for(let row of this.posItemRows!){
        this.txtTotalAmount += row.Amount
      }
    }

    CalculateReceiveAmountValues($event: any){


      this.netAmount =  this.modelTotalAmount! - ( this.modelTotalAmount! * this.discountPercnt! / 100);
      
      this.balanceAmount = this.receiveAmount! - this.netAmount;
    }

    onSaleComplete($event: any){      
      this.displayInvoiceTotalModal = false;
      debugger;
      let pos:POS = {};
      pos.Task = "UPDATE_MASTER";

      //save data to db. 
      let dbMst:InvoiceMaster = {
        InvoiceNo: this.form.InvoiceNo,
        InvoiceDate: this.form.InvoiceDate,
        CompanyID: this.form.CompanyID,
        ModuleID: this.form.ModuleID,
        TotalAmount: this.modelTotalAmount,
        DiscountPercent: this.discountPercnt,
        DiscountAmount: ( this.modelTotalAmount! * this.discountPercnt! / 100),
        SaleTaxPercent: 0,
        SaleTaxAmount: 0,
        NetAmount: this.netAmount,
        ReceivedAmount: this.receiveAmount,
        BalanceAmount: this.balanceAmount,
        InvoiceType: this.form.InvoiceType,
        FiscalYearID: this.form.FiscalYearID,
        OtherTaxPercent: 0,
        OtherTaxAmount: 0,
        CreditLimit: 0,
        ConsumedCredit: 0,
        BalanceCredit: 0,
        Status: 'P',
        BranchID: 1
      }
      pos.objInvoiceMaster = dbMst;

      // this.showLoader = true;
      this.posService.UpdatePosTrans(pos).subscribe({
        next: sr => {
          debugger;
          // this.showLoader = false;
          console.log( sr.Data);
          
          this.RefreshForm();
            
        },
        error:error =>{
        
          console.error(error);
        
        }});
    }

    NoItemsFound = true;
    GetSearchItems(pramQuery:string) {
      //debugger;        
      
      // this.showLoader = true;
      this.posService.GetSearchItems({
        Query: pramQuery,
        companyId: this.form.CompanyID
      }).subscribe(
        sr => {
          debugger;
          // this.showLoader = false;
          // console.log(sr.Data);
          this.posItemSearch = sr.Data;
          this.NoItemsFound = true;
          if(this.posItemSearch.length > 0 ){
            this.NoItemsFound = false;
          }
            
        });
    }
   /**
   * SAVE_MASSTER_WITH_DETAIL
   * @param mst 
   * @param dtl 
   */
    SaveFirstRow(mst:posTrans, dtl:posItemRow){      
      let pos:POS = {};
      pos.Task = "SAVE_MASSTER_WITH_DETAIL";
      
      let dbMst:InvoiceMaster = {InvoiceNo:0, InvoiceDate:mst.InvoiceDate, CompanyID:this.form.CompanyID, } ;
      dbMst.CustomerID = 1;
      dbMst.CreateUser = this.currentUser.UserID;
      dbMst.CreateDate = new Date();      
      dbMst.ModuleID = this.form.ModuleID;
      dbMst.TotalAmount = this.txtTotalAmount;
      dbMst.DiscountPercent = 0;
      dbMst.DiscountAmount = 0;
      dbMst.SaleTaxPercent = 0;
      dbMst.SaleTaxAmount = 0;
      dbMst.NetAmount = this.txtTotalAmount;
      dbMst.InvoiceType = this.form.InvoiceType;
      dbMst.FiscalYearID = this.form.FiscalYearID;
      dbMst.OtherTaxPercent = 0;
      dbMst.OtherTaxAmount = 0;
      dbMst.ConsumedCredit = 0;
      dbMst.BranchID = this.form.BranchID;      
      dbMst.Status = 'N';
      pos.objInvoiceMaster = dbMst;
      
      
      let dbDtl:InvoiceDetailItems = this.MapInvoiceDetailItems(dtl,1);

      pos.objInvoiceDetailItems = dbDtl;
      // this.showLoader = true;
      this.posService.SavePosTrans(pos).subscribe({
        next: (sr) => {
          debugger;
          // this.showLoader = false;
          this.form!.InvoiceNo = sr.Data;
            
        },
        error:(error) =>{
          console.error(error);
        }});
    }
    /**
     * saves the detail row of pos entry, this method is used to save the rows after first detail row has been saved with master row.
     * @param InvoiceNo 
     * @param dtl 
     */
    SaveDetailRow(InvoiceNo: number, dtl: posItemRow) {
      let pos:POS = {};
      pos.Task = "SAVE_DETAIL";
      
      
      
      let dbDtl:InvoiceDetailItems = this.MapInvoiceDetailItems(dtl, InvoiceNo);

      pos.objInvoiceDetailItems = dbDtl;
      // this.showLoader = true;
      this.posService.SavePosTrans(pos).subscribe(
        sr => {
          debugger;
          // this.showLoader = false;
          dtl.SrNo = sr.Data;
            
        },
        error =>{
          console.error(error);
        },);
    }
    
    UpdateDetailRow(InvoiceNo: number, dtl: posItemRow) {
      let pos:POS = {};
      pos.Task = "UPDATE_DETAIL";
      
      
      
      let dbDtl:InvoiceDetailItems = this.MapInvoiceDetailItems(dtl, InvoiceNo);

      pos.objInvoiceDetailItems = dbDtl;
      // this.showLoader = true;
      this.posService.UpdatePosTrans(pos).subscribe(
        sr => {
          debugger;
          // this.showLoader = false;
          console.log( sr.Data);
            
        },
        error =>{
          console.error(error);
        },);
    }
    MapInvoiceDetailItems( dtl:posItemRow, invNo:number):InvoiceDetailItems{
      let dbDtl:InvoiceDetailItems ={InvoiceNo :invNo};
      dbDtl.SrNo = dtl.SrNo;
      dbDtl.ItemCode = dtl.ItemId;
      dbDtl.ItemDescription = dtl.Description;
      dbDtl.Quantity = dtl.Quantity;
      dbDtl.Unit = 'Nos';
      dbDtl.InvoiceRate = dtl.SalePrice;
      dbDtl.InvoiceValue = dtl.Amount;
      dbDtl.CreateUser = this.currentUser.UserID;
      dbDtl.CreateDate = new Date();
      dbDtl.CompanyID = this.form.CompanyID;
      dbDtl.ModuleID = this.form.ModuleID;
      dbDtl.FiscalYearID = this.form.FiscalYearID;
      dbDtl.InvoiceType = this.form.InvoiceType;
      dbDtl.DiscountPercent = 0;
      dbDtl.DiscountAmount = 0;
      dbDtl.BranchID = 1;
      dbDtl.TaxPercent = 0;
      dbDtl.TaxAmount = 0;
      return dbDtl;
    }
    onCloseClick(event:any){
      this.router.navigate([`/`]);
    }
 }
