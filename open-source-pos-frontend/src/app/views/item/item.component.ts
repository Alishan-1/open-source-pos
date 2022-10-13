import { Component, OnInit } from '@angular/core';

import { posItem } from '../../models/posTrans';

import { ConfirmationService } from 'primeng/api';
import { MessageService } from 'primeng/api';
import { ItemService } from './item.service';
import { AuthService } from '../login/auth.service';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.scss'],
  styles: [`
        :host ::ng-deep .p-dialog .product-image {
            width: 150px;
            margin: 0 auto 2rem auto;
            display: block;
        }
    `],
    providers: [MessageService,ConfirmationService]
})
export class ItemComponent implements OnInit {



  public itemFormValidated: boolean = false;
  public isRequestProcessing: boolean = false;
  public isFormSubmitted: boolean = false;
  /**Is the item is being updated or created */
  isEditing:boolean = false;



  productDialog!: boolean;  

  items!: posItem[];
  totalItems!: number;  

  item!: posItem;
  selectedItems!: posItem[];  
  currentUser:any;

  constructor(private messageService: MessageService, 
    private confirmationService: ConfirmationService, private itemService: ItemService,
    private authService: AuthService, private activatedroute: ActivatedRoute) {
      this.currentUser = this.authService.GetlocalStorageUser();
    }

  ngOnInit() {
      debugger;
      let data = this.activatedroute.snapshot.routeConfig?.path;
      let params = {
        query: '',
        companyId: this.currentUser.CompanyID,
        limit:0,
        offset:0
      };
      this.itemService.GetItems(params).subscribe({
        next: (sr) => {
          debugger;
            
          this.items = sr.Data.Items;
          this.totalItems = sr.Data.Count;          
            
        },
        error:(error) =>{
          debugger;          
          console.error(error);
          this.messageService.add({severity:'error', summary: 'Error Loading Items!', detail: error, life: 3000});
        }});

        if(data && data == "items/new"){
          this.openNew();
        }
      
  }

  openNew() {
    this.isEditing = false;
      this.item = {
        CustomCode:"",
        Description:"",
        Id:"0",                
      };
      
      this.productDialog = true;
  }

  deleteSelectedItems() {
      this.confirmationService.confirm({
          message: 'You are not allowed to delete the selected Items',
          header: 'Not Allowed',
          icon: 'pi pi-exclamation-triangle',
          
      });
  }

  
  editProduct(item: posItem) {
      this.item = {...item};
      this.productDialog = true;
      this.isEditing = true;
  }

  deleteProduct(item: posItem) {
      this.confirmationService.confirm({
          message: 'You are not allowed to delete ' + item.Description + '?',
          header: 'Not Allowed',
          icon: 'pi pi-exclamation-triangle',
          
      });
  }

  hideDialog() {
      this.productDialog = false;
      
  }

  SaveItem() {
    debugger;
    this.itemFormValidated = true;
    this.isFormSubmitted = true;
    this.isRequestProcessing = true;    
    this.item.CompanyID =  this.currentUser.CompanyID;

    if(this.isEditing){
      this.item.UpdateUser =  this.currentUser.UserID;  
      this.itemService.UpdateItem(this.item).subscribe({
        next: (sr) => {
          debugger;
          this.isRequestProcessing = false;
          this.itemFormValidated = false;
          this.items[this.findIndexById(this.item.ItemId!)] = this.item;

          this.messageService.add({severity:'success', summary: 'Successful', detail: 'Product Updated', life: 3000});
          this.productDialog = false;
            
        },
        error:(error) =>{
          debugger;
          this.isRequestProcessing = false;
          console.error(error);
          this.messageService.add({severity:'error', summary: 'error', detail: error, life: 3000});
        }});
    }
    else{
      this.item.CreateUser =  this.currentUser.UserID;
      // this.showLoader = true;
      this.itemService.SaveItem(this.item).subscribe({
        next: (sr) => {
          debugger;
          this.isRequestProcessing = false;
          this.itemFormValidated = false;
          // this.showLoader = false;        
          this.item.ItemId = sr.Data;
          this.items.push(this.item);
          this.messageService.add({severity:'success', summary: 'Successful', detail: 'Product Created', life: 3000});
          this.productDialog = false;
            
        },
        error:(error) =>{
          debugger;
          this.isRequestProcessing = false;
          console.error(error);
          this.messageService.add({severity:'error', summary: 'error', detail: error, life: 3000});
        }});
    }
    this.items = [...this.items];
    
    this.item = {
      CustomCode:"",
      Description:"",
      Id:"0",                
    };
}  

  findIndexById(id: string): number {
      let index = -1;
      for (let i = 0; i < this.items.length; i++) {
          if (this.items[i].ItemId === id) {
              index = i;
              break;
          }
      }

      return index;
  }
}
