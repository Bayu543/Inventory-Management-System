using Microsoft.AspNetCore.Mvc;

namespace InventarisKKP.Controllers
{
    /// <summary>
    /// Controller untuk menangani error pages
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Halaman error umum
        /// </summary>
        [Route("Error/{statusCode?}")]
        public IActionResult Index(int? statusCode = null)
        {
            ViewBag.StatusCode = statusCode;
            
            switch (statusCode)
            {
                case 400:
                    ViewBag.ErrorMessage = "Permintaan tidak valid. Periksa data yang Anda masukkan.";
                    ViewBag.ErrorTitle = "Bad Request";
                    break;
                case 404:
                    ViewBag.ErrorMessage = "Halaman yang Anda cari tidak ditemukan.";
                    ViewBag.ErrorTitle = "Halaman Tidak Ditemukan";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Terjadi kesalahan internal server. Silakan coba lagi nanti.";
                    ViewBag.ErrorTitle = "Kesalahan Server";
                    break;
                default:
                    ViewBag.ErrorMessage = "Terjadi kesalahan yang tidak diketahui.";
                    ViewBag.ErrorTitle = "Kesalahan";
                    break;
            }
            
            return View();
        }
    }
}