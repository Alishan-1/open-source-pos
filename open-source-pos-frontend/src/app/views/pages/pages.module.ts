import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PagesRoutingModule } from './pages-routing.module';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { Page404Component } from './page404/page404.component';
import { Page500Component } from './page500/page500.component';
import { ButtonModule, CardModule, FormModule, GridModule, ToastModule,
  ModalModule  } from '@coreui/angular';
import { IconModule } from '@coreui/icons-angular';
import { CountdownTimerModule } from '../../shared/countdown-timer/index';
import { ConfirmationComponent } from './confirmation/confirmation.component';
import { NewPasswordComponent } from './new-password/new-password.component';
import { LogoutComponent } from './logout/logout.component';

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    Page404Component,
    Page500Component,
    ConfirmationComponent,
    NewPasswordComponent,
    LogoutComponent
  ],
  imports: [
    CommonModule,
    PagesRoutingModule,
    CardModule,
    ButtonModule,
    GridModule,
    IconModule,
    FormModule,
    FormsModule,
    ToastModule,
    ModalModule,
    CountdownTimerModule.forRoot(),
  ]
})
export class PagesModule {
}
