import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './header.html',
  styleUrls: ['./header.scss']
})
export class HeaderComponent implements OnInit {
  router = inject(Router);
  isLoggedIn = false;
  userName = '';

  ngOnInit() {
    this.checkLoginStatus();

    // מאזין לשינויים בניווט כדי לעדכן את מצב המשתמש
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.checkLoginStatus();
      });
  }

  checkLoginStatus() {
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');

    if (token && user) {
      this.isLoggedIn = true;

      try {
        const parsedUser = JSON.parse(user);
        this.userName = `${parsedUser.firstName} ${parsedUser.lastName}`;
      } catch {
        this.userName = '';
      }

    } else {
      this.isLoggedIn = false;
      this.userName = '';
    }
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
        window.location.reload(); 

  }
}
