import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, BehaviorSubject } from 'rxjs';
import { environment } from '../environments/environment';
import { AccountAction } from './account-actions.service';
import { Router } from '@angular/router';
import { NgZone } from '@angular/core';



@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl;
  private tokenSubject = new BehaviorSubject<string | null>(null);
  private accountActions: AccountAction[] = [];

  constructor(private http: HttpClient,private router: Router,  private zone: NgZone
) {
    const savedToken = localStorage.getItem('token');
    if (savedToken) this.tokenSubject.next(savedToken);
  }

  login(credentials: { emailOrId: string, password: string, institutionId: number }): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/login`, {
      identifier: credentials.emailOrId,
      password: credentials.password,
      institutionId: credentials.institutionId
    }).pipe(
      tap((res: any) => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('user', JSON.stringify(res.user));
        if (res.user?.firstName) {
          localStorage.setItem('userName', res.user.firstName);
        }
        this.tokenSubject.next(res.token);
      })
    );
  }

  fetchUserFromServer(userId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/auth/get-user/${userId}`);
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    localStorage.removeItem('userName');
    this.tokenSubject.next(null);
    this.accountActions = [];
     this.zone.run(() => {
    this.router.navigate(['/home']);
  });
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

getToken(): string | null {
  return localStorage.getItem('token');
}

  getTokenObservable(): Observable<string | null> {
    return this.tokenSubject.asObservable();
  }

  getCurrentUser(): any {
    const userJson = localStorage.getItem('user');
    return userJson ? JSON.parse(userJson) : null;
  }

  setAccountActions(actions: AccountAction[]) {
    this.accountActions = actions;
  }

  getAccountActions(): AccountAction[] {
    return this.accountActions;
  }

}
