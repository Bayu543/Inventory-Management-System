using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventarisKKP.Data;
using InventarisKKP.Models;
using InventarisKKP.Services;

namespace InventarisKKP.Controllers
{
    /// <summary>
    /// Controller untuk transaksi barang masuk dan keluar
    /// </summary>
    [Authorize]
    public class TransaksiController : Controller
    {
        private readonly InventarisDbContext _context;
        private readonly IActivityLogService _logService;

        public TransaksiController(InventarisDbContext context, IActivityLogService logService)
        {
            _context = context;
            _logService = logService;
        }

        #region Barang Masuk

        /// <summary>
        /// Daftar transaksi barang masuk
        /// </summary>
        public async Task<IActionResult> BarangMasuk()
        {
            var barangMasuks = await _context.BarangMasuks
                .Include(bm => bm.Barang)
                .ThenInclude(b => b!.Kategori)
                .OrderByDescending(bm => bm.TanggalMasuk)
                .ToListAsync();

            Console.WriteLine($"[DEBUG] Total barang masuk: {barangMasuks.Count}");
            foreach (var bm in barangMasuks)
            {
                Console.WriteLine($"[DEBUG] - MasukId: {bm.MasukId}, BarangId: {bm.BarangId}, Barang: {bm.Barang?.NamaBarang ?? "NULL"}, Jumlah: {bm.Jumlah}");
            }

            return View(barangMasuks);
        }

        /// <summary>
        /// Form input barang masuk
        /// </summary>
        public async Task<IActionResult> CreateBarangMasuk()
        {
            try
            {
                var barangs = await _context.Barangs
                    .Include(b => b.Kategori)
                    .OrderBy(b => b.NamaBarang)
                    .ToListAsync();

                Console.WriteLine($"[DEBUG] Total barang di database: {barangs.Count}");
                
                if (barangs.Any())
                {
                    foreach (var b in barangs)
                    {
                        Console.WriteLine($"[DEBUG] - {b.BarangId}: {b.NamaBarang} (Stok: {b.Stok})");
                    }
                }

                if (!barangs.Any())
                {
                    TempData["Warning"] = "Belum ada data barang. Silakan tambahkan barang terlebih dahulu.";
                    return RedirectToAction("Index", "Barang");
                }

                // Kirim data barang langsung ke view
                ViewBag.Barangs = barangs;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] CreateBarangMasuk: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack: {ex.StackTrace}");
                TempData["Error"] = "Terjadi kesalahan saat memuat data barang: " + ex.Message;
                return RedirectToAction("BarangMasuk");
            }
        }

        /// <summary>
        /// Proses input barang masuk
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBarangMasuk(BarangMasuk barangMasuk)
        {
            Console.WriteLine($"[DEBUG] ========== CreateBarangMasuk POST ==========");
            Console.WriteLine($"[DEBUG] BarangId: {barangMasuk.BarangId}");
            Console.WriteLine($"[DEBUG] Jumlah: {barangMasuk.Jumlah}");
            Console.WriteLine($"[DEBUG] TanggalMasuk: {barangMasuk.TanggalMasuk}");
            Console.WriteLine($"[DEBUG] ModelState.IsValid: {ModelState.IsValid}");
            
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Simpan transaksi barang masuk
                    _context.BarangMasuks.Add(barangMasuk);
                    await _context.SaveChangesAsync();

                    // Update stok barang (tambah)
                    var barang = await _context.Barangs.FindAsync(barangMasuk.BarangId);
                    if (barang != null)
                    {
                        barang.Stok += barangMasuk.Jumlah;
                        _context.Update(barang);
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();

                    await _logService.LogActivityAsync(
                        User.Identity?.Name ?? "Unknown",
                        "BARANG_MASUK",
                        $"Input barang masuk: {barang?.NamaBarang} sebanyak {barangMasuk.Jumlah}"
                    );

                    TempData["Success"] = "Barang masuk berhasil dicatat";
                    
                    // PENTING: Redirect untuk mencegah form resubmission
                    return RedirectToAction(nameof(BarangMasuk));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    
                    Console.WriteLine($"[ERROR] CreateBarangMasuk: {ex.Message}");
                    Console.WriteLine($"[ERROR] Stack: {ex.StackTrace}");
                    
                    TempData["Error"] = "Terjadi kesalahan saat menyimpan data: " + ex.Message;
                    
                    // Redirect ke halaman list meskipun error
                    return RedirectToAction(nameof(BarangMasuk));
                }
            }

            // Reload form jika ada error validasi
            Console.WriteLine("[DEBUG] Reloading form with validation errors");
            var barangs = await _context.Barangs
                .Include(b => b.Kategori)
                .OrderBy(b => b.NamaBarang)
                .ToListAsync();

            ViewBag.Barangs = barangs;
            return View(barangMasuk);
        }

        #endregion

        #region Barang Keluar

        /// <summary>
        /// Daftar transaksi barang keluar
        /// </summary>
        public async Task<IActionResult> BarangKeluar()
        {
            var barangKeluars = await _context.BarangKeluars
                .Include(bk => bk.Barang)
                .ThenInclude(b => b!.Kategori)
                .OrderByDescending(bk => bk.TanggalKeluar)
                .ToListAsync();

            return View(barangKeluars);
        }

        /// <summary>
        /// Form input barang keluar
        /// </summary>
        public async Task<IActionResult> CreateBarangKeluar()
        {
            // Hanya tampilkan barang yang stoknya > 0
            var barangsWithStock = await _context.Barangs
                .Include(b => b.Kategori)
                .Where(b => b.Stok > 0)
                .OrderBy(b => b.NamaBarang)
                .ToListAsync();

            if (!barangsWithStock.Any())
            {
                TempData["Warning"] = "Tidak ada barang dengan stok tersedia untuk transaksi keluar";
                return RedirectToAction(nameof(BarangKeluar));
            }

            // Format dropdown dengan informasi stok
            ViewBag.BarangId = new SelectList(
                barangsWithStock.Select(b => new {
                    BarangId = b.BarangId,
                    DisplayText = $"{b.NamaBarang} - {b.Kategori?.NamaKategori} (Stok: {b.Stok})"
                }), 
                "BarangId", 
                "DisplayText"
            );
            
            return View();
        }

        /// <summary>
        /// Proses input barang keluar
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBarangKeluar(BarangKeluar barangKeluar)
        {
            Console.WriteLine($"[DEBUG] ========== CreateBarangKeluar POST ==========");
            Console.WriteLine($"[DEBUG] BarangId: {barangKeluar.BarangId}");
            Console.WriteLine($"[DEBUG] Jumlah: {barangKeluar.Jumlah}");
            Console.WriteLine($"[DEBUG] TanggalKeluar: {barangKeluar.TanggalKeluar}");
            Console.WriteLine($"[DEBUG] ModelState.IsValid: {ModelState.IsValid}");
            
            // Validasi barang exists dan stok mencukupi
            var barang = await _context.Barangs
                .Include(b => b.Kategori)
                .FirstOrDefaultAsync(b => b.BarangId == barangKeluar.BarangId);
            
            if (barang == null)
            {
                ModelState.AddModelError("BarangId", "Barang tidak ditemukan");
            }
            else if (barang.Stok < barangKeluar.Jumlah)
            {
                ModelState.AddModelError("Jumlah", $"Stok tidak mencukupi. Stok tersedia: {barang.Stok} unit");
            }
            else if (barangKeluar.Jumlah <= 0)
            {
                ModelState.AddModelError("Jumlah", "Jumlah harus lebih dari 0");
            }
            
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Simpan transaksi barang keluar
                    _context.BarangKeluars.Add(barangKeluar);
                    await _context.SaveChangesAsync();

                    // Update stok barang (kurangi)
                    var stokSebelum = barang!.Stok;
                    barang.Stok -= barangKeluar.Jumlah;
                    _context.Update(barang);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    // Log aktivitas dengan detail lengkap
                    await _logService.LogActivityAsync(
                        User.Identity?.Name ?? "Unknown",
                        "BARANG_KELUAR",
                        $"Input barang keluar: {barang.NamaBarang} sebanyak {barangKeluar.Jumlah} unit. Stok: {stokSebelum} → {barang.Stok}"
                    );

                    // Peringatan jika stok menipis
                    if (barang.Stok <= 5 && barang.Stok > 0)
                    {
                        TempData["Warning"] = $"Peringatan: Stok {barang.NamaBarang} tinggal {barang.Stok} unit";
                    }
                    else if (barang.Stok == 0)
                    {
                        TempData["Warning"] = $"Peringatan: Stok {barang.NamaBarang} sudah habis!";
                    }

                    TempData["Success"] = $"Barang keluar berhasil dicatat. Stok {barang.NamaBarang} sekarang: {barang.Stok} unit";
                    
                    // PENTING: Redirect untuk mencegah form resubmission
                    return RedirectToAction(nameof(BarangKeluar));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    
                    Console.WriteLine($"[ERROR] CreateBarangKeluar: {ex.Message}");
                    Console.WriteLine($"[ERROR] Stack: {ex.StackTrace}");
                    
                    // Log error
                    await _logService.LogActivityAsync(
                        User.Identity?.Name ?? "Unknown",
                        "ERROR_BARANG_KELUAR",
                        $"Error saat input barang keluar: {ex.Message}"
                    );
                    
                    TempData["Error"] = "Terjadi kesalahan saat menyimpan data: " + ex.Message;
                    
                    // Redirect ke halaman list meskipun error
                    return RedirectToAction(nameof(BarangKeluar));
                }
            }

            // Jika ada error validasi, reload dropdown dengan barang yang memiliki stok
            Console.WriteLine("[DEBUG] Reloading form with validation errors");
            
            var barangsWithStock = await _context.Barangs
                .Include(b => b.Kategori)
                .Where(b => b.Stok > 0)
                .OrderBy(b => b.NamaBarang)
                .ToListAsync();

            ViewBag.BarangId = new SelectList(
                barangsWithStock.Select(b => new {
                    BarangId = b.BarangId,
                    DisplayText = $"{b.NamaBarang} - {b.Kategori?.NamaKategori} (Stok: {b.Stok})"
                }), 
                "BarangId", 
                "DisplayText", 
                barangKeluar.BarangId
            );
            
            return View(barangKeluar);
        }

        #endregion

        /// <summary>
        /// API untuk mendapatkan stok barang (untuk AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStokBarang(int barangId)
        {
            try
            {
                var barang = await _context.Barangs.FindAsync(barangId);
                if (barang == null)
                    return Json(new { success = false, message = "Barang tidak ditemukan" });

                return Json(new { 
                    success = true, 
                    stok = barang.Stok,
                    namaBarang = barang.NamaBarang
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
            }
        }

        /// <summary>
        /// API untuk mendapatkan daftar barang dengan stok > 0 (untuk AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBarangWithStock()
        {
            try
            {
                var barangs = await _context.Barangs
                    .Include(b => b.Kategori)
                    .Where(b => b.Stok > 0)
                    .Select(b => new {
                        barangId = b.BarangId,
                        namaBarang = b.NamaBarang,
                        kategori = b.Kategori!.NamaKategori,
                        stok = b.Stok
                    })
                    .ToListAsync();

                return Json(new { success = true, data = barangs });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
            }
        }
    }
}