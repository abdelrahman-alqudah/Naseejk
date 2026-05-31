import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { SocialAuthService } from '@abacritt/angularx-social-login';
import { AuthService } from '../../../core/services/auth.service';
import { of } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  // إعداد "Mock Services" لمحاكاة سلوك الخدمات الحقيقية
  const mockSocialAuthService = {
    authState: of(null), // محاكاة حالة عدم تسجيل الدخول
    signIn: jasmine.createSpy('signIn'),
    signOut: jasmine.createSpy('signOut')
  };

  const mockAuthService = {
    externalLogin: jasmine.createSpy('externalLogin').and.returnValue(of({ token: 'mock-jwt-token' })),
    getToken: jasmine.createSpy('getToken').and.returnValue('mock-jwt-token')
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      // بما أن LoginComponent هو Standalone، نضعه في imports وليس declarations
      imports: [LoginComponent],
      providers: [
        { provide: SocialAuthService, useValue: mockSocialAuthService },
        { provide: AuthService, useValue: mockAuthService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges(); // تفعيل دورة حياة المكون (Lifecycle hooks)
  });

  it('should create the LoginComponent', () => {
    expect(component).toBeTruthy();
  });

  it('should have a title for Naseejk Identity Hub', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    // التأكد من أن العنوان يظهر بشكل صحيح للمستخدم
    expect(compiled.querySelector('h1')?.textContent).toContain('Naseejk Identity Hub');
  });

  it('should call signInWithMicrosoft when MS button is clicked', () => {
    spyOn(component, 'signInWithMicrosoft');
    const button = fixture.nativeElement.querySelector('.btn-microsoft');
    button.click();
    expect(component.signInWithMicrosoft).toHaveBeenCalled();
  });
});