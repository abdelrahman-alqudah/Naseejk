import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { authGuard } from './core/guards/auth-guard'; // ⬅️ استيراد الحارس
// تأكد من إنشاء هذا الكومبوننت أولاً أو استبداله بالكومبوننت الصحيح لديك
import { DashboardComponent } from './features/dashboard/dashboard.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' }, // توجيه تلقائي للـ Login
  { path: 'login', component: LoginComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard], // 🔐 البوابة الآن محمية برمجياً
  },
];
