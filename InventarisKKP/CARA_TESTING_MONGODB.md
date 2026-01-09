# Cara Testing Integrasi MongoDB

## Persiapan

### 1. Pastikan MongoDB Berjalan
Buka MongoDB Compass atau jalankan service MongoDB:
```bash
# Windows Service
net start MongoDB

# Atau cek di Services (services.msc)
```

### 2. Pastikan Connection String Benar
Cek file `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  }
}
```

## Cara Testing

### Metode 1: Menggunakan Script Otomatis
```bash
# Jalankan script testing
test-mongodb.bat
```

Script ini akan:
1. Cek koneksi MongoDB
2. Cek database InventarisKKP
3. Jalankan aplikasi

### Metode 2: Manual Testing

#### Step 1: Build Aplikasi
```bash
dotnet build
```

#### Step 2: Jalankan Aplikasi
```bash
dotnet run
```

#### Step 3: Buka Browser
Buka http://localhost:5000

#### Step 4: Login
- Username: `admin`
- Password: `admin123`

#### Step 5: Test CRUD Kategori
1. Klik menu "Kategori"
2. Klik "Tambah Kategori"
3. Isi nama kategori (contoh: "Elektronik")
4. Klik "Simpan"
5. Cek di MongoDB Compass apakah data tersimpan

#### Step 6: Test CRUD Barang
1. Klik menu "Barang"
2. Klik "Tambah Barang"
3. Isi data barang:
   - Nama Barang: "Laptop Dell"
   - Kategori: Pilih kategori yang sudah dibuat
   - Stok: 10
4. Klik "Simpan"
5. Cek di MongoDB Compass apakah data tersimpan

#### Step 7: Test Edit Data
1. Klik tombol "Edit" pada data yang sudah dibuat
2. Ubah data
3. Klik "Simpan"
4. Cek di MongoDB Compass apakah data terupdate

#### Step 8: Test Delete Data
1. Klik tombol "Hapus" pada data
2. Konfirmasi hapus
3. Cek di MongoDB Compass apakah data terhapus

## Melihat Data di MongoDB

### Metode 1: Menggunakan Script PowerShell
```powershell
# Jalankan script untuk melihat data
.\view-mongodb-data.ps1
```

### Metode 2: Menggunakan MongoDB Compass
1. Buka MongoDB Compass
2. Connect ke `mongodb://localhost:27017`
3. Pilih database `InventarisKKP`
4. Lihat collections:
   - `Kategoris` - Data kategori
   - `Barangs` - Data barang

### Metode 3: Menggunakan MongoDB Shell
```bash
# Masuk ke MongoDB shell
mongosh

# Pilih database
use InventarisKKP

# Lihat semua kategori
db.Kategoris.find().pretty()

# Lihat semua barang
db.Barangs.find().pretty()

# Hitung jumlah data
db.Kategoris.countDocuments()
db.Barangs.countDocuments()
```

## Verifikasi Data

### Cek Sinkronisasi SQL Server dan MongoDB

#### 1. Cek di SQL Server
```sql
-- Buka SQL Server Management Studio
-- Jalankan query:
SELECT * FROM Kategoris
SELECT * FROM Barangs
```

#### 2. Cek di MongoDB
```javascript
// Di MongoDB Compass atau mongosh
db.Kategoris.find()
db.Barangs.find()
```

#### 3. Bandingkan Data
- Jumlah data harus sama
- ID kategori dan barang harus sama
- Nama kategori dan barang harus sama

## Troubleshooting

### Problem: Data tidak tersimpan di MongoDB
**Solusi:**
1. Cek log console aplikasi untuk error message
2. Pastikan MongoDB service berjalan
3. Cek connection string di appsettings.json
4. Cek firewall tidak memblokir port 27017

### Problem: Error "MongoDB connection failed"
**Solusi:**
1. Pastikan MongoDB service berjalan:
   ```bash
   net start MongoDB
   ```
2. Test koneksi manual:
   ```bash
   mongosh --eval "db.version()"
   ```

### Problem: Data tidak sinkron
**Solusi:**
1. Hapus semua data di MongoDB:
   ```javascript
   db.Kategoris.deleteMany({})
   db.Barangs.deleteMany({})
   ```
2. Tambah ulang data dari aplikasi

### Problem: Build error
**Solusi:**
1. Stop aplikasi yang sedang berjalan:
   ```bash
   taskkill /F /IM InventarisKKP.exe
   ```
2. Clean dan rebuild:
   ```bash
   dotnet clean
   dotnet build
   ```

## Log Messages

Aplikasi akan menampilkan log untuk setiap operasi MongoDB:

### Create
```
[CREATE] Successfully saved kategori to MongoDB with ID: 6789abc...
[CREATE BARANG] Successfully created barang in MongoDB with ID: 6789def...
```

### Update
```
[EDIT] Updated kategori in MongoDB: 6789abc...
[EDIT BARANG POST] Updated barang in MongoDB: 6789def...
```

### Delete
```
[DELETE] Deleted kategori from MongoDB: 6789abc...
[DELETE] Deleted barang from MongoDB: 6789def...
```

## Tips

1. **Selalu cek log console** untuk melihat apakah operasi MongoDB berhasil
2. **Gunakan MongoDB Compass** untuk visualisasi data yang lebih baik
3. **Backup data** sebelum melakukan testing delete
4. **Test di environment development** terlebih dahulu sebelum production
