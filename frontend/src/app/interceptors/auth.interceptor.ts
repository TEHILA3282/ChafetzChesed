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

  let instHeaders: Record<string, string> = {};
  try {
    const path = (window?.location?.pathname || '').toLowerCase(); 
    const firstSeg = path.split('/').filter(Boolean)[0];           
    if (firstSeg && firstSeg.startsWith('gmach')) {
      instHeaders['X-Institution-Slug'] = firstSeg;                 
    } else {
      instHeaders['X-Institution-Id'] = '1';                     
    }
  } catch {
  }

  const token = authService.getToken();
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
        ...instHeaders
      }
    });
  } else {
    req = req.clone({ setHeaders: { ...instHeaders } });
  }

  return next(req).pipe(
    catchError((err) => {
      if (err.status === 403 && (typeof err.error === 'string') && err.error.includes('גישה נדחתה')) {
        dialog.open(RejectedDialogComponent).afterClosed().subscribe(() => {
          authService.logout();
          router.navigate(['/login']);
        });
      } else if (err.status === 401) {
        authService.logout();
        router.navigate(['/login']);
      } else if (err.status === 400 && err.error?.message?.includes('Institution is not resolved')) {
        router.navigate(['/login']);
      }

      return throwError(() => err);
    })
  );
};
