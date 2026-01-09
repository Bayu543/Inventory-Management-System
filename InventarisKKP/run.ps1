# Script untuk menjalankan aplikasi Inventaris KKP
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   INVENTARIS KKP - START SERVER" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Function untuk kill process di port 5000
function Stop-PortProcess {
    param([int]$Port = 5000)
    
    Write-Host "Checking port $Port..." -ForegroundColor Yellow
    
    $connections = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue
    
    if ($connections) {
        foreach ($conn in $connections) {
            $processId = $conn.OwningProcess
            Write-Host "Stopping process $processId on port $Port..." -ForegroundColor Yellow
            Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
        }
        Write-Host "Port $Port is now free" -ForegroundColor Green
    } else {
        Write-Host "Port $Port is already free" -ForegroundColor Green
    }
    Write-Host ""
}

# Stop existing server
Stop-PortProcess -Port 5000

# Start server
Write-Host "Starting application..." -ForegroundColor Green
Write-Host ""
Write-Host "URL: " -NoNewline
Write-Host "http://localhost:5000" -ForegroundColor Cyan
Write-Host ""
Write-Host "Login:" -ForegroundColor Yellow
Write-Host "  - Admin: admin / admin123"
Write-Host "  - User: user / user123"
Write-Host ""
Write-Host "Press Ctrl+C to stop the server" -ForegroundColor Gray
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Run dotnet
try {
    dotnet run
}
catch {
    Write-Host ""
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Press any key to exit..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}
