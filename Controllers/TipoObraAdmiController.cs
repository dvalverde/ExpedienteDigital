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
    public class TipoObraAdmiController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: TipoObraAdmi
        public ActionResult Index()
        {
            return View(db.TipoObraAdmis.ToList());
        }

        // GET: TipoObraAdmi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoObraAdmi tipoObraAdmi = db.TipoObraAdmis.Find(id);
            if (tipoObraAdmi == null)
            {
                return HttpNotFound();
            }
            return View(tipoObraAdmi);
        }

        // GET: TipoObraAdmi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoObraAdmi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,nombre")] TipoObraAdmi tipoObraAdmi)
        {
            if (ModelState.IsValid)
            {
                db.TipoObraAdmis.Add(tipoObraAdmi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoObraAdmi);
        }

        // GET: TipoObraAdmi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoObraAdmi tipoObraAdmi = db.TipoObraAdmis.Find(id);
            if (tipoObraAdmi == null)
            {
                return HttpNotFound();
            }
            return View(tipoObraAdmi);
        }

        // POST: TipoObraAdmi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] TipoObraAdmi tipoObraAdmi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoObraAdmi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoObraAdmi);
        }

        // GET: TipoObraAdmi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoObraAdmi tipoObraAdmi = db.TipoObraAdmis.Find(id);
            if (tipoObraAdmi == null)
            {
                return HttpNotFound();
            }
            return View(tipoObraAdmi);
        }

        // POST: TipoObraAdmi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoObraAdmi tipoObraAdmi = db.TipoObraAdmis.Find(id);
            db.TipoObraAdmis.Remove(tipoObraAdmi);
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
