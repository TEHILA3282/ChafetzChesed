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
        // שמירה של הטוקן והמשתמש
        localStorage.setItem('token', res.token);
        localStorage.setItem('user', JSON.stringify(res.user));

        // שמירה נפרדת של השם הפרטי
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
}
