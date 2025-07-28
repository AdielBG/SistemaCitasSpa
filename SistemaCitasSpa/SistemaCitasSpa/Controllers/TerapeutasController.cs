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
        // GET: Terapeutas
        public async Task<IActionResult> Index(string mensaje = "")
        {
            ViewBag.Mensaje = mensaje;
            return View(await _context.Terapeuta.ToListAsync());
        }


        public IActionResult ExportarCSV()
        {
            var terapeutas = _context.Terapeuta.OrderBy(t => t.TerapeutaID).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("ID,Nombre,Especialidad,Telefono,Correo");

            foreach (var t in terapeutas)
            {
                sb.AppendLine($"{t.TerapeutaID},{t.Nombre},{t.Especialidad},{t.Telefono},{t.Correo}");
            }

            var data = Encoding.UTF8.GetBytes(sb.ToString());
            return File(data, "text/csv", "terapeutas.csv");
        }


    }
}
