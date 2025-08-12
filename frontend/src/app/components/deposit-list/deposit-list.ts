import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DepositTypeService, DepositType } from '../../services/deposit-type.service';

@Component({
  selector: 'app-deposit-list',
  templateUrl: './deposit-list.html',
  styleUrls: ['./deposit-list.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class DepositListComponent implements OnInit {
  depositTypes: DepositType[] = [];


  constructor(
    private router: Router,
    private depositTypeService: DepositTypeService
  ) {}
ngOnInit() {
  this.depositTypeService.getDepositTypes().subscribe(types => {
    this.depositTypes = types;
  });
}

goToDepositFreeze() {
  this.router.navigate(['/deposit-freeze']);
}
goToDepositWithdraw() {
  this.router.navigate(['/deposit-withdraw']);
}
  goToDeposit(deposit: DepositType) {
    this.router.navigate(['/deposit', deposit.id]);
  }
}