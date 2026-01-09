@echo off
echo ========================================
echo Reset Database Inventaris KKP
echo ========================================
echo.
echo PERINGATAN: Script ini akan menghapus database dan membuat ulang dengan data awal!
echo.
pause

cd /d "%~dp0"

echo.
echo [1/3] Menghapus database lama...
echo.

REM Hapus file database LocalDB jika ada
del /F /Q "%USERPROFILE%\InventarisKKP.mdf" 2>nul
del /F /Q "%USERPROFILE%\InventarisKKP_log.ldf" 2>nul

echo Database lama dihapus (jika ada)
echo.

echo [2/3] Membersihkan build...
dotnet clean >nul 2>&1

echo.
echo [3/3] Rebuild dan jalankan aplikasi...
echo Database akan dibuat ulang otomatis saat aplikasi start
echo.
dotnet build
if errorlevel 1 (
    echo.
    echo [ERROR] Build gagal!
    pause
    exit /b 1
)

echo.
echo ========================================
echo Database akan diinisialisasi saat aplikasi start
echo Cek console untuk pesan: "Database initialized successfully"
echo ========================================
echo.
echo Aplikasi akan berjalan di: http://localhost:5000
echo Login: admin / admin123
echo.
echo Tekan Ctrl+C untuk stop
echo ========================================
echo.

dotnet run

pause
