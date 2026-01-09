# ⚡ QUICK REFERENCE - MongoDB Barang Collection

## 📌 Connection
```
mongodb://localhost:27017/InventarisKKP
```

## 📊 Schema (Quick View)
```javascript
{
  _id: ObjectId,              // Auto
  nama_barang: String,        // Required, 1-200 char
  kategori: String,           // Required, 1-100 char
  jumlah_barang: Integer,     // Required, >= 0
  deskripsi: String,          // Optional, max 1000 char
  tanggal_masuk: Date,        // Required
  tanggal_update: Date        // Optional
}
```

## 🚀 Common Queries

### View All
```javascript
db.barang.find()
```

### Find by Category
```javascript
db.barang.find({ kategori: "Elektronik" })
```

### Low Stock (< 10)
```javascript
db.barang.find({ jumlah_barang: { $lt: 10 } })
```

### Search by Name
```javascript
db.barang.find({ nama_barang: /laptop/i })
```

### Count Documents
```javascript
db.barang.countDocuments()
```

## ✏️ CRUD Operations

### Insert
```javascript
db.barang.insertOne({
  nama_barang: "Item Name",
  kategori: "Category",
  jumlah_barang: NumberInt(10),
  deskripsi: "Description",
  tanggal_masuk: new Date(),
  tanggal_update: new Date()
})
```

### Update
```javascript
db.barang.updateOne(
  { nama_barang: "Item Name" },
  { 
    $set: { 
      jumlah_barang: NumberInt(20),
      tanggal_update: new Date()
    }
  }
)
```

### Increment Stock
```javascript
db.barang.updateOne(
  { nama_barang: "Item Name" },
  { 
    $inc: { jumlah_barang: 5 },
    $set: { tanggal_update: new Date() }
  }
)
```

### Delete
```javascript
db.barang.deleteOne({ nama_barang: "Item Name" })
```

## 📈 Aggregations

### Total by Category
```javascript
db.barang.aggregate([
  {
    $group: {
      _id: "$kategori",
      total: { $sum: "$jumlah_barang" },
      count: { $sum: 1 }
    }
  }
])
```

### Top 5 Items
```javascript
db.barang.find().sort({ jumlah_barang: -1 }).limit(5)
```

## 🔍 Indexes
```javascript
db.barang.createIndex({ nama_barang: 1 })
db.barang.createIndex({ kategori: 1 })
db.barang.createIndex({ tanggal_masuk: -1 })
```

## ✅ Validation Rules
- `nama_barang`: Required, String, 1-200 char
- `kategori`: Required, String, 1-100 char
- `jumlah_barang`: Required, Integer, >= 0
- `deskripsi`: Optional, String, max 1000 char
- `tanggal_masuk`: Required, Date
- `tanggal_update`: Optional, Date

## 🎯 Sample Categories
- Elektronik
- Furniture
- ATK

## 📁 Files
- `setup-barang-collection.js` - Setup script
- `query-examples.js` - Query examples
- `crud-examples.js` - CRUD examples
- `README-MONGODB.md` - Full documentation
- `PANDUAN-LENGKAP.md` - Complete guide (Indonesian)
- `DIAGRAM-STRUKTUR.md` - Structure diagrams

## 🔧 Setup Command
```bash
mongosh --file setup-barang-collection.js
```

## 📊 Current Status
✅ Database: InventarisKKP  
✅ Collection: barang  
✅ Sample Data: 10 documents  
✅ Indexes: 3 created  
✅ Validation: Active (strict)
