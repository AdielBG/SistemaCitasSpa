using Microsoft.AspNetCore.Mvc;

namespace SistemaCitasSpa.Controllers
{
    public class CitasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
