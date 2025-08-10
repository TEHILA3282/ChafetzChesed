import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoanTypeService, LoanType } from '../../services/loan-type.service';

@Component({
  selector: 'app-loans-list',
  templateUrl: './loans-list.html',
  styleUrls: ['./loans-list.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class LoansListComponent implements OnInit {
  loanTypes: LoanType[] = [];

  constructor(
    private router: Router,
    private loanTypeService: LoanTypeService
  ) {}

  ngOnInit() {
    this.loanTypeService.getLoanTypes().subscribe(types => {
      this.loanTypes = types;
    });
  }

  goToLoan(loan: LoanType) {
    this.router.navigate(['/loan', loan.id]);
  }
}