# 📦 Sistem Inventaris KKP

Aplikasi manajemen inventaris barang berbasis web menggunakan **ASP.NET Core MVC** dengan **dual database** (SQL Server & MongoDB).

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4)
![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?logo=microsoftsqlserver)
![MongoDB](https://img.shields.io/badge/MongoDB-4EA94B?logo=mongodb\&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap\&logoColor=white)

---

## 🧑‍🎓 Profil

| Variable        | Isi                                 |
| --------------- | ----------------------------------- |
| **Nama**        | Bayu Aji Yuwono                     |
| **NIM**         | 312310492                           |
| **Kelas**       | TI.23.A.5                           |
| **Mata Kuliah** | Pemrograman Visual (Desktop)        |
| **Dosen**       | Dr. Muhamad Fatchan, S.Kom., M.Kom. |

---

## 📋 Deskripsi

**Sistem Inventaris KKP** adalah aplikasi web untuk mengelola inventaris barang dengan fitur lengkap seperti manajemen kategori, barang, transaksi masuk/keluar, serta sistem autentikasi berbasis **role** (Admin & User). Aplikasi ini menerapkan **dual database** untuk fleksibilitas dan kebutuhan backup data.

---

## ✨ Fitur Utama

* 🔐 **Authentication & Authorization** — Login dengan role Admin dan User
* 📦 **Manajemen Barang** — CRUD barang terintegrasi kategori
* 🏷️ **Manajemen Kategori** — Pengelompokan barang
* 📊 **Transaksi** — Pencatatan barang masuk dan keluar
* 👥 **Manajemen User** — Pengelolaan pengguna (khusus Admin)
* 📈 **Dashboard & Laporan** — Visualisasi data inventaris
* 💾 **Dual Database** — SQL Server (utama) + MongoDB (backup)
* 🔒 **Security** — Password hashing (BCrypt), CSRF protection

---

## 🛠️ Teknologi

### Backend

* **ASP.NET Core 9.0** — Framework web MVC
* **Entity Framework Core 9.0** — ORM untuk SQL Server
* **SQL Server Express / LocalDB** — Database relasional utama
* **MongoDB 2.23.1** — Database NoSQL (backup)

### Frontend

* **Razor Pages** — Template engine
* **Bootstrap 5** — CSS framework
* **JavaScript** — Client-side scripting

### Security & Authentication

* **Cookie Authentication** — Session management
* **BCrypt.Net** — Password hashing
* **AntiForgery Token** — CSRF protection

---

## 📁 Struktur Project

```text
InventarisKKP/
├── Controllers/          # MVC Controllers
│   ├── AuthController.cs
│   ├── BarangController.cs
│   ├── KategoriController.cs
│   ├── TransaksiController.cs
│   ├── UserController.cs
│   └── HomeController.cs
├── Models/               # Data Models
│   ├── User.cs
│   ├── Barang.cs
│   ├── Kategori.cs
│   ├── MongoBarang.cs
│   └── MongoKategori.cs
├── Views/                # Razor Views
│   ├── Auth/
│   ├── Barang/
│   ├── Kategori/
│   ├── Transaksi/
│   ├── User/
│   └── Shared/
├── Data/                 # Database Context
│   ├── InventarisDbContext.cs
│   └── DbInitializer.cs
├── Services/             # Business Logic
│   ├── MongoDbService.cs
│   ├── MongoBarangService.cs
│   ├── MongoKategoriService.cs
│   ├── BarangService.cs
│   └── ActivityLogService.cs
├── MongoDB/              # MongoDB Documentation
├── Scripts/              # Deployment Scripts
├── wwwroot/              # Static Files
├── Program.cs            # Application Entry Point
└── appsettings.json      # Configuration
```

---

## 🚀 Instalasi & Setup

### Prerequisites

Pastikan sudah terinstall:

* ✅ [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* ✅ SQL Server Express / LocalDB
* ✅ MongoDB Community Server (opsional)
* ✅ Git

### Clone Repository

```bash
git clone https://github.com/username/inventaris-kkp.git
cd inventaris-kkp
```

### Konfigurasi Database

**SQL Server** — edit `InventarisKKP/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=InventarisKKP;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false"
  }
}
```

**MongoDB (Opsional)** — Pastikan berjalan di:

```
mongodb://127.0.0.1:27017
```

### Menjalankan Aplikasi

#### Opsi 1 — Script Otomatis (Disarankan)

```bash
cd InventarisKKP
run.bat
```

PowerShell:

```powershell
cd InventarisKKP
.\run.ps1
```

Menu Lengkap:

```bash
cd InventarisKKP
quick-start.bat
```

#### Opsi 2 — Manual

```bash
cd InventarisKKP
dotnet restore
dotnet run
```

### Akses Aplikasi

Buka browser:

```
http://localhost:5000
```

#### Login Default

| Role  | Username | Password |
| ----- | -------- | -------- |
| Admin | admin    | admin123 |
| User  | user     | user123  |

---

## 📖 Dokumentasi Teknis

### Database Schema

**SQL Server**

* Users
* Kategoris
* Barangs
* TransaksiMasuks
* TransaksiKeluars

**MongoDB**

* Kategoris (backup)
* Barangs (denormalisasi)

### API Endpoints (Ringkas)

| Method | Endpoint            | Role       |
| ------ | ------------------- | ---------- |
| GET    | /Auth/Login         | Public     |
| POST   | /Auth/Login         | Public     |
| GET    | /Barang             | User/Admin |
| POST   | /Barang/Create      | Admin      |
| POST   | /Barang/Edit/{id}   | Admin      |
| POST   | /Barang/Delete/{id} | Admin      |
| GET    | /Kategori           | User/Admin |
| POST   | /Kategori/Create    | Admin      |
| GET    | /User               | Admin      |
| POST   | /User/Create        | Admin      |

---

## 🔧 Utility Scripts

* **run.bat / run.ps1** — Menjalankan aplikasi otomatis
* **quick-start.bat** — Menu interaktif (run, reset DB, stop server)
* **reset-database.bat** — Reset database + seed ulang
* **restart-and-test.bat** — Restart & auto-open browser

---

## 🐛 Troubleshooting

### Port 5000 Digunakan

```bash
netstat -ano | findstr :5000
taskkill /F /PID <PID>
```

Atau gunakan `run.bat`.

### Database Error

```bash
reset-database.bat
```

### MongoDB Error

MongoDB bersifat **opsional**. Aplikasi tetap berjalan dengan SQL Server.

---

## 🧪 Testing

1. Login sebagai Admin
2. Tambah kategori
3. Tambah barang
4. Transaksi barang masuk
5. Transaksi barang keluar
6. Cek dashboard

Verifikasi MongoDB:

```bash
mongosh
use InventarisKKP
db.Kategoris.find()
db.Barangs.find()
```

---

## 🔒 Security

* BCrypt password hashing
* CSRF protection
* Role-based authorization
* Cookie session (timeout 8 jam)
* SQL Injection prevention (EF Core)
* XSS protection (Razor encoding)

---

## 📝 Catatan

* Database dibuat otomatis saat pertama run
* Data default di-seed otomatis
* Password admin di-reset ke `admin123` saat start
* Environment: Development
* Port default: **5000**

---

## 🤝 Contributing

1. Fork repository
2. Buat branch fitur
3. Commit perubahan
4. Push ke branch
5. Buat Pull Request

---

## 📄 License

MIT License

---

## 👨‍💻 Author

**Tim KKP**

---

## 🎯 Roadmap

* Export laporan (Excel/PDF)
* Notifikasi stok minimum
* Barcode scanner
* Multi-warehouse
* REST API (mobile)
* Real-time dashboard (SignalR)
* Audit trail
* Email notification

---

**Version**: 1.0.0
**Status**: ✅ Production Ready
**Last Updated**: January 2026

