import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { HomeComponent } from './components/home/home';
import { AuthGuard } from './guards/auth-guard';
import { AccountActionsComponent } from './components/account-actions/account-actions';
import { DepositComponent } from './components/deposit/deposit';
import { MessagesBoxComponent } from './components/messages-box/messages-box';
import { AdminComponent } from './components/admin-dashboard/admin-dashboard';
import { AdminGuard } from './guards/admin.guard';
import { Register } from './components/register/register';
import { PerformingActionsComponent } from './components/performing-actions/performing-actions';
import { DepositListComponent } from './components/deposit-list/deposit-list'; // עדכני נתיב לפי המיקום שלך
import {AwaitingApprovalComponent} from './components/awaiting-approval/awaiting-approval'
import {PendingGuard}from './guards/pending.guard'
import { UpdateDetailsComponent } from './components/update-details/update-details';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: Register },
  { path: 'home', component: HomeComponent },

  { path: 'messages', component: MessagesBoxComponent, canActivate: [AuthGuard,PendingGuard] },
  { path: 'performing-actions', component: PerformingActionsComponent,  canActivate: [AuthGuard,PendingGuard] },
  { path: 'account', component: AccountActionsComponent, canActivate: [AuthGuard,PendingGuard] },
  { path: 'deposit', component: DepositComponent , canActivate: [AuthGuard] },
  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: 'awaiting-approval', component: AwaitingApprovalComponent },
  { path: 'update-details', component: UpdateDetailsComponent,canActivate: [AuthGuard] },
  { path: 'deposit/:id', component: DepositComponent },
  { path: 'deposits', component: DepositListComponent }

];
