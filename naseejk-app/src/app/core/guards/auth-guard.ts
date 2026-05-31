import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map, take } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // نتحقق من السيرفر إذا كانت الجلسة (الكوكي) لا تزال صالحة
  return authService.checkSession().pipe(
    take(1),
    map((isLoggedIn) => {
      if (isLoggedIn) {
        return true; // مسموح بالدخول
      } else {
        router.navigate(['/login']); // طرد لصفحة الدخول
        return false;
      }
    }),
  );
};
