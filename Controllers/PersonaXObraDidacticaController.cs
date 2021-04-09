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
    public class PersonaXObraDidacticaController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: PersonaXObraDidactica
        public ActionResult Index()
        {
            var personaXObraDidacticas = db.PersonaXObraDidacticas.Include(p => p.ObraDidactica).Include(p => p.Persona);
            return View(personaXObraDidacticas.ToList());
        }

        // GET: PersonaXObraDidactica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXObraDidactica personaXObraDidactica = db.PersonaXObraDidacticas.Find(id);
            if (personaXObraDidactica == null)
            {
                return HttpNotFound();
            }
            return View(personaXObraDidactica);
        }

        // GET: PersonaXObraDidactica/Create
        public ActionResult Create()
        {
            ViewBag.id_obra_didactica = new SelectList(db.ObraDidacticas, "ID", "nombre");
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula");
            return View();
        }

        // POST: PersonaXObraDidactica/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_persona,id_obra_didactica,distribucionAutoria")] PersonaXObraDidactica personaXObraDidactica)
        {
            if (ModelState.IsValid)
            {
                db.PersonaXObraDidacticas.Add(personaXObraDidactica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_obra_didactica = new SelectList(db.ObraDidacticas, "ID", "nombre", personaXObraDidactica.id_obra_didactica);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXObraDidactica.id_persona);
            return View(personaXObraDidactica);
        }

        // GET: PersonaXObraDidactica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXObraDidactica personaXObraDidactica = db.PersonaXObraDidacticas.Find(id);
            if (personaXObraDidactica == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_obra_didactica = new SelectList(db.ObraDidacticas, "ID", "nombre", personaXObraDidactica.id_obra_didactica);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXObraDidactica.id_persona);
            return View(personaXObraDidactica);
        }

        // POST: PersonaXObraDidactica/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_persona,id_obra_didactica,distribucionAutoria")] PersonaXObraDidactica personaXObraDidactica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personaXObraDidactica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_obra_didactica = new SelectList(db.ObraDidacticas, "ID", "nombre", personaXObraDidactica.id_obra_didactica);
            ViewBag.id_persona = new SelectList(db.Personas, "ID", "cedula", personaXObraDidactica.id_persona);
            return View(personaXObraDidactica);
        }

        // GET: PersonaXObraDidactica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaXObraDidactica personaXObraDidactica = db.PersonaXObraDidacticas.Find(id);
            if (personaXObraDidactica == null)
            {
                return HttpNotFound();
            }
            return View(personaXObraDidactica);
        }

        // POST: PersonaXObraDidactica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonaXObraDidactica personaXObraDidactica = db.PersonaXObraDidacticas.Find(id);
            db.PersonaXObraDidacticas.Remove(personaXObraDidactica);
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
