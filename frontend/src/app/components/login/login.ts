import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ForgotPassword } from '../forgot-password/forgot-password';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    ReactiveFormsModule,
    ForgotPassword,
    RouterModule
  ],
  templateUrl: './login.html',
  styleUrls: ['./login.scss']
})
export class LoginComponent {
  hide = true;
  form: FormGroup;
  errorMessage = '';

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({
      emailOrId: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const hostname = window.location.hostname;
      const subdomain = hostname.includes('localhost') ? 'localhost' : hostname.split('.')[0];
      const institutionId = 1; 

      const credentials = {
        emailOrId: this.form.value.emailOrId,
        password: this.form.value.password,
        institutionId
      };

      console.log('נשלח לשרת:', credentials);

      this.authService.login(credentials).subscribe({
        next: (res) => {
          console.log('מה הגיע מהשרת:', res);
          const role = res.user?.role;
          if (role === 'Admin') {
            this.router.navigate(['/admin']);
          } else {
            this.router.navigate(['/home']);
          }
        },
        error: (err: any) => {
          console.error('שגיאה בהתחברות', err);
          this.errorMessage =
            typeof err.error?.message === 'string'
              ? err.error.message
              : 'כתובת מייל/ת"ז או סיסמה שגויים';
        }
      });
    }
  }

  openForgotPassword() {
    this.dialog.open(ForgotPassword, {
      width: '400px'
    });
  }
}
