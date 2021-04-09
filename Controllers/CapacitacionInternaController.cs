using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpDigital.Models;
using System.IO;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using ExpDigital.ViewModels;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Drawing;
using System.Transactions;

namespace ExpDigital.Controllers
{
    public class CapacitacionInternaController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: CapacitacionInterna
        public ActionResult Index()
        {
            return View(db.CapacitacionInternas.ToList());
        }

        // GET: CapacitacionInterna/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CapacitacionInterna capacitacionInterna = db.CapacitacionInternas.Find(id);
            if (capacitacionInterna == null)
            {
                return HttpNotFound();
            }
            return View(capacitacionInterna);
        }

        // GET: CapacitacionInterna/Create
        public ActionResult CreateCapacitacion()
        {
            return View();
        }

        // POST: CapacitacionInterna/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCapacitacion([Bind(Include = "ID,nombre")] CapacitacionInterna capacitacionInterna, HttpPostedFileBase upload)
        {
            try
            {
                CapacitacionInterna capacitacion = new CapacitacionInterna();
                capacitacion.nombre = capacitacionInterna.nombre;
                db.CapacitacionInternas.Add(capacitacion);
                db.SaveChanges();

                PersonaXCapacitacionInterna personaCapacitacion = new PersonaXCapacitacionInterna();
                personaCapacitacion.id_persona = Int32.Parse(Session["ID"].ToString());
                personaCapacitacion.id_capacitacion = capacitacion.ID;
                db.PersonaXCapacitacionInternas.Add(personaCapacitacion);
                db.SaveChanges();


                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertCapaDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, capacitacion.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                ViewBag.capaAgrega = capacitacion.nombre;
                return View();
            }
            catch (Exception e) {
                ViewBag.errorcapa = "Error : No se pudo agregar la capacitacion Interna";
                return View();
            }

        }
        public static void InsertCapaDoc(System.Guid FileID, string FileName, string filecol, int id_capacitacion)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoCapacitacionInterna](FileID,FileName,id_capacitacion)
	VALUES (@FileID,@FileName,@id_capacitacion);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoCapacitacionInterna
          WHERE FileID = @FileID";

            string serverPath;
            byte[] serverTxn;
            string ConnStr = System.Configuration.ConfigurationManager.
        ConnectionStrings["ExpedienteDigitalEntities"].ConnectionString;
            int indice = ConnStr.IndexOf("data source");
            ConnStr = ConnStr.Substring(indice, ConnStr.Length - 2 - indice);

            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(InsertTSql, conn))
                    {
                        cmd.Parameters.Add("@FileID", SqlDbType.UniqueIdentifier).Value = FileID;
                        cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = FileName;
                        cmd.Parameters.Add("@id_capacitacion", SqlDbType.Int).Value = id_capacitacion;
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            serverPath = rdr.GetSqlString(0).Value;
                            serverTxn = rdr.GetSqlBinary(1).Value;
                            rdr.Close();
                        }
                    }
                    SavePhotoFile(filecol, serverPath, serverTxn);
                }
                ts.Complete();
            }
        }

        private static void SavePhotoFile //QUEDA DE FABRICA 
          (string clientPath, string serverPath, byte[] serverTxn)
        {
            const int BlockSize = 1024 * 512;

            using (FileStream source =
              new FileStream(clientPath, FileMode.Open, FileAccess.Read))
            {
                using (SqlFileStream dest =
                  new SqlFileStream(serverPath, serverTxn, FileAccess.Write))
                {
                    byte[] buffer = new byte[BlockSize];
                    int bytesRead;
                    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        dest.Write(buffer, 0, bytesRead);
                        dest.Flush();
                    }
                    dest.Close();
                }
                source.Close();
            }
        }

        // GET: CapacitacionInterna/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CapacitacionInterna capacitacionInterna = db.CapacitacionInternas.Find(id);
            if (capacitacionInterna == null)
            {
                return HttpNotFound();
            }
            return View(capacitacionInterna);
        }

        // POST: CapacitacionInterna/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] CapacitacionInterna capacitacionInterna)
        {
            if (ModelState.IsValid)
            {
                db.Entry(capacitacionInterna).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(capacitacionInterna);
        }

        // GET: CapacitacionInterna/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CapacitacionInterna capacitacionInterna = db.CapacitacionInternas.Find(id);
            if (capacitacionInterna == null)
            {
                return HttpNotFound();
            }
            return View(capacitacionInterna);
        }

        // POST: CapacitacionInterna/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CapacitacionInterna capacitacionInterna = db.CapacitacionInternas.Find(id);
            db.CapacitacionInternas.Remove(capacitacionInterna);
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
