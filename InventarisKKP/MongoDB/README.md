# 🗄️ MongoDB Database - Sistem Informasi Inventaris Barang

## ✅ Status: Database Siap Digunakan!

Database MongoDB untuk koleksi `barang` sudah berhasil dibuat dengan:
- ✅ Schema validation (strict mode)
- ✅ 10 sample documents
- ✅ 3 indexes untuk performa
- ✅ Dokumentasi lengkap

---

## 🚀 Quick Start

### 1. Lihat Data
```bash
mongosh --file view-data.js
```

### 2. Buka di MongoDB Compass
1. Buka MongoDB Compass
2. Connect: `mongodb://localhost:27017`
3. Database: `InventarisKKP`
4. Collection: `barang`

---

## 📚 Dokumentasi

### Untuk Pemula
- **[INDEX.md](INDEX.md)** - Mulai dari sini! Navigasi lengkap semua dokumentasi
- **[PANDUAN-LENGKAP.md](PANDUAN-LENGKAP.md)** - Panduan lengkap (Bahasa Indonesia)
- **[QUICK-REFERENCE.md](QUICK-REFERENCE.md)** - Referensi cepat query

### Untuk Developer
- **[README-MONGODB.md](README-MONGODB.md)** - Technical documentation
- **[DIAGRAM-STRUKTUR.md](DIAGRAM-STRUKTUR.md)** - Visual diagrams

---

## 📊 Schema

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

---

## 📁 File Structure

```
MongoDB/
├── README.md                      ← Anda di sini
├── INDEX.md                       ← Navigasi lengkap
├── PANDUAN-LENGKAP.md            ← Panduan lengkap (ID)
├── QUICK-REFERENCE.md            ← Quick reference
├── README-MONGODB.md             ← Technical docs
├── DIAGRAM-STRUKTUR.md           ← Visual diagrams
│
├── setup-barang-collection.js    ← Setup script
├── barang-schema.json            ← Schema validation
├── barang-sample-data.json       ← Sample data
│
├── query-examples.js             ← Query examples
├── crud-examples.js              ← CRUD examples
├── view-data.js                  ← View data script
│
├── run-setup.bat                 ← Windows setup
└── view-data.ps1                 ← PowerShell viewer
```

---

## 🎯 Sample Data

**Total:** 10 dokumen

### Kategori Elektronik (4 items)
- Laptop Dell Latitude 5420 (15 unit)
- Printer HP LaserJet Pro (8 unit) ⚠️ Stok rendah
- Monitor LED 24 inch (20 unit)
- Mouse Wireless Logitech (35 unit)

### Kategori Furniture (3 items)
- Meja Kerja Kayu Jati (25 unit)
- Kursi Kantor Ergonomis (30 unit)
- Lemari Arsip Besi (10 unit)

### Kategori ATK (3 items)
- Pulpen Pilot G2 (500 unit)
- Kertas A4 80gsm (200 unit)
- Stapler Besar (45 unit)

---

## 💻 Common Commands

### View All Data
```bash
mongosh --file view-data.js
```

### Run Query Examples
```bash
mongosh --file query-examples.js
```

### Run CRUD Examples
```bash
mongosh --file crud-examples.js
```

### MongoDB Shell
```bash
mongosh
use InventarisKKP
db.barang.find().pretty()
```

---

## 🔍 Quick Queries

### Find by Category
```javascript
db.barang.find({ kategori: "Elektronik" })
```

### Low Stock Alert
```javascript
db.barang.find({ jumlah_barang: { $lt: 10 } })
```

### Total by Category
```javascript
db.barang.aggregate([
  { $group: { _id: "$kategori", total: { $sum: "$jumlah_barang" } } }
])
```

---

## 📞 Connection Info

**Connection String:** `mongodb://localhost:27017/InventarisKKP`  
**Database:** `InventarisKKP`  
**Collection:** `barang`

---

## 🎓 Learning Resources

1. **Mulai di sini:** [INDEX.md](INDEX.md)
2. **Panduan lengkap:** [PANDUAN-LENGKAP.md](PANDUAN-LENGKAP.md)
3. **Quick reference:** [QUICK-REFERENCE.md](QUICK-REFERENCE.md)
4. **Visual diagrams:** [DIAGRAM-STRUKTUR.md](DIAGRAM-STRUKTUR.md)

---

## ✅ Checklist

- [x] Database `InventarisKKP` created
- [x] Collection `barang` created
- [x] Schema validation applied
- [x] Indexes created (3 indexes)
- [x] Sample data inserted (10 documents)
- [x] Documentation complete
- [x] Query examples available
- [x] CRUD examples available
- [x] Visual diagrams available

---

## 🛠️ Troubleshooting

### MongoDB not running?
```bash
net start MongoDB
```

### Can't connect?
Check if MongoDB is listening on port 27017:
```bash
netstat -an | findstr 27017
```

### Need help?
See [PANDUAN-LENGKAP.md](PANDUAN-LENGKAP.md) - Section TROUBLESHOOTING

---

**Created:** 8 Januari 2026  
**Version:** 1.0  
**Status:** ✅ Production Ready
