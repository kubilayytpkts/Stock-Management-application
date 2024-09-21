using Microsoft.AspNetCore.Mvc;

namespace StockManagementUI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
