// ========================================
// SETUP KOLEKSI BARANG - MongoDB
// Sistem Informasi Inventaris Barang
// ========================================

// Gunakan database InventarisKKP
db = db.getSiblingDB('InventarisKKP');

// Hapus koleksi lama jika ada (opsional - hati-hati!)
// db.barang.drop();

// Buat koleksi 'barang' dengan validasi schema
db.createCollection("barang", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["nama_barang", "kategori", "jumlah_barang", "tanggal_masuk"],
      properties: {
        _id: {
          bsonType: "objectId",
          description: "ID unik otomatis dari MongoDB"
        },
        nama_barang: {
          bsonType: "string",
          minLength: 1,
          maxLength: 200,
          description: "Nama barang - wajib diisi, maksimal 200 karakter"
        },
        kategori: {
          bsonType: "string",
          minLength: 1,
          maxLength: 100,
          description: "Kategori barang - wajib diisi"
        },
        jumlah_barang: {
          bsonType: "int",
          minimum: 0,
          description: "Jumlah stok barang - wajib diisi, minimal 0"
        },
        deskripsi: {
          bsonType: "string",
          maxLength: 1000,
          description: "Deskripsi detail barang - opsional"
        },
        tanggal_masuk: {
          bsonType: "date",
          description: "Tanggal barang masuk - wajib diisi"
        },
        tanggal_update: {
          bsonType: "date",
          description: "Tanggal terakhir update"
        }
      }
    }
  },
  validationLevel: "strict",
  validationAction: "error"
});

print("✓ Koleksi 'barang' berhasil dibuat dengan validasi schema");

// Buat index untuk performa query
db.barang.createIndex({ "nama_barang": 1 });
db.barang.createIndex({ "kategori": 1 });
db.barang.createIndex({ "tanggal_masuk": -1 });

print("✓ Index berhasil dibuat");

// Insert sample data
db.barang.insertMany([
  {
    nama_barang: "Laptop Dell Latitude 5420",
    kategori: "Elektronik",
    jumlah_barang: NumberInt(15),
    deskripsi: "Laptop untuk keperluan kantor dengan spesifikasi Intel Core i5, RAM 8GB, SSD 256GB",
    tanggal_masuk: new Date("2024-01-15T08:00:00Z"),
    tanggal_update: new Date("2024-01-15T08:00:00Z")
  },
  {
    nama_barang: "Meja Kerja Kayu Jati",
    kategori: "Furniture",
    jumlah_barang: NumberInt(25),
    deskripsi: "Meja kerja ukuran 120x60 cm dengan laci 3 tingkat",
    tanggal_masuk: new Date("2024-02-10T09:30:00Z"),
    tanggal_update: new Date("2024-02-10T09:30:00Z")
  },
  {
    nama_barang: "Printer HP LaserJet Pro",
    kategori: "Elektronik",
    jumlah_barang: NumberInt(8),
    deskripsi: "Printer laser monochrome untuk kebutuhan cetak dokumen kantor",
    tanggal_masuk: new Date("2024-03-05T10:15:00Z"),
    tanggal_update: new Date("2024-03-20T14:30:00Z")
  },
  {
    nama_barang: "Kursi Kantor Ergonomis",
    kategori: "Furniture",
    jumlah_barang: NumberInt(30),
    deskripsi: "Kursi kantor dengan sandaran punggung adjustable dan roda",
    tanggal_masuk: new Date("2024-02-20T11:00:00Z"),
    tanggal_update: new Date("2024-02-20T11:00:00Z")
  },
  {
    nama_barang: "Pulpen Pilot G2",
    kategori: "ATK",
    jumlah_barang: NumberInt(500),
    deskripsi: "Pulpen gel warna hitam 0.7mm",
    tanggal_masuk: new Date("2024-01-08T07:45:00Z"),
    tanggal_update: new Date("2024-03-15T16:20:00Z")
  },
  {
    nama_barang: "Kertas A4 80gsm",
    kategori: "ATK",
    jumlah_barang: NumberInt(200),
    deskripsi: "Kertas HVS A4 80gsm per rim (500 lembar)",
    tanggal_masuk: new Date("2024-01-10T08:30:00Z"),
    tanggal_update: new Date("2024-01-10T08:30:00Z")
  },
  {
    nama_barang: "Monitor LED 24 inch",
    kategori: "Elektronik",
    jumlah_barang: NumberInt(20),
    deskripsi: "Monitor LED 24 inch Full HD 1920x1080 dengan port HDMI dan VGA",
    tanggal_masuk: new Date("2024-02-01T09:00:00Z"),
    tanggal_update: new Date("2024-02-01T09:00:00Z")
  },
  {
    nama_barang: "Lemari Arsip Besi",
    kategori: "Furniture",
    jumlah_barang: NumberInt(10),
    deskripsi: "Lemari arsip 4 laci dengan kunci untuk penyimpanan dokumen",
    tanggal_masuk: new Date("2024-03-01T10:00:00Z"),
    tanggal_update: new Date("2024-03-01T10:00:00Z")
  },
  {
    nama_barang: "Mouse Wireless Logitech",
    kategori: "Elektronik",
    jumlah_barang: NumberInt(35),
    deskripsi: "Mouse wireless dengan sensor optical dan baterai AA",
    tanggal_masuk: new Date("2024-01-20T08:15:00Z"),
    tanggal_update: new Date("2024-03-10T13:45:00Z")
  },
  {
    nama_barang: "Stapler Besar",
    kategori: "ATK",
    jumlah_barang: NumberInt(45),
    deskripsi: "Stapler besar kapasitas 50 lembar dengan isi staples No.23",
    tanggal_masuk: new Date("2024-01-12T09:20:00Z"),
    tanggal_update: new Date("2024-01-12T09:20:00Z")
  }
]);

print("✓ Sample data berhasil diinsert");

// Tampilkan jumlah dokumen
var count = db.barang.countDocuments();
print("\n📊 Total dokumen di koleksi 'barang': " + count);

// Tampilkan contoh dokumen
print("\n📄 Contoh dokumen:");
printjson(db.barang.findOne());

print("\n✅ Setup koleksi 'barang' selesai!");
