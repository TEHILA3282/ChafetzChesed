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
    ForgotPassword
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
      const credentials = {
        emailOrId: this.form.value.emailOrId,
        password: this.form.value.password
      };

      this.authService.login(credentials).subscribe({
        next: () => {
          console.log('התחברות הצליחה');
          this.router.navigate(['/home']); 
        },
        error: (err: any) => {
          console.error('שגיאה בהתחברות', err);
          this.errorMessage = err.error || 'כתובת מייל/ת"ז או סיסמה שגויים';
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
