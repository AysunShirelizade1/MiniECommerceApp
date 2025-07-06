Mini E-Ticarət API
Mini E-Ticarət API — .NET 8 Web API əsasında hazırlanmış sadə e-ticarət backend layihəsidir. Layihə istifadəçilərə məhsulların idarəsi, sifarişlərin emalı, istifadəçi idarəsi və email bildirişləri funksiyalarını təqdim edir.

Əsas Xüsusiyyətlər
İstifadəçi qeydiyyatı və autentifikasiyası (JWT ilə)

Rol əsaslı səlahiyyətlər (Admin, Seller, Buyer və s.)

Məhsulların əlavə olunması, redaktəsi və silinməsi

Kateqoriyalar üzrə məhsulların filtrasiyası

Sifarişlərin yaradılması və statusunun dəyişdirilməsi

İstifadəçiyə email vasitəsilə sifariş təsdiqi göndərilməsi (SMTP ilə)

Məhsul şəkillərinin idarəsi

RESTful API endpointlər

Texnologiyalar
.NET 8 Web API

Entity Framework Core (SQL Server)

JWT Authentication

SMTP Email Service

Repository və Service Patternlər

Swagger (OpenAPI) sənədləşdirmə

API İstifadəsi
Layihənin Swagger UI-dan və ya Postman kimi vasitələrdən istifadə etməklə bütün endpointlərə sorğular göndərmək mümkündür.

İstifadəçi Rolları
Admin: Bütün idarəetmə hüquqları

Seller: Məhsul əlavə etmə, redaktə və silmə

Buyer: Məhsulları baxış və sifariş vermə

Gələcək Planlar
İstifadəçi reytinq və rəylər sistemi əlavə etmək

Ödəniş inteqrasiyası (Payment Gateway)

Daha geniş statistik və hesabat modulları
