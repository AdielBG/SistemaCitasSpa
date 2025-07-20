//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;
using SistemaCitasSpa.Models;
using Microsoft.AspNetCore.Mvc;

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
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al guardar el paciente: " + ex.Message;
            }

            return View(paciente);
        }
    }
}
