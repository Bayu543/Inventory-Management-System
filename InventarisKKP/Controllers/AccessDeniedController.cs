using Microsoft.AspNetCore.Mvc;

namespace InventarisKKP.Controllers
{
    /// <summary>
    /// Controller untuk halaman Access Denied
    /// </summary>
    public class AccessDeniedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
