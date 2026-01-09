using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using InventarisKKP.Data;
using InventarisKKP.Models;
using InventarisKKP.Services;

namespace InventarisKKP.Controllers
{
    /// <summary>
    /// Controller untuk manajemen user (hanya Admin)
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly InventarisDbContext _context;
        private readonly IActivityLogService _logService;

        public UserController(InventarisDbContext context, IActivityLogService logService)
        {
            _context = context;
            _logService = logService;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
            return View(users);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, string PasswordConfirm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Password) || user.Password != PasswordConfirm)
                {
                    ModelState.AddModelError("", "Password dan konfirmasi password tidak cocok");
                    return View(user);
                }

                // Cek username sudah ada
                if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Username sudah digunakan");
                    return View(user);
                }

                // Hash password
                user.Password = PasswordHashService.HashPassword(user.Password);
                user.CreatedAt = DateTime.Now;
                
                // Default role adalah User jika tidak diset
                if (string.IsNullOrWhiteSpace(user.Role))
                {
                    user.Role = "User";
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await _logService.LogActivityAsync(User.Identity?.Name ?? "System", "CREATE_USER", 
                    $"User baru dibuat: {user.Username} dengan role {user.Role}");

                TempData["Success"] = "User berhasil ditambahkan";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Terjadi kesalahan: " + ex.Message);
                return View(user);
            }
        }

        // GET: User/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user, string? NewPassword, string? PasswordConfirm)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Cek username duplikat (kecuali user sendiri)
                if (await _context.Users.AnyAsync(u => u.Username == user.Username && u.UserId != id))
                {
                    ModelState.AddModelError("Username", "Username sudah digunakan");
                    return View(user);
                }

                // Update data
                existingUser.Username = user.Username;
                existingUser.NamaLengkap = user.NamaLengkap;
                existingUser.Role = user.Role;
                existingUser.IsActive = user.IsActive;

                // Update password jika diisi
                if (!string.IsNullOrWhiteSpace(NewPassword))
                {
                    if (NewPassword != PasswordConfirm)
                    {
                        ModelState.AddModelError("", "Password baru dan konfirmasi tidak cocok");
                        return View(user);
                    }
                    existingUser.Password = PasswordHashService.HashPassword(NewPassword);
                }

                await _context.SaveChangesAsync();

                await _logService.LogActivityAsync(User.Identity?.Name ?? "System", "UPDATE_USER", 
                    $"User diupdate: {user.Username}");

                TempData["Success"] = "User berhasil diupdate";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Terjadi kesalahan: " + ex.Message);
                return View(user);
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "User tidak ditemukan" });
                }

                // Tidak bisa hapus diri sendiri
                if (user.Username == User.Identity?.Name)
                {
                    return Json(new { success = false, message = "Tidak dapat menghapus akun sendiri" });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                await _logService.LogActivityAsync(User.Identity?.Name ?? "System", "DELETE_USER", 
                    $"User dihapus: {user.Username}");

                return Json(new { success = true, message = "User berhasil dihapus" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
            }
        }
    }
}
