// ========================================
// CONTOH QUERY MONGODB
// Koleksi: barang
// ========================================

db = db.getSiblingDB('InventarisKKP');

print("\n=== 1. TAMPILKAN SEMUA BARANG ===");
db.barang.find().pretty();

print("\n=== 2. CARI BARANG KATEGORI ELEKTRONIK ===");
db.barang.find({ kategori: "Elektronik" }).pretty();

print("\n=== 3. CARI BARANG DENGAN STOK < 10 ===");
db.barang.find({ jumlah_barang: { $lt: 10 } }).pretty();

print("\n=== 4. CARI BARANG DENGAN STOK >= 20 ===");
db.barang.find({ jumlah_barang: { $gte: 20 } }).pretty();

print("\n=== 5. CARI BARANG BERDASARKAN NAMA (CONTAINS) ===");
db.barang.find({ nama_barang: /Laptop/i }).pretty();

print("\n=== 6. TOTAL BARANG PER KATEGORI ===");
db.barang.aggregate([
  {
    $group: {
      _id: "$kategori",
      total_stok: { $sum: "$jumlah_barang" },
      jumlah_jenis: { $sum: 1 }
    }
  },
  { $sort: { total_stok: -1 } }
]).pretty();

print("\n=== 7. TOP 5 BARANG DENGAN STOK TERBANYAK ===");
db.barang.find()
  .sort({ jumlah_barang: -1 })
  .limit(5)
  .pretty();

print("\n=== 8. BARANG YANG BARU MASUK (30 HARI TERAKHIR) ===");
var thirtyDaysAgo = new Date();
thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
db.barang.find({ 
  tanggal_masuk: { $gte: thirtyDaysAgo } 
}).pretty();

print("\n=== 9. COUNT TOTAL DOKUMEN ===");
print("Total barang: " + db.barang.countDocuments());

print("\n=== 10. STATISTIK KOLEKSI ===");
db.barang.stats();
