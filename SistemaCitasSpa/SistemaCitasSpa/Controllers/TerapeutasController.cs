﻿using Microsoft.AspNetCore.Mvc;
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

        // GET: ExportarCSV
        public async Task<FileResult> ExportarCSV()
        {
            try
            {
                var terapeutas = await _context.Terapeuta.OrderBy(t => t.TerapeutaID).ToListAsync();
                var sb = new StringBuilder();
                sb.AppendLine("ID,Nombre,Especialidad,Telefono,Correo");

                foreach (var t in terapeutas)
                {
                    sb.AppendLine($"{t.TerapeutaID},{t.Nombre},{t.Especialidad},{t.Telefono},{t.Correo}");
                }

                var data = Encoding.UTF8.GetBytes(sb.ToString());
                return File(data, "text/csv", "terapeutas.csv");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al exportar terapeutas: " + ex.Message;
                return null; // O redirigir a una acción
            }
        }

        // GET: Terapeutas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Terapeutas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Terapeutum terapeuta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Terapeuta.Add(terapeuta);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Terapeuta registrado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error al registrar el terapeuta: " + ex.Message;
                }
            }

            return View(terapeuta);
        }

        // GET: Terapeutas/Edit/
        public IActionResult Edit(int id)
        {
            var terapeuta = _context.Terapeuta.Find(id);
            if (terapeuta == null)
                return NotFound();

            return View(terapeuta);
        }

        // POST: Terapeutas/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Terapeutum terapeuta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Terapeuta.Update(terapeuta);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Terapeuta actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error al actualizar: " + ex.Message;
                }
            }

            return View(terapeuta);
        }

        // Terapeutas/Details/
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terapeuta = _context.Terapeuta.FirstOrDefault(t => t.TerapeutaID == id);

            if (terapeuta == null)
            {
                return NotFound();
            }

            return View(terapeuta);
        }

        // Terapeutas/Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terapeuta = _context.Terapeuta.FirstOrDefault(t => t.TerapeutaID == id);

            if (terapeuta == null)
            {
                return NotFound();
            }

            return View(terapeuta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var terapeuta = _context.Terapeuta.FirstOrDefault(t => t.TerapeutaID == id);
            if (terapeuta != null)
            {
                _context.Terapeuta.Remove(terapeuta);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Terapeuta eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
