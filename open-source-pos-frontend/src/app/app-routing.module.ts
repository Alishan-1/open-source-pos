import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DefaultLayoutComponent } from './containers';
import { Page404Component } from './views/pages/page404/page404.component';
import { Page500Component } from './views/pages/page500/page500.component';
import { LoginComponent } from './views/pages/login/login.component';
import { RegisterComponent } from './views/pages/register/register.component';
import { ConfirmationComponent } from './views/pages/confirmation/confirmation.component'
import { NewPasswordComponent } from './views/pages/new-password/new-password.component'

import { PosComponent } from './views/pos/pos.component';
import { ProblemOneComponent } from './views/pos/problem-one/problem-one.component';
import { InvoicesListComponent } from './views/pos/invoices-list/invoices-list.component';
import { ItemComponent } from './views/item/item.component';
import { CompanyUsersComponent } from './views/company-users/company-users.component';
import { LogoutComponent } from './views/pages/logout/logout.component';

import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {
    path: 'pos',
    component: PosComponent,
    data: {
      title: 'Point of Sale'
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'posp01',
    component: ProblemOneComponent,
    data: {
      title: 'Point of Sale'
    },
    canActivate: [AuthGuard]
  },
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
    
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./views/dashboard/dashboard.module').then((m) => m.DashboardModule),
          canActivate: [AuthGuard]
      },
      // {
      //   path: 'theme',
      //   loadChildren: () =>
      //     import('./views/theme/theme.module').then((m) => m.ThemeModule)
      // },
      // {
      //   path: 'base',
      //   loadChildren: () =>
      //     import('./views/base/base.module').then((m) => m.BaseModule)
      // },
      // {
      //   path: 'buttons',
      //   loadChildren: () =>
      //     import('./views/buttons/buttons.module').then((m) => m.ButtonsModule)
      // },
      // {
      //   path: 'forms',
      //   loadChildren: () =>
      //     import('./views/forms/forms.module').then((m) => m.CoreUIFormsModule)
      // },
      // {
      //   path: 'charts',
      //   loadChildren: () =>
      //     import('./views/charts/charts.module').then((m) => m.ChartsModule)
      // },
      // {
      //   path: 'icons',
      //   loadChildren: () =>
      //     import('./views/icons/icons.module').then((m) => m.IconsModule)
      // },
      {
        path: 'notifications',
        loadChildren: () =>
          import('./views/notifications/notifications.module').then((m) => m.NotificationsModule)
      },
      {
        path: 'widgets',
        loadChildren: () =>
          import('./views/widgets/widgets.module').then((m) => m.WidgetsModule)
      },
      {
        path: 'pages',
        loadChildren: () =>
          import('./views/pages/pages.module').then((m) => m.PagesModule)
      },
// new paths for pos app
      {
        path: 'pos/invoices-list',
        component: InvoicesListComponent,
        data: {
          title: 'Invoices List'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'items',
        component: ItemComponent,
        data: {
          title: '"Items/Products" List'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'items/new',
        component: ItemComponent,
        data: {
          title: '"Items/Products" List'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'company-users',
        component: CompanyUsersComponent,
        data: {
          title: 'Company Users'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'company-users/new',
        component: CompanyUsersComponent,
        data: {
          title: 'Company Users'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'users/log-out',
        component: LogoutComponent,
        data: {
          title: 'log out'
        },
        canActivate: [AuthGuard]
      },
    ]
  },
  {
    path: '404',
    component: Page404Component,
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    component: Page500Component,
    data: {
      title: 'Page 500'
    }
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: {
      title: 'Register Page'
    }
  },
  {
    path: 'confirm',
    component: ConfirmationComponent,
    data: {
      title: 'Confirmation Page'
    }
  },
  {
    path: 'newPassword',
    component: NewPasswordComponent,
    data: {
      title: 'Change Password'
    }
  },
  {path: '**', redirectTo: 'dashboard'}
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'top',
      anchorScrolling: 'enabled',
      initialNavigation: 'enabledBlocking'
      // relativeLinkResolution: 'legacy'
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
