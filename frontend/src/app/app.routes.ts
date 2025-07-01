import { Routes } from '@angular/router';
import { Register } from './components/register/register';
import { LoginComponent } from './components/login/login';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: Register },
  { path: '', redirectTo: 'register', pathMatch: 'full' }
];

