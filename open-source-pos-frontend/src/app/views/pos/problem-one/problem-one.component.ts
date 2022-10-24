import { Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-problem-one',
  templateUrl: './problem-one.component.html',  
})
export class ProblemOneComponent implements OnInit {

  posItemRows: any[] =  [];  
  
  displayItemSearchModal:boolean = false;  
  posItemSearch:any[] = 
  [
    {
      "Id": "23",
      "CustomCode": "900078612",
      "Description": "Mutton",      
      "ItemId": "23",      
    },
    {
      "Id": "11",
      "CustomCode": "115866",
      "Description": "biscuits",      
      "ItemId": "11",      
    },
    {
      "Id": "5",
      "CustomCode": "78963214",
      "Description": "Blue T-Shirt",      
      "ItemId": "5",      
    }
  ]  ;

  selectedItemFromSearch:any = {
    Id: "",
    CustomCode: "",
    ItemId: "",
    Description: "",
    SalePrice: 0
  };  
  SelectedIndexOfDtl:number = 0;  
  

    /**Is the item is being updated or created */
  @Input()  isEditing:boolean = false;
  @Input()  showInv!: any;
  constructor() { 
   
   
  }

  ngOnInit() {      
    
      this.RefreshForm();      
    
  }
    
  RefreshForm(){      
    this.posItemRows= [
      {
        SrNo:1,
        id:"1",
        Description: "",
        customCode:"",
      },
    ];         
    this.displayItemSearchModal = false;
  }
    /**
     *  on enter key press in custom code field
     *  */
    onKeydown( rowData: any, rowIndex:number){
    debugger;      
      this.SelectedIndexOfDtl = rowIndex;
      if(rowData.customCode.length <= 0)    
      {        
        this.displayItemSearchModal = true;
      }
    }
    onSearchClick( rowData: any, rowIndex:number){
      debugger;      
        this.SelectedIndexOfDtl = rowIndex;
        if(rowData.customCode.length <= 0)    
        {        
          this.displayItemSearchModal = true;
        }
      }
    onPosItemRowSelect(){
      debugger;
      // let oldItem = this.posItemRows![this.SelectedIndexOfDtl!];
      let selItem = this.selectedItemFromSearch;
      // this.posItemRows![this.SelectedIndexOfDtl!] = {
      //   id: oldItem.id,
      //   SrNo: oldItem.SrNo,
      //   customCode: selItem?.CustomCode || "",
      //   Description: selItem?.Description || "",
      //   ItemId: selItem?.ItemId,
      // }

      // this.posItemRows![this.SelectedIndexOfDtl!].id = oldItem.id;
      // this.posItemRows![this.SelectedIndexOfDtl!].SrNo = oldItem.SrNo;
      this.posItemRows![this.SelectedIndexOfDtl!].customCode = selItem?.CustomCode || "";
      this.posItemRows![this.SelectedIndexOfDtl!].Description = selItem?.Description || "";
      this.posItemRows![this.SelectedIndexOfDtl!].ItemId = selItem?.ItemId;

      this.displayItemSearchModal = false;
    }
 }
