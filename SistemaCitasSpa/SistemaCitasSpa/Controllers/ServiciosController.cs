using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCitasSpa.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace SistemaCitasSpa.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly SpaDbContext _context;

        public ServiciosController(SpaDbContext context)
        {
            _context = context;
        }
        // GET: Servicios
        public async Task<IActionResult> Index(string mensaje = "")
        {
            ViewBag.Mensaje = mensaje;
            return View(await _context.Servicios.ToListAsync());
        }


        // GET: ExportarCSV
        public async Task<FileResult> ExportarCSV()
        {
            var servicios = await _context.Servicios.ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("ID,Nombre,Descripción,Precio");

            foreach (var serv in servicios)
            {
                builder.AppendLine($"{serv.ServicioID},{serv.NombreServicio},{serv.Descripcion},{serv.Precio}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Servicios.csv");

        }

        }
    }
