import { Component, OnInit } from '@angular/core';
import { Product } from './temp/product';
import { InvoiceMasterListing } from '../../../models/posTrans';
import { ProductService } from './temp/productservice';
import { PosService } from '../pos.service';
import { ConfirmationService } from 'primeng/api';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../login/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-invoices-list',
  templateUrl: './invoices-list.component.html',
  styleUrls: ['./invoices-list.component.scss'],
  styles: [`
        :host ::ng-deep .p-dialog .product-image {
            width: 150px;
            margin: 0 auto 2rem auto;
            display: block;
        }
    `],
    providers: [MessageService,ConfirmationService]
})
export class InvoicesListComponent implements OnInit {

  
  posDialog!: boolean;

  products!: Product[];

  product!: Product;

  
  selectedInvoices!: InvoiceMasterListing[];
  submitted!: boolean;

  statuses!: any;

  invoices!: InvoiceMasterListing[];
  totalInvoices!: number;  

  invoice!: InvoiceMasterListing;
  
  currentUser:any;

  constructor(private productService: ProductService, private messageService: MessageService, private router: Router, 
    private posService: PosService, private confirmationService: ConfirmationService, private authService: AuthService) { 
        this.currentUser = this.authService.GetlocalStorageUser();
    }

  ngOnInit() {
    debugger;
      
      let params = {
        query: '',
        companyId: this.currentUser.CompanyID,
        limit:0,
        offset:0
      };
      this.posService.GetInvoicesList(params).subscribe({
        next: (sr) => {
          debugger;
            
          this.invoices = sr.Data.Invoices;
          this.totalInvoices = sr.Data.Count;          
            
        },
        error:(error) =>{
          debugger;          
          console.error(error);
          this.messageService.add({severity:'error', summary: 'Error Loading Invoices!', detail: error, life: 3000});
        }});
      this.statuses = {
        N:'New',
        P:'Posted'
      }


  }

  openNew() {
      this.router.navigate([`/pos`]);
  }

  deleteSelectedInvoices() {
      this.confirmationService.confirm({
          message: 'Are you sure you want to delete the selected (Un Posted) Invoices?',
          header: 'Confirm',
          icon: 'pi pi-exclamation-triangle',
          accept: () => {
            debugger;
            this.selectedInvoices = this.selectedInvoices.filter(val => val.Status !== 'P');
            if(this.selectedInvoices.length <= 0){
                return;
            }
            this.posService.DeleteInvoicesList(this.selectedInvoices).subscribe({
                next: (sr) => {
                    debugger;
                    this.invoices = this.invoices.filter(val => !this.selectedInvoices.includes(val));
                      this.selectedInvoices = [];
                      this.messageService.add({severity:'success', summary: 'Successful', detail: sr.Message, life: 3000});
                },
                error:(error) =>{
                  console.error(error);
                  this.messageService.add({severity:'error', summary: error.Title, detail: error.Message, life: 3000});
                }});              
          }
      });
  }

  editInvoice(invoice: any) {
      this.invoice = invoice;
      
      this.posDialog = true;
  }

  deleteInvoice(invoice: InvoiceMasterListing) {
    
    let isPosted = invoice.Status! === "P";
    if(isPosted){
        
        this.messageService.add({severity:'warn', summary: 'warning', detail: 'Posted invoice cannot be deleted', life: 3000});
        return;
    }
    
    this.confirmationService.confirm({
          message: 'Are you sure you want to delete Invoice No: ' + invoice.InvoiceNo + '?',
          header: 'Confirm',
          icon: 'pi pi-exclamation-triangle',
          accept: () => {
              this.invoices = this.invoices.filter(val => val.InvoiceNo !== invoice.InvoiceNo);
              this.posService.DeleteInvoice(invoice).subscribe({
                next: (sr) => {
                    this.messageService.add({severity:'success', summary: 'Successful', detail: 'Invoice Deleted', life: 3000});
                },
                error:(error) =>{
                  console.error(error);
                  this.messageService.add({severity:'error', summary: error.Title, detail: error.Message, life: 3000});
                }});
              
          }
      });
  }

}
