import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';


@Injectable({ providedIn: 'root' })
export class RegistrationService {
  constructor(private http: HttpClient) {}

  register(data: any): Observable<any> {
    return this.http.post('https://localhost:7150/api/registration', data);
  }
  // registration.service.ts
private apiUrl = environment.apiUrl;

checkEmailOrIdExists(email: string, id: string, institutionId: number): Observable<boolean> {
  return this.http.get<boolean>(`${this.apiUrl}/registration/check-exists`, {
    params: { email, id, institutionId }
  });
}

}
