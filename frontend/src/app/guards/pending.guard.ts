import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PendingGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Observable<boolean> {
    const user = this.authService.getCurrentUser();

    if (!user) {
      this.router.navigate(['/login']);
      return of(false);
    }

    return this.authService.fetchUserFromServer(user.id).pipe(
      map((updatedUser: any) => {
        if (updatedUser.registrationStatus === 'ממתין') {
          this.router.navigate(['/awaiting-approval']);
          return false;
        }
        return true;
      }),
      catchError(err => {
        console.error('שגיאה בקבלת המשתמש מהשרת', err);
        this.router.navigate(['/login']);
        return of(false);
      })
    );
  }

}