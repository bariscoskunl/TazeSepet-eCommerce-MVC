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
- [Katmanlar ve Sorumluluklar](#-katmanlar-ve-sorumluluklar)
- [Kurulum ve Çalıştırma](#-kurulum-ve-çalıştırma)
- [Varsayılan Kullanıcılar](#-varsayılan-kullanıcılar)

---

## 🎯 Proje Hakkında

**TazeSepet**, ASP.NET Core MVC 8.0 üzerine inşa edilmiş, katmanlı mimari prensiplerine uygun bir e-ticaret uygulamasıdır.

Proje iki ana bileşenden oluşur:

- **E-Ticaret Sitesi (`eTicaretUygulamasi.Mvc`)**
  - Ogani UI teması kullanılarak geliştirilmiştir.
  - Müşteriler ve satıcılar tarafından kullanılır.

- **Admin Paneli (`Areas/Admin`)**
  - SB Admin 2 Dashboard teması kullanılarak geliştirilmiştir.
  - Yönetim işlemlerinin tamamı bu bölümden gerçekleştirilir.

Sistem;

- **Admin**
- **Seller (Satıcı)**
- **Buyer (Alıcı)**

olmak üzere üç farklı kullanıcı rolünü desteklemektedir.

Kimlik doğrulama işlemleri Custom JWT altyapısı ile gerçekleştirilmektedir.

---

## 📂 Katmanlar ve Sorumluluklar

### `App.Data` — Veri Erişim Katmanı

| Klasör          | İçerik                                                        | Sorumluluk                                                |
| --------------- | ------------------------------------------------------------- | --------------------------------------------------------- |
| `Context/`      | `AppDbContext.cs`, Seed Data                                  | Veritabanı bağlantısı ve başlangıç verilerinin yüklenmesi |
| `Entities/`     | `Product`, `User`, `Order`, `Category`, `Cart`, `OrderDetail` | EF Core Code-First varlık modelleri                       |
| `Repositories/` | `IDataRepository<T>`, `DataRepository<T>`                     | Generic CRUD işlemleri                                    |

---

## ⚙️ Kurulum ve Çalıştırma

### Ön Koşullar

- .NET 8 SDK
- Microsoft SQL Server
- Visual Studio 2022 veya VS Code

### 1. Depoyu Klonlayın

```bash
git clone https://github.com/bariscoskunl/TazeSepet-eCommerce-MVC.git
cd TazeSepet-eCommerce-MVC
```

### 2. appsettings.json Dosyalarını Oluşturun

Güvenlik nedeniyle gerçek veritabanı bağlantıları ve JWT anahtarları GitHub deposunda yer almamaktadır.

Her katmandaki `appsettings.Example.json` dosyasını kopyalayıp adını `appsettings.json` olarak değiştirin.

Örnek yapılandırma:

```json
{
  "ConnectionStrings": {
    "Default": "Server=YOUR_SERVER_NAME;Database=eTicaretDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Secret": "EN_AZ_32_KARAKTERLI_GUVENLI_BIR_ANAHTAR"
  }
}
```

SQL Server Express kullanıyorsanız:

```json
"Server=.\\SQLEXPRESS;"
```

şeklinde güncelleyebilirsiniz.

### 3. Projeyi Çalıştırın

```bash
cd eTicaretUygulamasi.Mvc
dotnet run
```

### 4. Veritabanı Otomatik Oluşturulur

Uygulama ilk kez çalıştırıldığında:

- Veritabanı oluşturulur.
- Tablolar oluşturulur.
- Seed Data sisteme yüklenir.
- Örnek kullanıcılar ve ürünler eklenir.

Bu proje `EnsureCreatedAsync()` kullandığı için ayrıca Migration oluşturmanız gerekmez.

---

## 👤 Varsayılan Kullanıcılar

| Rol    | E-Posta                                   | Şifre |
| ------ | ----------------------------------------- | ----- |
| Admin  | [admin@mail.com](mailto:admin@mail.com)   | 1234  |
| Seller | [ahmet@mail.com](mailto:ahmet@mail.com)   | 1234  |
| Buyer  | [zeynep@mail.com](mailto:zeynep@mail.com) | 1234  |

---

## 🛠️ Kullanılan Teknolojiler

- ASP.NET Core MVC 8
- Entity Framework Core
- SQL Server
- LINQ
- JWT Authentication
- N-Tier Architecture
- Repository Pattern
- Bootstrap 5
- Ogani UI Theme
- SB Admin 2 Dashboard

---

## 👨‍💻 Geliştirici

**Barış Coşkun**

GitHub: https://github.com/bariscoskunl

---

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.
