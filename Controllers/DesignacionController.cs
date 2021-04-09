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

namespace ExpDigital.Content
{
    public class DesignacionController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: Designacion
        public ActionResult Index()
        {
            return View(db.Designacions.ToList());
        }

        // GET: Designacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designacion designacion = db.Designacions.Find(id);
            if (designacion == null)
            {
                return HttpNotFound();
            }
            return View(designacion);
        }

        // GET: Designacion/Create
        public ActionResult CreateDesignacion()
        {
            return View();
        }

        // POST: Designacion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDesignacion([Bind(Include = "ID,nombre")] Designacion designacion, HttpPostedFileBase upload)
        {
            try
            {
                db.Designacions.Add(designacion);
                db.SaveChanges();

                PersonaXDesignacion personaX = new PersonaXDesignacion();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_designacion = designacion.ID;
                db.PersonaXDesignacions.Add(personaX);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertLibroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, designacion.ID);
                    //Path.GetFullPath(upload.FileName);
                }

                //return RedirectToAction("Index");

                ViewBag.designacionAgregado = designacion.nombre;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorDesignacion = "Error : No se pudo agregar " + e;
                return View();
            }
        }
        public static void InsertLibroDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_designacion)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoDesignacion](FileID,FileName,id_designacion)
	VALUES (@FileID,@FileName,@id_designacion);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoDesignacion
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
                        cmd.Parameters.Add("@id_designacion", SqlDbType.Int).Value = id_designacion;
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

        // GET: Designacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designacion designacion = db.Designacions.Find(id);
            if (designacion == null)
            {
                return HttpNotFound();
            }
            return View(designacion);
        }

        // POST: Designacion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] Designacion designacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(designacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(designacion);
        }

        // GET: Designacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designacion designacion = db.Designacions.Find(id);
            if (designacion == null)
            {
                return HttpNotFound();
            }
            return View(designacion);
        }

        // POST: Designacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Designacion designacion = db.Designacions.Find(id);
            db.Designacions.Remove(designacion);
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
