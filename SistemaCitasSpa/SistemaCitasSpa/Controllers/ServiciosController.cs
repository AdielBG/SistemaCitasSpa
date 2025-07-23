using Microsoft.AspNetCore.Mvc;

namespace SistemaCitasSpa.Controllers
{
    public class ServiciosController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
