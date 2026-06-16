# 🛒 TazeSepet — E-Commerce Platform

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=flat-square&logo=dotnet)
![EF Core](https://img.shields.io/badge/Entity%20Framework%20Core-Code--First-purple?style=flat-square)
![SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-MSSQL-CC2927?style=flat-square&logo=microsoftsqlserver)
![JWT Auth](https://img.shields.io/badge/Auth-Custom%20JWT-orange?style=flat-square)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

> Taze gıda, teknoloji, kitap ve daha fazlasını kapsayan; Admin, Satıcı ve Alıcı rollerinin tümünü destekleyen kapsamlı bir N-Tier e-ticaret platformu.

---

## 📋 İçindekiler

- [Proje Hakkında](#-proje-hakkında)
- [Mimari Genel Bakış](#-mimari-genel-bakış)
- [Katmanlar ve Sorumluluklar](#-katmanlar-ve-sorumluluklar)
- [Veritabanı Modeli](#-veritabanı-modeli)
- [E-Ticaret Sitesi — Özellikler](#-e-ticaret-sitesi--özellikler)
- [Admin Paneli — Özellikler](#-admin-paneli--özellikler)
- [Rol Bazlı Yetkilendirme](#-rol-bazlı-yetkilendirme)
- [JWT Kimlik Doğrulama Akışı](#-jwt-kimlik-doğrulama-akışı)
- [Kullanılan Teknolojiler](#-kullanılan-teknolojiler)
- [Kurulum ve Çalıştırma](#%EF%B8%8F-kurulum-ve-çalıştırma)
- [Proje Yapısı (Tam Ağaç)](#-proje-yapısı-tam-ağaç)
- [Ekran Görüntüleri](#-ekran-görüntüleri)

---

## 🎯 Proje Hakkında

**TazeSepet**, ASP.NET Core MVC 8.0 üzerine inşa edilmiş, katmanlı mimari prensiplerine uygun bir e-ticaret uygulamasıdır. Proje iki ana bileşenden oluşur:

- **E-Ticaret Sitesi** (`eTicaretUygulamasi.Mvc`): Ogani UI temasıyla tasarlanmış, müşterilerin ve satıcıların kullandığı ana platform.
- **Admin Paneli** (`Areas/Admin`): SB Admin 2 dashboard temasıyla izole edilmiş yönetim arayüzü.

Sistem; **Admin**, **Seller (Satıcı)** ve **Buyer (Alıcı)** olmak üzere üç farklı kullanıcı rolünü destekler ve Custom JWT tabanlı kimlik doğrulaması kullanır.

---

## 🏗️ Mimari Genel Bakış

Proje, **Separation of Concerns (Sorumlulukların Ayrılığı)** prensibine dayanan **N-Tier (Çok Katmanlı) Mimari** ile geliştirilmiştir.

```
┌──────────────────────────────────────────────────────┐
│                   PRESENTATION LAYER                 │
│  ┌──────────────────────┐  ┌────────────────────┐   │
│  │  E-Commerce Site     │  │    Admin Panel     │   │
│  │  (Ogani UI / MVC)    │  │  (SB Admin 2 / MVC)│   │
│  └──────────┬───────────┘  └─────────┬──────────┘   │
└─────────────┼─────────────────────────┼──────────────┘
              │                         │
┌─────────────▼─────────────────────────▼──────────────┐
│                  BUSINESS LOGIC LAYER                │
│   Controllers → ViewModels → Repository Pattern      │
└─────────────────────────┬────────────────────────────┘
                          │
┌─────────────────────────▼────────────────────────────┐
│                   DATA ACCESS LAYER                  │
│   App.Data → AppDbContext → EF Core → SQL Server     │
└──────────────────────────────────────────────────────┘
```

---

## 📂 Katmanlar ve Sorumluluklar

### `App.Data` — Veri Erişim Katmanı

| Klasör          | İçerik                                                        | Sorumluluk                                                         |
| --------------- | ------------------------------------------------------------- | ------------------------------------------------------------------ |
| `Context/`      | `AppDbContext.cs`, Seed Data                                  | Veritabanı bağlantısı ve başlangıç verilerinin otomatik yüklenmesi |
| `Entities/`     | `Product`, `User`, `Order`, `Category`, `Cart`, `OrderDetail` | EF Core Code-First varlık modelleri                                |
| `Repositories/` | `IDataRepository<T>`, `DataRepository<T>`                     | Generic CRUD işlemleri; tüm varlık türleri için tek bir sözleşme   |

#### Repository Pattern

```csharp
// Generic arayüz — tüm varlıklar için ortak sözleşme
public interface IDataRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

// Somut uygulama — EF Core üzerinden çalışır
public class DataRepository<T> : IDataRepository<T> where T : class { ... }
```

---

### `eTicaretUygulamasi.Mvc` — Sunum Katmanı

#### Controller Listesi ve Sorumlulukları

| Controller          | Route      | Roller             | Temel İşlevler                                           |
| ------------------- | ---------- | ------------------ | -------------------------------------------------------- |
| `AuthController`    | `/Auth`    | Herkese açık       | Kayıt, giriş, JWT token üretimi, çıkış                   |
| `HomeController`    | `/`        | Herkese açık       | Ana sayfa, ürün listeleme, kategori filtreleme, iletişim |
| `CartController`    | `/Cart`    | Buyer, Seller      | Sepete ekleme, adet güncelleme (stok kontrolü), silme    |
| `OrderController`   | `/Order`   | Buyer, Seller      | Checkout formu, sipariş oluşturma, sipariş detayı        |
| `ProductController` | `/Product` | Seller, Admin      | Ürün ekleme, düzenleme, silme (yalnızca kendi ürünleri)  |
| `ProfileController` | `/Profile` | Giriş yapan herkes | Profil görüntüleme/düzenleme, sipariş geçmişi, ürünlerim |

#### Admin Area Controller Listesi

| Controller            | Route              | Temel İşlevler                                                |
| --------------------- | ------------------ | ------------------------------------------------------------- |
| `DashboardController` | `/Admin/Dashboard` | Genel istatistikler: toplam kullanıcı, ürün, sipariş sayıları |
| `UserController`      | `/Admin/User`      | Tüm kullanıcıları listeleme, rol atama, hesap yönetimi        |
| `ProductController`   | `/Admin/Product`   | Tüm ürünleri görüntüleme, onaylama/silme                      |
| `OrderController`     | `/Admin/Order`     | Tüm siparişleri görüntüleme ve durum güncelleme               |

---

## 🗃️ Veritabanı Modeli

Veritabanı **Code-First** yaklaşımıyla oluşturulur. `EnsureCreatedAsync` ile uygulama ilk çalıştığında tüm tablolar ve seed veriler otomatik olarak yaratılır.

### Varlıklar (Entities)

```
┌──────────────┐       ┌──────────────────┐       ┌───────────────┐
│    User      │       │    Product       │       │   Category    │
├──────────────┤       ├──────────────────┤       ├───────────────┤
│ Id (PK)      │◄──┐   │ Id (PK)          │──────►│ Id (PK)       │
│ FullName     │   │   │ Name             │       │ Name          │
│ Email        │   │   │ Description      │       │ ImageUrl      │
│ PasswordHash │   │   │ Price            │       └───────────────┘
│ Role         │   │   │ Stock            │
│ PhoneNumber  │   │   │ ImageUrl         │
│ Address      │   │   │ CategoryId (FK)  │
└──────┬───────┘   │   │ SellerId   (FK)──┘
       │           │   └──────────────────┘
       │           │
       │   ┌───────┴──────┐      ┌───────────────────┐
       │   │    Order     │      │    OrderDetail    │
       │   ├──────────────┤      ├───────────────────┤
       └──►│ Id (PK)      │◄─────│ Id (PK)           │
           │ UserId (FK)  │      │ OrderId    (FK)   │
           │ TotalPrice   │      │ ProductId  (FK)   │
           │ CreatedAt    │      │ Quantity          │
           │ Status       │      │ UnitPrice         │
           └──────────────┘      └───────────────────┘

┌──────────────────┐
│    CartItem      │
├──────────────────┤
│ Id (PK)          │
│ UserId    (FK)   │
│ ProductId (FK)   │
│ Quantity         │
└──────────────────┘
```

### Kullanıcı Rolleri

| Rol      | Değer    | Yetkiler                  |
| -------- | -------- | ------------------------- |
| `Admin`  | "Admin"  | Her şeye tam erişim       |
| `Seller` | "Seller" | Ürün yönetimi + alışveriş |
| `Buyer`  | "Buyer"  | Yalnızca alışveriş        |

---

## 🛍️ E-Ticaret Sitesi — Özellikler

### Ana Sayfa (`HomeController`)

- **Ürün Listeleme** (`/Home/Listing`): Tüm ürünler kategori filtreleri ile listelenir.
- **Kategori Filtresi**: URL parametresiyle kategori bazlı ürün filtreleme.
- **Ürün Detay**: Ürün adı, açıklama, fiyat, stok durumu, satıcı bilgisi.
- **İletişim Sayfası** (`/Home/Contact`): Statik iletişim formu.

### Sepet (`CartController`)

Sepet, veritabanında `CartItem` tablosuyla kalıcı olarak saklanır (session tabanlı değil).

```
Sepete Ekle → Stok Kontrolü → Güncelle / Çıkar → Sipariş Oluştur
```

**Stok Doğrulama:** Kullanıcı sepette adet artırmak istediğinde, mevcut stokla karşılaştırılır. Stok yetersizse güncelleme engellenir ve kullanıcıya bilgi verilir.

### Sipariş (`OrderController`)

```
Sepet Özeti
    └─► Adres & Teslimat Formu (Checkout)
            └─► Sipariş Oluşturma
                    ├─► CartItem'lar → OrderDetail'e kopyalanır
                    ├─► Stok otomatik düşülür
                    └─► Sipariş Geçmişi (Profile/Orders)
```

### Profil (`ProfileController`)

| Sekme        | İçerik                                               |
| ------------ | ---------------------------------------------------- |
| Profilim     | Ad soyad, e-posta, telefon, adres düzenleme          |
| Siparişlerim | Geçmiş siparişler, tarih, tutar, ürün detayları      |
| Ürünlerim    | Yalnızca Seller rolü: kendi ürün listesi ve yönetimi |

### Ürün Yönetimi (`ProductController`)

Satıcılar kendi ürünlerini yönetebilir:

- Yeni ürün ekleme (ad, açıklama, fiyat, stok, kategori, görsel URL)
- Mevcut ürün düzenleme
- Ürün silme (yalnızca kendi ürünleri)

---

## 🔧 Admin Paneli — Özellikler

Admin paneli, `Areas/Admin` altında izole edilmiş ayrı bir MVC alanıdır. **SB Admin 2** dashboard teması kullanılmaktadır.

### Dashboard (`/Admin/Dashboard`)

Ana kontrol ekranında şu istatistikler görüntülenir:

| Kart             | Gösterilen Veri                         |
| ---------------- | --------------------------------------- |
| Toplam Kullanıcı | Sistemdeki tüm kayıtlı kullanıcı sayısı |
| Toplam Ürün      | Aktif listedeki ürün sayısı             |
| Toplam Sipariş   | Oluşturulan sipariş sayısı              |
| Toplam Gelir     | Tamamlanan sipariş tutarlarının toplamı |

### Kullanıcı Yönetimi (`/Admin/User`)

- Tüm kullanıcıları listeleme (Ad, E-posta, Rol, Kayıt Tarihi)
- Kullanıcı rolünü değiştirme (Buyer → Seller / Seller → Admin vb.)
- Kullanıcı hesabını devre dışı bırakma / silme

### Ürün Yönetimi (`/Admin/Product`)

- Tüm satıcılara ait tüm ürünleri listeleme
- İstenen ürünü silme veya düzenleme
- Kategori bazlı filtreleme

### Sipariş Yönetimi (`/Admin/Order`)

- Tüm siparişleri görüntüleme (Kullanıcı, Tarih, Tutar, Durum)
- Sipariş durumunu güncelleme (Beklemede → Onaylandı → Tamamlandı)
- Sipariş detayı inceleme (hangi ürünler, kaç adet)

---

## 👥 Rol Bazlı Yetkilendirme

Yetkilendirme, controller'lardaki `[Authorize(Roles = "...")]` attribute'ları ve custom JWT middleware aracılığıyla sağlanır.

```
Ziyaretçi (Giriş Yok)
  ├── Ana Sayfa ✓
  ├── Ürün Listesi ✓
  ├── İletişim ✓
  ├── Giriş / Kayıt ✓
  └── Sepet ✗ → Giriş'e yönlendir

Buyer (Alıcı)
  ├── Tüm ziyaretçi sayfaları ✓
  ├── Sepet işlemleri ✓
  ├── Sipariş oluşturma ✓
  ├── Profil yönetimi ✓
  └── Ürün ekleme/düzenleme ✗

Seller (Satıcı)
  ├── Tüm Buyer yetkileri ✓
  ├── Ürün ekleme ✓
  ├── Kendi ürünlerini düzenleme/silme ✓
  ├── Ürünlerim sayfası ✓
  └── Admin Paneli ✗

Admin
  ├── Tüm Seller yetkileri ✓
  ├── Admin Paneli - Dashboard ✓
  ├── Tüm kullanıcı yönetimi ✓
  ├── Tüm ürün yönetimi ✓
  └── Tüm sipariş yönetimi ✓
```

---

## 🔐 JWT Kimlik Doğrulama Akışı

Custom JWT implementasyonu `AuthController` içinde yer alır. Token, cookie üzerinde taşınır.

```
1. Kullanıcı → POST /Auth/Login (email + şifre)
        │
2. AuthController → Kullanıcıyı DB'den çek, şifreyi doğrula
        │
3. JWT Token Üret (Payload: UserId, Email, Role, Expiry)
        │
4. Token → HTTP-only Cookie'ye yaz
        │
5. Sonraki istekler: Middleware cookie'deki token'ı okur
        │
6. Token geçerli → ClaimsPrincipal oluştur → Controller'a ilet
        │
7. [Authorize(Roles="Admin")] → Role claim kontrol edilir
```

**Token Payload Örneği:**

```json
{
  "sub": "42",
  "email": "satici@example.com",
  "role": "Seller",
  "exp": 1720000000
}
```

---

## 🛠️ Kullanılan Teknolojiler

### Backend

| Teknoloji             | Versiyon | Kullanım Alanı                            |
| --------------------- | -------- | ----------------------------------------- |
| ASP.NET Core MVC      | 8.0      | Web framework, routing, controller yapısı |
| Entity Framework Core | 8.x      | ORM, Code-First veritabanı yönetimi       |
| Microsoft SQL Server  | 2019+    | İlişkisel veritabanı                      |
| Custom JWT            | —        | Kimlik doğrulama ve yetkilendirme         |
| Repository Pattern    | —        | Veri erişim soyutlaması                   |

### Frontend

| Teknoloji           | Kullanım Alanı               |
| ------------------- | ---------------------------- |
| HTML5, CSS3         | Temel işaretleme ve stil     |
| Bootstrap 4/5       | Duyarlı (responsive) layout  |
| jQuery              | DOM manipülasyonu, AJAX      |
| Ogani E-Commerce UI | E-ticaret sitesi teması      |
| SB Admin 2          | Admin panel dashboard teması |
| SCSS                | Admin panel özelleştirme     |

---

## ⚙️ Kurulum ve Çalıştırma

### Ön Koşullar

- .NET 8.0 SDK
- Microsoft SQL Server (LocalDB, Express veya Full)
- Visual Studio 2022 veya VS Code

### Adım Adım Kurulum

**1. Depoyu klonlayın:**

```bash
git clone https://github.com/bariscoskunl/TazeSepet-eCommerce-MVC.git
cd TazeSepet-eCommerce-MVC
```

**2. Bağlantı dizesini yapılandırın:**

`eTicaretUygulamasi.Mvc/appsettings.json` dosyasını açın:

```json
{
  "ConnectionStrings": {
    "Default": "Server=.;Database=eTicaretDb;Trusted_Connection=True;TrustServerCertificate=Yes;"
  }
}
```

> **Not:** SQL Server instance adınız `SQLEXPRESS` ise `Server=.\SQLEXPRESS` olarak değiştirin.

**3. Projeyi başlatın:**

Visual Studio'da `F5` tuşuna basın ya da:

```bash
cd eTicaretUygulamasi.Mvc
dotnet run
```

**4. Veritabanı otomatik oluşturulur:**

`Program.cs` içindeki `EnsureCreatedAsync` çağrısı sayesinde:

- Veritabanı tablolar otomatik oluşturulur
- Örnek kategoriler, ürünler ve kullanıcılar (Seed Data) yüklenir
- Ekstra `dotnet ef migrations` komutu **gerekmez**

**5. Varsayılan giriş bilgileri (Seed Data):**

| Rol    | E-posta              | Şifre      |
| ------ | -------------------- | ---------- |
| Admin  | admin@tazesepet.com  | Admin123!  |
| Seller | satici@tazesepet.com | Seller123! |
| Buyer  | alici@tazesepet.com  | Buyer123!  |

> Seed data'nın gerçek değerlerini `App.Data/Context/` klasöründeki ilgili dosyada bulabilirsiniz.

---

## 📁 Proje Yapısı (Tam Ağaç)

```
📦 eTicaretUygulamasi (Solution)
 │
 ┣━━ 📂 App.Data                          ← Veri Erişim Katmanı
 │    ┣━━ 📂 Context
 │    │    ┣━━ 📄 AppDbContext.cs          ← EF Core DbContext, DbSet tanımları
 │    │    ┗━━ 📄 SeedData.cs             ← Başlangıç verileri (otomatik yükleme)
 │    ┣━━ 📂 Entities
 │    │    ┣━━ 📄 User.cs                 ← Kullanıcı modeli (rol alanı dahil)
 │    │    ┣━━ 📄 Product.cs              ← Ürün modeli (stok, fiyat, kategori)
 │    │    ┣━━ 📄 Category.cs             ← Kategori modeli
 │    │    ┣━━ 📄 Order.cs                ← Sipariş başlığı (toplam tutar, durum)
 │    │    ┣━━ 📄 OrderDetail.cs          ← Sipariş satırı (ürün, adet, birim fiyat)
 │    │    ┗━━ 📄 CartItem.cs             ← Sepet kalemi (kullanıcı + ürün + adet)
 │    ┗━━ 📂 Repositories
 │         ┣━━ 📄 IDataRepository.cs      ← Generic CRUD arayüzü
 │         ┗━━ 📄 DataRepository.cs       ← EF Core tabanlı uygulama
 │
 ┗━━ 📂 eTicaretUygulamasi.Mvc            ← Sunum Katmanı (Web Uygulaması)
      ┣━━ 📂 Areas
      │    ┗━━ 📂 Admin                   ← Admin Paneli (izole alan)
      │         ┣━━ 📂 Controllers
      │         │    ┣━━ 📄 DashboardController.cs
      │         │    ┣━━ 📄 UserController.cs
      │         │    ┣━━ 📄 ProductController.cs
      │         │    ┗━━ 📄 OrderController.cs
      │         ┗━━ 📂 Views
      │              ┣━━ 📂 Dashboard     ← İstatistik kartları
      │              ┣━━ 📂 User          ← Kullanıcı listesi ve yönetimi
      │              ┣━━ 📂 Product       ← Tüm ürünler listesi
      │              ┗━━ 📂 Order         ← Tüm siparişler listesi
      ┣━━ 📂 Controllers                  ← E-Ticaret Ana Site
      │    ┣━━ 📄 AuthController.cs       ← Kayıt, giriş, çıkış, JWT
      │    ┣━━ 📄 CartController.cs       ← Sepet CRUD, stok doğrulama
      │    ┣━━ 📄 HomeController.cs       ← Anasayfa, listeleme, iletişim
      │    ┣━━ 📄 OrderController.cs      ← Checkout, sipariş oluşturma
      │    ┣━━ 📄 ProductController.cs    ← Satıcı ürün yönetimi
      │    ┗━━ 📄 ProfileController.cs    ← Profil & sipariş geçmişi
      ┣━━ 📂 Models                       ← ViewModels ve Form Modelleri
      │    ┣━━ 📄 LoginViewModel.cs
      │    ┣━━ 📄 RegisterViewModel.cs
      │    ┣━━ 📄 ProductViewModel.cs
      │    ┣━━ 📄 CartViewModel.cs
      │    ┣━━ 📄 OrderViewModel.cs
      │    ┗━━ 📄 ProfileViewModel.cs
      ┣━━ 📂 Views                        ← Ogani E-Commerce UI
      │    ┣━━ 📂 Auth
      │    │    ┣━━ 📄 Login.cshtml
      │    │    ┗━━ 📄 Register.cshtml
      │    ┣━━ 📂 Cart
      │    │    ┗━━ 📄 Edit.cshtml        ← Sepet görünümü (adet güncelleme)
      │    ┣━━ 📂 Home
      │    │    ┣━━ 📄 Index.cshtml       ← Vitrin anasayfa
      │    │    ┣━━ 📄 Listing.cshtml     ← Ürün listesi + kategori filtresi
      │    │    ┗━━ 📄 Contact.cshtml
      │    ┣━━ 📂 Order
      │    │    ┣━━ 📄 Create.cshtml      ← Checkout formu
      │    │    ┗━━ 📄 Details.cshtml     ← Sipariş özeti
      │    ┣━━ 📂 Product
      │    │    ┣━━ 📄 Create.cshtml      ← Yeni ürün ekleme formu
      │    │    ┗━━ 📄 Edit.cshtml        ← Ürün düzenleme formu
      │    ┣━━ 📂 Profile
      │    │    ┣━━ 📄 Index.cshtml       ← Profil detayı
      │    │    ┣━━ 📄 Orders.cshtml      ← Siparişlerim
      │    │    ┣━━ 📄 Products.cshtml    ← Ürünlerim (Seller)
      │    │    ┗━━ 📄 Edit.cshtml        ← Profil düzenleme
      │    ┗━━ 📂 Shared
      │         ┣━━ 📄 _Layout.cshtml     ← Dinamik header (rol bazlı menü)
      │         ┗━━ 📄 _Footer.cshtml
      ┣━━ 📄 Program.cs                   ← DI kayıtları, middleware, EnsureCreated
      ┗━━ 📄 appsettings.json             ← Bağlantı dizesi, JWT ayarları
```

---

## 📸 Ekran Görüntüleri

| Sayfa              | Açıklama                                    |
| ------------------ | ------------------------------------------- |
| Ana Sayfa          | Ogani temalı vitrin, öne çıkan ürünler      |
| Ürün Listesi       | Kategori filtreleri ile ürün grid görünümü  |
| Ürün Detay         | Fiyat, stok, satıcı bilgisi, sepete ekle    |
| Sepet              | Ürün listesi, adet güncelleme, toplam tutar |
| Checkout           | Adres formu, sipariş özeti                  |
| Profil             | Siparişlerim sekmesi, ürünlerim sekmesi     |
| Admin Dashboard    | SB Admin 2 istatistik kartları ve grafikler |
| Admin Kullanıcılar | Kullanıcı listesi ve rol yönetimi tablosu   |

---

## 📄 Lisans

Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır.

---

## 👨‍💻 Geliştirici

**Barış Coşkun** — [@bariscoskunl](https://github.com/bariscoskunl)
