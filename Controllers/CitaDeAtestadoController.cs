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
    public class CitaDeAtestadoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: CitaDeAtestado
        public ActionResult Index()
        {
            return View(db.CitaDeAtestados.ToList());
        }

        // GET: CitaDeAtestado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CitaDeAtestado citaDeAtestado = db.CitaDeAtestados.Find(id);
            if (citaDeAtestado == null)
            {
                return HttpNotFound();
            }
            return View(citaDeAtestado);
        }

        // GET: CitaDeAtestado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CitaDeAtestado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,fechaInicio,fechaFin,contrasenna")] CitaDeAtestado citaDeAtestado)
        {
            if (ModelState.IsValid)
            {
                db.CitaDeAtestados.Add(citaDeAtestado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(citaDeAtestado);
        }

        // GET: CitaDeAtestado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CitaDeAtestado citaDeAtestado = db.CitaDeAtestados.Find(id);
            if (citaDeAtestado == null)
            {
                return HttpNotFound();
            }
            return View(citaDeAtestado);
        }

        // POST: CitaDeAtestado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,fechaInicio,fechaFin,contrasenna")] CitaDeAtestado citaDeAtestado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(citaDeAtestado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(citaDeAtestado);
        }

        // GET: CitaDeAtestado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CitaDeAtestado citaDeAtestado = db.CitaDeAtestados.Find(id);
            if (citaDeAtestado == null)
            {
                return HttpNotFound();
            }
            return View(citaDeAtestado);
        }

        // POST: CitaDeAtestado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CitaDeAtestado citaDeAtestado = db.CitaDeAtestados.Find(id);
            db.CitaDeAtestados.Remove(citaDeAtestado);
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
