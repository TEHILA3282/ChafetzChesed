import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatButtonModule,
    MatSnackBarModule
  ],
  templateUrl: './admin-dashboard.html',
  styleUrls: ['./admin-dashboard.scss'],
})
export class AdminComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'email', 'status', 'actions'];
  pendingUsers: any[] = [];
  forms: any[] = [];

  apiUrl = 'https://localhost:7150/api/admin';

  constructor(private http: HttpClient, private snackBar: MatSnackBar) {}

  ngOnInit() {
    this.loadData(); // טוען הכל בהתחלה
  }

  // טוען גם משתמשים וגם טפסים
  loadData() {
    this.loadPendingUsers();
    this.loadForms();
  }

  loadPendingUsers() {
    this.http.get<any[]>(`${this.apiUrl}/pending`).subscribe({
      next: data => this.pendingUsers = data,
      error: err => {
        console.error('שגיאה בטעינת משתמשים ממתינים', err);
        this.snackBar.open('❌ שגיאה בטעינת משתמשים ממתינים', 'סגור', { duration: 3000 });
      }
    });
  }

  updateStatus(id: string, newStatus: string) {
    this.http.post(`${this.apiUrl}/update-status`, {
      registrationId: id,
      newStatus
    }).subscribe({
      next: () => {
        this.snackBar.open(`✅ הסטטוס עודכן ל-${newStatus}`, 'סגור', { duration: 3000 });
        this.loadPendingUsers();
      },
      error: err => {
        console.error('שגיאה בעדכון סטטוס', err);
        this.snackBar.open('❌ שגיאה בעדכון סטטוס', 'סגור', { duration: 3000 });
      }
    });
  }

  loadForms() {
    this.http.get<any[]>(`${this.apiUrl}/external-forms`).subscribe({
      next: data => this.forms = data,
      error: err => {
        console.error('שגיאה בטעינת טפסים חיצוניים', err);
        this.snackBar.open('❌ שגיאה בטעינת טפסים חיצוניים', 'סגור', { duration: 3000 });
      }
    });
  }

  syncUsers() {
    this.http.post(`${this.apiUrl}/sync-users`, {}).subscribe({
      next: (res: any) => {
        this.snackBar.open(`✅ ${res}`, 'סגור', { duration: 3000 });
        this.loadPendingUsers(); // מרענן את הטבלה
      },
      error: err => {
        console.error('שגיאה בסנכרון', err);
        this.snackBar.open('❌ שגיאה בסנכרון משתמשים', 'סגור', { duration: 3000 });
      }
    });
  }
}
