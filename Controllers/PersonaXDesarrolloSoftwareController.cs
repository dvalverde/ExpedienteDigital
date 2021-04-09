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
    public class PersonaXDesarrolloSoftwareController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: PersonaXDesarrolloSoftware
        public ActionResult Index()
        {
            var personaXDesarrolloSoftwares = db.PersonaXDesarrolloSoftwares.Include(p => p.DesarrolloSoftware).Include(p => p.Persona);
            return View(personaXDesarrolloSoftwares.ToList());
        }

        // GET: PersonaXDesarrolloSoftware/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXDesarrolloSoftware personaXDesarrolloSoftware = db.PersonaXDesarrolloSoftwares.Find(id);
            if (personaXDesarrolloSoftware == null)
            {
                return HttpNotFound();
            }
            return View(personaXDesarrolloSoftware);
        }

        // GET: PersonaXDesarrolloSoftware/Create
        public ActionResult Create()
        {
            ViewBag.id_desarrollo_software = new SelectList(db.DesarrolloSoftwares, "ID", "nombre");
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula");
            return View();
        }

        // POST: PersonaXDesarrolloSoftware/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_persona,id_desarrollo_software,distribucionAutoria")] PersonaXDesarrolloSoftware personaXDesarrolloSoftware)
        {
            if (ModelState.IsValid)
            {
                db.PersonaXDesarrolloSoftwares.Add(personaXDesarrolloSoftware);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_desarrollo_software = new SelectList(db.DesarrolloSoftwares, "ID", "nombre", personaXDesarrolloSoftware.id_desarrollo_software);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXDesarrolloSoftware.id_persona);
            return View(personaXDesarrolloSoftware);
        }

        // GET: PersonaXDesarrolloSoftware/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXDesarrolloSoftware personaXDesarrolloSoftware = db.PersonaXDesarrolloSoftwares.Find(id);
            if (personaXDesarrolloSoftware == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_desarrollo_software = new SelectList(db.DesarrolloSoftwares, "ID", "nombre", personaXDesarrolloSoftware.id_desarrollo_software);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXDesarrolloSoftware.id_persona);
            return View(personaXDesarrolloSoftware);
        }

        // POST: PersonaXDesarrolloSoftware/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_persona,id_desarrollo_software,distribucionAutoria")] PersonaXDesarrolloSoftware personaXDesarrolloSoftware)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personaXDesarrolloSoftware).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_desarrollo_software = new SelectList(db.DesarrolloSoftwares, "ID", "nombre", personaXDesarrolloSoftware.id_desarrollo_software);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXDesarrolloSoftware.id_persona);
            return View(personaXDesarrolloSoftware);
        }

        // GET: PersonaXDesarrolloSoftware/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXDesarrolloSoftware personaXDesarrolloSoftware = db.PersonaXDesarrolloSoftwares.Find(id);
            if (personaXDesarrolloSoftware == null)
            {
                return HttpNotFound();
            }
            return View(personaXDesarrolloSoftware);
        }

        // POST: PersonaXDesarrolloSoftware/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonaXDesarrolloSoftware personaXDesarrolloSoftware = db.PersonaXDesarrolloSoftwares.Find(id);
            db.PersonaXDesarrolloSoftwares.Remove(personaXDesarrolloSoftware);
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
