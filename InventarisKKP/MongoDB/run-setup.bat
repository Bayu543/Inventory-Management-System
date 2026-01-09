@echo off
echo ========================================
echo   SETUP MONGODB - KOLEKSI BARANG
echo   Sistem Informasi Inventaris Barang
echo ========================================
echo.

echo Menjalankan setup MongoDB...
echo.

mongosh --file setup-barang-collection.js

echo.
echo ========================================
echo Setup selesai!
echo.
echo Buka MongoDB Compass untuk melihat data:
echo - Database: InventarisKKP
echo - Collection: barang
echo ========================================
pause
