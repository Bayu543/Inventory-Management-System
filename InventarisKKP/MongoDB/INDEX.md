# 📚 INDEX - MongoDB Documentation
## Sistem Informasi Inventaris Barang

---

## 🎯 Mulai Dari Sini

### Untuk Pemula
1. 📖 **[PANDUAN-LENGKAP.md](PANDUAN-LENGKAP.md)** - Panduan lengkap dalam Bahasa Indonesia
2. ⚡ **[QUICK-REFERENCE.md](QUICK-REFERENCE.md)** - Referensi cepat untuk query umum
3. 📊 **[DIAGRAM-STRUKTUR.md](DIAGRAM-STRUKTUR.md)** - Visualisasi struktur database

### Untuk Developer
1. 📄 **[README-MONGODB.md](README-MONGODB.md)** - Technical documentation (English)
2. 🔧 **[setup-barang-collection.js](setup-barang-collection.js)** - Setup script
3. 🔍 **[query-examples.js](query-examples.js)** - Query examples
4. ✏️ **[crud-examples.js](crud-examples.js)** - CRUD operations

---

## 📁 Struktur File

```
MongoDB/
│
├── 📚 DOKUMENTASI
│   ├── INDEX.md                      ← Anda di sini
│   ├── README-MONGODB.md             ← Dokumentasi teknis lengkap
│   ├── PANDUAN-LENGKAP.md            ← Panduan lengkap (ID)
│   ├── QUICK-REFERENCE.md            ← Referensi cepat
│   └── DIAGRAM-STRUKTUR.md           ← Diagram visual
│
├── 🔧 SETUP & CONFIGURATION
│   ├── setup-barang-collection.js    ← Script setup database
│   ├── barang-schema.json            ← Schema validation
│   └── run-setup.bat                 ← Windows setup script
│
├── 📊 DATA
│   └── barang-sample-data.json       ← Sample data (10 items)
│
└── 💻 EXAMPLES
    ├── query-examples.js             ← Contoh query
    └── crud-examples.js              ← Contoh CRUD operations
```

---

## 🚀 Quick Start

### 1. Setup Database (First Time)
```bash
cd InventarisKKP/MongoDB
mongosh --file setup-barang-collection.js
```

### 2. View Data
```bash
mongosh
use InventarisKKP
db.barang.find().pretty()
```

### 3. Run Examples
```bash
mongosh --file query-examples.js
mongosh --file crud-examples.js
```

---

## 📖 Dokumentasi Berdasarkan Kebutuhan

### Saya ingin...

#### 🎓 Belajar MongoDB dari awal
→ Baca: **[PANDUAN-LENGKAP.md](PANDUAN-LENGKAP.md)**
- Penjelasan detail setiap field
- Contoh lengkap dengan penjelasan
- Tips & best practices

#### ⚡ Cari query cepat
→ Baca: **[QUICK-REFERENCE.md](QUICK-REFERENCE.md)**
- Common queries
- CRUD operations
- Aggregations

#### 📊 Lihat struktur visual
→ Baca: **[DIAGRAM-STRUKTUR.md](DIAGRAM-STRUKTUR.md)**
- Diagram arsitektur
- Flow operasi
- Relationship diagram

#### 🔧 Setup database baru
→ Jalankan: **[setup-barang-collection.js](setup-barang-collection.js)**
```bash
mongosh --file setup-barang-collection.js
```

#### 💻 Lihat contoh kode
→ Jalankan: **[query-examples.js](query-examples.js)** atau **[crud-examples.js](crud-examples.js)**
```bash
mongosh --file query-examples.js
mongosh --file crud-examples.js
```

#### 📄 Dokumentasi teknis lengkap
→ Baca: **[README-MONGODB.md](README-MONGODB.md)**
- Schema specification
- Validation rules
- Security guidelines
- Performance optimization

---

## 📊 Database Info

**Database Name:** `InventarisKKP`  
**Collection Name:** `barang`  
**Connection:** `mongodb://localhost:27017/InventarisKKP`

### Schema Summary
```javascript
{
  _id: ObjectId,              // Auto-generated
  nama_barang: String,        // Required, 1-200 char
  kategori: String,           // Required, 1-100 char
  jumlah_barang: Integer,     // Required, >= 0
  deskripsi: String,          // Optional, max 1000 char
  tanggal_masuk: Date,        // Required
  tanggal_update: Date        // Optional
}
```

### Sample Data
- ✅ 10 dokumen sample
- 📦 3 kategori: Elektronik, Furniture, ATK
- 🔍 3 index untuk performa

---

## 🎯 Use Cases

### Use Case 1: Barang Masuk
1. Insert dokumen baru dengan `insertOne()`
2. Set `tanggal_masuk` = sekarang
3. Set `jumlah_barang` = jumlah masuk

**File:** [crud-examples.js](crud-examples.js) - Section CREATE

### Use Case 2: Barang Keluar
1. Update dokumen dengan `updateOne()`
2. Decrement `jumlah_barang` dengan `$inc`
3. Update `tanggal_update` = sekarang

**File:** [crud-examples.js](crud-examples.js) - Section UPDATE

### Use Case 3: Laporan Stok
1. Query dengan filter kategori
2. Agregasi dengan `$group`
3. Sort hasil

**File:** [query-examples.js](query-examples.js) - Section AGREGASI

---

## 🔍 Troubleshooting

### Error: "Document failed validation"
→ Lihat: [PANDUAN-LENGKAP.md](PANDUAN-LENGKAP.md) - Section TROUBLESHOOTING

### Error: "Connection refused"
→ Pastikan MongoDB service running
```bash
net start MongoDB
```

### Data tidak muncul
→ Pastikan database dan collection benar
```bash
mongosh
show dbs
use InventarisKKP
show collections
```

---

## 📞 Referensi External

- [MongoDB Official Documentation](https://docs.mongodb.com/)
- [MongoDB University (Free Courses)](https://university.mongodb.com/)
- [MongoDB Compass (GUI Tool)](https://www.mongodb.com/products/compass)

---

## ✅ Checklist

- [x] Database setup selesai
- [x] Sample data tersedia
- [x] Dokumentasi lengkap
- [x] Query examples tersedia
- [x] CRUD examples tersedia
- [x] Diagram visual tersedia
- [x] Quick reference tersedia

---

## 🎓 Learning Path

### Level 1: Beginner
1. Baca [PANDUAN-LENGKAP.md](PANDUAN-LENGKAP.md)
2. Jalankan [setup-barang-collection.js](setup-barang-collection.js)
3. Coba query di MongoDB Compass

### Level 2: Intermediate
1. Pelajari [query-examples.js](query-examples.js)
2. Praktik CRUD dengan [crud-examples.js](crud-examples.js)
3. Baca [DIAGRAM-STRUKTUR.md](DIAGRAM-STRUKTUR.md)

### Level 3: Advanced
1. Baca [README-MONGODB.md](README-MONGODB.md)
2. Pelajari aggregation pipeline
3. Optimasi dengan indexes

---

## 📝 Notes

- Semua script sudah tested dan working ✅
- Sample data realistic untuk aplikasi inventaris
- Validation strict untuk data integrity
- Index sudah dibuat untuk performa optimal

---

**Last Updated:** 8 Januari 2026  
**Version:** 1.0  
**Status:** ✅ Production Ready
