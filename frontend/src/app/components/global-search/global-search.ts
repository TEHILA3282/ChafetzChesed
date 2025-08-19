import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { SearchService, SearchResult } from '../../services/search.service';

@Component({
  selector: 'app-global-search',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule],
  templateUrl: './global-search.html',
  styleUrls: ['./global-search.scss']
})
export class GlobalSearchComponent {
  searchTerm = '';
  results: SearchResult[] = [];

  constructor(private router: Router, private searchService: SearchService) {}

  // הצעות מיידיות תוך כדי הקלדה
  onInputChange() {
    const term = this.searchTerm.trim();
    if (term.length < 1) {
      this.results = [];
      return;
    }

    this.searchService.suggest(term).subscribe(res => {
      this.results = res;
    });
  }

  onSearch() {
    const term = this.searchTerm.trim();
    if (!term) {
      this.results = [];
      return;
    }

    this.searchService.search(term).subscribe(res => {
      this.results = res;
    });
  }

  navigateTo(result: SearchResult) {
    const isLoggedIn = !!localStorage.getItem('token');
    this.router.navigate([isLoggedIn ? result.route : '/login']);
  }
}
