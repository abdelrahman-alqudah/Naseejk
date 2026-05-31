import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService, UserData } from '../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  userData: UserData | null = null;
  isLoading = true;

  ngOnInit(): void {
    // 🚀 السطر رقم 20 القديم والمسبب للخطأ تم سحقه وإعدامه نهائياً من هنا!
    this.userData = this.authService.getUserData();
    console.log('📊 Dashboard Initial User Data:', this.userData);

    console.log('🚀 Triggering Secure Session Verification...');

    this.authService.checkSession().subscribe({
      next: (isValid) => {
        this.isLoading = false;
        console.log('🛡️ Session Verification Response:', isValid);

        if (isValid === true || (isValid as any) === 'true') {
          console.log('✅ Session is 100% Valid. Access Granted.');
          this.cdr.detectChanges(); // قهر تجمد الـ Zone.js وإجبار الشاشة على الظهور
        } else {
          console.warn('❌ Session check returned false! Triggering Logout...');
          this.authService.logout();
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.error('💥 CRITICAL: checkSession HTTP Request Failed!', err);
        this.authService.logout();
      },
    });
  }

  logout(): void {
    console.log('🚪 Manual Logout Triggered by User.');
    this.authService.logout();
  }
}
