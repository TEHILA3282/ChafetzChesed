import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { environment } from '../../environments/environment';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';

@Component({
  selector: 'app-update-details',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatRadioModule
  ],
  templateUrl: './update-details.html',
  styleUrls: ['./update-details.scss']
})
export class UpdateDetailsComponent implements OnInit {
personalStatuses: string[] = ['רווק/ה', 'נשוי/ה', 'גרוש/ה', 'אלמן/ה'];

  detailsForm!: FormGroup;
  bankForm!: FormGroup;

  apiUrl = environment.apiUrl;

  constructor(
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.detailsForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: [''],
      landlineNumber: [''],
      email: ['', [Validators.required, Validators.email]],
      dateOfBirth: [''],
      personalStatus: [''],
      street: [''],
      city: [''],
      houseNumber: ['']
    });
    this.bankForm = this.fb.group({
  bankNumber: ['', [Validators.required, Validators.pattern(/^\d{2,3}$/)]],
  branchNumber: ['', [Validators.required, Validators.pattern(/^\d{1,5}$/)]], 
  accountNumber: ['', [Validators.required, Validators.pattern(/^\d{4,10}$/)]], 
  accountOwnerName: ['', [Validators.required, Validators.minLength(2)]],
  hasDirectDebit: [false]
});

    this.http.get<any>(`${this.apiUrl}/auth/get-current-user`).subscribe({
      next: data => {
        this.detailsForm.patchValue(data);
      },
      error: err => {
        console.error('שגיאה בטעינת נתוני משתמש:', err);
        this.snackBar.open('שגיאה בטעינת נתונים', '', { duration: 3000 });
      }
    });
      this.http.get<any>(`${this.apiUrl}/auth/get-bank-details`).subscribe({
    next: data => {
      this.bankForm.patchValue({
        bankNumber: data.bankNumber,
        branchNumber: data.branchNumber,
        accountNumber: data.accountNumber,
        accountOwnerName: data.accountOwnerName,
        hasDirectDebit: data.hasDirectDebit
      });
    },
    error: err => {
      console.error('שגיאה בטעינת פרטי הבנק:', err);
      this.snackBar.open('⚠️ לא נמצאו פרטי חשבון בנק', '', { duration: 3000 });
    }
  });
  }

  submit(): void {
    if (this.detailsForm.valid) {
      this.http.put(`${this.apiUrl}/registration/update-personal`, this.detailsForm.value,{ responseType: 'text' }).subscribe({
        next: () => this.snackBar.open('הפרטים עודכנו בהצלחה', '', { duration: 3000 }),
        error: err => {
          console.error('שגיאה בעדכון:', err);
          this.snackBar.open('שגיאה בעדכון: ' + err.message, '', { duration: 3000 });
        }
      });
    } else {
      this.snackBar.open('יש למלא את כל השדות החובה', '', { duration: 3000 });
    }
  }
    submitBank(): void {
    if (this.bankForm.valid) {
      this.http.put(`${this.apiUrl}/registration/update-bank`, this.bankForm.value,{ responseType: 'text' }).subscribe({
        next: () => this.snackBar.open('פרטי הבנק עודכנו בהצלחה', '', { duration: 3000 }),
        error: err => {
          console.error('שגיאה בעדכון בנק:', err);
          this.snackBar.open('שגיאה בעדכון: ' + err.message, '', { duration: 3000 });
        }
      });
    }
  }
}

