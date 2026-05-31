import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth-interceptor';
import {
  SocialAuthServiceConfig,
  GoogleLoginProvider,
  MicrosoftLoginProvider,
} from '@abacritt/angularx-social-login';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    // 🚀 نقل الإعدادات إلى هنا لتعمل في كل مكان في التطبيق
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(
              '118218060206-8scr9l52bj451rgk3834n2dqq2q6igr9.apps.googleusercontent.com',
            ),
          },
          {
            id: MicrosoftLoginProvider.PROVIDER_ID,
            provider: new MicrosoftLoginProvider('4c5c4fb2-a2f6-45bc-8f8e-5c97864c1ac2', {
              scopes: ['openid', 'profile', 'email', 'offline_access'],
            }),
          },
        ],
        onError: (err) => console.error('Social Auth Error:', err),
      } as SocialAuthServiceConfig,
    },
  ],
};
