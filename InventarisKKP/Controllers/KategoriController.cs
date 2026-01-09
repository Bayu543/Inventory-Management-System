using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using InventarisKKP.Data;
using InventarisKKP.Models;
using InventarisKKP.Services;

namespace InventarisKKP.Controllers
{
    /// <summary>
    /// Controller untuk CRUD Kategori (hanya Admin)
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class KategoriController : BaseController
    {
        private readonly InventarisDbContext _context;
        private readonly IActivityLogService _logService;
        private readonly IMongoKategoriService _mongoKategoriService;

        public KategoriController(
            InventarisDbContext context, 
            IActivityLogService logService,
            IMongoKategoriService mongoKategoriService)
        {
            _context = context;
            _logService = logService;
            _mongoKategoriService = mongoKategoriService;
        }

        /// <summary>
        /// Menampilkan daftar kategori
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var kategoris = await _context.Kategoris
                    .Include(k => k.Barangs)
                    .OrderBy(k => k.NamaKategori)
                    .ToListAsync();
                return View(kategoris);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Terjadi kesalahan saat memuat data kategori: " + ex.Message;
                return View(new List<Kategori>());
            }
        }

        /// <summary>
        /// Form tambah kategori
        /// </summary>
        public IActionResult Create()
        {
            PreventResubmission();
            return View();
        }

        /// <summary>
        /// Proses tambah kategori
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(Kategori kategori)
        {
            try
            {
                Console.WriteLine($"[CREATE] Received kategori: {kategori.NamaKategori}");
                
                if (ModelState.IsValid)
                {
                    Console.WriteLine("[CREATE] ModelState is valid");
                    
                    // Trim whitespace
                    kategori.NamaKategori = kategori.NamaKategori?.Trim() ?? "";
                    Console.WriteLine($"[CREATE] After trim: {kategori.NamaKategori}");
                    
                    if (string.IsNullOrWhiteSpace(kategori.NamaKategori))
                    {
                        Console.WriteLine("[CREATE] Nama kategori is empty after trim");
                        ModelState.AddModelError("NamaKategori", "Nama kategori tidak boleh kosong");
                        PreventResubmission();
                        return View(kategori);
                    }

                    // Cek duplikasi nama kategori di SQL Server
                    var existingKategori = await _context.Kategoris
                        .FirstOrDefaultAsync(k => k.NamaKategori.ToLower() == kategori.NamaKategori.ToLower());
                    
                    if (existingKategori != null)
                    {
                        Console.WriteLine($"[CREATE] Kategori '{kategori.NamaKategori}' already exists");
                        ModelState.AddModelError("NamaKategori", "Nama kategori sudah ada");
                        PreventResubmission();
                        return View(kategori);
                    }

                    // Cek duplikasi di MongoDB
                    var existsInMongo = await _mongoKategoriService.ExistsAsync(kategori.NamaKategori);
                    if (existsInMongo)
                    {
                        Console.WriteLine($"[CREATE] Kategori '{kategori.NamaKategori}' already exists in MongoDB");
                        ModelState.AddModelError("NamaKategori", "Nama kategori sudah ada di MongoDB");
                        PreventResubmission();
                        return View(kategori);
                    }

                    Console.WriteLine($"[CREATE] Adding kategori to SQL Server: {kategori.NamaKategori}");
                    _context.Kategoris.Add(kategori);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"[CREATE] Successfully saved kategori to SQL Server with ID: {kategori.KategoriId}");

                    // Simpan juga ke MongoDB
                    var mongoKategori = new MongoKategori
                    {
                        KategoriId = kategori.KategoriId,
                        NamaKategori = kategori.NamaKategori
                    };
                    await _mongoKategoriService.CreateAsync(mongoKategori);
                    Console.WriteLine($"[CREATE] Successfully saved kategori to MongoDB with ID: {mongoKategori.Id}");

                    await _logService.LogActivityAsync(
                        User.Identity?.Name ?? "Unknown",
                        "CREATE_KATEGORI",
                        $"Menambah kategori: {kategori.NamaKategori}"
                    );

                    TempData["Success"] = $"Kategori '{kategori.NamaKategori}' berhasil ditambahkan ke SQL Server dan MongoDB";
                    
                    // Redirect untuk mencegah form resubmission
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Log validation errors
                    Console.WriteLine("[CREATE] ModelState is invalid");
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"[CREATE] Validation error: {error.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CREATE] Exception: {ex.Message}");
                Console.WriteLine($"[CREATE] Stack trace: {ex.StackTrace}");
                TempData["Error"] = "Terjadi kesalahan saat menyimpan kategori: " + ex.Message;
            }
            
            PreventResubmission();
            return View(kategori);
        }

        /// <summary>
        /// Form edit kategori
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var kategori = await _context.Kategoris.FindAsync(id);
            if (kategori == null) return NotFound();

            return View(kategori);
        }

        /// <summary>
        /// Proses edit kategori
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kategori kategori)
        {
            if (id != kategori.KategoriId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Update di SQL Server
                    _context.Update(kategori);
                    await _context.SaveChangesAsync();

                    // Update di MongoDB
                    var mongoKategori = await _mongoKategoriService.GetByKategoriIdAsync(kategori.KategoriId);
                    if (mongoKategori != null)
                    {
                        mongoKategori.NamaKategori = kategori.NamaKategori;
                        await _mongoKategoriService.UpdateAsync(mongoKategori.Id!, mongoKategori);
                        Console.WriteLine($"[EDIT] Updated kategori in MongoDB: {mongoKategori.Id}");
                    }

                    await _logService.LogActivityAsync(
                        User.Identity?.Name ?? "Unknown",
                        "UPDATE_KATEGORI",
                        $"Mengubah kategori: {kategori.NamaKategori}"
                    );

                    TempData["Success"] = "Kategori berhasil diubah di SQL Server dan MongoDB";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriExists(kategori.KategoriId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        /// <summary>
        /// Hapus kategori
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var kategori = await _context.Kategoris.FindAsync(id);
            if (kategori != null)
            {
                // Cek apakah kategori masih digunakan
                var hasBarang = await _context.Barangs.AnyAsync(b => b.KategoriId == id);
                if (hasBarang)
                {
                    TempData["Error"] = "Kategori tidak dapat dihapus karena masih digunakan oleh barang";
                    return RedirectToAction(nameof(Index));
                }

                // Hapus dari SQL Server
                _context.Kategoris.Remove(kategori);
                await _context.SaveChangesAsync();

                // Hapus dari MongoDB
                var mongoKategori = await _mongoKategoriService.GetByKategoriIdAsync(id);
                if (mongoKategori != null)
                {
                    await _mongoKategoriService.DeleteAsync(mongoKategori.Id!);
                    Console.WriteLine($"[DELETE] Deleted kategori from MongoDB: {mongoKategori.Id}");
                }

                await _logService.LogActivityAsync(
                    User.Identity?.Name ?? "Unknown",
                    "DELETE_KATEGORI",
                    $"Menghapus kategori: {kategori.NamaKategori}"
                );

                TempData["Success"] = "Kategori berhasil dihapus dari SQL Server dan MongoDB";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KategoriExists(int id)
        {
            return _context.Kategoris.Any(e => e.KategoriId == id);
        }
    }
}