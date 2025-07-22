//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCitasSpa.Models;
using System.Text;

namespace SistemaCitasSpa.Controllers
{
    public class PacientesController : Controller
    {

        private readonly SpaDbContext _context;

        // Constructor con inyección de dependencias
        public PacientesController(SpaDbContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public ActionResult Index()
        {
            try
            {
                var pacientes = _context.Pacientes.ToList();
                return View(pacientes);
            }
            catch (Exception ex)
            {
                // Maneja el error y lo muestra en una vista amigable o lo redirige
                ViewBag.Error = "Ocurrió un error al cargar los pacientes: " + ex.Message;
                return View(new List<Paciente>());
            }
        }



        // GET: Pacientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Paciente paciente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Pacientes.Add(paciente);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Paciente registrado exitosamente.";
                    return RedirectToAction(nameof(Index));

                    //return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al guardar el paciente: " + ex.Message;
            }

            return View(paciente);
        }


        // GET: Pacientes/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }



        // POST: Pacientes/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Paciente paciente)
        {
            if (id != paciente.PacienteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos del paciente actualizados correctamente.";
                    return RedirectToAction(nameof(Index));

                    //return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Pacientes.Any(e => e.PacienteID == paciente.PacienteID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Error = "Verifica los datos ingresados.";
            return View(paciente);
        }


        //Pacientes/Details/
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pac = _context.Pacientes.FirstOrDefault(p => p.PacienteID == id);

            if (pac == null)
            {
                return NotFound();
            }

            return View(pac);
        }


        //Pacientes/Delete/
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pac = _context.Pacientes.FirstOrDefault(p => p.PacienteID == id);

            if (pac == null)
            {
                return NotFound();
            }

            return View(pac);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var pac = _context.Pacientes.FirstOrDefault(p => p.PacienteID == id);
            if (pac != null)
            {
                _context.Pacientes.Remove(pac);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Paciente eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }



        //Exponrtar CSV
        public IActionResult ExportarCSV()
        {
            var pacientes = _context.Pacientes.ToList();

            var sb = new StringBuilder();
            sb.AppendLine("ID,Nombre,Apellido,Telefono,Correo");

            foreach (var pac in pacientes)
            {
                sb.AppendLine($"{pac.PacienteID},{pac.Nombre},{pac.Apellido},{pac.Telefono},{pac.Correo}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var output = new FileContentResult(bytes, "text/csv")
            {
                FileDownloadName = "Pacientes.csv"
            };

            return output;
        }

        //-------------------------------------------------------------------------------

    }
}
