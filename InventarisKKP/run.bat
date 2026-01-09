@echo off
chcp 65001 >nul
cls

echo ========================================
echo    INVENTARIS KKP
echo ========================================
echo.
echo Checking port 5000...

:: Kill process yang menggunakan port 5000
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5000') do (
    echo Stopping existing server (PID: %%a)...
    taskkill /F /PID %%a >nul 2>&1
)

echo.
echo Starting application...
echo.
echo URL: http://localhost:5000
echo.
echo Login:
echo - Admin: admin / admin123
echo - User: user / user123
echo.
echo Press Ctrl+C to stop
echo ========================================
echo.

dotnet run
