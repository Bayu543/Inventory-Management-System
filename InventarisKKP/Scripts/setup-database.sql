-- Script untuk setup database SQL Server
-- Jalankan script ini di SQL Server Management Studio

-- 1. Buat database jika belum ada
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'InventarisKKP')
BEGIN
    CREATE DATABASE InventarisKKP;
    PRINT 'Database InventarisKKP berhasil dibuat';
END
ELSE
BEGIN
    PRINT 'Database InventarisKKP sudah ada';
END

-- 2. Gunakan database
USE InventarisKKP;

-- 3. Buat user untuk aplikasi (opsional)
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'InventarisUser')
BEGIN
    CREATE LOGIN InventarisUser WITH PASSWORD = 'Inventaris123!';
    CREATE USER InventarisUser FOR LOGIN InventarisUser;
    
    -- Berikan permission
    ALTER ROLE db_datareader ADD MEMBER InventarisUser;
    ALTER ROLE db_datawriter ADD MEMBER InventarisUser;
    ALTER ROLE db_ddladmin ADD MEMBER InventarisUser;
    
    PRINT 'User InventarisUser berhasil dibuat';
END

-- 4. Verifikasi koneksi
SELECT 
    'Database Setup Complete' as Status,
    DB_NAME() as DatabaseName,
    GETDATE() as SetupTime;