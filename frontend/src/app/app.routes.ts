import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { HomeComponent } from './components/home/home';
import { AuthGuard } from './guards/auth-guard';

import { ActionsComponent } from './components/actions/actions';
import { AccountComponent } from './components/account/account';
import { DepositComponent } from './components/deposit/deposit';
import { MessagesBoxComponent } from './components/messages-box/messages-box';
import { MessagesComponent } from './components/messages/messages';
import { AdminComponent } from './components/admin-dashboard/admin-dashboard';
import { AdminGuard } from './guards/admin.guard';
import { Register } from './components/register/register';
import { PerformingActionsComponent } from './components/performing-actions/performing-actions';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: Register },
  { path: 'home', component: HomeComponent },

  { path: 'messages', component: MessagesComponent, canActivate: [AuthGuard] },
  { path: 'messages-box', component: MessagesBoxComponent, canActivate: [AuthGuard] },

  { path: 'performing-actions', component: PerformingActionsComponent },

  { path: 'account', component: AccountComponent, canActivate: [AuthGuard] },
  { path: 'deposit', component: DepositComponent , canActivate: [AuthGuard] },

  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard, AdminGuard] }
];
