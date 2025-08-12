import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import {
  MatFormFieldModule,
  MAT_FORM_FIELD_DEFAULT_OPTIONS
} from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { RegistrationService } from '../../services/registration.service';
import { InstitutionService, InstitutionConfig } from '../../services/institution.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCardModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    HttpClientModule
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss',
  providers: [
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: { appearance: 'fill' }
    }
  ]
})
export class Register {
  form: FormGroup;
  statuses = ['רווק/ה', 'נשוי/ה', 'גרוש/ה', 'אלמן/ה'];
  errorMessage: string = '';
  institution: InstitutionConfig;

  constructor(
    private fb: FormBuilder,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private router: Router,
    private institutionService: InstitutionService
  ) {
    this.institution = this.institutionService.getInstitution();
    const institutionId = this.institution.id;

    this.form = this.fb.group(
      {
        FirstName: ['', [Validators.required, Validators.pattern(/^[\u0590-\u05FFa-zA-Z\s'-]{2,}$/)]],
        LastName: ['', [Validators.required, Validators.pattern(/^[\u0590-\u05FFa-zA-Z\s'-]{2,}$/)]],
        ID: ['', [Validators.required, Validators.pattern(/^\d{8,9}$/)]],
        PhoneNumber: ['', [Validators.pattern(/^\d{9}$/)]],
        LandlineNumber: ['', [Validators.pattern(/^\d{10}$/)]],
        Email: ['', [Validators.required, Validators.email]],
        Role: ['User'],
        DateOfBirth: ['', [Validators.required, this.noFutureDateValidator]],
        PersonalStatus: ['', Validators.required],
        City: ['', [Validators.required, Validators.pattern(/^[\u0590-\u05FFa-zA-Z\s'-]+$/)]],
        Street: ['', [Validators.required, Validators.pattern(/^[\u0590-\u05FFa-zA-Z\s'-]+$/)]],
        HouseNumber: ['', [Validators.required, Validators.pattern(/^\d+[א-ת]?[a-zA-Z]?$/)]],
        Password: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$/)
          ]
        ],
        ConfirmPassword: ['', Validators.required],
        RegistrationStatus: ['ממתין'],
        StatusUpdatedAt: [new Date()],
        InstitutionId: [institutionId]
      },
      { validators: this.passwordsMatchValidator }
    );
  }

  noFutureDateValidator(control: AbstractControl): ValidationErrors | null {
    const inputDate = new Date(control.value);
    const today = new Date();
    return inputDate > today ? { futureDate: true } : null;
  }

  passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('Password')?.value;
    const confirm = group.get('ConfirmPassword')?.value;
    return password === confirm ? null : { passwordsMismatch: true };
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { ConfirmPassword, ...formData } = this.form.value;

    this.registrationService
      .checkEmailOrIdExists(formData.Email, formData.ID, formData.InstitutionId)
      .subscribe({
        next: (exists: boolean) => {
          if (exists) {
            this.errorMessage = 'משתמש עם כתובת מייל או תעודת זהות זו כבר קיים במערכת';
            return;
          }

          this.registrationService.register(formData).subscribe({
            next: (res) => {
              console.log('נרשמת בהצלחה', res);
              this.router.navigate(['/login']);
            },
            error: (err) => {
              console.error('שגיאה בהרשמה', err);
              this.errorMessage = 'ארעה שגיאה במהלך ההרשמה';
            }
          });
        },
        error: () => {
          this.errorMessage = 'שגיאה בבדיקת תקינות המידע';
        }
      });
  }
}
