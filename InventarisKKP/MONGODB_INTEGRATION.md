# Integrasi MongoDB untuk Kategori dan Barang

## Deskripsi
Aplikasi InventarisKKP sekarang menyimpan data kategori dan barang ke dua database:
1. **SQL Server** - Database utama menggunakan Entity Framework Core
2. **MongoDB** - Database NoSQL untuk backup dan fleksibilitas

## Fitur yang Diimplementasikan

### 1. Model MongoDB
- `MongoKategori` - Model untuk koleksi Kategoris di MongoDB
- `MongoBarang` - Model untuk koleksi Barangs di MongoDB

### 2. Services MongoDB
- `MongoDbService` - Service untuk koneksi ke MongoDB
- `IMongoKategoriService` & `MongoKategoriService` - Service untuk operasi CRUD kategori
- `IMongoBarangService` & `MongoBarangService` - Service untuk operasi CRUD barang

### 3. Controller Updates
- `KategoriController` - Sekarang menyimpan ke SQL Server dan MongoDB
- `BarangController` - Sekarang menyimpan ke SQL Server dan MongoDB

## Cara Kerja

### Tambah Kategori
1. Data disimpan ke SQL Server terlebih dahulu
2. Setelah berhasil, data juga disimpan ke MongoDB
3. Jika ada error, transaksi akan di-rollback

### Tambah Barang
1. Data disimpan ke SQL Server terlebih dahulu
2. Nama kategori diambil dari MongoDB
3. Data barang disimpan ke MongoDB dengan informasi kategori

### Edit Data
1. Data diupdate di SQL Server
2. Data juga diupdate di MongoDB berdasarkan ID yang sama

### Hapus Data
1. Data dihapus dari SQL Server
2. Data juga dihapus dari MongoDB

## Konfigurasi

### Connection String
Di `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  }
}
```

### Database dan Collection
- Database: `InventarisKKP`
- Collections:
  - `Kategoris` - Menyimpan data kategori
  - `Barangs` - Menyimpan data barang

## Struktur Data MongoDB

### Collection: Kategoris
```json
{
  "_id": "ObjectId",
  "kategoriId": 1,
  "namaKategori": "Elektronik",
  "createdAt": "2026-01-05T10:00:00Z",
  "updatedAt": "2026-01-05T11:00:00Z"
}
```

### Collection: Barangs
```json
{
  "_id": "ObjectId",
  "barangId": 1,
  "namaBarang": "Laptop Dell",
  "kategoriId": 1,
  "namaKategori": "Elektronik",
  "stok": 10,
  "createdAt": "2026-01-05T10:00:00Z",
  "updatedAt": "2026-01-05T11:00:00Z"
}
```

## Testing

### 1. Pastikan MongoDB Running
```bash
# Cek status MongoDB
mongosh --eval "db.version()"
```

### 2. Jalankan Aplikasi
```bash
cd InventarisKKP
dotnet run
```

### 3. Test CRUD Operations
1. Login sebagai admin (username: admin, password: admin123)
2. Tambah kategori baru
3. Tambah barang baru
4. Cek di MongoDB Compass apakah data tersimpan
5. Edit data dan cek perubahan di MongoDB
6. Hapus data dan cek di MongoDB

## Monitoring

### Cek Data di MongoDB Compass
1. Buka MongoDB Compass
2. Connect ke `mongodb://localhost:27017`
3. Pilih database `InventarisKKP`
4. Lihat collections `Kategoris` dan `Barangs`

### Cek Log di Console
Aplikasi akan menampilkan log untuk setiap operasi MongoDB:
```
[CREATE] Successfully saved kategori to MongoDB with ID: 6789abc...
[EDIT] Updated kategori in MongoDB: 6789abc...
[DELETE] Deleted kategori from MongoDB: 6789abc...
```

## Troubleshooting

### Error: MongoDB connection failed
- Pastikan MongoDB service berjalan
- Cek connection string di appsettings.json
- Cek firewall tidak memblokir port 27017

### Error: Data tidak tersimpan di MongoDB
- Cek log console untuk error message
- Pastikan MongoDB service berjalan
- Cek permission database

### Data tidak sinkron antara SQL Server dan MongoDB
- Ini bisa terjadi jika ada error saat menyimpan ke MongoDB
- Solusi: Hapus data di MongoDB dan tambah ulang dari aplikasi
