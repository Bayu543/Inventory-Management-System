using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventarisKKP.Models;
using InventarisKKP.Services;

namespace InventarisKKP.Controllers
{
    /// <summary>
    /// Controller untuk CRUD Barang
    /// </summary>
    [Authorize]
    public class BarangController : BaseController
    {
        private readonly IBarangService _barangService;
        private readonly IActivityLogService _logService;
        private readonly IMongoBarangService _mongoBarangService;
        private readonly IMongoKategoriService _mongoKategoriService;

        public BarangController(
            IBarangService barangService, 
            IActivityLogService logService,
            IMongoBarangService mongoBarangService,
            IMongoKategoriService mongoKategoriService)
        {
            _barangService = barangService;
            _logService = logService;
            _mongoBarangService = mongoBarangService;
            _mongoKategoriService = mongoKategoriService;
        }

        /// <summary>
        /// Menampilkan daftar barang
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var barangs = await _barangService.GetAllBarangsAsync();
                return View(barangs);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Terjadi kesalahan saat memuat data barang: {ex.Message}";
                return View(new List<Barang>());
            }
        }

        /// <summary>
        /// Form tambah barang
        /// </summary>
        public async Task<IActionResult> Create()
        {
            try
            {
                await _barangService.EnsureKategoriDataAsync();
                ViewBag.KategoriId = await _barangService.GetKategoriDropdownAsync();
                PreventResubmission();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Terjadi kesalahan saat memuat data: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Proses tambah barang
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(Barang barang, string? formToken)
        {
            try
            {
                Console.WriteLine($"[CREATE BARANG] Received: NamaBarang={barang.NamaBarang}, KategoriId={barang.KategoriId}, Stok={barang.Stok}");
                
                if (IsResubmission(formToken))
                {
                    Console.WriteLine("[CREATE BARANG] Form resubmission detected");
                    TempData["Warning"] = "Form sudah pernah disubmit. Silakan refresh halaman.";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    Console.WriteLine("[CREATE BARANG] ModelState is valid");
                    
                    // Simpan ke SQL Server
                    await _barangService.CreateBarangAsync(barang);
                    Console.WriteLine($"[CREATE BARANG] Successfully created barang in SQL Server with ID: {barang.BarangId}");

                    // Ambil nama kategori
                    var kategori = await _mongoKategoriService.GetByKategoriIdAsync(barang.KategoriId);
                    var namaKategori = kategori?.NamaKategori ?? "Unknown";

                    // Simpan ke MongoDB
                    var mongoBarang = new MongoBarang
                    {
                        BarangId = barang.BarangId,
                        NamaBarang = barang.NamaBarang,
                        KategoriId = barang.KategoriId,
                        NamaKategori = namaKategori,
                        Stok = barang.Stok
                    };
                    await _mongoBarangService.CreateAsync(mongoBarang);
                    Console.WriteLine($"[CREATE BARANG] Successfully created barang in MongoDB with ID: {mongoBarang.Id}");

                    await _logService.LogActivityAsync(
                        User.Identity?.Name ?? "Unknown",
                        "CREATE_BARANG",
                        $"Menambah barang: {barang.NamaBarang}"
                    );

                    TempData["Success"] = "Barang berhasil ditambahkan ke SQL Server dan MongoDB";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Console.WriteLine("[CREATE BARANG] ModelState is invalid");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"  - Error: {error.ErrorMessage}");
                    }
                }

                ViewBag.KategoriId = await _barangService.GetKategoriDropdownAsync(barang.KategoriId);
                PreventResubmission();
                return View(barang);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CREATE BARANG] Exception: {ex.Message}");
                TempData["Error"] = $"Terjadi kesalahan saat menyimpan barang: {ex.Message}";
                ViewBag.KategoriId = await _barangService.GetKategoriDropdownAsync(barang.KategoriId);
                PreventResubmission();
                return View(barang);
            }
        }

        /// <summary>
        /// Form edit barang
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var barang = await _barangService.GetBarangByIdAsync(id.Value);
                if (barang == null) return NotFound();

                ViewBag.KategoriId = await _barangService.GetKategoriDropdownAsync(barang.KategoriId);
                
                // Prevent caching
                SetNoCacheHeaders();
                
                return View(barang);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Terjadi kesalahan: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Proses edit barang
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Barang barang)
        {
            Console.WriteLine($"[EDIT BARANG POST] Received: id={id}, BarangId={barang.BarangId}, NamaBarang={barang.NamaBarang}");
            
            if (id != barang.BarangId)
            {
                Console.WriteLine($"[EDIT BARANG POST] ID mismatch: id={id}, BarangId={barang.BarangId}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"[EDIT BARANG POST] Updating barang id={id}");
                    
                    // Update di SQL Server
                    await _barangService.UpdateBarangAsync(id, barang);

                    // Ambil nama kategori
                    var kategori = await _mongoKategoriService.GetByKategoriIdAsync(barang.KategoriId);
                    var namaKategori = kategori?.NamaKategori ?? "Unknown";

                    // Update di MongoDB
                    var mongoBarang = await _mongoBarangService.GetByBarangIdAsync(barang.BarangId);
                    if (mongoBarang != null)
                    {
                        mongoBarang.NamaBarang = barang.NamaBarang;
                        mongoBarang.KategoriId = barang.KategoriId;
                        mongoBarang.NamaKategori = namaKategori;
                        mongoBarang.Stok = barang.Stok;
                        await _mongoBarangService.UpdateAsync(mongoBarang.Id!, mongoBarang);
                        Console.WriteLine($"[EDIT BARANG POST] Updated barang in MongoDB: {mongoBarang.Id}");
                    }

                    await _logService.LogActivityAsync(
                        User.Identity?.Name ?? "Unknown",
                        "UPDATE_BARANG",
                        $"Mengubah barang: {barang.NamaBarang}"
                    );

                    Console.WriteLine($"[EDIT BARANG POST] Success");
                    TempData["Success"] = "Barang berhasil diubah di SQL Server dan MongoDB";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[EDIT BARANG POST] Error: {ex.Message}");
                    TempData["Error"] = $"Terjadi kesalahan: {ex.Message}";
                }
            }
            else
            {
                Console.WriteLine($"[EDIT BARANG POST] ModelState invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"  - Error: {error.ErrorMessage}");
                }
            }

            ViewBag.KategoriId = await _barangService.GetKategoriDropdownAsync(barang.KategoriId);
            return View(barang);
        }

        /// <summary>
        /// Hapus barang
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Hapus dari SQL Server
                await _barangService.DeleteBarangAsync(id);

                // Hapus dari MongoDB
                var mongoBarang = await _mongoBarangService.GetByBarangIdAsync(id);
                if (mongoBarang != null)
                {
                    await _mongoBarangService.DeleteAsync(mongoBarang.Id!);
                    Console.WriteLine($"[DELETE] Deleted barang from MongoDB: {mongoBarang.Id}");
                }

                await _logService.LogActivityAsync(
                    User.Identity?.Name ?? "Unknown",
                    "DELETE_BARANG",
                    $"Menghapus barang dengan ID: {id}"
                );

                TempData["Success"] = "Barang berhasil dihapus dari SQL Server dan MongoDB";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}