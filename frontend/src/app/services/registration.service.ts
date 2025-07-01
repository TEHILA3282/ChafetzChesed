import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'; // ← זה נכון
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RegistrationService {
  constructor(private http: HttpClient) {}

  register(data: any): Observable<any> {
    return this.http.post('https://localhost:7150/api/registration', data);
  }
}
