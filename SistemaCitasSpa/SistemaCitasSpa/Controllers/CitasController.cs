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
        public IActionResult Index()
        {
            try
            {
                var citas = _context.Cita
                    .Include(c => c.Paciente)
                    .Include(c => c.Servicio)
                    .Include(c => c.Terapeuta)
                    .ToList();

                return View(citas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar las citas: " + ex.Message;
                return View(new List<Citum>());
            }
        }

        // Exportar CSV
        public IActionResult ExportarCSV()
        {
            try
            {
                var citas = _context.Cita
                    .Include(c => c.Paciente)
                    .Include(c => c.Servicio)
                    .Include(c => c.Terapeuta)
                    .ToList();

                var sb = new StringBuilder();
                sb.AppendLine("CitaID,Paciente,Servicio,Terapeuta,Fecha,Hora,Duracion,Días Restantes,Estado");

                foreach (var cita in citas)
                {
                    var fechaHora = new DateTime(cita.Fecha.Year, cita.Fecha.Month, cita.Fecha.Day,
                                                 cita.Hora.Hour, cita.Hora.Minute, 0);
                    var ahora = DateTime.Now;
                    var tiempoRestante = fechaHora - ahora;

                    string estado;
                    if (fechaHora.Date > ahora.Date)
                        estado = "Vigente";
                    else if (fechaHora.Date == ahora.Date)
                        estado = "En proceso";
                    else
                        estado = "Finalizado";

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
        public ActionResult Create()
        {
            ViewBag.Pacientes = new SelectList(_context.Pacientes.ToList(), "PacienteID", "Nombre");
            ViewBag.Servicios = new SelectList(_context.Servicios.ToList(), "ServicioID", "Nombre");
            ViewBag.Terapeutas = new SelectList(_context.Terapeuta.ToList(), "TerapeutaID", "Nombre");
            return View(new Citum
            {
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Hora = TimeOnly.FromDateTime(DateTime.Now)
            });
        }


        // POST: Citas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Citum cita)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Cita.Add(cita);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Cita registrada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al guardar la cita: " + ex.Message;
            }

            ViewBag.Pacientes = new SelectList(_context.Pacientes, "PacienteID", "Nombre", cita.PacienteID);
            ViewBag.Servicios = new SelectList(_context.Servicios, "ServicioID", "Nombre", cita.ServicioID);
            ViewBag.Terapeutas = new SelectList(_context.Terapeuta, "TerapeutaID", "Nombre", cita.TerapeutaID);
            return View(cita);
        }


    }
}
