import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import {
  SocialAuthService,
  MicrosoftLoginProvider,
  GoogleSigninButtonModule,
} from '@abacritt/angularx-social-login';
import { AuthService } from '../../../core/services/auth.service';
import { Subscription } from 'rxjs'; // 🚀 استيراد كائن الاشتراك

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, GoogleSigninButtonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit, OnDestroy {
  private socialAuthService = inject(SocialAuthService);
  private naseejkAuth = inject(AuthService);
  private router = inject(Router);

  // 🚀 حاوية قفل قسرية للاشتراك لمنع تسريب الذاكرة
  private authSubscription?: Subscription;

  ngOnInit() {
    this.authSubscription = this.socialAuthService.authState.subscribe((user) => {
      if (user) {
        console.log('⚡ Social Auth State Emitted for:', user.email);
        const microsoftRefresh = user.response?.refreshToken || '';

        this.naseejkAuth.naseejkExternalLogin(user.idToken, microsoftRefresh).subscribe({
          next: (response: any) => {
            const source = response.data || response;
            const accessToken = source.accessToken || source.AccessToken || source.token;
            const refreshToken = source.refreshToken || source.RefreshToken;

            if (accessToken && refreshToken) {
              localStorage.setItem('naseejk_access_token', accessToken);
              localStorage.setItem('naseejk_refresh_token', refreshToken);
              this.router.navigate(['/dashboard']);
            }
          },
          error: (err) => {
            console.error('❌ Backend Login Failed:', err);
          },
        });
      }
    });
  }

  signInWithMicrosoft(): void {
    this.socialAuthService.signIn(MicrosoftLoginProvider.PROVIDER_ID);
  }

  // 🚨 إعدام الاشتراكات المعلقة فور تدمير الكومبوننت
  ngOnDestroy(): void {
    if (this.authSubscription) {
      this.authSubscription.unsubscribe();
      console.log('🧹 Clean Up: Social Auth Subscription successfully killed to prevent loops.');
    }
  }
}
