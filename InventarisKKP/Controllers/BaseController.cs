using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InventarisKKP.Controllers
{
    /// <summary>
    /// Base controller dengan anti-resubmission protection
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Prevent form resubmission dengan token
        /// </summary>
        protected void PreventResubmission()
        {
            var token = Guid.NewGuid().ToString();
            TempData["FormToken"] = token;
            ViewBag.FormToken = token;
        }

        /// <summary>
        /// Check apakah form sudah pernah disubmit
        /// </summary>
        protected bool IsResubmission(string? submittedToken)
        {
            var expectedToken = TempData["FormToken"]?.ToString();
            return string.IsNullOrEmpty(submittedToken) || 
                   string.IsNullOrEmpty(expectedToken) || 
                   submittedToken != expectedToken;
        }

        /// <summary>
        /// Set cache headers untuk prevent caching
        /// </summary>
        protected void SetNoCacheHeaders()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
        }

        /// <summary>
        /// Override OnActionExecuting untuk set cache headers
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Set no-cache headers untuk semua form pages
            if (context.ActionDescriptor.DisplayName?.Contains("Create") == true ||
                context.ActionDescriptor.DisplayName?.Contains("Edit") == true)
            {
                SetNoCacheHeaders();
            }

            base.OnActionExecuting(context);
        }
    }
}