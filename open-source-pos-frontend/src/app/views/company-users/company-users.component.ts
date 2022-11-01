import { Component, OnInit } from '@angular/core';

import { posItem } from '../../models/posTrans';
import { User } from '../../models/user.model'
import { ConfirmationService } from 'primeng/api';
import { MessageService } from 'primeng/api';
import { ItemService } from '../item/item.service';

import { RegistrationServic } from '../../services/registration.services';

import { AuthService } from '../login/auth.service';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';

@Component({
  selector: 'app-company-users',
  templateUrl: './company-users.component.html',
  styleUrls: ['./company-users.component.scss'],
  providers: [MessageService,ConfirmationService]
})
export class CompanyUsersComponent implements OnInit {



  public itemFormValidated: boolean = false;
  public isRequestProcessing: boolean = false;
  public isFormSubmitted: boolean = false;
  /**Is the item is being updated or created */
  isEditing:boolean = false;



  productDialog!: boolean;  

  /** remove these two */
  items!: posItem[];
  totalItems!: number;  
  
  users!: User[];
  totalUsers!: number;  


/** remove these two */
  item!: posItem;
  selectedItems!: posItem[];  
  
  xUser!: User;
  selectedUsers!: User[];  
  
  currentUser:any;

  constructor(private messageService: MessageService, 
    private confirmationService: ConfirmationService, private itemService: ItemService, private _registrationServic: RegistrationServic,
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
      this._registrationServic.GetCompanyUsers(params).subscribe({
        next: (sr) => {
          debugger;
            
          this.users = sr.Data.Users;
          this.totalUsers = sr.Data.Count;          
            
        },
        error:(error) =>{
          debugger;          
          console.error(error);
          this.messageService.add({severity:'error', summary: 'Error Loading Users!', detail: error, life: 3000});
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
          this.item = {
            CustomCode:"",
            Description:"",
            Id:"0",                
          };
          this.items = [...this.items];
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
          this.item = {
            CustomCode:"",
            Description:"",
            Id:"0",                
          };
          this.items = [...this.items];
        },
        error:(error) =>{
          debugger;
          this.isRequestProcessing = false;
          console.error(error);
          this.messageService.add({severity:'error', summary: 'error', detail: error, life: 3000});
        }});
    }
    
    
    
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
