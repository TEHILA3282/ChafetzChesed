import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-deposit-withdraw',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './deposit-withdraw.html',
  styleUrls: ['./deposit-withdraw.scss']
})
export class DepositWithdrawComponent {
  amount: number | null = null;
  requestText: string = '';

  onSubmit() {
    // כאן אפשר להוסיף שליחת הבקשה לשרת
    alert('הבקשה נשלחה');
  }
}