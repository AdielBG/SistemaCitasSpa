using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCitasSpa.Models;
using System.Text;

namespace SistemaCitasSpa.Controllers
{
    public class CitasController : Controller
    {
        private readonly SpaDbContext _context;

        public CitasController(SpaDbContext context)
        {
            _context = context;
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            try
            {
                var citas = await _context.Cita
                    .Include(c => c.Paciente)
                    .Include(c => c.Servicio)
                    .Include(c => c.Terapeuta)
                    .ToListAsync();

                return View(citas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar las citas: " + ex.Message;
                return View(new List<Citum>());
            }
        }

        // Exportar CSV
        public async Task<IActionResult> ExportarCSV()
        {
            try
            {
                var citas = await _context.Cita
                    .Include(c => c.Paciente)
                    .Include(c => c.Servicio)
                    .Include(c => c.Terapeuta)
                    .ToListAsync();

                var sb = new StringBuilder();
                sb.AppendLine("CitaID,Paciente,Servicio,Terapeuta,Fecha,Hora,Duracion,Días Restantes,Estado");

                foreach (var cita in citas)
                {
                    var fechaHora = new DateTime(cita.Fecha.Year, cita.Fecha.Month, cita.Fecha.Day,
                                                 cita.Hora.Hour, cita.Hora.Minute, 0);
                    var ahora = DateTime.Now;
                    var tiempoRestante = fechaHora - ahora;

                    string estado = fechaHora.Date > ahora.Date ? "Vigente" :
                                    fechaHora.Date == ahora.Date ? "En proceso" : "Finalizado";

                    sb.AppendLine($"{cita.CitaID}," +
                                  $"{cita.Paciente?.Nombre}," +
                                  $"{cita.Servicio?.NombreServicio}," +
                                  $"{cita.Terapeuta?.Nombre}," +
                                  $"{cita.Fecha}," +
                                  $"{cita.Hora}," +
                                  $"{cita.Servicio?.DuracionEnMinutos} minutos," +
                                  $"{(tiempoRestante.TotalMinutes > 0 ? tiempoRestante.ToString(@"dd\.hh\:mm") : "0")}," +
                                  $"{estado}");
                }

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(bytes, "text/csv", "Citas.csv");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al exportar citas: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Citas/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                await CargarViewBagsAsync();

                var nuevaCita = new Citum
                {
                    Fecha = DateOnly.FromDateTime(DateTime.Today),
                    Hora = TimeOnly.FromDateTime(DateTime.Now)
                };

                return View(nuevaCita);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar la página: " + ex.Message;
                return View(new Citum());
            }
        }

        // POST: Citas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Citum cita)
        {
            try
            {
                ModelState.Remove("Paciente");
                ModelState.Remove("Servicio");
                ModelState.Remove("Terapeuta");

                if (cita.PacienteID <= 0)
                    ModelState.AddModelError("PacienteID", "Debe seleccionar un paciente.");

                if (cita.ServicioID <= 0)
                    ModelState.AddModelError("ServicioID", "Debe seleccionar un servicio.");

                if (cita.TerapeutaID <= 0)
                    ModelState.AddModelError("TerapeutaID", "Debe seleccionar un terapeuta.");

                if (!ModelState.IsValid)
                {
                    ViewBag.ValidationErrors = "Hay errores de validación en el formulario.";
                    await CargarViewBagsAsync(cita);
                    return View(cita);
                }

                _context.Cita.Add(cita);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cita registrada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al guardar la cita: " + ex.Message;
            }

            await CargarViewBagsAsync(cita);
            return View(cita);
        }

        // Método helper para cargar datos en los dropdowns
        private async Task CargarViewBagsAsync(Citum cita = null)
        {
            try
            {
                ViewBag.Pacientes = new SelectList(await _context.Pacientes.ToListAsync(), "PacienteID", "Nombre", cita?.PacienteID);
                ViewBag.Servicios = new SelectList(await _context.Servicios.ToListAsync(), "ServicioID", "NombreServicio", cita?.ServicioID);
                ViewBag.Terapeutas = new SelectList(await _context.Terapeuta.ToListAsync(), "TerapeutaID", "Nombre", cita?.TerapeutaID);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar las listas: " + ex.Message;
                ViewBag.Pacientes = new SelectList(new List<object>(), "PacienteID", "Nombre");
                ViewBag.Servicios = new SelectList(new List<object>(), "ServicioID", "NombreServicio");
                ViewBag.Terapeutas = new SelectList(new List<object>(), "TerapeutaID", "Nombre");
            }
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .Include(c => c.Paciente)
                .Include(c => c.Servicio)
                .Include(c => c.Terapeuta)
                .FirstOrDefaultAsync(c => c.CitaID == id);

            if (cita == null)
            {
                return NotFound();
            }

            // Cargar ViewBags para los dropdowns
            CargarViewBagsEdit(cita);

            return View(cita);
        }

        // POST: Citas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Citum cita)
        {
            if (id != cita.CitaID)
            {
                return NotFound();
            }

            // Remover errores de validación de las propiedades de navegación
            ModelState.Remove("Paciente");
            ModelState.Remove("Servicio");
            ModelState.Remove("Terapeuta");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cita actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Cita.Any(e => e.CitaID == cita.CitaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error al actualizar la cita: " + ex.Message;
                }
            }

            // IMPORTANTE: Recargar las relaciones cuando hay errores de validación
            // Esto evita el NullReferenceException en las propiedades calculadas
            cita = await _context.Cita
                .Include(c => c.Paciente)
                .Include(c => c.Servicio)
                .Include(c => c.Terapeuta)
                .FirstOrDefaultAsync(c => c.CitaID == id) ?? cita;

            ViewBag.Error = ViewBag.Error ?? "Verifica los datos ingresados.";
            CargarViewBagsEdit(cita);

            return View(cita);
        }

        // Método helper específico para Edit
        private void CargarViewBagsEdit(Citum cita)
        {
            try
            {
                ViewBag.PacienteID = new SelectList(_context.Pacientes.ToList(), "PacienteID", "Nombre", cita?.PacienteID);
                ViewBag.ServicioID = new SelectList(_context.Servicios.ToList(), "ServicioID", "NombreServicio", cita?.ServicioID);
                ViewBag.TerapeutaID = new SelectList(_context.Terapeuta.ToList(), "TerapeutaID", "Nombre", cita?.TerapeutaID);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar las listas: " + ex.Message;
                ViewBag.PacienteID = new SelectList(new List<object>(), "PacienteID", "Nombre");
                ViewBag.ServicioID = new SelectList(new List<object>(), "ServicioID", "NombreServicio");
                ViewBag.TerapeutaID = new SelectList(new List<object>(), "TerapeutaID", "Nombre");
            }
        }


        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .Include(c => c.Paciente)
                .Include(c => c.Servicio)
                .Include(c => c.Terapeuta)
                .FirstOrDefaultAsync(c => c.CitaID == id);

            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .Include(c => c.Paciente)
                .Include(c => c.Servicio)
                .Include(c => c.Terapeuta)
                .FirstOrDefaultAsync(c => c.CitaID == id);

            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cita = await _context.Cita.FirstOrDefaultAsync(c => c.CitaID == id);
            if (cita != null)
            {
                _context.Cita.Remove(cita);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"La cita #{id} fue eliminada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = $"No se encontró la cita #{id}.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
