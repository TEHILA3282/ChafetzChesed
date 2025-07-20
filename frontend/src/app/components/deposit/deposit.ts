import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-deposit',
  templateUrl: './deposit.html',
  styleUrls: ['./deposit.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
})
export class DepositComponent {
  amount: number | null = null;
  largeString: string = '';

  depositMethod: 'contact' | 'automatic' | null = null;

  automaticDepositDateChoice: 'immediate' | 'other' | null = null;
  paymentMethod: string | null = null;

  otherDate: Date | null = null;

  onSubmit() {
    const data = {
      amount: this.amount,
      largeString: this.largeString,
      depositMethod: this.depositMethod,
      automaticDepositDateChoice: this.automaticDepositDateChoice,
      paymentMethod: this.paymentMethod,
      otherDate: this.otherDate,
    };
    console.log('Submitted data:', data);
    // הוסף כאן את הלוגיקה לשליחה לשרת או כל פעולה אחרת
  }
}
