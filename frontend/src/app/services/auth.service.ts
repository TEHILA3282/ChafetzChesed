import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7150/api/auth';

  constructor(private http: HttpClient) {}

  login(credentials: { emailOrId: string, password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, {
      identifier: credentials.emailOrId,
      password: credentials.password
    }).pipe(
      tap((res: any) => {
        // שומרים את הטוקן ב־localStorage
        localStorage.setItem('token', res.token);
        localStorage.setItem('user', JSON.stringify(res.user));
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
}
