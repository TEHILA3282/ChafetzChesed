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
import { InstitutionService } from '../../services/institution.service';
import { AccountActionsService } from '../../services/account-actions.service';
import { InstitutionConfig } from '../../services/institution.service';
import {RejectedDialogComponent} from '../rejected-dialog/rejected-dialog'

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
    RouterModule
  ],
  templateUrl: './login.html',
  styleUrls: ['./login.scss']
})
export class LoginComponent {
  hide = true;
  form: FormGroup;
  errorMessage = '';
  institution: InstitutionConfig;

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private institutionService: InstitutionService,
    private actionsService: AccountActionsService
  ) {
    this.institution = this.institutionService.getInstitution();

    this.form = this.fb.group({
      emailOrId: ['', Validators.required],
      password: ['', Validators.required]
    });
  }
onSubmit() {
  if (this.form.valid) {
    const institutionId = this.institutionService.getInstitutionId();

    const credentials = {
      emailOrId: this.form.value.emailOrId,
      password: this.form.value.password,
      institutionId
    };

    this.authService.login(credentials).subscribe({
      next: (res) => {
        localStorage.setItem('token', res.token);

        const user = res.user;
        const role = user?.role;
        const status = user?.registrationStatus;

        if (status === 'מאושר') {
          this.actionsService.getAllActions().subscribe(actions => {
            this.authService.setAccountActions(actions);

            if (role === 'Admin') {
              this.router.navigate(['/admin']);
            } else {
              this.router.navigate(['/home']);
            }
          });
        } else if (status === 'ממתין') {
          this.router.navigate(['/awaiting-approval']);
        } else if (status === 'נדחה') {
          this.dialog.open(RejectedDialogComponent); 
        }
      },
      error: (err: any) => {
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

  getLogoPath(): string {
    const logo = this.institution.logo;
    return logo.startsWith('http') ? logo : `/assets/${logo}`;
  }
}
