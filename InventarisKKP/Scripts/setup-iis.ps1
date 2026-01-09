# PowerShell Script untuk Setup IIS dan Application Pool
# Jalankan sebagai Administrator

Write-Host "========================================" -ForegroundColor Green
Write-Host "    SETUP IIS UNTUK INVENTARIS KKP" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green

# Variables
$siteName = "InventarisKKP"
$appPoolName = "InventarisKKP_AppPool"
$sitePath = "C:\inetpub\wwwroot\InventarisKKP"
$port = 8080

try {
    # 1. Enable IIS Features
    Write-Host "`n1. Mengaktifkan IIS Features..." -ForegroundColor Yellow
    
    $features = @(
        "IIS-WebServerRole",
        "IIS-WebServer",
        "IIS-CommonHttpFeatures",
        "IIS-HttpErrors",
        "IIS-HttpLogging",
        "IIS-RequestFiltering",
        "IIS-StaticContent",
        "IIS-DefaultDocument",
        "IIS-DirectoryBrowsing",
        "IIS-ASPNET45",
        "IIS-NetFxExtensibility45",
        "IIS-ISAPIExtensions",
        "IIS-ISAPIFilter",
        "IIS-HttpCompressionStatic",
        "IIS-WebServerManagementTools",
        "IIS-ManagementConsole"
    )
    
    foreach ($feature in $features) {
        Enable-WindowsOptionalFeature -Online -FeatureName $feature -All -NoRestart
    }
    
    Write-Host "   IIS Features berhasil diaktifkan" -ForegroundColor Green

    # 2. Install ASP.NET Core Hosting Bundle (Manual step)
    Write-Host "`n2. ASP.NET Core Hosting Bundle..." -ForegroundColor Yellow
    Write-Host "   PENTING: Download dan install ASP.NET Core Hosting Bundle dari:" -ForegroundColor Red
    Write-Host "   https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Red
    Write-Host "   Pilih 'Hosting Bundle' untuk Windows" -ForegroundColor Red

    # 3. Import WebAdministration Module
    Import-Module WebAdministration -ErrorAction SilentlyContinue

    # 4. Create Application Pool
    Write-Host "`n3. Membuat Application Pool..." -ForegroundColor Yellow
    
    if (Get-IISAppPool -Name $appPoolName -ErrorAction SilentlyContinue) {
        Remove-WebAppPool -Name $appPoolName
        Write-Host "   Application Pool lama dihapus" -ForegroundColor Yellow
    }
    
    New-WebAppPool -Name $appPoolName
    Set-ItemProperty -Path "IIS:\AppPools\$appPoolName" -Name processModel.identityType -Value ApplicationPoolIdentity
    Set-ItemProperty -Path "IIS:\AppPools\$appPoolName" -Name managedRuntimeVersion -Value ""
    Set-ItemProperty -Path "IIS:\AppPools\$appPoolName" -Name enable32BitAppOnWin64 -Value $false
    
    Write-Host "   Application Pool '$appPoolName' berhasil dibuat" -ForegroundColor Green

    # 5. Create Website
    Write-Host "`n4. Membuat Website..." -ForegroundColor Yellow
    
    if (Get-Website -Name $siteName -ErrorAction SilentlyContinue) {
        Remove-Website -Name $siteName
        Write-Host "   Website lama dihapus" -ForegroundColor Yellow
    }
    
    # Create directory if not exists
    if (!(Test-Path $sitePath)) {
        New-Item -ItemType Directory -Path $sitePath -Force
    }
    
    New-Website -Name $siteName -Port $port -PhysicalPath $sitePath -ApplicationPool $appPoolName
    
    Write-Host "   Website '$siteName' berhasil dibuat di port $port" -ForegroundColor Green

    # 6. Set Permissions
    Write-Host "`n5. Setting Permissions..." -ForegroundColor Yellow
    
    $acl = Get-Acl $sitePath
    $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule("IIS_IUSRS", "FullControl", "ContainerInherit,ObjectInherit", "None", "Allow")
    $acl.SetAccessRule($accessRule)
    Set-Acl $sitePath $acl
    
    Write-Host "   Permissions berhasil diset" -ForegroundColor Green

    # 7. Start Application Pool and Website
    Write-Host "`n6. Starting Services..." -ForegroundColor Yellow
    
    Start-WebAppPool -Name $appPoolName
    Start-Website -Name $siteName
    
    Write-Host "   Application Pool dan Website berhasil distart" -ForegroundColor Green

    # 8. Summary
    Write-Host "`n========================================" -ForegroundColor Green
    Write-Host "    SETUP IIS SELESAI!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "Website Name    : $siteName" -ForegroundColor Cyan
    Write-Host "App Pool Name   : $appPoolName" -ForegroundColor Cyan
    Write-Host "Physical Path   : $sitePath" -ForegroundColor Cyan
    Write-Host "Port            : $port" -ForegroundColor Cyan
    Write-Host "URL             : http://localhost:$port" -ForegroundColor Cyan
    Write-Host "`nLangkah selanjutnya:" -ForegroundColor Yellow
    Write-Host "1. Install ASP.NET Core Hosting Bundle" -ForegroundColor White
    Write-Host "2. Deploy aplikasi ke folder: $sitePath" -ForegroundColor White
    Write-Host "3. Restart IIS: iisreset" -ForegroundColor White
    Write-Host "========================================" -ForegroundColor Green

} catch {
    Write-Host "`nERROR: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Pastikan menjalankan PowerShell sebagai Administrator" -ForegroundColor Red
}

Read-Host "`nTekan Enter untuk keluar"