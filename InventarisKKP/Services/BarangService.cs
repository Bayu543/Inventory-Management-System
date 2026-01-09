using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventarisKKP.Data;
using InventarisKKP.Models;

namespace InventarisKKP.Services
{
    /// <summary>
    /// Service untuk mengelola Barang
    /// </summary>
    public class BarangService : IBarangService
    {
        private readonly InventarisDbContext _context;

        public BarangService(InventarisDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Mengambil semua barang
        /// </summary>
        public async Task<List<Barang>> GetAllBarangsAsync()
        {
            try
            {
                return await _context.Barangs
                    .Include(b => b.Kategori)
                    .OrderBy(b => b.NamaBarang)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error mengambil data barang: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Mengambil barang berdasarkan ID
        /// </summary>
        public async Task<Barang?> GetBarangByIdAsync(int id)
        {
            try
            {
                return await _context.Barangs
                    .Include(b => b.Kategori)
                    .FirstOrDefaultAsync(b => b.BarangId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error mengambil barang dengan ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Membuat barang baru
        /// </summary>
        public async Task<bool> CreateBarangAsync(Barang barang)
        {
            try
            {
                _context.Barangs.Add(barang);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error membuat barang: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Update barang
        /// </summary>
        public async Task<bool> UpdateBarangAsync(int id, Barang barang)
        {
            try
            {
                if (id != barang.BarangId)
                    return false;

                _context.Update(barang);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error update barang: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Hapus barang
        /// </summary>
        public async Task<bool> DeleteBarangAsync(int id)
        {
            try
            {
                var barang = await _context.Barangs.FindAsync(id);
                if (barang == null)
                    return false;

                // Cek apakah barang masih ada transaksi
                var hasTransaksi = await _context.BarangMasuks.AnyAsync(bm => bm.BarangId == id) ||
                                   await _context.BarangKeluars.AnyAsync(bk => bk.BarangId == id);

                if (hasTransaksi)
                    throw new Exception("Barang tidak dapat dihapus karena masih ada transaksi");

                _context.Barangs.Remove(barang);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error menghapus barang: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Mengambil dropdown kategori
        /// </summary>
        public async Task<SelectList> GetKategoriDropdownAsync(int? selectedId = null)
        {
            try
            {
                var kategoris = await _context.Kategoris
                    .OrderBy(k => k.NamaKategori)
                    .ToListAsync();

                Console.WriteLine($"[GetKategoriDropdown] Found {kategoris.Count} kategoris");
                foreach (var k in kategoris)
                {
                    Console.WriteLine($"  - ID: {k.KategoriId}, Name: {k.NamaKategori}");
                }

                var selectList = new SelectList(kategoris, "KategoriId", "NamaKategori", selectedId);
                Console.WriteLine($"[GetKategoriDropdown] SelectList created with {selectList.Count()} items");
                
                return selectList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetKategoriDropdown] Error: {ex.Message}");
                throw new Exception($"Error mengambil dropdown kategori: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Memastikan ada data kategori (force seed jika perlu)
        /// </summary>
        public async Task EnsureKategoriDataAsync()
        {
            try
            {
                var existingCount = await _context.Kategoris.CountAsync();

                if (existingCount == 0)
                {
                    var kategoris = new[]
                    {
                        new Kategori { NamaKategori = "Elektronik" },
                        new Kategori { NamaKategori = "Furniture" },
                        new Kategori { NamaKategori = "Alat Tulis" }
                    };

                    _context.Kategoris.AddRange(kategoris);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error memastikan data kategori: {ex.Message}", ex);
            }
        }
    }
}
