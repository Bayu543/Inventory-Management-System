# Database MongoDB - Sistem Informasi Inventaris Barang

## 📋 Struktur Database

### Database: `InventarisKKP`
### Koleksi: `barang`

---

## 🗂️ Schema Koleksi Barang

### Atribut dan Penjelasan

| Field | Tipe Data | Required | Deskripsi |
|-------|-----------|----------|-----------|
| `_id` | ObjectId | ✅ Auto | ID unik otomatis dari MongoDB |
| `nama_barang` | String | ✅ | Nama barang (max 200 karakter) |
| `kategori` | String | ✅ | Kategori barang (max 100 karakter) |
| `jumlah_barang` | Integer | ✅ | Jumlah stok barang (minimal 0) |
| `deskripsi` | String | ❌ | Deskripsi detail barang (max 1000 karakter) |
| `tanggal_masuk` | Date | ✅ | Tanggal barang pertama kali masuk |
| `tanggal_update` | Date | ❌ | Tanggal terakhir data diupdate |

---

## 📝 Penjelasan Detail Field

### 1. **_id** (ObjectId)
- ID unik yang otomatis dibuat oleh MongoDB
- Format: 24 karakter hexadecimal
- Contoh: `507f1f77bcf86cd799439011`
- Fungsi: Identifikasi unik setiap dokumen barang

### 2. **nama_barang** (String) - WAJIB
- Nama lengkap barang
- Minimal 1 karakter, maksimal 200 karakter
- Contoh: "Laptop Dell Latitude 5420", "Meja Kerja Kayu Jati"
- Fungsi: Identifikasi nama barang dalam sistem

### 3. **kategori** (String) - WAJIB
- Kategori/jenis barang
- Minimal 1 karakter, maksimal 100 karakter
- Contoh: "Elektronik", "Furniture", "ATK"
- Fungsi: Pengelompokan barang berdasarkan jenis

### 4. **jumlah_barang** (Integer) - WAJIB
- Jumlah stok barang yang tersedia
- Harus berupa angka bulat (integer)
- Minimal 0 (tidak boleh negatif)
- Contoh: 15, 100, 0
- Fungsi: Tracking stok barang

### 5. **deskripsi** (String) - OPSIONAL
- Deskripsi detail tentang barang
- Maksimal 1000 karakter
- Boleh kosong/null
- Contoh: "Laptop untuk keperluan kantor dengan spesifikasi Intel Core i5, RAM 8GB"
- Fungsi: Informasi tambahan tentang spesifikasi atau kondisi barang

### 6. **tanggal_masuk** (Date) - WAJIB
- Tanggal barang pertama kali masuk ke inventaris
- Format: ISO Date
- Contoh: `2024-01-15T08:00:00.000Z`
- Fungsi: Tracking kapan barang mulai tercatat dalam sistem

### 7. **tanggal_update** (Date) - OPSIONAL
- Tanggal terakhir data barang diupdate
- Format: ISO Date
- Otomatis diupdate setiap kali ada perubahan data
- Contoh: `2024-03-20T14:30:00.000Z`
- Fungsi: Audit trail untuk perubahan data

---

## 📄 Contoh Dokumen

```json
{
  "_id": ObjectId("507f1f77bcf86cd799439011"),
  "nama_barang": "Laptop Dell Latitude 5420",
  "kategori": "Elektronik",
  "jumlah_barang": 15,
  "deskripsi": "Laptop untuk keperluan kantor dengan spesifikasi Intel Core i5, RAM 8GB, SSD 256GB",
  "tanggal_masuk": ISODate("2024-01-15T08:00:00.000Z"),
  "tanggal_update": ISODate("2024-01-15T08:00:00.000Z")
}
```

---

## ✅ Validasi Data

### Required Fields (Wajib Diisi)
- `nama_barang` - tidak boleh kosong
- `kategori` - tidak boleh kosong
- `jumlah_barang` - harus ada dan >= 0
- `tanggal_masuk` - harus ada

### Validasi Tipe Data
- `nama_barang`: String (1-200 karakter)
- `kategori`: String (1-100 karakter)
- `jumlah_barang`: Integer (>= 0)
- `deskripsi`: String (max 1000 karakter)
- `tanggal_masuk`: Date
- `tanggal_update`: Date

### Validation Level
- **Level**: `strict` - Semua insert dan update harus memenuhi schema
- **Action**: `error` - Tolak operasi jika tidak valid

---

## 🚀 Cara Setup Database

### 1. Menggunakan MongoDB Shell

```bash
# Masuk ke MongoDB shell
mongosh

# Jalankan script setup
load("setup-barang-collection.js")
```

### 2. Menggunakan MongoDB Compass

1. Buka MongoDB Compass
2. Connect ke `localhost:27017`
3. Buat database baru: `InventarisKKP`
4. Buat collection baru: `barang`
5. Klik "Validation" tab
6. Copy-paste isi dari `barang-schema.json`
7. Import data dari `barang-sample-data.json`

### 3. Menggunakan Command Line

```bash
# Import sample data
mongoimport --db InventarisKKP --collection barang --file barang-sample-data.json --jsonArray
```

---

## 📊 Index untuk Performa

```javascript
// Index untuk pencarian nama barang
db.barang.createIndex({ "nama_barang": 1 })

// Index untuk filter kategori
db.barang.createIndex({ "kategori": 1 })

// Index untuk sorting tanggal
db.barang.createIndex({ "tanggal_masuk": -1 })
```

---

## 🔍 Query Contoh

### Mencari semua barang
```javascript
db.barang.find()
```

### Mencari barang berdasarkan kategori
```javascript
db.barang.find({ "kategori": "Elektronik" })
```

### Mencari barang dengan stok rendah
```javascript
db.barang.find({ "jumlah_barang": { $lt: 10 } })
```

### Update jumlah barang
```javascript
db.barang.updateOne(
  { "nama_barang": "Laptop Dell Latitude 5420" },
  { 
    $set: { 
      "jumlah_barang": 20,
      "tanggal_update": new Date()
    }
  }
)
```

### Insert barang baru
```javascript
db.barang.insertOne({
  "nama_barang": "Keyboard Mechanical",
  "kategori": "Elektronik",
  "jumlah_barang": 10,
  "deskripsi": "Keyboard mechanical RGB dengan switch blue",
  "tanggal_masuk": new Date(),
  "tanggal_update": new Date()
})
```

### Delete barang
```javascript
db.barang.deleteOne({ "nama_barang": "Keyboard Mechanical" })
```

---

## 📈 Agregasi Contoh

### Total barang per kategori
```javascript
db.barang.aggregate([
  {
    $group: {
      _id: "$kategori",
      total_barang: { $sum: "$jumlah_barang" },
      jumlah_jenis: { $sum: 1 }
    }
  }
])
```

### Barang dengan stok terbanyak
```javascript
db.barang.aggregate([
  { $sort: { "jumlah_barang": -1 } },
  { $limit: 5 }
])
```

---

## 🔐 Keamanan

### Best Practices
1. Gunakan authentication untuk production
2. Buat user dengan role terbatas
3. Backup database secara berkala
4. Validasi input di aplikasi level

### Contoh Buat User
```javascript
use InventarisKKP
db.createUser({
  user: "inventaris_user",
  pwd: "password_aman",
  roles: [
    { role: "readWrite", db: "InventarisKKP" }
  ]
})
```

---

## 📞 Koneksi dari Aplikasi

### Connection String
```
mongodb://localhost:27017/InventarisKKP
```

### Dengan Authentication
```
mongodb://inventaris_user:password_aman@localhost:27017/InventarisKKP
```

---

## 📚 Referensi

- [MongoDB Documentation](https://docs.mongodb.com/)
- [Schema Validation](https://docs.mongodb.com/manual/core/schema-validation/)
- [BSON Types](https://docs.mongodb.com/manual/reference/bson-types/)
- [MongoDB Indexes](https://docs.mongodb.com/manual/indexes/)

---

**Dibuat untuk:** Sistem Informasi Inventaris Barang  
**Database:** MongoDB (NoSQL)  
**Versi:** 1.0  
**Tanggal:** 2026
