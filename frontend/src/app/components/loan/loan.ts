import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonModule } from '@angular/material/button';
import { GuarantorsFormComponent } from '../guarantors/guarantors-form/guarantors-form';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LoanTypeService, LoanType } from '../../services/loan-type.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-loan',
  templateUrl: './loan.html',
  styleUrls: ['./loan.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatButtonModule,
    MatCheckboxModule,
    GuarantorsFormComponent
  ]
})
export class LoanComponent implements OnInit {
  @Input() loanTypeId!: number;

  loanTypeTitle: string = '';
  loanTypeSubtitle: string = '';

  amount: number | null = null;
  paymentsCount: number | null = null;
  loanPurpose: string = '';
  description: string = '';
  isForApartment: string = 'no';
  apartmentConfirmed: boolean = false;

  loanPurposes: string[] = [
    'רכישת דירה',
    'חתונה בן / בת',
    'בר מצווה / בת מצווה',
    'הרחבת דירה',
    'שיפוץ דירה',
    'שמחה משפחתית',
    'כיסוי חובות',
    'חובות לדירה',
    'לימודים'
  ];

constructor(private loanTypeService: LoanTypeService, private route: ActivatedRoute) {}

ngOnInit(): void {
  const id = Number(this.route.snapshot.paramMap.get('id'));
  this.loanTypeId = id;
  console.log('loanTypeId:', this.loanTypeId);
  if (this.loanTypeId) {
    this.loanTypeService.getLoanTypeById(this.loanTypeId).subscribe((type: LoanType | undefined) => {
      if (type) {
        this.loanTypeTitle = type.name;
        this.loanTypeSubtitle = type.description;
      }
    });
  }
}

  onSubmit() {
    const loanData = {
      amount: this.amount,
      paymentsCount: this.paymentsCount,
      loanPurpose: this.loanPurpose,
      description: this.description,
      isForApartment: this.isForApartment === 'yes',
      apartmentConfirmed: this.apartmentConfirmed
    };

    console.log('Loan Request:', loanData);
  }
}

//צריך לשלוח לשרת את הבקשות