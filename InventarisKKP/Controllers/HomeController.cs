using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventarisKKP.Data;
using InventarisKKP.Services;
using Microsoft.EntityFrameworkCore;

namespace InventarisKKP.Controllers
{
    /// <summary>
    /// Controller utama untuk halaman dashboard dan laporan
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly InventarisDbContext _context;
        private readonly IActivityLogService _logService;

        public HomeController(InventarisDbContext context, IActivityLogService logService)
        {
            _context = context;
            _logService = logService;
        }

        /// <summary>
        /// Halaman dashboard utama
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Statistik untuk dashboard
            ViewBag.TotalKategori = await _context.Kategoris.CountAsync();
            ViewBag.TotalBarang = await _context.Barangs.CountAsync();
            ViewBag.TotalStok = await _context.Barangs.SumAsync(b => b.Stok);

            return View();
        }

        /// <summary>
        /// Laporan inventaris - menampilkan stok semua barang (hanya Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Laporan()
        {
            var barangs = await _context.Barangs
                .Include(b => b.Kategori)
                .OrderBy(b => b.NamaBarang)
                .ToListAsync();

            await _logService.LogActivityAsync(
                User.Identity?.Name ?? "Unknown",
                "VIEW_REPORT",
                "Melihat laporan inventaris"
            );

            return View(barangs);
        }
    }
}