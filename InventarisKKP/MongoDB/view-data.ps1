# ========================================
# VIEW MONGODB DATA - PowerShell Script
# Sistem Informasi Inventaris Barang
# ========================================

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   MONGODB DATA VIEWER" -ForegroundColor Cyan
Write-Host "   Database: InventarisKKP" -ForegroundColor Cyan
Write-Host "   Collection: barang" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if mongosh is available
$mongoshPath = Get-Command mongosh -ErrorAction SilentlyContinue
if (-not $mongoshPath) {
    Write-Host "❌ Error: mongosh tidak ditemukan!" -ForegroundColor Red
    Write-Host "   Pastikan MongoDB sudah terinstall" -ForegroundColor Yellow
    exit 1
}

Write-Host "✓ MongoDB Shell ditemukan" -ForegroundColor Green
Write-Host ""

# Run mongosh commands
Write-Host "Mengambil data dari MongoDB..." -ForegroundColor Yellow
Write-Host ""

mongosh --quiet --file view-data.js

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ Selesai!" -ForegroundColor Green
Write-Host ""
Write-Host "Untuk melihat di MongoDB Compass:" -ForegroundColor Yellow
Write-Host "1. Buka MongoDB Compass" -ForegroundColor White
Write-Host "2. Connect ke: mongodb://localhost:27017" -ForegroundColor White
Write-Host "3. Pilih database: InventarisKKP" -ForegroundColor White
Write-Host "4. Pilih collection: barang" -ForegroundColor White
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
