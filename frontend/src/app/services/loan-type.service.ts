import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

export interface LoanType {
  id: number;
  name: string;
  description: string;      
}

@Injectable({ providedIn: 'root' })
export class LoanTypeService {
  // מצב דטה אמיתי או מוק דטה
  private isMock = true; 

  private mockData: LoanType[] = [
    { id: 1, name: 'הלוואה לדירה', description: 'הלוואה לרכישת דירה או שיפוץ' },
    { id: 2, name: 'הלוואה לשמחה', description: 'הלוואה לאירועים משפחתיים'},
    { id: 3, name: 'הלוואה לכיסוי חובות', description: 'הלוואה לכיסוי חובות קיימים', },
    { id: 4, name: 'הלוואה ללימודים', description: 'הלוואה למימון לימודים' },
  ];

  constructor(private http: HttpClient) {}

  // שליפת כל סוגי ההלוואות
  getLoanTypes(): Observable<LoanType[]> {
    if (this.isMock) {
      return of(this.mockData);
    } else {
      return this.http.get<LoanType[]>('/api/LoanTypes');
    }
  }

  // שליפת סוג הלוואה לפי מזהה
  getLoanTypeById(id: number): Observable<LoanType | undefined> {
    if (this.isMock) {
      const type = this.mockData.find(t => t.id === id);
      return of(type);
    } else {
      return this.http.get<LoanType>(`/api/loan-types/${id}`);
    }
  }
}