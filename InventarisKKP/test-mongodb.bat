@echo off
echo ========================================
echo Testing MongoDB Integration
echo ========================================
echo.

echo [1] Checking MongoDB Connection...
mongosh --eval "db.version()" --quiet
if %errorlevel% neq 0 (
    echo ERROR: MongoDB is not running!
    echo Please start MongoDB service first.
    pause
    exit /b 1
)
echo MongoDB is running!
echo.

echo [2] Checking Database...
mongosh InventarisKKP --eval "db.getCollectionNames()" --quiet
echo.

echo [3] Starting Application...
echo Application will run on http://localhost:5000
echo.
echo Login credentials:
echo   Username: admin
echo   Password: admin123
echo.
echo Press Ctrl+C to stop the application
echo.

dotnet run
