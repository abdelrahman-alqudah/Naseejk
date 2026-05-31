import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // 1. استخراج التوكن من جيب الأنجولار (LocalStorage)
  const token = localStorage.getItem('naseejk_access_token');

  // 2. إذا كان التوكن موجوداً، نقوم بنسخ الطلب وإضافة الـ Header الأمني
  if (token) {
    const clonedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
    // إرسال الطلب المعدل إلى الباك-إند
    return next(clonedRequest);
  }

  // 3. إذا لم يكن هناك توكن، أرسل الطلب كما هو (سيتم رفضه بـ 401 إذا كان المسار محمياً)
  return next(req);
};
