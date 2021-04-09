using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpDigital.Models;

namespace ExpDigital.Controllers
{
    public class PersonaXCitaController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: PersonaXCita
        public ActionResult Index()
        {
            var personaXCitas = db.PersonaXCitas.Include(p => p.CitaDeAtestado).Include(p => p.Persona);
            return View(personaXCitas.ToList());
        }

        // GET: PersonaXCita/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXCita personaXCita = db.PersonaXCitas.Find(id);
            if (personaXCita == null)
            {
                return HttpNotFound();
            }
            return View(personaXCita);
        }

        // GET: PersonaXCita/Create
        public ActionResult Create()
        {
            ViewBag.id_cita = new SelectList(db.CitaDeAtestados, "ID", "contrasenna");
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula");
            return View();
        }

        // POST: PersonaXCita/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_persona_x_cita,id_persona,id_cita,fechaEntrega")] PersonaXCita personaXCita)
        {
            if (ModelState.IsValid)
            {
                db.PersonaXCitas.Add(personaXCita);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cita = new SelectList(db.CitaDeAtestados, "ID", "contrasenna", personaXCita.id_cita);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXCita.id_persona);
            return View(personaXCita);
        }

        // GET: PersonaXCita/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXCita personaXCita = db.PersonaXCitas.Find(id);
            if (personaXCita == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cita = new SelectList(db.CitaDeAtestados, "ID", "contrasenna", personaXCita.id_cita);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXCita.id_persona);
            return View(personaXCita);
        }

        // POST: PersonaXCita/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_persona_x_cita,id_persona,id_cita,fechaEntrega")] PersonaXCita personaXCita)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personaXCita).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cita = new SelectList(db.CitaDeAtestados, "ID", "contrasenna", personaXCita.id_cita);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXCita.id_persona);
            return View(personaXCita);
        }

        // GET: PersonaXCita/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXCita personaXCita = db.PersonaXCitas.Find(id);
            if (personaXCita == null)
            {
                return HttpNotFound();
            }
            return View(personaXCita);
        }

        // POST: PersonaXCita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonaXCita personaXCita = db.PersonaXCitas.Find(id);
            db.PersonaXCitas.Remove(personaXCita);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
