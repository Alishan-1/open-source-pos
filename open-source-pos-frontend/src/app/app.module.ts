import { NgModule } from '@angular/core';
import { HashLocationStrategy, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { HttpClientModule } from '@angular/common/http';

import {
  PERFECT_SCROLLBAR_CONFIG,
  PerfectScrollbarConfigInterface,
  PerfectScrollbarModule,
} from 'ngx-perfect-scrollbar';

// Import routing module
import { AppRoutingModule } from './app-routing.module';

// Import app component
import { AppComponent } from './app.component';

// Import containers
import {
  DefaultFooterComponent,
  DefaultHeaderComponent,
  DefaultLayoutComponent,
} from './containers';


import { RegisterComponent } from './views/register/register.component';
import { PosComponent } from './views/pos/pos.component';
import { AuthGuard } from './guards/auth.guard';

// Import prime ng components
import {TableModule} from 'primeng/table';
import {MessageModule} from 'primeng/message';
// import {DropdownModule} from 'primeng/dropdown';
import {InputTextModule} from 'primeng/inputtext';
import {ButtonModule as pButtonModule } from 'primeng/button';
import {DialogModule} from 'primeng/dialog';
import {ToastModule} from 'primeng/toast';

import {ToolbarModule} from 'primeng/toolbar';
import {FileUploadModule} from 'primeng/fileupload';
import {RatingModule} from 'primeng/rating';
import {DropdownModule as pDropdownModule} from 'primeng/dropdown';
import { ConfirmDialogModule } from 'primeng/confirmdialog';


import {
  AvatarModule,
  BadgeModule,
  BreadcrumbModule,
  ButtonGroupModule,
  ButtonModule,
  CardModule,
  DropdownModule,
  FooterModule,
  FormModule,
  GridModule,
  HeaderModule,
  ListGroupModule,
  NavModule,
  ProgressModule,
  SharedModule,
  SidebarModule,
  TabsModule,
  UtilitiesModule,
} from '@coreui/angular';

import { IconModule, IconSetService } from '@coreui/icons-angular';

import { WINDOW_PROVIDERS } from './window.provider';
import { Configuration } from './app.constants';
import { UtilService } from './services/util.service';
import { InvoicesListComponent } from './views/pos/invoices-list/invoices-list.component';
// temp service for testing p table crud component
import { ProductService } from './views/pos/invoices-list/temp/productservice';
import { ItemComponent } from './views/item/item.component';
import { ProblemOneComponent } from './views/pos/problem-one/problem-one.component';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true,
};

const APP_CONTAINERS = [
  DefaultFooterComponent,
  DefaultHeaderComponent,
  DefaultLayoutComponent,
];

@NgModule({
  declarations: [
    AppComponent, 
    ...APP_CONTAINERS,
    
    RegisterComponent,
    PosComponent,
    InvoicesListComponent,
    ItemComponent,
    ProblemOneComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AvatarModule,
    BreadcrumbModule,
    FooterModule,
    DropdownModule,
    GridModule,
    HeaderModule,
    SidebarModule,
    IconModule,
    PerfectScrollbarModule,
    NavModule,
    ButtonModule,
    pButtonModule,
    FormModule,
    UtilitiesModule,
    ButtonGroupModule,
    ReactiveFormsModule,

    FormsModule,

    SidebarModule,
    SharedModule,
    TabsModule,
    ListGroupModule,
    ProgressModule,
    BadgeModule,
    ListGroupModule,
    CardModule,

    TableModule,
    MessageModule,
    // DropdownModule,
    InputTextModule,
    // ButtonModule,
    DialogModule,
    ToastModule,
    ToolbarModule,
    FileUploadModule,
    RatingModule,
    pDropdownModule,
    ConfirmDialogModule,

    HttpClientModule,
  ],
  providers: [
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy,
    },
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG,
    },
    IconSetService,
    Title,

    WINDOW_PROVIDERS,
    Configuration,
    UtilService,
    AuthGuard,
    ProductService
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
}
