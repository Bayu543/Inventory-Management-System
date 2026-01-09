// View MongoDB Data
db = db.getSiblingDB('InventarisKKP');

print('\n========================================');
print('   MONGODB DATA - KOLEKSI BARANG');
print('========================================\n');

print('=== STATISTIK DATABASE ===');
var total = db.barang.countDocuments();
print('Total dokumen: ' + total + '\n');

print('=== SEMUA BARANG ===');
db.barang.find().sort({ kategori: 1, nama_barang: 1 }).forEach(function(doc) {
    print('\n📦 ' + doc.nama_barang);
    print('   Kategori: ' + doc.kategori);
    print('   Stok: ' + doc.jumlah_barang);
    if (doc.deskripsi) {
        print('   Deskripsi: ' + doc.deskripsi.substring(0, 60) + '...');
    }
});

print('\n\n=== TOTAL PER KATEGORI ===');
db.barang.aggregate([
    {
        $group: {
            _id: '$kategori',
            total_stok: { $sum: '$jumlah_barang' },
            jumlah_jenis: { $sum: 1 }
        }
    },
    { $sort: { total_stok: -1 } }
]).forEach(function(doc) {
    print('\n📊 Kategori: ' + doc._id);
    print('   Total Stok: ' + doc.total_stok);
    print('   Jumlah Jenis: ' + doc.jumlah_jenis);
});

print('\n\n=== BARANG DENGAN STOK RENDAH (<10) ===');
var lowStock = db.barang.find({ jumlah_barang: { $lt: 10 } }).toArray();
if (lowStock.length > 0) {
    lowStock.forEach(function(doc) {
        print('⚠️  ' + doc.nama_barang + ' - Stok: ' + doc.jumlah_barang);
    });
} else {
    print('✅ Tidak ada barang dengan stok rendah');
}

print('\n========================================');
print('✅ Selesai!');
print('========================================\n');
