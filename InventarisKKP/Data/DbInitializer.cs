using InventarisKKP.Models;
using InventarisKKP.Services;

namespace InventarisKKP.Data
{
    /// <summary>
    /// Class untuk inisialisasi database dan seed data
    /// </summary>
    public static class DbInitializer
    {
        public static void Initialize(InventarisDbContext context)
        {
            try
            {
                // Pastikan database terbuat
                context.Database.EnsureCreated();

                // Seed Users jika belum ada
                if (!context.Users.Any())
                {
                    Console.WriteLine("[DEBUG] Creating default users...");
                    
                    var users = new User[]
                    {
                        new User
                        {
                            Username = "admin",
                            Password = PasswordHashService.HashPassword("admin123"),
                            NamaLengkap = "Administrator",
                            Role = "Admin",
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        },
                        new User
                        {
                            Username = "user",
                            Password = PasswordHashService.HashPassword("user123"),
                            NamaLengkap = "User Biasa",
                            Role = "User",
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        }
                    };

                    foreach (var user in users)
                    {
                        context.Users.Add(user);
                    }
                    context.SaveChanges();
                    
                    Console.WriteLine("[SUCCESS] Default users created:");
                    Console.WriteLine("  - Username: admin | Password: admin123 | Role: Admin");
                    Console.WriteLine("  - Username: user  | Password: user123  | Role: User");
                }
                else
                {
                    Console.WriteLine($"[DEBUG] Users already exist in database: {context.Users.Count()}");
                    
                    // Pastikan user 'user' ada
                    var regularUser = context.Users.FirstOrDefault(u => u.Username == "user");
                    if (regularUser == null)
                    {
                        Console.WriteLine("[INFO] Adding default 'user' account...");
                        context.Users.Add(new User
                        {
                            Username = "user",
                            Password = PasswordHashService.HashPassword("user123"),
                            NamaLengkap = "User Biasa",
                            Role = "User",
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        });
                        context.SaveChanges();
                        Console.WriteLine("[SUCCESS] Default 'user' account created");
                    }
                    
                    var adminUser = context.Users.FirstOrDefault(u => u.Username == "admin");
                    if (adminUser != null)
                    {
                        Console.WriteLine($"[DEBUG] Admin user found - IsActive: {adminUser.IsActive}");
                    }
                    else
                    {
                        Console.WriteLine("[WARNING] Admin user not found in database!");
                    }
                }

                // Seed Kategoris jika belum ada (terpisah dari Users)
                if (!context.Kategoris.Any())
                {
                    var kategoris = new Kategori[]
                    {
                        new Kategori { NamaKategori = "Elektronik" },
                        new Kategori { NamaKategori = "Furniture" },
                        new Kategori { NamaKategori = "Alat Tulis" }
                    };

                    foreach (var kategori in kategoris)
                    {
                        context.Kategoris.Add(kategori);
                    }
                    context.SaveChanges();
                    Console.WriteLine($"Berhasil menambahkan {kategoris.Length} kategori");
                }

                // Seed Barangs jika belum ada (setelah kategori tersimpan)
                if (!context.Barangs.Any())
                {
                    var barangs = new Barang[]
                    {
                        new Barang 
                        { 
                            NamaBarang = "Laptop Dell", 
                            KategoriId = 1, 
                            Stok = 5 
                        },
                        new Barang 
                        { 
                            NamaBarang = "Meja Kantor", 
                            KategoriId = 2, 
                            Stok = 10 
                        },
                        new Barang 
                        { 
                            NamaBarang = "Pulpen", 
                            KategoriId = 3, 
                            Stok = 100 
                        },
                        new Barang 
                        { 
                            NamaBarang = "Printer Canon", 
                            KategoriId = 1, 
                            Stok = 3 
                        }
                    };

                    foreach (var barang in barangs)
                    {
                        context.Barangs.Add(barang);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Log error untuk debugging
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw;
            }
        }
    }
}