import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

export interface UserData {
  id?: string;
  name?: string;
  email?: string;
  institution?: string; // 🚀 أصبحت ديناميكية الآن
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7180/api/auth';
  private http = inject(HttpClient);
  private router = inject(Router);

  public naseejkExternalLogin(idToken: string, microsoftRefreshToken: string): Observable<any> {
    const deviceId = this.getOrCreateDeviceId();
    const body = { provider: 'Microsoft', idToken: idToken };
    const headers = new HttpHeaders({
      'X-Device-Id': deviceId,
      'X-External-Refresh-Token': microsoftRefreshToken || '',
    });

    return this.http.post(`${this.apiUrl}/external-login`, body, { headers });
  }

  public checkSession(): Observable<boolean> {
    const token = localStorage.getItem('naseejk_access_token');
    if (!token) return of(false);
    return this.http.get<boolean>(`${this.apiUrl}/check-session`);
  }

  public getUserData(): UserData | null {
    const token = localStorage.getItem('naseejk_access_token');
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);

      return {
        // 🎯 سحب الاسم الرباعي من UniqueName
        name: decoded.unique_name || 'User',

        email: decoded.email || decoded.sub,

        // 🚀 القضاء على الـ Static: نسحب اسم المؤسسة من الـ Claim الجديد
        // إذا لم يرسله السيرفر، سنظهر "N/A" لنعلم أن هناك نقصاً في البيانات
        institution: decoded.institution || 'N/A',
      };
    } catch (error) {
      console.error('JWT Decode Error:', error);
      return null;
    }
  }

  public logout(): void {
    const deviceId = localStorage.getItem('naseejk_device_id') || '';
    const headers = new HttpHeaders({ 'X-Device-Id': deviceId });

    this.http.post(`${this.apiUrl}/logout`, {}, { headers }).subscribe({
      next: () => this.clearLocalSession(),
      error: () => this.clearLocalSession(),
    });
  }

  private getOrCreateDeviceId(): string {
    let deviceId = localStorage.getItem('naseejk_device_id');
    if (!deviceId) {
      deviceId = crypto.randomUUID();
      localStorage.setItem('naseejk_device_id', deviceId);
    }
    return deviceId;
  }

  private clearLocalSession(): void {
    localStorage.removeItem('naseejk_access_token');
    localStorage.removeItem('naseejk_refresh_token');
    this.router.navigate(['/login']);
  }
}
