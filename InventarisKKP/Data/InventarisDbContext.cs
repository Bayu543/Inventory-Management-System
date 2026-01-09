using Microsoft.EntityFrameworkCore;
using InventarisKKP.Models;

namespace InventarisKKP.Data
{
    /// <summary>
    /// DbContext untuk SQL Server menggunakan Entity Framework Core
    /// </summary>
    public class InventarisDbContext : DbContext
    {
        public InventarisDbContext(DbContextOptions<InventarisDbContext> options) : base(options)
        {
        }

        // DbSet untuk setiap tabel
        public DbSet<User> Users { get; set; }
        public DbSet<Kategori> Kategoris { get; set; }
        public DbSet<Barang> Barangs { get; set; }
        public DbSet<BarangMasuk> BarangMasuks { get; set; }
        public DbSet<BarangKeluar> BarangKeluars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurasi relasi dan constraint
            modelBuilder.Entity<Barang>()
                .HasOne(b => b.Kategori)
                .WithMany(k => k.Barangs)
                .HasForeignKey(b => b.KategoriId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<BarangMasuk>()
                .HasOne(bm => bm.Barang)
                .WithMany(b => b.BarangMasuks)
                .HasForeignKey(bm => bm.BarangId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BarangKeluar>()
                .HasOne(bk => bk.Barang)
                .WithMany(b => b.BarangKeluars)
                .HasForeignKey(bk => bk.BarangId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data kategori default
            modelBuilder.Entity<Kategori>().HasData(
                new Kategori { KategoriId = 1, NamaKategori = "Elektronik" },
                new Kategori { KategoriId = 2, NamaKategori = "Furniture" },
                new Kategori { KategoriId = 3, NamaKategori = "Alat Tulis" }
            );

            // Seed data user default
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    UserId = 1, 
                    Username = "admin", 
                    Password = "admin123", // Dalam production gunakan hash
                    NamaLengkap = "Administrator",
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}