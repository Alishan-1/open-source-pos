import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

//import { Car } from '../../models/car';
import { posTrans, posItemRow, posItem, POS, FNN_INV_MST_TR, FNN_INV_DTL_ITEM_TR } from '../../models/posTrans';
import { PosService } from './pos.service';
import { Configuration } from '../../app.constants';
// import { MessageService } from 'primeng/api/messageservice';
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
    POSCode: ""
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
  constructor(private posService: PosService,  private _configuration: Configuration /*private messageService: MessageService*/) { 
    this.DateTimeFormate = Configuration.DateTimeFormate;
  }

  ngOnInit() {
    
    let dt = new Date(); 
    this.form = {
      InvoiceNo: 10000,
      InvoiceDate: dt,
      Cashier: "Usman Waheed",
      POSCode: "02"
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
    GetSearchItems(pramQuery:string) {
      //debugger;        
      
      // this.showLoader = true;
      this.posService.GetSearchItems({
        Query: pramQuery,
        companyId: 50
      }).subscribe(
        sr => {
          //debugger;
          // this.showLoader = false;
          // console.log(sr.Data);
          this.posItemSearch = sr.Data;
            
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
      
      let dbMst:FNN_INV_MST_TR = {INV_NO:0, INV_DATE:mst.InvoiceDate, COMPANY_ID:50, SUB_COMPANY_ID:4 } ;
      dbMst.BUYER_ID = 1;
      dbMst.CREATE_USER = 1;
      dbMst.CREATE_DATE = new Date();      
      dbMst.RT_ID = 'FNN';
      dbMst.INV_AMT = this.txtTotalAmount;
      dbMst.DISCOUNT_PER = 0;
      dbMst.DISCOUNT_AMT = 0;
      dbMst.STAX_PER = 0;
      dbMst.STAX_AMT = 0;
      dbMst.TOTAL_AMT = this.txtTotalAmount;
      dbMst.NET_AMOUNT = this.txtTotalAmount;
      dbMst.INV_TYPE = 'CIS';
      dbMst.fiscal_year_id = 15;
      dbMst.TOT_AMT = this.txtTotalAmount;
      dbMst.linked = 'N';
      dbMst.SALE_TYPE = 'S';
      dbMst.CALC_STYLE = 1;
      dbMst.OTHER_TAX_PER = 0;
      dbMst.OTHER_TAX_AMT = 0;
      dbMst.INV_DUE_DAYS = 0;
      dbMst.Shipto_ID = 1;
      dbMst.ConsumedCredit = 0;
      dbMst.BRANCH_ID =1;
      dbMst.SAL_ACC = 681;
      dbMst.DISC_ACC = 409;
      
      pos.objFNN_INV_MST_TR = dbMst;
      
      
      let dbDtl:FNN_INV_DTL_ITEM_TR = this.MapFNN_INV_DTL_ITEM_TR(dtl,1);

      pos.objFNN_INV_DTL_ITEM_TR = dbDtl;
      // this.showLoader = true;
      this.posService.SavePosTrans(pos).subscribe(
        sr => {
          debugger;
          // this.showLoader = false;
          this.form!.InvoiceNo = sr.Data;
            
        },
        error =>{
          console.error(error);
        },);
    }
    /**
     * saves the detail row of pos entry, this method is used to save the rows after first detail row has been saved with master row.
     * @param InvoiceNo 
     * @param dtl 
     */
    SaveDetailRow(InvoiceNo: number, dtl: posItemRow) {
      let pos:POS = {};
      pos.Task = "SAVE_DETAIL";
      
      
      
      let dbDtl:FNN_INV_DTL_ITEM_TR = this.MapFNN_INV_DTL_ITEM_TR(dtl, InvoiceNo);

      pos.objFNN_INV_DTL_ITEM_TR = dbDtl;
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
      
      
      
      let dbDtl:FNN_INV_DTL_ITEM_TR = this.MapFNN_INV_DTL_ITEM_TR(dtl, InvoiceNo);

      pos.objFNN_INV_DTL_ITEM_TR = dbDtl;
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
    MapFNN_INV_DTL_ITEM_TR( dtl:posItemRow, invNo:number):FNN_INV_DTL_ITEM_TR{
      let dbDtl:FNN_INV_DTL_ITEM_TR ={INV_NO :invNo};
      dbDtl.S_NO = dtl.SrNo;
      dbDtl.ARTICLE_NO = dtl.Description;
      dbDtl.GRADE = 0;
      dbDtl.PCS = 0;
      dbDtl.QTY = dtl.Quantity;
      dbDtl.UNIT = 'Nos';
      dbDtl.DC_RATE = 0;
      dbDtl.INV_RATE = dtl.SalePrice;
      dbDtl.INV_AMOUNT = dtl.Amount;
      dbDtl.CREATE_USER = 1;
      dbDtl.CREATE_DATE = new Date();
      dbDtl.COMPANY_ID = 50;
      dbDtl.SUB_COMPANY_ID = 4;
      dbDtl.RT_ID = 'FNN';
      dbDtl.CARTON_NO = 0;
      dbDtl.COLOUR_DESC = 0;
      dbDtl.DC_NO = 0;
      dbDtl.CONTRACT_NO = 0;
      dbDtl.fiscal_year_id = 15;
      dbDtl.SIDES = 0;
      dbDtl.INV_TYPE = 'CIS';
      dbDtl.SALE_TYPE = 'S';
      dbDtl.DISCOUNT_PER = 0;
      dbDtl.DISCOUNT_AMT = 0;
      dbDtl.SCH_CODE = dtl.ItemId;
      dbDtl.BRANCH_ID = 1;
      dbDtl.EMPLOYEE_ID = 80;
      dbDtl.TAX_PER = 0;
      dbDtl.TAX_AMT = 0;
      dbDtl.EMPLOYEE = 'ZAHID IKRAM';

      return dbDtl;
    }
 }
