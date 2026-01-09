# 📊 DIAGRAM STRUKTUR DATABASE MONGODB
## Sistem Informasi Inventaris Barang

---

## 🗄️ ARSITEKTUR DATABASE

```
┌─────────────────────────────────────────┐
│     MongoDB Server (localhost:27017)    │
└─────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────┐
│      Database: InventarisKKP            │
└─────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────┐
│       Collection: barang                │
│                                         │
│  ┌───────────────────────────────────┐ │
│  │  Document 1 (Laptop Dell...)      │ │
│  ├───────────────────────────────────┤ │
│  │  Document 2 (Meja Kerja...)       │ │
│  ├───────────────────────────────────┤ │
│  │  Document 3 (Printer HP...)       │ │
│  ├───────────────────────────────────┤ │
│  │  ...                              │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
```

---

## 📄 STRUKTUR DOKUMEN

```
┌─────────────────────────────────────────────────────────┐
│                    DOKUMEN BARANG                       │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  _id: ObjectId("695fb3020ec944c6c0cebea4")            │
│  ├─ Tipe: ObjectId                                     │
│  ├─ Required: ✅ (Auto-generated)                      │
│  └─ Fungsi: ID unik dokumen                            │
│                                                         │
│  nama_barang: "Laptop Dell Latitude 5420"              │
│  ├─ Tipe: String                                       │
│  ├─ Required: ✅                                        │
│  ├─ Min: 1 char, Max: 200 char                        │
│  └─ Fungsi: Nama lengkap barang                        │
│                                                         │
│  kategori: "Elektronik"                                │
│  ├─ Tipe: String                                       │
│  ├─ Required: ✅                                        │
│  ├─ Min: 1 char, Max: 100 char                        │
│  └─ Fungsi: Pengelompokan barang                       │
│                                                         │
│  jumlah_barang: 15                                     │
│  ├─ Tipe: Integer (int32)                             │
│  ├─ Required: ✅                                        │
│  ├─ Min: 0                                             │
│  └─ Fungsi: Tracking stok                              │
│                                                         │
│  deskripsi: "Laptop untuk keperluan kantor..."         │
│  ├─ Tipe: String                                       │
│  ├─ Required: ❌ (Optional)                            │
│  ├─ Max: 1000 char                                     │
│  └─ Fungsi: Informasi detail                           │
│                                                         │
│  tanggal_masuk: ISODate("2024-01-15T08:00:00.000Z")   │
│  ├─ Tipe: Date                                         │
│  ├─ Required: ✅                                        │
│  └─ Fungsi: Tanggal barang masuk                       │
│                                                         │
│  tanggal_update: ISODate("2024-01-15T08:00:00.000Z")  │
│  ├─ Tipe: Date                                         │
│  ├─ Required: ❌ (Optional)                            │
│  └─ Fungsi: Tanggal terakhir update                    │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

## 🔄 FLOW OPERASI CRUD

### CREATE (Insert)
```
┌──────────────┐
│   Aplikasi   │
└──────┬───────┘
       │ insertOne() / insertMany()
       ▼
┌──────────────┐
│  Validasi    │ ◄── Schema Validation
│  Schema      │     - Required fields?
└──────┬───────┘     - Tipe data benar?
       │             - Range valid?
       ▼
┌──────────────┐
│   MongoDB    │
│  Collection  │ ──► Document tersimpan
└──────────────┘     dengan _id baru
```

### READ (Query)
```
┌──────────────┐
│   Aplikasi   │
└──────┬───────┘
       │ find() / findOne()
       ▼
┌──────────────┐
│   Index      │ ◄── Gunakan index untuk
│  Lookup      │     performa lebih cepat
└──────┬───────┘
       │
       ▼
┌──────────────┐
│   MongoDB    │
│  Collection  │ ──► Return dokumen
└──────────────┘
```

### UPDATE (Modify)
```
┌──────────────┐
│   Aplikasi   │
└──────┬───────┘
       │ updateOne() / updateMany()
       ▼
┌──────────────┐
│  Validasi    │ ◄── Schema Validation
│  Schema      │     untuk data baru
└──────┬───────┘
       │
       ▼
┌──────────────┐
│   MongoDB    │
│  Collection  │ ──► Document terupdate
└──────────────┘     + tanggal_update
```

### DELETE (Remove)
```
┌──────────────┐
│   Aplikasi   │
└──────┬───────┘
       │ deleteOne() / deleteMany()
       ▼
┌──────────────┐
│   MongoDB    │
│  Collection  │ ──► Document terhapus
└──────────────┘
```

---

## 🎯 DISTRIBUSI DATA SAMPLE

### Berdasarkan Kategori
```
Elektronik  ████████████████████ 4 items (40%)
Furniture   ███████████████ 3 items (30%)
ATK         ███████████████ 3 items (30%)
```

### Berdasarkan Range Stok
```
0-10        ██ 2 items (20%)
11-50       ████████ 6 items (60%)
51-500      ██ 2 items (20%)
```

---

## 🔍 INDEX STRUCTURE

```
Collection: barang
│
├─ Index 1: _id (Default)
│  └─ Unique, Auto-created
│
├─ Index 2: nama_barang (Ascending)
│  └─ Untuk pencarian nama barang
│
├─ Index 3: kategori (Ascending)
│  └─ Untuk filter kategori
│
└─ Index 4: tanggal_masuk (Descending)
   └─ Untuk sorting tanggal
```

---

## 🔐 VALIDATION FLOW

```
┌─────────────────────────────────────────┐
│         Insert/Update Request           │
└────────────────┬────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────┐
│      Cek Required Fields                │
│  ✓ nama_barang ada?                     │
│  ✓ kategori ada?                        │
│  ✓ jumlah_barang ada?                   │
│  ✓ tanggal_masuk ada?                   │
└────────────────┬────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────┐
│      Cek Tipe Data                      │
│  ✓ nama_barang = String?                │
│  ✓ kategori = String?                   │
│  ✓ jumlah_barang = Integer?             │
│  ✓ tanggal_masuk = Date?                │
└────────────────┬────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────┐
│      Cek Range/Length                   │
│  ✓ nama_barang: 1-200 char?             │
│  ✓ kategori: 1-100 char?                │
│  ✓ jumlah_barang >= 0?                  │
│  ✓ deskripsi <= 1000 char?              │
└────────────────┬────────────────────────┘
                 │
                 ▼
         ┌───────┴────────┐
         │                │
         ▼                ▼
    ┌────────┐      ┌─────────┐
    │  PASS  │      │  FAIL   │
    │   ✅   │      │   ❌    │
    └────┬───┘      └────┬────┘
         │               │
         ▼               ▼
   ┌──────────┐    ┌──────────┐
   │  Simpan  │    │  Reject  │
   │  ke DB   │    │  + Error │
   └──────────┘    └──────────┘
```

---

## 📊 RELATIONSHIP DIAGRAM

```
┌─────────────────────────────────────────┐
│         Collection: barang              │
├─────────────────────────────────────────┤
│                                         │
│  Kategori: "Elektronik"                 │
│  ├─ Laptop Dell Latitude 5420           │
│  ├─ Printer HP LaserJet Pro             │
│  ├─ Monitor LED 24 inch                 │
│  └─ Mouse Wireless Logitech             │
│                                         │
│  Kategori: "Furniture"                  │
│  ├─ Meja Kerja Kayu Jati                │
│  ├─ Kursi Kantor Ergonomis              │
│  └─ Lemari Arsip Besi                   │
│                                         │
│  Kategori: "ATK"                        │
│  ├─ Pulpen Pilot G2                     │
│  ├─ Kertas A4 80gsm                     │
│  └─ Stapler Besar                       │
│                                         │
└─────────────────────────────────────────┘
```

---

## 🔄 LIFECYCLE DOKUMEN

```
┌──────────────┐
│   CREATE     │
│  (Insert)    │
└──────┬───────┘
       │
       │ tanggal_masuk = now()
       │ tanggal_update = now()
       │
       ▼
┌──────────────┐
│   ACTIVE     │ ◄──┐
│  (Tersimpan) │    │
└──────┬───────┘    │
       │            │
       │ UPDATE     │
       ├────────────┘
       │ tanggal_update = now()
       │
       ▼
┌──────────────┐
│   DELETE     │
│  (Terhapus)  │
└──────────────┘
```

---

## 🎨 CONTOH USE CASE

### Use Case 1: Barang Masuk
```
1. User input data barang baru
   ↓
2. Validasi data (required fields, tipe data)
   ↓
3. Insert ke MongoDB
   ↓
4. Set tanggal_masuk = sekarang
   ↓
5. Return success + _id baru
```

### Use Case 2: Update Stok (Barang Keluar)
```
1. User pilih barang
   ↓
2. Input jumlah keluar
   ↓
3. Validasi: stok cukup?
   ↓
4. Update: jumlah_barang -= jumlah_keluar
   ↓
5. Update: tanggal_update = sekarang
   ↓
6. Return success
```

### Use Case 3: Laporan Stok
```
1. Query semua barang
   ↓
2. Group by kategori
   ↓
3. Hitung total per kategori
   ↓
4. Sort by total (descending)
   ↓
5. Return hasil agregasi
```

---

## 📈 PERFORMA OPTIMIZATION

```
┌─────────────────────────────────────────┐
│         Query Optimization              │
├─────────────────────────────────────────┤
│                                         │
│  1. Gunakan Index                       │
│     ✓ nama_barang (search)              │
│     ✓ kategori (filter)                 │
│     ✓ tanggal_masuk (sort)              │
│                                         │
│  2. Projection (pilih field)            │
│     ✓ Hanya ambil field yang dibutuhkan │
│                                         │
│  3. Limit Results                       │
│     ✓ Pagination untuk data banyak      │
│                                         │
│  4. Aggregation Pipeline                │
│     ✓ Untuk query kompleks              │
│                                         │
└─────────────────────────────────────────┘
```

---

## 🛡️ SECURITY LAYERS

```
┌─────────────────────────────────────────┐
│         Application Layer               │
│  - Input validation                     │
│  - Sanitization                         │
│  - Business logic                       │
└────────────────┬────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────┐
│         MongoDB Driver                  │
│  - Connection pooling                   │
│  - Query building                       │
└────────────────┬────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────┐
│         MongoDB Server                  │
│  - Schema validation                    │
│  - Authentication                       │
│  - Authorization                        │
└─────────────────────────────────────────┘
```

---

**Diagram ini menjelaskan struktur lengkap database MongoDB untuk Sistem Informasi Inventaris Barang**
