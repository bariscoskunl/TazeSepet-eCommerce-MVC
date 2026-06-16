# TazeSepet (e-Commerce Platform) 🛒

[![TR](https://img.shields.io/badge/Language-TR-red.svg)](#türkçe-tr) [![EN](https://img.shields.io/badge/Language-EN-blue.svg)](#english-en)

---

## Türkçe (TR) 🇹🇷

TazeSepet, kullanıcıların taze gıda, teknoloji, kitap gibi çeşitli kategorilerdeki ürünleri keşfedebileceği, sepetine ekleyebileceği ve güvenle sipariş oluşturabileceği, modern mimariyle geliştirilmiş kapsamlı bir e-ticaret platformudur. Ogani e-ticaret teması üzerine kurulan kullanıcı dostu arayüzünün arkasında, sağlam ve güvenilir bir .NET altyapısı çalışır.

### 🏗️ Mimari ve Teknolojiler

Proje, katmanlı mimari prensiplerine (N-Tier Architecture) sadık kalınarak, Separation of Concerns (Sorumlulukların Ayrılığı) prensibiyle iki ana proje üzerinden geliştirilmiştir.

* **Backend Framework:** ASP.NET Core MVC 8.0
* **Veritabanı Erişimi:** Entity Framework Core (Code-First Approach)
* **Veritabanı:** Microsoft SQL Server
* **Kimlik Doğrulama (Auth):** Custom JWT (JSON Web Token) Authentication
* **Tasarım Deseni:** Repository Pattern (`IDataRepository` & `DataRepository`)
* **Arayüz (E-Ticaret):** HTML5, CSS3, Bootstrap, jQuery (Ogani E-Commerce UI)
* **Arayüz (Admin Paneli):** SB Admin 2 Dashboard UI

#### 📂 Proje Dizin Yapısı (Architecture Tree)

```text
📦 eTicaretUygulamasi (Solution)
 ┣ 📂 App.Data (Veri Erişim Katmanı / Data Layer)
 ┃ ┣ 📂 Context      # Veritabanı bağlantısı (AppDbContext) ve Otomatik Veri Ekleme (Seed Data)
 ┃ ┣ 📂 Entities     # Veritabanı tablolarının modelleri (Product, User, Order, Category vb.)
 ┃ ┗ 📂 Repositories # Generic Veritabanı işlemleri (IDataRepository & DataRepository)
 ┃
 ┗ 📂 eTicaretUygulamasi.Mvc (Sunum Katmanı / Presentation Layer)
   ┣ 📂 Areas
   ┃ ┗ 📂 Admin      # YÖNETİCİ PANELİ (SB Admin 2)
   ┃   ┣ 📂 Controllers # Dashboard, Kullanıcı ve İstatistik Yönetimi
   ┃   ┗ 📂 Views       # Admin paneli arayüz sayfaları
   ┃
   ┣ 📂 Controllers  # E-TİCARET ANA SAYFASI (Routing & İş Mantığı)
   ┃ ┣ 📜 AuthController.cs    # Kayıt, Giriş, JWT Token Üretimi
   ┃ ┣ 📜 CartController.cs    # Sepet İşlemleri
   ┃ ┣ 📜 HomeController.cs    # Ana Sayfa ve Ürün Listeleme
   ┃ ┣ 📜 OrderController.cs   # Ödeme ve Sipariş Oluşturma
   ┃ ┣ 📜 ProductController.cs # Satıcılar için Yeni Ürün Ekleme/Düzenleme
   ┃ ┗ 📜 ProfileController.cs # Kullanıcı Profili ve Geçmiş Siparişler
   ┃
   ┣ 📂 Models       # ViewModels (Arayüze taşınan güvenli veri yapıları ve form doğrulamaları)
   ┃
   ┗ 📂 Views        # E-TİCARET ARAYÜZÜ (Ogani Template)
     ┣ 📂 Auth       # Login ve Register ekranları
     ┣ 📂 Cart       # Sepet (Cart Edit)
     ┣ 📂 Home       # Anasayfa (Index), Ürün Listesi (Listing), İletişim (Contact)
     ┣ 📂 Order      # Ödeme/Checkout (Create), Sipariş Özeti (Details)
     ┣ 📂 Product    # Ürün Yönetimi
     ┣ 📂 Profile    # Siparişlerim, Ürünlerim, Profil Düzenle
     ┗ 📂 Shared     # Dinamik Header, Footer ve Layout dosyaları
```

### 🚀 Özellikler

* **Rol Bazlı Yetkilendirme:** `Admin`, `Seller` (Satıcı) ve `Buyer` (Alıcı) rolleri mevcuttur.
* **Ürün ve Kategori Yönetimi:** Satıcılar kendi ürünlerini ekleyebilir, silebilir ve stoklarını güncelleyebilir.
* **Sepet (Cart) İşlemleri:** Ürünler sepete eklenebilir, adet güncellemeleri stok durumuna göre (Stok sınırı) doğrulanır.
* **Sipariş Yönetimi:** Sepetteki ürünlerle sipariş oluşturulur ve `Siparişlerim` sayfasından geçmiş siparişlerin detayı incelenebilir.
* **Admin Paneli:** Yöneticiye özel dashboard, kullanıcı ve ürün istatistiklerinin kontrol edildiği, ayrı bir `Area` içerisinde izole edilmiş yönetim arayüzü.

### ⚙️ Kurulum ve Çalıştırma

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

1. Depoyu bilgisayarınıza klonlayın.
2. `eTicaretUygulamasi.Mvc` projesi altındaki `appsettings.json` dosyasını açın.
3. **Veritabanı Bağlantı Ayarı:** Proje varsayılan olarak yerel SQL Server örneğinizde çalışacak şekilde ayarlanmıştır (`Server=.`).
   ```json
   "ConnectionStrings": {
     "Default": "Server=.;Database=eTicaretDb;Trusted_Connection=True;TrustServerCertificate=Yes;"
   }
   ```
   *Eğer SQL Server instance adınız `.` (localhost) değilse (örneğin `SQLEXPRESS`), `Server=.\SQLEXPRESS` olarak değiştirin.*
4. Visual Studio üzerinden projeyi **IIS Express** veya **eTicaretUygulamasi.Mvc** profili ile başlatın (F5).
5. Sistem çalışırken `Program.cs` içerisindeki `EnsureCreatedAsync` sayesinde veritabanı ve örnek veriler (Seed Data) otomatik olarak oluşturulacaktır. Herhangi bir ekstra *Migration* komutu çalıştırmanıza gerek yoktur!

---

## English (EN) 🇬🇧

TazeSepet is a comprehensive e-commerce platform built with modern architecture where users can discover products across various categories, add them to their carts, and place orders securely. Behind its user-friendly interface built on the Ogani e-commerce template, lies a solid and reliable .NET backend.

### 🏗️ Architecture and Technologies

The project was developed adhering to N-Tier Architecture and Separation of Concerns principles, divided into two main projects.

* **Backend Framework:** ASP.NET Core MVC 8.0
* **Database Access:** Entity Framework Core (Code-First Approach)
* **Database:** Microsoft SQL Server
* **Authentication:** Custom JWT (JSON Web Token) Authentication
* **Design Pattern:** Repository Pattern (`IDataRepository` & `DataRepository`)
* **Frontend (E-Commerce):** HTML5, CSS3, Bootstrap, jQuery (Ogani E-Commerce UI)
* **Frontend (Admin Panel):** SB Admin 2 Dashboard UI

#### 📂 Project Directory Structure (Architecture Tree)

```text
📦 eTicaretUygulamasi (Solution)
 ┣ 📂 App.Data (Data Access Layer)
 ┃ ┣ 📂 Context      # DB Connection (AppDbContext) and Auto Seed Data
 ┃ ┣ 📂 Entities     # Database table models (Product, User, Order, Category etc.)
 ┃ ┗ 📂 Repositories # Generic DB operations (IDataRepository & DataRepository)
 ┃
 ┗ 📂 eTicaretUygulamasi.Mvc (Presentation Layer)
   ┣ 📂 Areas
   ┃ ┗ 📂 Admin      # ADMIN PANEL (SB Admin 2)
   ┃   ┣ 📂 Controllers # Dashboard, User and Statistics Management
   ┃   ┗ 📂 Views       # Admin panel interface pages
   ┃
   ┣ 📂 Controllers  # E-COMMERCE MAIN SITE (Routing & Business Logic)
   ┃ ┣ 📜 AuthController.cs    # Registration, Login, JWT Token Generation
   ┃ ┣ 📜 CartController.cs    # Shopping Cart Operations
   ┃ ┣ 📜 HomeController.cs    # Home Page and Product Listing
   ┃ ┣ 📜 OrderController.cs   # Checkout and Order Creation
   ┃ ┣ 📜 ProductController.cs # Product Add/Edit for Sellers
   ┃ ┗ 📜 ProfileController.cs # User Profile and Past Orders
   ┃
   ┣ 📂 Models       # ViewModels (Secure data transfer objects and form validations)
   ┃
   ┗ 📂 Views        # E-COMMERCE UI (Ogani Template)
     ┣ 📂 Auth       # Login and Register screens
     ┣ 📂 Cart       # Cart Edit/View
     ┣ 📂 Home       # Index, Listing, Contact
     ┣ 📂 Order      # Checkout (Create), Order Details (Details)
     ┣ 📂 Product    # Product Management
     ┣ 📂 Profile    # My Orders, My Products, Edit Profile
     ┗ 📂 Shared     # Dynamic Headers, Footers, and Layouts
```

### 🚀 Features

* **Role-Based Authorization:** Features `Admin`, `Seller`, and `Buyer` roles.
* **Product & Category Management:** Sellers can add, delete, and update their own products and stocks.
* **Cart Operations:** Products can be added to the cart; quantity updates are validated against the available stock limit.
* **Order Management:** Orders can be placed using items in the cart, and past order details can be viewed from the `My Orders` page.
* **Admin Dashboard:** A dedicated management interface isolated in its own `Area` for administrators to monitor user and product statistics.

### ⚙️ Installation & Running Locally

Follow the steps below to run the project on your local machine:

1. Clone the repository to your computer.
2. Open the `appsettings.json` file located under the `eTicaretUygulamasi.Mvc` project.
3. **Database Connection String:** The project is configured to run on your local SQL Server instance by default using (`Server=.`).
   ```json
   "ConnectionStrings": {
     "Default": "Server=.;Database=eTicaretDb;Trusted_Connection=True;TrustServerCertificate=Yes;"
   }
   ```
   *If your SQL Server instance name is not `.` (localhost) (for example, `SQLEXPRESS`), change it to `Server=.\SQLEXPRESS`.*
4. Start the project via Visual Studio using the **IIS Express** or **eTicaretUygulamasi.Mvc** profile (F5).
5. When the system starts, the database and seed data will be generated automatically thanks to `EnsureCreatedAsync` in `Program.cs`. You do not need to run any extra *Migration* commands!
