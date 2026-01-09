# 📦 Sistem Inventaris KKP

Aplikasi manajemen inventaris barang berbasis web menggunakan ASP.NET Core MVC dengan dual database (SQL Server & MongoDB).

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4)
![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?logo=microsoftsqlserver)
![MongoDB](https://img.shields.io/badge/MongoDB-4EA94B?logo=mongodb&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap&logoColor=white)

---

## 📋 Deskripsi

Sistem Inventaris KKP adalah aplikasi web untuk mengelola inventaris barang dengan fitur lengkap seperti manajemen kategori, barang, transaksi masuk/keluar, serta sistem autentikasi berbasis role (Admin & User). Aplikasi ini menggunakan dual database untuk fleksibilitas dan backup data.

### ✨ Fitur Utama

- 🔐 **Authentication & Authorization** - Sistem login dengan role Admin dan User
- 📦 **Manajemen Barang** - CRUD barang dengan kategori
- 🏷️ **Manajemen Kategori** - Organisasi barang berdasarkan kategori
- 📊 **Transaksi** - Pencatatan barang masuk dan keluar
- 👥 **Manajemen User** - Admin dapat mengelola user (khusus Admin)
- 📈 **Dashboard & Laporan** - Visualisasi data inventaris
- 💾 **Dual Database** - SQL Server (utama) + MongoDB (backup/fleksibilitas)
- 🔒 **Security** - Password hashing dengan BCrypt, CSRF protection

---

## 🛠️ Teknologi

### Backend
- **ASP.NET Core 9.0** - Framework web MVC
- **Entity Framework Core 9.0** - ORM untuk SQL Server
- **SQL Server Express** - Database relasional utama
- **MongoDB 2.23.1** - Database NoSQL untuk backup

### Frontend
- **Razor Pages** - Template engine
- **Bootstrap 5** - CSS framework
- **JavaScript** - Client-side scripting

### Security & Authentication
- **Cookie Authentication** - Session management
- **BCrypt.Net** - Password hashing
- **AntiForgery Token** - CSRF protection

---

## 📁 Struktur Project

```
InventarisKKP/
├── Controllers/          # MVC Controllers
│   ├── AuthController.cs
│   ├── BarangController.cs
│   ├── KategoriController.cs
│   ├── TransaksiController.cs
│   ├── UserController.cs
│   └── HomeController.cs
├── Models/              # Data Models
│   ├── User.cs
│   ├── Barang.cs
│   ├── Kategori.cs
│   ├── MongoBarang.cs
│   └── MongoKategori.cs
├── Views/               # Razor Views
│   ├── Auth/
│   ├── Barang/
│   ├── Kategori/
│   ├── Transaksi/
│   ├── User/
│   └── Shared/
├── Data/                # Database Context
│   ├── InventarisDbContext.cs
│   └── DbInitializer.cs
├── Services/            # Business Logic
│   ├── MongoDbService.cs
│   ├── MongoBarangService.cs
│   ├── MongoKategoriService.cs
│   ├── BarangService.cs
│   └── ActivityLogService.cs
├── MongoDB/             # MongoDB Documentation
├── Scripts/             # Deployment Scripts
├── wwwroot/             # Static Files
├── Program.cs           # Application Entry Point
└── appsettings.json     # Configuration
```

---

## 🚀 Instalasi & Setup

### Prerequisites

Pastikan sudah terinstall:
- ✅ [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- ✅ [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads) atau LocalDB
- ✅ [MongoDB Community Server](https://www.mongodb.com/try/download/community) (opsional)
- ✅ Git

### Clone Repository

```bash
git clone https://github.com/username/inventaris-kkp.git
cd inventaris-kkp
```

### Konfigurasi Database

1. **SQL Server**: Edit `InventarisKKP/appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=InventarisKKP;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false"
  }
}
```

2. **MongoDB** (opsional): Pastikan MongoDB berjalan di `mongodb://127.0.0.1:27017`

### Menjalankan Aplikasi

#### Opsi 1: Script Otomatis (Recommended)

**Windows Command Prompt:**
```bash
cd InventarisKKP
run.bat
```

**PowerShell:**
```powershell
cd InventarisKKP
.\run.ps1
```

**Menu Lengkap:**
```bash
cd InventarisKKP
quick-start.bat
```

#### Opsi 2: Manual

```bash
cd InventarisKKP
dotnet restore
dotnet run
```

### Akses Aplikasi

Buka browser dan akses: **http://localhost:5000**

#### Login Default
| Role  | Username | Password  |
|-------|----------|-----------|
| Admin | `admin`  | `admin123`|
| User  | `user`   | `user123` |

---

## 📖 Dokumentasi

### Database Schema

#### SQL Server Tables
- **Users** - Data pengguna dan autentikasi
- **Kategoris** - Kategori barang
- **Barangs** - Data barang
- **TransaksiMasuks** - Transaksi barang masuk
- **TransaksiKeluars** - Transaksi barang keluar

#### MongoDB Collections
- **Kategoris** - Backup data kategori
- **Barangs** - Backup data barang dengan denormalisasi

### API Endpoints

| Method | Endpoint | Deskripsi | Role |
|--------|----------|-----------|------|
| GET | `/Auth/Login` | Halaman login | Public |
| POST | `/Auth/Login` | Proses login | Public |
| GET | `/Barang` | List barang | User/Admin |
| POST | `/Barang/Create` | Tambah barang | Admin |
| POST | `/Barang/Edit/{id}` | Edit barang | Admin |
| POST | `/Barang/Delete/{id}` | Hapus barang | Admin |
| GET | `/Kategori` | List kategori | User/Admin |
| POST | `/Kategori/Create` | Tambah kategori | Admin |
| GET | `/User` | List user | Admin |
| POST | `/User/Create` | Tambah user | Admin |

---

## 🔧 Utility Scripts

### run.bat / run.ps1
Script untuk menjalankan aplikasi dengan cepat. Otomatis:
- ✅ Kill process yang menggunakan port 5000
- ✅ Start aplikasi
- ✅ Tampilkan URL dan login info

### quick-start.bat
Menu interaktif dengan opsi:
1. Run Development Server
2. Reset Database
3. Stop Running Server
4. Exit

### reset-database.bat
Reset database ke kondisi awal (hapus semua data dan seed ulang)

### restart-and-test.bat
Restart aplikasi dan buka browser otomatis

---

## 🐛 Troubleshooting

### Error: Port 5000 Already in Use

**Solusi 1 - Gunakan Script:**
```bash
run.bat
```
Script akan otomatis kill process yang menggunakan port 5000.

**Solusi 2 - Manual:**
```bash
# Cari process
netstat -ano | findstr :5000

# Kill process
taskkill /F /PID <PID>
```

**Solusi 3 - PowerShell:**
```powershell
Get-NetTCPConnection -LocalPort 5000 | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
```

### Error: Couldn't find a project to run

**Penyebab**: Anda tidak berada di folder InventarisKKP

**Solusi**:
```bash
cd InventarisKKP
dotnet run
```

### Error: Database Connection Failed

**Solusi**: Reset database
```bash
cd InventarisKKP
reset-database.bat
```

### MongoDB Connection Error

MongoDB bersifat opsional. Jika MongoDB tidak tersedia, aplikasi tetap berjalan normal dengan SQL Server saja.

---

## 🧪 Testing

### Manual Testing
1. Login sebagai admin
2. Tambah kategori baru
3. Tambah barang dengan kategori tersebut
4. Lakukan transaksi barang masuk
5. Lakukan transaksi barang keluar
6. Cek laporan di dashboard

### MongoDB Verification
```bash
# Masuk ke MongoDB shell
mongosh

# Gunakan database
use InventarisKKP

# Lihat data kategori
db.Kategoris.find().pretty()

# Lihat data barang
db.Barangs.find().pretty()
```

---

## 🔒 Security Features

- **Password Hashing**: BCrypt dengan salt rounds
- **CSRF Protection**: AntiForgeryToken pada semua form
- **Role-Based Authorization**: Admin dan User roles
- **Session Management**: Cookie-based dengan timeout 8 jam
- **SQL Injection Prevention**: Entity Framework parameterized queries
- **XSS Protection**: Razor automatic encoding

---

## 📝 Catatan Penting

### Database
- Database SQL Server dibuat otomatis saat pertama kali run
- Data default (admin, user, kategori, barang) di-seed otomatis
- Password admin direset ke `admin123` setiap kali aplikasi start
- MongoDB bersifat opsional untuk backup dan fleksibilitas

### Development
- Hot reload: Tidak aktif (restart manual untuk melihat perubahan)
- Environment: Development
- Logging: Console output
- Port default: 5000

### Production
- Ubah connection string di `appsettings.Production.json`
- Set environment variable: `ASPNETCORE_ENVIRONMENT=Production`
- Gunakan HTTPS
- Ubah password default

---

## 🤝 Contributing

Kontribusi sangat diterima! Silakan:
1. Fork repository ini
2. Buat branch fitur (`git checkout -b feature/AmazingFeature`)
3. Commit perubahan (`git commit -m 'Add some AmazingFeature'`)
4. Push ke branch (`git push origin feature/AmazingFeature`)
5. Buat Pull Request

---

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

---

## 👨‍💻 Author

**Tim KKP**

---

## 📞 Support

Jika mengalami masalah:
1. Pastikan berada di folder `InventarisKKP`
2. Gunakan `run.bat` untuk start otomatis
3. Gunakan `reset-database.bat` jika ada masalah database
4. Clear browser cache jika tampilan tidak update
5. Buka issue di GitHub untuk bug report

---

## ✅ Checklist Deployment

- [ ] .NET 9.0 SDK terinstall
- [ ] SQL Server Express/LocalDB terinstall
- [ ] MongoDB terinstall (opsional)
- [ ] Port 5000 tidak digunakan
- [ ] Connection string sudah dikonfigurasi
- [ ] Firewall mengizinkan port 5000
- [ ] Browser modern (Chrome, Firefox, Edge)

---

**Version**: 1.0.0  
**Status**: ✅ Production Ready  
**Last Updated**: January 2026

---

## 🎯 Roadmap

- [ ] Export laporan ke Excel/PDF
- [ ] Notifikasi stok minimum
- [ ] Barcode scanner integration
- [ ] Multi-warehouse support
- [ ] REST API untuk mobile app
- [ ] Real-time dashboard dengan SignalR
- [ ] Audit trail lengkap
- [ ] Email notifications

---

Made with ❤️ by Tim KKP
