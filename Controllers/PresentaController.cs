using Microsoft.AspNetCore.Mvc;

namespace ConectDB.Controllers
{
    public class PresentaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
