import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../environments/environment';


export interface InstitutionPublicInfo {
  institutionId: number;
  phone: string;
  availabilityText: string;
}

export interface ContactRequestCreate {
  institutionId: number;
  firstName: string;
  lastName: string;
  email: string;
  subject: string;
  message: string;
}

@Injectable({ providedIn: 'root' })
export class ContactService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  getPublicInfo() {
    return this.http.get<InstitutionPublicInfo>(`${this.apiUrl}/institutions/public-info`);
  }

  async submitContact(payload: ContactRequestCreate) {
    return await firstValueFrom(
      this.http.post<{id:number}>(`${this.apiUrl}/contact`, payload)
    );
  }
}
