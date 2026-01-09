# Script untuk melihat data di MongoDB
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "MongoDB Data Viewer - InventarisKKP" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Cek koneksi MongoDB
Write-Host "[1] Checking MongoDB Connection..." -ForegroundColor Yellow
try {
    $version = mongosh --eval "db.version()" --quiet 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ MongoDB is running (Version: $version)" -ForegroundColor Green
    } else {
        Write-Host "✗ MongoDB is not running!" -ForegroundColor Red
        Write-Host "Please start MongoDB service first." -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ MongoDB is not running!" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Tampilkan data Kategoris
Write-Host "[2] Data Kategoris:" -ForegroundColor Yellow
mongosh InventarisKKP --eval "db.Kategoris.find().pretty()" --quiet
Write-Host ""

# Tampilkan jumlah kategori
$countKategori = mongosh InventarisKKP --eval "db.Kategoris.countDocuments()" --quiet
Write-Host "Total Kategoris: $countKategori" -ForegroundColor Cyan
Write-Host ""

# Tampilkan data Barangs
Write-Host "[3] Data Barangs:" -ForegroundColor Yellow
mongosh InventarisKKP --eval "db.Barangs.find().pretty()" --quiet
Write-Host ""

# Tampilkan jumlah barang
$countBarang = mongosh InventarisKKP --eval "db.Barangs.countDocuments()" --quiet
Write-Host "Total Barangs: $countBarang" -ForegroundColor Cyan
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Done!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
