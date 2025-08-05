import { Component, OnInit } from '@angular/core';
import { AccountAction } from '../../services/account-actions.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { UserChartComponent } from '../user-chart/user-chart'; 

@Component({
  selector: 'app-account-actions',
  templateUrl: './account-actions.html',
  styleUrls: ['./account-actions.scss'],
  standalone: true,
  imports: [CommonModule,UserChartComponent],
})
export class AccountActionsComponent implements OnInit {
  balanceSummary: AccountAction[] = [];
  recentActions: AccountAction[] = [];

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    const allActions = this.authService.getAccountActions();
    this.balanceSummary = allActions.filter(a => a.important === 0 || a.important === 1);
    this.recentActions = allActions.filter(a => a.important >= 3);
  }

  getClass(important: number): string {
    switch (important) {
      case 3: return 'red';
      case 4: return 'blue';
      case 5: return 'green';
      case 1: return 'bold';
      default: return '';
    }
  }
}
