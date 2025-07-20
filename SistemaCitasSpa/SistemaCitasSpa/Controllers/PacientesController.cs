//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCitasSpa.Models;

namespace SistemaCitasSpa.Controllers
{
    public class PacientesController : Controller
    {

        private SpaDbContext db = new SpaDbContext();
        
        // GET: Pacientes
        public ActionResult Index()
        {
            try
            {
                var pacientes = db.Pacientes.ToList();
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
                    db.Pacientes.Add(paciente);
                    db.SaveChanges();
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

            var paciente = await db.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
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
                    db.Update(paciente);
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos del paciente actualizados correctamente.";
                    return RedirectToAction(nameof(Index));

                    //return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!db.Pacientes.Any(e => e.PacienteID == paciente.PacienteID))
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


    }
}
