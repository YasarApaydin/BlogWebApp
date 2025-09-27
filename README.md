# 🌐 Blog Yönetim Paneli

![Blog Yönetim Paneli](https://i.ibb.co/NdyQS4Dr/Ekran-g-r-nt-s-2025-09-26-182656.png)

Merhaba! Benim geliştirdiğim **Blog Yönetim Paneli**, modern web teknolojileriyle geliştirilmiş tam teşekküllü bir içerik yönetim sistemi (CMS)’dir.  

> Bu proje, çok katmanlı mimari yapısı, kullanıcı yönetimi ve özelleştirilmiş dashboard'u ile bir blog sisteminin tüm ihtiyaçlarını karşılamak üzere tasarlanmıştır.

---

## 🚀 Projenin Amacı

Bu projeyi geliştirirken hedefim şunlardı:  

- 🛡️ Güvenli kullanıcı ve rol yönetimi  
- ✏️ Admin paneli ile makale, kategori ve yorumları yönetebilme  
- 💼 Kullanıcıların kendi CV’lerini oluşturup PDF olarak indirebilmesi  
- 🖼️ Fotoğraf ve medya içeriklerini yönetebilme  
- 📚 Blog yazılarını, yorumları ve taslakları profil üzerinden düzenleyebilme  
- 🔐 Kullanıcının şifre, e-posta gibi bilgilerini güncelleyebilmesi  
- 🔍 Gelişmiş arama sistemi (başlık, etiket, kategori)  
- 📊 En çok okunan 3 blog yazısını listeleme  
- 🖊️ Blog yazısı oluştururken başlık, özet, görsel, içerik ve etiket ekleyebilme  
- 📱 Mobil uyumlu, modern bir kullanıcı arayüzü sunmak  

---

## ✨ Öne Çıkan Özellikler

### 👤 Kullanıcı ve Profil Yönetimi  
- Kullanıcı kayıt, giriş, düzenleme ve silme  
- Rol bazlı yetkilendirme (Admin / User)  
- SHA-256 ile şifre güvenliği  
- Profil fotoğrafı yükleme / güncelleme  
- Kullanıcı CV oluşturma ve PDF indirme 🧾  

### 📝 Makale ve Kategori Yönetimi  
- Makale CRUD işlemleri  
- Kategorilere göre filtreleme  
- Yorum ekleme / silme / onaylama  
- Geri dönüşüm kutusu → Silinen içerikleri geri yükleme ♻️  

### 🖼️ Medya ve Dosya Yönetimi  
- Fotoğrafları [https://ibb.co](https://ibb.co) gibi CDN'lerde barındırma  
- Medya URL yönetimi ve otomatik görsel yükleme sistemi  

### 🔒 Güvenlik ve Identity  
- ASP.NET Core Identity  
- 5 başarısız denemede hesap kilitleme  
- Cookie tabanlı oturum yönetimi  
- Role / Claim bazlı erişim kontrolü  

### 🎨 Dashboard ve UI  
- Bootstrap ve Custom CSS ile modern UI  
- DataTables ile gelişmiş tablo yönetimi  
- Profil kartları, okuma sayıları, grafikler  
- Tamamen responsive yapı (mobil / tablet uyumu)

---

## 🛠️ Kullanılan Teknolojiler

| Katman | Teknolojiler |
|--------|--------------|
| Backend | `ASP.NET Core MVC (.NET 9)` |
| ORM | `Entity Framework Core 8` |
| Frontend | `HTML5`, `CSS3`, `JavaScript`, `FontAwesome`, `Boxicons` |
| Veritabanı | `Microsoft SQL Server` |
| Bildirimler | `NToastNotify (Toastr)` |
| Güvenlik | `ASP.NET Core Identity`, `SHA-256`, `Cookie Auth` |
| Mimarisi | `N-Katmanlı Yapı` (Entity, Data, Service, Web) |

---

## 🏗️ N-Katmanlı Mimari

Bu proje, SOLID prensiplerine uygun, sürdürülebilir ve genişletilebilir bir **N-Katmanlı Mimari** ile yapılandırılmıştır:

1. **🧩 Entity / DTO Katmanı:**  
   `UserDto`, `ArticleDto`, `CategoryDto` gibi veri transfer nesneleri  

2. **📂 Data / Repository Katmanı:**  
   `AppDbContext`, `EfRepository`, `Fluent API`, konfigürasyonlar,Mapping kullanımı

3. **⚙️ Service / Business Katmanı:**  
   `Describers` ve servisler aracılığıyla iş mantığı, validation, özel senaryolar  

4. **🎯 Web / UI Katmanı:**  
   Razor Pages + MVC yapısı ile frontend geliştirme, ViewComponent kullanımı, arayüz kontrolleri  

---

## 📸 Örnek Ekran Görüntüleri

| Dashboard | Profil Sayfası | Makale Oluştur | Email Doğrulama |
|----------|----------------|----------------|----------------|
| ![dash](https://i.ibb.co/wZ40HLQD/Ekran-g-r-nt-s-2025-09-27-154058.png) ![dash](https://i.ibb.co/9KNnqhw/Ekran-g-r-nt-s-2025-09-27-154124.png) | ![profil](https://i.ibb.co/WNpjtt1S/Ekran-g-r-nt-s-2025-09-26-182313.png) ![profil](https://i.ibb.co/1YSWKFyX/Ekran-g-r-nt-s-2025-09-27-154526.png) | ![makale](https://i.ibb.co/wrw4Xwpj/Ekran-g-r-nt-s-2025-09-26-182131.png)  ![makale](https://i.ibb.co/p6Bd45tB/Ekran-g-r-nt-s-2025-09-27-152800.png)|![email](https://i.ibb.co/whhf8NT9/Ekran-g-r-nt-s-2025-09-26-183216.png) |

---

