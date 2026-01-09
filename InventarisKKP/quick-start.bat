@echo off
chcp 65001 >nul
cls

:MENU
echo ========================================
echo    INVENTARIS KKP - QUICK START
echo ========================================
echo.
echo 1. Run Development Server
echo 2. Reset Database
echo 3. Stop Running Server
echo 4. Exit
echo.
set /p choice="Pilih opsi (1-4): "

if "%choice%"=="1" goto RUN_SERVER
if "%choice%"=="2" goto RESET_DB
if "%choice%"=="3" goto STOP_SERVER
if "%choice%"=="4" goto EXIT
goto MENU

:RUN_SERVER
cls
echo ========================================
echo    STARTING SERVER
echo ========================================
echo.
echo Checking port 5000...

:: Kill process yang menggunakan port 5000
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5000') do (
    echo Killing process %%a...
    taskkill /F /PID %%a >nul 2>&1
)

echo Port 5000 is now free
echo.
echo Starting application...
echo URL: http://localhost:5000
echo.
echo Login:
echo - Admin: admin / admin123
echo - User: user / user123
echo.
echo Press Ctrl+C to stop the server
echo ========================================
echo.

dotnet run

goto MENU

:RESET_DB
cls
echo ========================================
echo    RESET DATABASE
echo ========================================
echo.
echo WARNING: This will delete all data!
echo.
set /p confirm="Are you sure? (Y/N): "

if /i "%confirm%"=="Y" (
    echo.
    echo Stopping server...
    for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5000') do (
        taskkill /F /PID %%a >nul 2>&1
    )
    
    echo Deleting database files...
    if exist "InventarisKKP.db" del /f /q "InventarisKKP.db"
    if exist "InventarisKKP.db-shm" del /f /q "InventarisKKP.db-shm"
    if exist "InventarisKKP.db-wal" del /f /q "InventarisKKP.db-wal"
    
    echo.
    echo Database reset complete!
    echo Run the server to create a new database.
    echo.
) else (
    echo.
    echo Reset cancelled.
    echo.
)

pause
goto MENU

:STOP_SERVER
cls
echo ========================================
echo    STOP SERVER
echo ========================================
echo.
echo Stopping all processes on port 5000...

for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5000') do (
    echo Killing process %%a...
    taskkill /F /PID %%a >nul 2>&1
)

echo.
echo Server stopped!
echo.
pause
goto MENU

:EXIT
cls
echo.
echo Stopping server before exit...
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5000') do (
    taskkill /F /PID %%a >nul 2>&1
)
echo.
echo Goodbye!
echo.
timeout /t 2 >nul
exit
