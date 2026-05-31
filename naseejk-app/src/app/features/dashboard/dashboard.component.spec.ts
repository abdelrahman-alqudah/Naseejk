import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DashboardComponent } from './dashboard.component'; // 👈 التصحيح هنا
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      // 🎯 ملاحظة للمهندس: أضفت HttpClientTestingModule لأن الكومبوننت يعتمد على AuthService
      imports: [DashboardComponent, HttpClientTestingModule],
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges(); // 👈 استخدام detectChanges بدلاً من await fixture.whenStable() في البداية
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
