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
import { DepositListComponent } from './components/deposit-list/deposit-list'; 
import {AwaitingApprovalComponent} from './components/awaiting-approval/awaiting-approval'
import {PendingGuard}from './guards/pending.guard'
import { UpdateDetailsComponent } from './components/update-details/update-details';
import { LoanComponent } from './components/loan/loan';
import { LoansListComponent } from './components/loans-list/loans-list';
import {FreezeRequestComponent} from './components/payments-freeze/payments-freeze';
import { DepositFreezeComponent } from './components/deposit-freeze/deposit-freeze';
import { DepositWithdrawComponent } from './components/deposit-withdraw/deposit-withdraw';

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
  { path: 'awaiting-approval', component: AwaitingApprovalComponent,canActivate: [AuthGuard]  },
  { path: 'update-details', component: UpdateDetailsComponent,canActivate: [AuthGuard] },
  { path: 'deposit/:id', component: DepositComponent , canActivate: [AuthGuard]  },
  { path: 'deposits', component: DepositListComponent , canActivate: [AuthGuard] },
  { path: 'loan/:id', component: LoanComponent , canActivate: [AuthGuard] },
  {path: 'loans-list', component:LoansListComponent, canActivate: [AuthGuard] },
  {path: 'payments-freeze', component:FreezeRequestComponent, canActivate: [AuthGuard] },
  { path: 'deposit-withdraw', component: DepositWithdrawComponent },

];
