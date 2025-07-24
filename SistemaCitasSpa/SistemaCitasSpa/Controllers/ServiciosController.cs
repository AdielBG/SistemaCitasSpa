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


        // Servicios/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Servicios.Add(servicio);
                    _context.SaveChanges();
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
        public IActionResult Edit(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Servicios.Update(servicio);
                    _context.SaveChanges();
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
        public IActionResult DeleteConfirmed(int id)
        {
            var servicio = _context.Servicios.FirstOrDefault(s => s.ServicioID == id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Servicio eliminado correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }




    }
}
