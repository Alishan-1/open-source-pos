import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  AlertModule,
  BadgeModule,
  ButtonModule,
  CardModule,
  FormModule,
  GridModule,
  ModalModule,
  PopoverModule,
  ProgressModule,
  SharedModule,
  ToastModule,
  TooltipModule,
  UtilitiesModule
} from '@coreui/angular';

import { DocsComponentsModule } from '@docs-components/docs-components.module';
import { NotificationsRoutingModule } from './notifications-routing.module';

import { AlertsComponent } from './alerts/alerts.component';
import { BadgesComponent } from './badges/badges.component';
import { ModalsComponent } from './modals/modals.component';
// import { ToastsComponent } from './toasts/toasts.component';
import { ToastersComponent } from './toasters/toasters.component';
import { PosToastersComponent } from './pos-toasters/toasters.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AppToastComponent } from './toasters/toast-simple/toast.component';
import { AppPosToastComponent } from './pos-toasters/toast-simple/toast.component';

@NgModule({
  declarations: [
    BadgesComponent,
    AlertsComponent,
    ModalsComponent,
    // ToastsComponent,
    ToastersComponent,
    PosToastersComponent,
    AppToastComponent,
    AppPosToastComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NotificationsRoutingModule,
    DocsComponentsModule,
    AlertModule,
    GridModule,
    CardModule,
    BadgeModule,
    ButtonModule,
    FormModule,
    ModalModule,
    ToastModule,
    SharedModule,
    UtilitiesModule,
    TooltipModule,
    PopoverModule,
    ProgressModule
  ],
})
export class NotificationsModule {
}
