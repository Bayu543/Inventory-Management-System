@echo off
echo ========================================
echo Restart dan Test Aplikasi Inventaris
echo ========================================
echo.

cd /d "%~dp0"

echo [1/4] Membersihkan build lama...
dotnet clean >nul 2>&1

echo [2/4] Rebuild aplikasi...
dotnet build
if errorlevel 1 (
    echo.
    echo [ERROR] Build gagal! Periksa error di atas.
    pause
    exit /b 1
)

echo.
echo [3/4] Aplikasi siap dijalankan!
echo.
echo ========================================
echo INSTRUKSI:
echo ========================================
echo 1. Aplikasi akan berjalan di: http://localhost:5000
echo 2. Login dengan:
echo    Username: admin
echo    Password: admin123
echo 3. Buka menu: Transaksi ^> Input Barang Masuk
echo 4. Test dropdown "Pilih Barang"
echo.
echo Tekan Ctrl+C untuk stop aplikasi
echo ========================================
echo.

echo [4/4] Menjalankan aplikasi...
echo.
dotnet run

pause
