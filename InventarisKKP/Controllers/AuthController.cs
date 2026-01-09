using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using InventarisKKP.Data;
using InventarisKKP.Services;

namespace InventarisKKP.Controllers
{
    /// <summary>
    /// Controller untuk autentikasi login/logout sederhana
    /// </summary>
    public class AuthController : Controller
    {
        private readonly IActivityLogService _logService;
        private readonly InventarisDbContext _context;

        public AuthController(IActivityLogService logService, InventarisDbContext context)
        {
            _logService = logService;
            _context = context;
        }

        /// <summary>
        /// Halaman login
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Proses login dengan database
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                Console.WriteLine($"[DEBUG] Login attempt - Username: {username}");
                
                // Cari user di database
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

                Console.WriteLine($"[DEBUG] User found: {user != null}");
                if (user != null)
                {
                    Console.WriteLine($"[DEBUG] User details - Username: {user.Username}, IsActive: {user.IsActive}");
                    var passwordMatch = PasswordHashService.VerifyPassword(password, user.Password);
                    Console.WriteLine($"[DEBUG] Password match: {passwordMatch}");
                }

                if (user != null && PasswordHashService.VerifyPassword(password, user.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("NamaLengkap", user.NamaLengkap)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );

                    await _logService.LogActivityAsync(user.Username, "LOGIN", "User berhasil login");

                    return RedirectToAction("Index", "Home");
                }

                Console.WriteLine("[DEBUG] Login failed - Invalid credentials");
                ViewBag.Error = "Username atau password salah";
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Login exception: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                ViewBag.Error = "Terjadi kesalahan saat login: " + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity?.Name ?? "Unknown";
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            await _logService.LogActivityAsync(username, "LOGOUT", "User logout");

            return RedirectToAction("Login");
        }
    }
}