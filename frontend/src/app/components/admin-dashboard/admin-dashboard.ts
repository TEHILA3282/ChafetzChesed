import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { EditStatusDialogComponent } from '../edit-status-dialog/edit-status-dialog';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatButtonModule,
    MatSnackBarModule,
    MatDialogModule,
    EditStatusDialogComponent
  ],
  templateUrl: './admin-dashboard.html',
  styleUrls: ['./admin-dashboard.scss'],
})
export class AdminComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'email', 'status', 'actions'];
  pendingUsers: any[] = [];
  approvedUsers: any[] = [];
  rejectedUsers: any[] = [];
  forms: any[] = [];
  isSyncing = false;

  apiUrl = 'https://localhost:7150/api/admin';

  constructor(
    private http: HttpClient,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.loadPendingUsers();
    this.loadApprovedUsers();
    this.loadRejectedUsers();
    this.loadForms();
  }

  loadPendingUsers() {
    this.http.get<any[]>(`${this.apiUrl}/pending`).subscribe({
      next: data => {
        setTimeout(() => this.pendingUsers = data);
      },
      error: err => {
        if (!this.isSyncing) {
          this.snackBar.open('❌ שגיאה בטעינת משתמשים ממתינים', 'סגור', { duration: 3000 });
        }
      }
    });
  }

  loadApprovedUsers() {
    this.http.get<any[]>(`${this.apiUrl}/approved`).subscribe({
      next: data => {
        setTimeout(() => this.approvedUsers = data);
      },
      error: err => {
        if (!this.isSyncing) {
          this.snackBar.open('❌ שגיאה בטעינת משתמשים מאושרים', 'סגור', { duration: 3000 });
        }
      }
    });
  }

  loadRejectedUsers() {
    this.http.get<any[]>(`${this.apiUrl}/rejected`).subscribe({
      next: data => {
        setTimeout(() => this.rejectedUsers = data);
      },
      error: err => {
        if (!this.isSyncing) {
          this.snackBar.open('❌ שגיאה בטעינת משתמשים שנדחו', 'סגור', { duration: 3000 });
        }
      }
    });
  }

  updateStatus(id: string, newStatus: string) {
    this.http.post<{ message: string }>(`${this.apiUrl}/update-status`, { registrationId: id, newStatus }).subscribe({
      next: (res) => {
        this.snackBar.open(`✅ ${res.message}`, 'סגור', { duration: 3000 });
        setTimeout(() => this.loadData(), 0);
      },
      error: err => {
        console.error('שגיאה:', err);
        this.snackBar.open('❌ שגיאה בעדכון סטטוס', 'סגור', { duration: 3000 });
      }
    });
  }

  loadForms() {
    this.http.get<any[]>(`${this.apiUrl}/external-forms`).subscribe({
      next: data => {
        setTimeout(() => this.forms = data);
      },
      error: err => {
        if (!this.isSyncing) {
          this.snackBar.open('❌ שגיאה בטעינת טפסים חיצוניים', 'סגור', { duration: 3000 });
        }
      }
    });
  }

 syncUsers() {
  this.isSyncing = true;

  this.http.post<{ message: string }>(`${this.apiUrl}/sync-users`, {}).subscribe({
    next: (res) => {
      this.snackBar.open(`✅ ${res.message}`, 'סגור', { duration: 3000 });
      setTimeout(() => {
        this.isSyncing = false;
        this.loadData();
      }, 300);
    },
    error: err => {
      this.isSyncing = false;
      this.snackBar.open('❌ שגיאה בסנכרון המשתמשים', 'סגור', { duration: 3000 });
      console.error('שגיאה בסנכרון:', err);
    }
  });
}


  openEditDialog(user: any) {
    const dialogRef = this.dialog.open(EditStatusDialogComponent, { data: user });

    dialogRef.afterClosed().subscribe(result => {
      if (result?.newStatus && result.newStatus !== user.registrationStatus) {
        this.updateStatus(user.id, result.newStatus);
      }
    });
  }
}
