import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
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

  constructor(private fb: FormBuilder, private registrationService: RegistrationService) {
    this.form = this.fb.group({
      FirstName: ['', Validators.required],
      LastName: ['', Validators.required],
      ID: ['', Validators.required],
      PhoneNumber: [''],
      LandlineNumber: [''],
      Email: ['', [Validators.required, Validators.email]],
      DateOfBirth: ['', Validators.required],
      PersonalStatus: ['', Validators.required],
      City: ['', Validators.required],
      Street: ['', Validators.required],
      HouseNumber: ['', Validators.required],
      Password: ['', Validators.required],
      RegistrationStatus: ['ממתין'],
      StatusUpdatedAt: [new Date()]
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.registrationService.register(this.form.value).subscribe({
        next: (res) => {
          console.log('נרשמת בהצלחה', res);
        },
        error: (err) => {
          console.error('שגיאה בהרשמה', err);
        }
      });
    }
  }
}
