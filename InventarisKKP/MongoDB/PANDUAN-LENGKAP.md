# 📚 PANDUAN LENGKAP - Database MongoDB
## Sistem Informasi Inventaris Barang

---

## 🎯 Ringkasan Cepat

**Database:** `InventarisKKP`  
**Koleksi:** `barang`  
**Total Sample Data:** 10 dokumen  
**Status:** ✅ Sudah dibuat dan diisi data

---

## 📊 STRUKTUR SCHEMA

```javascript
{
  _id: ObjectId("..."),              // Auto-generated
  nama_barang: "String",             // REQUIRED, max 200 char
  kategori: "String",                // REQUIRED, max 100 char
  jumlah_barang: 0,                  // REQUIRED, integer >= 0
  deskripsi: "String",               // OPTIONAL, max 1000 char
  tanggal_masuk: ISODate("..."),     // REQUIRED
  tanggal_update: ISODate("...")     // OPTIONAL
}
```

---

## 📋 DETAIL FIELD

### 1️⃣ _id (ObjectId)
```
Tipe: ObjectId
Required: ✅ (Auto)
Contoh: ObjectId("507f1f77bcf86cd799439011")
Fungsi: ID unik setiap dokumen
```

### 2️⃣ nama_barang (String)
```
Tipe: String
Required: ✅
Min Length: 1
Max Length: 200
Contoh: "Laptop Dell Latitude 5420"
Fungsi: Nama lengkap barang
```

### 3️⃣ kategori (String)
```
Tipe: String
Required: ✅
Min Length: 1
Max Length: 100
Contoh: "Elektronik", "Furniture", "ATK"
Fungsi: Pengelompokan barang
```

### 4️⃣ jumlah_barang (Integer)
```
Tipe: Integer (int32)
Required: ✅
Minimum: 0
Contoh: 15, 100, 0
Fungsi: Tracking stok barang
```

### 5️⃣ deskripsi (String)
```
Tipe: String
Required: ❌ (Optional)
Max Length: 1000
Contoh: "Laptop untuk keperluan kantor..."
Fungsi: Informasi detail barang
```

### 6️⃣ tanggal_masuk (Date)
```
Tipe: Date (ISODate)
Required: ✅
Format: ISO 8601
Contoh: ISODate("2024-01-15T08:00:00.000Z")
Fungsi: Tanggal barang masuk inventaris
```

### 7️⃣ tanggal_update (Date)
```
Tipe: Date (ISODate)
Required: ❌ (Optional)
Format: ISO 8601
Contoh: ISODate("2024-03-20T14:30:00.000Z")
Fungsi: Tanggal terakhir update data
```

---

## 📝 CONTOH DOKUMEN LENGKAP

```json
{
  "_id": ObjectId("695fb3020ec944c6c0cebea4"),
  "nama_barang": "Laptop Dell Latitude 5420",
  "kategori": "Elektronik",
  "jumlah_barang": 15,
  "deskripsi": "Laptop untuk keperluan kantor dengan spesifikasi Intel Core i5, RAM 8GB, SSD 256GB",
  "tanggal_masuk": ISODate("2024-01-15T08:00:00.000Z"),
  "tanggal_update": ISODate("2024-01-15T08:00:00.000Z")
}
```

---

## ✅ VALIDASI SCHEMA

### Required Fields
- ✅ `nama_barang` - Wajib diisi
- ✅ `kategori` - Wajib diisi
- ✅ `jumlah_barang` - Wajib diisi
- ✅ `tanggal_masuk` - Wajib diisi

### Optional Fields
- ❌ `deskripsi` - Boleh kosong
- ❌ `tanggal_update` - Boleh kosong

### Validation Rules
```javascript
{
  validationLevel: "strict",    // Semua operasi harus valid
  validationAction: "error"     // Tolak jika tidak valid
}
```

### Contoh Error Validasi
```javascript
// ❌ GAGAL - nama_barang kosong
db.barang.insertOne({
  nama_barang: "",
  kategori: "Elektronik",
  jumlah_barang: 10
})
// Error: Document failed validation

// ❌ GAGAL - jumlah_barang negatif
db.barang.insertOne({
  nama_barang: "Test",
  kategori: "ATK",
  jumlah_barang: -5
})
// Error: Document failed validation

// ✅ BERHASIL
db.barang.insertOne({
  nama_barang: "Pulpen",
  kategori: "ATK",
  jumlah_barang: 100,
  tanggal_masuk: new Date()
})
```

---

## 🗂️ SAMPLE DATA (10 Dokumen)

### Kategori: Elektronik (5 items)
1. **Laptop Dell Latitude 5420** - Stok: 15
2. **Printer HP LaserJet Pro** - Stok: 8
3. **Monitor LED 24 inch** - Stok: 20
4. **Mouse Wireless Logitech** - Stok: 35

### Kategori: Furniture (3 items)
5. **Meja Kerja Kayu Jati** - Stok: 25
6. **Kursi Kantor Ergonomis** - Stok: 30
7. **Lemari Arsip Besi** - Stok: 10

### Kategori: ATK (3 items)
8. **Pulpen Pilot G2** - Stok: 500
9. **Kertas A4 80gsm** - Stok: 200
10. **Stapler Besar** - Stok: 45

---

## 🚀 CARA MENGGUNAKAN

### 1. Setup Database (Sudah Selesai ✅)
```bash
cd InventarisKKP/MongoDB
mongosh --file setup-barang-collection.js
```

### 2. Lihat Data di MongoDB Compass
1. Buka MongoDB Compass
2. Connect ke: `mongodb://localhost:27017`
3. Pilih database: `InventarisKKP`
4. Pilih collection: `barang`
5. Lihat 10 dokumen sample data

### 3. Query via MongoDB Shell
```bash
mongosh
use InventarisKKP
db.barang.find().pretty()
```

---

## 🔍 QUERY EXAMPLES

### Tampilkan Semua Barang
```javascript
db.barang.find()
```

### Cari Berdasarkan Kategori
```javascript
db.barang.find({ kategori: "Elektronik" })
```

### Cari Barang Stok Rendah (< 10)
```javascript
db.barang.find({ jumlah_barang: { $lt: 10 } })
```

### Cari Barang Berdasarkan Nama
```javascript
db.barang.find({ nama_barang: /Laptop/i })
```

### Sort Berdasarkan Stok (Terbanyak)
```javascript
db.barang.find().sort({ jumlah_barang: -1 })
```

### Limit 5 Hasil
```javascript
db.barang.find().limit(5)
```

### Count Total Dokumen
```javascript
db.barang.countDocuments()
```

### Agregasi - Total per Kategori
```javascript
db.barang.aggregate([
  {
    $group: {
      _id: "$kategori",
      total_stok: { $sum: "$jumlah_barang" },
      jumlah_jenis: { $sum: 1 }
    }
  }
])
```

---

## ✏️ OPERASI CRUD

### CREATE - Insert Barang Baru
```javascript
db.barang.insertOne({
  nama_barang: "Keyboard Mechanical",
  kategori: "Elektronik",
  jumlah_barang: NumberInt(10),
  deskripsi: "Keyboard mechanical RGB",
  tanggal_masuk: new Date(),
  tanggal_update: new Date()
})
```

### READ - Baca Data
```javascript
// Baca semua
db.barang.find()

// Baca satu
db.barang.findOne({ nama_barang: "Laptop Dell Latitude 5420" })

// Baca dengan filter
db.barang.find({ kategori: "ATK", jumlah_barang: { $gte: 100 } })
```

### UPDATE - Update Data
```javascript
// Update satu dokumen
db.barang.updateOne(
  { nama_barang: "Laptop Dell Latitude 5420" },
  { 
    $set: { 
      jumlah_barang: NumberInt(20),
      tanggal_update: new Date()
    }
  }
)

// Increment stok (barang masuk)
db.barang.updateOne(
  { nama_barang: "Pulpen Pilot G2" },
  { 
    $inc: { jumlah_barang: 50 },
    $set: { tanggal_update: new Date() }
  }
)

// Decrement stok (barang keluar)
db.barang.updateOne(
  { nama_barang: "Laptop Dell Latitude 5420" },
  { 
    $inc: { jumlah_barang: -2 },
    $set: { tanggal_update: new Date() }
  }
)
```

### DELETE - Hapus Data
```javascript
// Hapus satu dokumen
db.barang.deleteOne({ nama_barang: "Keyboard Mechanical" })

// Hapus banyak dokumen
db.barang.deleteMany({ jumlah_barang: 0 })
```

---

## 📈 INDEX UNTUK PERFORMA

Database sudah dilengkapi dengan 3 index:

```javascript
// 1. Index untuk pencarian nama
db.barang.createIndex({ "nama_barang": 1 })

// 2. Index untuk filter kategori
db.barang.createIndex({ "kategori": 1 })

// 3. Index untuk sorting tanggal
db.barang.createIndex({ "tanggal_masuk": -1 })
```

### Lihat Index
```javascript
db.barang.getIndexes()
```

---

## 🔗 KONEKSI STRING

### Local Development
```
mongodb://localhost:27017/InventarisKKP
```

### Dengan Authentication
```
mongodb://username:password@localhost:27017/InventarisKKP
```

### Connection dari C# (.NET)
```csharp
var connectionString = "mongodb://localhost:27017";
var client = new MongoClient(connectionString);
var database = client.GetDatabase("InventarisKKP");
var collection = database.GetCollection<Barang>("barang");
```

---

## 📁 FILE YANG TERSEDIA

```
MongoDB/
├── README-MONGODB.md              # Dokumentasi lengkap
├── PANDUAN-LENGKAP.md            # Panduan ini
├── setup-barang-collection.js    # Script setup database
├── barang-schema.json            # Schema validation
├── barang-sample-data.json       # Sample data (JSON)
├── query-examples.js             # Contoh query
├── crud-examples.js              # Contoh CRUD operations
└── run-setup.bat                 # Script Windows untuk setup
```

---

## 🎓 TIPS & BEST PRACTICES

### 1. Selalu Update tanggal_update
```javascript
db.barang.updateOne(
  { _id: ObjectId("...") },
  { 
    $set: { 
      jumlah_barang: 20,
      tanggal_update: new Date()  // ✅ Jangan lupa!
    }
  }
)
```

### 2. Gunakan NumberInt untuk Integer
```javascript
// ✅ BENAR
jumlah_barang: NumberInt(10)

// ❌ SALAH (akan jadi float)
jumlah_barang: 10
```

### 3. Validasi di Aplikasi Level
```javascript
// Validasi sebelum insert
if (jumlah_barang < 0) {
  throw new Error("Jumlah barang tidak boleh negatif");
}
```

### 4. Backup Database Berkala
```bash
# Backup
mongodump --db InventarisKKP --out ./backup

# Restore
mongorestore --db InventarisKKP ./backup/InventarisKKP
```

### 5. Gunakan Projection untuk Performa
```javascript
// ✅ Hanya ambil field yang dibutuhkan
db.barang.find(
  { kategori: "Elektronik" },
  { nama_barang: 1, jumlah_barang: 1 }
)

// ❌ Ambil semua field (lebih lambat)
db.barang.find({ kategori: "Elektronik" })
```

---

## 🛠️ TROUBLESHOOTING

### Error: "Document failed validation"
**Penyebab:** Data tidak sesuai schema  
**Solusi:** Pastikan semua required field terisi dan tipe data benar

### Error: "Connection refused"
**Penyebab:** MongoDB tidak running  
**Solusi:** Start MongoDB service
```bash
net start MongoDB
```

### Data Tidak Muncul di Compass
**Penyebab:** Salah database/collection  
**Solusi:** Pastikan pilih database `InventarisKKP` dan collection `barang`

---

## 📞 REFERENSI

- [MongoDB Manual](https://docs.mongodb.com/manual/)
- [Schema Validation](https://docs.mongodb.com/manual/core/schema-validation/)
- [CRUD Operations](https://docs.mongodb.com/manual/crud/)
- [Aggregation](https://docs.mongodb.com/manual/aggregation/)
- [Indexes](https://docs.mongodb.com/manual/indexes/)

---

## ✅ CHECKLIST SETUP

- [x] Database `InventarisKKP` dibuat
- [x] Collection `barang` dibuat
- [x] Schema validation diterapkan
- [x] Index dibuat (3 index)
- [x] Sample data diinsert (10 dokumen)
- [x] Dokumentasi lengkap tersedia
- [x] Script query & CRUD tersedia

---

**Status:** ✅ Database MongoDB Siap Digunakan!  
**Dibuat:** 8 Januari 2026  
**Versi:** 1.0
