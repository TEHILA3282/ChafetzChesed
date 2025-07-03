import { Routes } from '@angular/router';
import { Register } from './components/register/register';
import { LoginComponent } from './components/login/login';
import { HomeComponent } from './components/home/home';
import { AuthGuard } from './guards/auth-guard';
import { MessagesComponent } from './components/messages/messages';
import { ActionsComponent } from './components/actions/actions';
import { AccountComponent } from './components/account/account';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: Register },
  { path: 'home', component: HomeComponent },

    { path: 'messages', component: MessagesComponent, canActivate: [AuthGuard] },
  { path: 'actions', component: ActionsComponent, canActivate: [AuthGuard] },
  { path: 'account', component: AccountComponent, canActivate: [AuthGuard] }
];

