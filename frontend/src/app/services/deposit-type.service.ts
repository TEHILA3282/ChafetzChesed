import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

export interface DepositType {
  id: number;
  name: string;
  description: string;      // כותרת משנה
  isDirectDebit: boolean;   // האם הוראת קבע
}

@Injectable({ providedIn: 'root' })
export class DepositTypeService {
  // מצב דטה אמיתי או מוק דטה
  private isMock = true; // שנה ל-false כדי לעבוד מול API אמיתי

  // מוק דטה
  private mockData: DepositType[] = [
    { id: 1, name: 'הפקדה לתכנית חסכון', description: 'הפקדה לתכנית חסכון', isDirectDebit: true },
    { id: 2, name: 'הפקדה לתכנית נישואי ילדים', description: 'הפקדה לנישואי ילדים', isDirectDebit: false },
    { id: 3, name: 'הפקדה לתכנית הטבה לחתנים', description: 'הפקדה להטבה לחתנים', isDirectDebit: true },
    { id: 4, name: 'הפקדה לתכנית הפקדה יחידה', description: 'הפקדה יחידה', isDirectDebit: false },
  ];

  constructor(private http: HttpClient) {}

  // שליפת כל סוגי ההפקדות
  getDepositTypes(): Observable<DepositType[]> {
    if (this.isMock) {
      return of(this.mockData);
    } else {
      return this.http.get<DepositType[]>('/api/deposit-types');
    }
  }

  // שליפת סוג הפקדה לפי מזהה
  getDepositTypeById(id: number): Observable<DepositType | undefined> {
    if (this.isMock) {
      const type = this.mockData.find(t => t.id === id);
      return of(type);
    } else {
      return this.http.get<DepositType>(`/api/deposit-types/${id}`);
    }
  }
}