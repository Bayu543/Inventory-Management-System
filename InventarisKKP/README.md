# 📦 Sistem Inventaris KKP

Aplikasi manajemen inventaris berbasis ASP.NET Core MVC.

---

## 🚀 Cara Menjalankan

### Opsi 1: Script Otomatis (Recommended)

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

### Opsi 2: Manual

```bash
cd InventarisKKP
dotnet run
```

### Opsi 3: Dari Root Folder

```bash
cd "C:\Inventory Management System\InventarisKKP"
dotnet run
```

---

## 🌐 Akses Aplikasi

**URL**: http://localhost:5000

### Login Default
- **Admin**: `admin` / `admin123`
- **User**: `user` / `user123`

---

## ✨ Fitur

- 🔐 Authentication & Authorization (Admin/User)
- 📦 Manajemen Barang & Kategori
- 📊 Transaksi Barang Masuk/Keluar
- 👥 Manajemen User (Admin only)
- 📈 Dashboard & Laporan

---

## 🛠️ Teknologi

- ASP.NET Core 8.0 MVC
- SQL Server (LocalDB)
- Entity Framework Core
- Bootstrap 5
- BCrypt (Password Hashing)

---

## 📁 Struktur Project

```
InventarisKKP/
├── Controllers/    # MVC Controllers
├── Models/         # Data Models
├── Views/          # Razor Views
├── Data/           # DbContext
├── Services/       # Business Logic
├── wwwroot/        # Static Files
├── Scripts/        # Deployment Scripts
│
├── run.bat         # Quick start (CMD)
├── run.ps1         # Quick start (PowerShell)
├── quick-start.bat # Menu lengkap
└── README.md       # Dokumentasi ini
```

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
Reset database ke kondisi awal (hapus semua data)

### restart-and-test.bat
Restart aplikasi dan buka browser

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

---

### Error: Couldn't find a project to run

**Penyebab**: Anda tidak berada di folder InventarisKKP

**Solusi**:
```bash
cd InventarisKKP
dotnet run
```

Atau gunakan path lengkap:
```bash
cd "C:\Inventory Management System\InventarisKKP"
dotnet run
```

---

### Error: Database Connection Failed

**Solusi**: Reset database
```bash
reset-database.bat
```

Atau manual:
```bash
# Hapus file database
del InventarisKKP.db
del InventarisKKP.db-shm
del InventarisKKP.db-wal

# Jalankan ulang aplikasi
dotnet run
```

---

### Browser Cache Issues

**Solusi**: Clear cache
```
Ctrl + Shift + R  (Chrome/Edge)
Ctrl + F5         (Firefox)
```

Atau gunakan Incognito/Private mode.

---

## 📝 Catatan Penting

### Database
- Database dibuat otomatis saat pertama kali run
- Data default (admin, user, kategori, barang) di-seed otomatis
- Password admin direset ke `admin123` setiap kali aplikasi start

### Security
- Password di-hash dengan BCrypt
- CSRF protection dengan AntiForgeryToken
- Role-based authorization
- Session timeout: 8 jam

### Development
- Hot reload: Tidak aktif (restart manual untuk melihat perubahan)
- Environment: Development
- Logging: Console output

---

## 🎯 Quick Reference

### Start Server
```bash
run.bat
```

### Stop Server
```bash
Ctrl + C
```

### Reset Database
```bash
reset-database.bat
```

### Check Port
```bash
netstat -ano | findstr :5000
```

### Kill Process
```bash
taskkill /F /PID <PID>
```

---

## 📞 Support

Jika mengalami masalah:
1. Pastikan berada di folder `InventarisKKP`
2. Gunakan `run.bat` untuk start otomatis
3. Gunakan `reset-database.bat` jika ada masalah database
4. Clear browser cache jika tampilan tidak update

---

## ✅ Checklist Sebelum Run

- [ ] Berada di folder `InventarisKKP`
- [ ] .NET 8.0 SDK terinstall
- [ ] Port 5000 tidak digunakan
- [ ] SQL Server LocalDB terinstall

---

**Version**: 1.0.0  
**Status**: ✅ Production Ready  
**Last Updated**: 2026-01-04
