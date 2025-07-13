// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7150/api/auth';

  constructor(private http: HttpClient) {}

  login(credentials: { emailOrId: string, password: string, institutionId: number }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, {
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
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    localStorage.removeItem('userName');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getCurrentUser() {
    const userJson = localStorage.getItem('user');
    return userJson ? JSON.parse(userJson) : null;
  }
}
