import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgChartsModule } from 'ng2-charts';
import { ChartData } from 'chart.js';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-user-chart',
  standalone: true,
  imports: [CommonModule, NgChartsModule],
  templateUrl: './user-chart.html',
  styleUrls: ['./user-chart.scss']
})
export class UserChartComponent implements OnInit {
  doughnutChartData: ChartData<'doughnut'> = {
    labels: ['הלוואות', 'החזרים', 'הפקדות', 'תרומות'],
    datasets: [
      {
        data: [0, 0, 0, 0],
        backgroundColor: ['#ef4444', '#3b82f6', '#10b981', '#facc15'],
        hoverOffset: 14
      }
    ]
  };

  chartOptions = {
    responsive: true,
    maintainAspectRatio: false
  };

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.getUserSummary().subscribe(summary => {
      if (!summary) return;

      this.doughnutChartData = {
        labels: ['הלוואות', 'החזרים', 'הפקדות', 'תרומות'],
        datasets: [
          {
            data: [
              summary.totalLoans,
              summary.totalRepayments,
              summary.totalDeposits,
              summary.totalDonations
            ],
            backgroundColor: ['#ef4444', '#3b82f6', '#10b981', '#facc15'],
            hoverOffset: 14
          }
        ]
      };
    });
  }
}
