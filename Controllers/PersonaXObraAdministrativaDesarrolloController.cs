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
    public class PersonaXObraAdministrativaDesarrolloController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: PersonaXObraAdministrativaDesarrollo
        public ActionResult Index()
        {
            var personaXObraAdministrativaDesarrolloes = db.PersonaXObraAdministrativaDesarrolloes.Include(p => p.ObraAdministrativaDesarrollo).Include(p => p.Persona);
            return View(personaXObraAdministrativaDesarrolloes.ToList());
        }

        // GET: PersonaXObraAdministrativaDesarrollo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXObraAdministrativaDesarrollo personaXObraAdministrativaDesarrollo = db.PersonaXObraAdministrativaDesarrolloes.Find(id);
            if (personaXObraAdministrativaDesarrollo == null)
            {
                return HttpNotFound();
            }
            return View(personaXObraAdministrativaDesarrollo);
        }

        // GET: PersonaXObraAdministrativaDesarrollo/Create
        public ActionResult Create()
        {
            ViewBag.id_obra_administrativa = new SelectList(db.ObraAdministrativaDesarrolloes, "ID", "titulo");
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula");
            return View();
        }

        // POST: PersonaXObraAdministrativaDesarrollo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_persona,id_obra_administrativa,distribucionAutoria")] PersonaXObraAdministrativaDesarrollo personaXObraAdministrativaDesarrollo)
        {
            if (ModelState.IsValid)
            {
                db.PersonaXObraAdministrativaDesarrolloes.Add(personaXObraAdministrativaDesarrollo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_obra_administrativa = new SelectList(db.ObraAdministrativaDesarrolloes, "ID", "titulo", personaXObraAdministrativaDesarrollo.id_obra_administrativa);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXObraAdministrativaDesarrollo.id_persona);
            return View(personaXObraAdministrativaDesarrollo);
        }

        // GET: PersonaXObraAdministrativaDesarrollo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXObraAdministrativaDesarrollo personaXObraAdministrativaDesarrollo = db.PersonaXObraAdministrativaDesarrolloes.Find(id);
            if (personaXObraAdministrativaDesarrollo == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_obra_administrativa = new SelectList(db.ObraAdministrativaDesarrolloes, "ID", "titulo", personaXObraAdministrativaDesarrollo.id_obra_administrativa);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXObraAdministrativaDesarrollo.id_persona);
            return View(personaXObraAdministrativaDesarrollo);
        }

        // POST: PersonaXObraAdministrativaDesarrollo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_persona,id_obra_administrativa,distribucionAutoria")] PersonaXObraAdministrativaDesarrollo personaXObraAdministrativaDesarrollo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personaXObraAdministrativaDesarrollo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_obra_administrativa = new SelectList(db.ObraAdministrativaDesarrolloes, "ID", "titulo", personaXObraAdministrativaDesarrollo.id_obra_administrativa);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXObraAdministrativaDesarrollo.id_persona);
            return View(personaXObraAdministrativaDesarrollo);
        }

        // GET: PersonaXObraAdministrativaDesarrollo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXObraAdministrativaDesarrollo personaXObraAdministrativaDesarrollo = db.PersonaXObraAdministrativaDesarrolloes.Find(id);
            if (personaXObraAdministrativaDesarrollo == null)
            {
                return HttpNotFound();
            }
            return View(personaXObraAdministrativaDesarrollo);
        }

        // POST: PersonaXObraAdministrativaDesarrollo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonaXObraAdministrativaDesarrollo personaXObraAdministrativaDesarrollo = db.PersonaXObraAdministrativaDesarrolloes.Find(id);
            db.PersonaXObraAdministrativaDesarrolloes.Remove(personaXObraAdministrativaDesarrollo);
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
