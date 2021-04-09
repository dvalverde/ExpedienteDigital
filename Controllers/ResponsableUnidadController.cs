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
    public class ResponsableUnidadController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ResponsableUnidad
        public ActionResult Index()
        {
            return View(db.ResponsableUnidads.ToList());
        }

        // GET: ResponsableUnidad/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponsableUnidad responsableUnidad = db.ResponsableUnidads.Find(id);
            if (responsableUnidad == null)
            {
                return HttpNotFound();
            }
            return View(responsableUnidad);
        }

        // GET: ResponsableUnidad/Create
        public ActionResult CreateResponsableUnidad()
        {
            return View();
        }

        // POST: ResponsableUnidad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateResponsableUnidad([Bind(Include = "ID,nombre")] ResponsableUnidad responsableUnidad, HttpPostedFileBase upload)
        {
            try
            {
                db.ResponsableUnidads.Add(responsableUnidad);
                db.SaveChanges();

                PersonaXResponsableUnidad personaX = new PersonaXResponsableUnidad();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_unidad = responsableUnidad.ID;
                db.PersonaXResponsableUnidads.Add(personaX);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertLibroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, responsableUnidad.ID);
                    //Path.GetFullPath(upload.FileName);
                }

                //return RedirectToAction("Index");

                ViewBag.responsableUnidadAgregado = responsableUnidad.nombre;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorResponsableUnidad = "Error : No se pudo agregar " + e;
                return View();
            }


        }

        public static void InsertLibroDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_unidad)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoResponsableUnidad](FileID,FileName,id_unidad)
	VALUES (@FileID,@FileName,@id_unidad);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoResponsableUnidad
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
                        cmd.Parameters.Add("@id_unidad", SqlDbType.Int).Value = id_unidad;
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

        // GET: ResponsableUnidad/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponsableUnidad responsableUnidad = db.ResponsableUnidads.Find(id);
            if (responsableUnidad == null)
            {
                return HttpNotFound();
            }
            return View(responsableUnidad);
        }

        // POST: ResponsableUnidad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] ResponsableUnidad responsableUnidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(responsableUnidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(responsableUnidad);
        }

        // GET: ResponsableUnidad/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponsableUnidad responsableUnidad = db.ResponsableUnidads.Find(id);
            if (responsableUnidad == null)
            {
                return HttpNotFound();
            }
            return View(responsableUnidad);
        }

        // POST: ResponsableUnidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResponsableUnidad responsableUnidad = db.ResponsableUnidads.Find(id);
            db.ResponsableUnidads.Remove(responsableUnidad);
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
