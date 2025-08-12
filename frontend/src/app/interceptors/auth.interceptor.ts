import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { RejectedDialogComponent } from '../components/rejected-dialog/rejected-dialog';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const dialog = inject(MatDialog);

  const token = authService.getToken();

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req).pipe(
    catchError((err) => {
      if (err.status === 403 && err.error?.includes("גישה נדחתה")) {
       dialog.open(RejectedDialogComponent)
  .afterClosed()
  .subscribe(() => {
    authService.logout();
    router.navigate(['/login']);
  });
    
      } else if (err.status === 401) {
        alert("ההתחברות שלך פגה – נא להתחבר מחדש.");
        authService.logout();
        router.navigate(['/login']);
      }

      return throwError(() => err);
    })
  );
};
