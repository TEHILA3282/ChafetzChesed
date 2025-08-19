import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { GlobalSearchComponent } from '../global-search/global-search';

@Component({
  selector: 'app-home-main-section',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule,GlobalSearchComponent],
  templateUrl: './home-main-section.html',
  styleUrls: ['./home-main-section.scss'],
})
export class HomeMainSectionComponent {
  searchTerm: string = '';
  filteredResults: { title: string; route: string }[] = [];

  allOptions = [
    { title: 'הלוואות אישיות', route: '/actions/loan' },
    { title: 'בקשה להלוואה', route: '/forms/loan-request' },
    { title: 'מצב החשבון', route: '/account' },
    { title: 'יצירת קשר', route: '/contact' },
    { title: 'הפקדת פיקדון', route: '/actions/deposit' },
    { title: 'משיכת פיקדון', route: '/actions/withdraw' },
    { title: 'תיבת הודעות', route: '/messages' },
    { title: 'תוכניות שותפים', route: '/programs' },
    { title: 'איך פועלת הקופה', route: '/info' }
  ];

  constructor(private router: Router) {}

  onSearch() {
    const term = this.searchTerm.trim().toLowerCase();
    this.filteredResults = this.allOptions.filter(opt =>
      opt.title.toLowerCase().includes(term)
    );
  }

  navigateTo(result: { route: string }) {
    const isLoggedIn = !!localStorage.getItem('token');
    this.router.navigate([isLoggedIn ? result.route : '/login']);
  }

  goTo(route: string) {
    const isLoggedIn = !!localStorage.getItem('token');
    this.router.navigate([isLoggedIn ? route : '/login']);
  }
}
