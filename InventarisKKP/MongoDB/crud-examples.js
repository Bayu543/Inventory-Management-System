// ========================================
// CONTOH OPERASI CRUD MONGODB
// Koleksi: barang
// ========================================

db = db.getSiblingDB('InventarisKKP');

// ========================================
// CREATE - Insert Data Baru
// ========================================

print("\n=== INSERT SATU BARANG ===");
var result1 = db.barang.insertOne({
  nama_barang: "Keyboard Mechanical RGB",
  kategori: "Elektronik",
  jumlah_barang: NumberInt(12),
  deskripsi: "Keyboard mechanical dengan RGB lighting dan switch blue",
  tanggal_masuk: new Date(),
  tanggal_update: new Date()
});
print("Inserted ID: " + result1.insertedId);

print("\n=== INSERT BANYAK BARANG ===");
var result2 = db.barang.insertMany([
  {
    nama_barang: "Whiteboard 120x90cm",
    kategori: "Furniture",
    jumlah_barang: NumberInt(8),
    deskripsi: "Whiteboard magnetic dengan frame aluminium",
    tanggal_masuk: new Date(),
    tanggal_update: new Date()
  },
  {
    nama_barang: "Spidol Whiteboard Snowman",
    kategori: "ATK",
    jumlah_barang: NumberInt(100),
    deskripsi: "Spidol whiteboard warna hitam, biru, merah",
    tanggal_masuk: new Date(),
    tanggal_update: new Date()
  }
]);
print("Inserted " + result2.insertedIds.length + " documents");

// ========================================
// READ - Baca Data
// ========================================

print("\n=== BACA SEMUA BARANG ===");
db.barang.find().limit(3).pretty();

print("\n=== BACA BARANG TERTENTU ===");
db.barang.findOne({ nama_barang: "Keyboard Mechanical RGB" });

print("\n=== BACA DENGAN FILTER ===");
db.barang.find({ 
  kategori: "Elektronik",
  jumlah_barang: { $gte: 10 }
}).pretty();

print("\n=== BACA DENGAN PROJECTION (PILIH FIELD) ===");
db.barang.find(
  { kategori: "ATK" },
  { nama_barang: 1, jumlah_barang: 1, _id: 0 }
).pretty();

// ========================================
// UPDATE - Update Data
// ========================================

print("\n=== UPDATE SATU DOKUMEN ===");
var updateResult1 = db.barang.updateOne(
  { nama_barang: "Keyboard Mechanical RGB" },
  { 
    $set: { 
      jumlah_barang: NumberInt(15),
      tanggal_update: new Date()
    }
  }
);
print("Modified: " + updateResult1.modifiedCount + " document(s)");

print("\n=== UPDATE BANYAK DOKUMEN ===");
var updateResult2 = db.barang.updateMany(
  { kategori: "ATK" },
  { 
    $set: { 
      tanggal_update: new Date()
    }
  }
);
print("Modified: " + updateResult2.modifiedCount + " document(s)");

print("\n=== INCREMENT JUMLAH BARANG ===");
db.barang.updateOne(
  { nama_barang: "Pulpen Pilot G2" },
  { 
    $inc: { jumlah_barang: 50 },
    $set: { tanggal_update: new Date() }
  }
);

print("\n=== DECREMENT JUMLAH BARANG (BARANG KELUAR) ===");
db.barang.updateOne(
  { nama_barang: "Laptop Dell Latitude 5420" },
  { 
    $inc: { jumlah_barang: -2 },
    $set: { tanggal_update: new Date() }
  }
);

// ========================================
// DELETE - Hapus Data
// ========================================

print("\n=== DELETE SATU DOKUMEN ===");
var deleteResult1 = db.barang.deleteOne(
  { nama_barang: "Keyboard Mechanical RGB" }
);
print("Deleted: " + deleteResult1.deletedCount + " document(s)");

print("\n=== DELETE BANYAK DOKUMEN (HATI-HATI!) ===");
// Contoh: hapus barang dengan stok 0
var deleteResult2 = db.barang.deleteMany(
  { jumlah_barang: 0 }
);
print("Deleted: " + deleteResult2.deletedCount + " document(s)");

// ========================================
// ADVANCED OPERATIONS
// ========================================

print("\n=== UPSERT (UPDATE OR INSERT) ===");
db.barang.updateOne(
  { nama_barang: "Proyektor Epson" },
  { 
    $set: {
      kategori: "Elektronik",
      jumlah_barang: NumberInt(5),
      deskripsi: "Proyektor untuk presentasi",
      tanggal_masuk: new Date(),
      tanggal_update: new Date()
    }
  },
  { upsert: true }
);

print("\n=== REPLACE DOKUMEN ===");
db.barang.replaceOne(
  { nama_barang: "Proyektor Epson" },
  {
    nama_barang: "Proyektor Epson EB-X41",
    kategori: "Elektronik",
    jumlah_barang: NumberInt(5),
    deskripsi: "Proyektor 3600 lumens untuk presentasi",
    tanggal_masuk: new Date(),
    tanggal_update: new Date()
  }
);

print("\n=== FIND AND MODIFY ===");
var modifiedDoc = db.barang.findOneAndUpdate(
  { nama_barang: "Proyektor Epson EB-X41" },
  { 
    $inc: { jumlah_barang: 3 },
    $set: { tanggal_update: new Date() }
  },
  { returnNewDocument: true }
);
printjson(modifiedDoc);

print("\n✅ Semua operasi CRUD selesai!");
