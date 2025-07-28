using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCitasSpa.Models;
using System.Text;

namespace SistemaCitasSpa.Controllers
{
    public class TerapeutasController : Controller
    {
        private readonly SpaDbContext _context;

        public TerapeutasController(SpaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
