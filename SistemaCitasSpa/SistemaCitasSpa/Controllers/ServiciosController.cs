using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCitasSpa.Models;
using System.Text;

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
            try
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al exportar servicios: " + ex.Message;
                return null; // O redirigir a una acción
            }
        }

        // Servicios/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Servicios.AddAsync(servicio);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Servicio registrado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error al registrar el servicio: " + ex.Message;
                }
            }

            return View(servicio);
        }

        //Servicios/Details
        public IActionResult Details(int id)
        {
            var servicio = _context.Servicios.FirstOrDefault(s => s.ServicioID == id);
            if (servicio == null)
                return NotFound();

            return View(servicio);
        }

        //Servicios/Edit
        public IActionResult Edit(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if (servicio == null)
                return NotFound();

            return View(servicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Servicios.Update(servicio);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Servicio actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error al actualizar: " + ex.Message;
                }
            }

            return View(servicio);
        }

        // Servicios/Delete/
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = _context.Servicios.FirstOrDefault(s => s.ServicioID == id);

            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.ServicioID == id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Servicio eliminado correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
