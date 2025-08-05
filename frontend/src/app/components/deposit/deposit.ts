import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { ActivatedRoute } from '@angular/router';
import { DepositTypeService, DepositType } from '../../services/deposit-type.service';

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
export class DepositComponent implements OnInit {
  amount: number | null = null;
  largeString: string = '';
  depositType: DepositType | null = null;

  depositMethod: 'contact' | 'automatic' | null = null;
  automaticDepositDateChoice: 'immediate' | 'other' | null = null;
  paymentMethod: string | null = null;
  otherDate: Date | null = null;

  constructor(
    private route: ActivatedRoute,
    private depositTypeService: DepositTypeService
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.depositTypeService.getDepositTypeById(id).subscribe(type => {
      this.depositType = type || null;
    });
  }

  onSubmit() {
    const data = {
      amount: this.amount,
      largeString: this.largeString,
      depositMethod: this.depositMethod,
      automaticDepositDateChoice: this.automaticDepositDateChoice,
      paymentMethod: this.paymentMethod,
      otherDate: this.otherDate,
      depositTypeId: this.depositType?.id,
    };
    console.log('Submitted data:', data);
    // כאן אפשר להוסיף שליחה לשרת או טיפול נוסף
  }
}