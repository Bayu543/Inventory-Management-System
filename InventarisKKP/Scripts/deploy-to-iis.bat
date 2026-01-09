@echo off
echo ========================================
echo    DEPLOYMENT SCRIPT INVENTARIS KKP
echo ========================================
echo.

REM Set variables
set PROJECT_NAME=InventarisKKP
set PUBLISH_PATH=C:\inetpub\wwwroot\%PROJECT_NAME%
set PROJECT_PATH=%~dp0..

echo 1. Membersihkan folder publish...
if exist "%PUBLISH_PATH%" (
    rmdir /s /q "%PUBLISH_PATH%"
    echo    Folder lama berhasil dihapus
)

echo.
echo 2. Membuat folder publish...
mkdir "%PUBLISH_PATH%"
mkdir "%PUBLISH_PATH%\logs"
echo    Folder publish berhasil dibuat

echo.
echo 3. Building project...
cd /d "%PROJECT_PATH%"
dotnet clean
dotnet restore
if %errorlevel% neq 0 (
    echo    ERROR: Restore gagal!
    pause
    exit /b 1
)

echo.
echo 4. Publishing project...
dotnet publish -c Release -o "%PUBLISH_PATH%" --self-contained false
if %errorlevel% neq 0 (
    echo    ERROR: Publish gagal!
    pause
    exit /b 1
)

echo.
echo 5. Setting permissions...
icacls "%PUBLISH_PATH%" /grant "IIS_IUSRS:(OI)(CI)F" /T
icacls "%PUBLISH_PATH%\logs" /grant "IIS_IUSRS:(OI)(CI)F" /T
echo    Permissions berhasil diset

echo.
echo 6. Restarting IIS...
iisreset /noforce
echo    IIS berhasil direstart

echo.
echo ========================================
echo    DEPLOYMENT SELESAI!
echo    Path: %PUBLISH_PATH%
echo    URL: http://localhost/%PROJECT_NAME%
echo ========================================
echo.
pause