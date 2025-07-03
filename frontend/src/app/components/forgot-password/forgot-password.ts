import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule,
    HttpClientModule,
    MatSnackBarModule,
  ],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.scss',
})
export class ForgotPassword {
  emailOrUser: string = '';

  constructor(
    private dialogRef: MatDialogRef<ForgotPassword>,
    private http: HttpClient,
    private snackBar: MatSnackBar
  ) {}

  sendResetLink() {
    const email = this.emailOrUser.trim();
    if (!email) {
      this.snackBar.open('יש להזין כתובת אימייל', 'סגור', { duration: 3000 });
      return;
    }

    this.http.post('https://localhost:7150/api/auth/forgot-password', email, {
      headers: { 'Content-Type': 'application/json' },
      responseType: 'text' 
    }).subscribe({
      next: () => {
        this.snackBar.open('סיסמה חדשה נשלחה למייל שלך', 'סגור', { duration: 4000 });
        this.dialogRef.close();
      },
      error: (err) => {
        const msg = err.status === 404 ? 'האימייל לא נמצא במערכת' : 'שגיאה בשליחת הסיסמה';
        this.snackBar.open(msg, 'סגור', { duration: 4000 });
      }
    });
  }

  close() {
    this.dialogRef.close();
  }
}
