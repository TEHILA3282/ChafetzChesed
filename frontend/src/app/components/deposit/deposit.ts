import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DepositTypeService, DepositType } from '../../services/deposit-type.service';
import { DepositService, CreateDepositDto } from '../../services/deposit.service';
import { AuthService } from '../../services/auth.service';

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

  isReady = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private depositService: DepositService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');
    const id = Number(idParam);

    if (!id || isNaN(id)) {
      console.error(`ID שגוי או חסר בנתיב: ${idParam}`);
      this.router.navigate(['/deposit-list']);
      return;
    }

    const allTypes = this.authService.getDepositTypes();
    const found = allTypes.find(t => t.id === id);

    if (!found) {
      console.error(`לא נמצא סוג הפקדה עם id = ${id}`);
      this.router.navigate(['/deposit-list']);
      return;
    }

    this.depositType = found;
    this.isReady = true;
  }

  onSubmit(form: NgForm) {
    if (!this.depositType) return;

    const deposit: CreateDepositDto = {
      depositTypeId: this.depositType.id,
      amount: this.amount !== null ? Number(this.amount) : null,
      purposeDetails: this.largeString,
      isDirectDeposit: this.depositMethod === 'automatic',
      depositDate: new Date(),
      depositReceivedDate: this.otherDate ?? new Date(),
      paymentMethod: this.paymentMethod
    };

    this.depositService.addDeposit(deposit).subscribe({
      next: () => {
        alert('ההפקדה נשלחה בהצלחה!');

        form.resetForm();
        this.amount = null;
        this.largeString = '';
        this.depositMethod = null;
        this.automaticDepositDateChoice = null;
        this.paymentMethod = null;
        this.otherDate = null;
      },
      error: (err: any) => {
        console.error('שגיאה בשליחה:', err);
        const msg = err?.error?.detail || err?.error?.title || err?.message || 'שגיאה בשליחת ההפקדה';
        alert(msg);
      }
    });
  }
}
