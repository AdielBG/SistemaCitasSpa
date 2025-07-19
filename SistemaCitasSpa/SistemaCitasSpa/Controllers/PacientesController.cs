using Microsoft.AspNetCore.Mvc;

namespace SistemaCitasSpa.Controllers
{
    public class PacientesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
