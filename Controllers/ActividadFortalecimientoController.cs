using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using ExpDigital.Models;

namespace ExpDigital.Controllers
{
    public class ActividadFortalecimientoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ActividadFortalecimiento
        public ActionResult Index()
        {
            return View(db.ActividadFortalecimientoes.ToList());
        }

        // GET: ActividadFortalecimiento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActividadFortalecimiento actividadFortalecimiento = db.ActividadFortalecimientoes.Find(id);
            if (actividadFortalecimiento == null)
            {
                return HttpNotFound();
            }
            return View(actividadFortalecimiento);
        }

        // GET: ActividadFortalecimiento/Create
        public ActionResult CreateActividadFortalecimiento()
        {
            return View();
        }

        // POST: ActividadFortalecimiento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActividadFortalecimiento([Bind(Include = "ID,nombre")] ActividadFortalecimiento actividadFortalecimiento, HttpPostedFileBase upload)
        {
            try
            {
                db.ActividadFortalecimientoes.Add(actividadFortalecimiento);
                db.SaveChanges();

                PersonaXActividadFortalecimiento personaFortalecimiento = new PersonaXActividadFortalecimiento();
                personaFortalecimiento.id_persona = Int32.Parse(Session["ID"].ToString());
                personaFortalecimiento.id_actividad = actividadFortalecimiento.ID;
                db.PersonaXActividadFortalecimientoes.Add(personaFortalecimiento);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertFortalecimientoDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, actividadFortalecimiento.ID);

                }

                ViewBag.fortalecimientoAgregado = actividadFortalecimiento.nombre;
                return View();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ViewBag.errorFortalecimiento = "Error: " + dbEx;
                        Trace.TraceInformation("Property: {0} Error: {1}",
                        validationError.PropertyName,
                        validationError.ErrorMessage);
                    }
                }
                return View();
            }
        }

        public static void InsertFortalecimientoDoc(System.Guid FileID, string FileName, string filecol, int id_actividad)
        {
            const string InsertTSql = @"
            INSERT INTO [DocumentoActividadFortalecimiento](FileID,FileName,id_actividad)
	        VALUES (@FileID,@FileName,@id_actividad);
            SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
            FROM   DocumentoActividadFortalecimiento
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
                        cmd.Parameters.Add("@id_actividad", SqlDbType.Int).Value = id_actividad;
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

        private static void SavePhotoFile
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



        // GET: ActividadFortalecimiento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActividadFortalecimiento actividadFortalecimiento = db.ActividadFortalecimientoes.Find(id);
            if (actividadFortalecimiento == null)
            {
                return HttpNotFound();
            }
            return View(actividadFortalecimiento);
        }

        // POST: ActividadFortalecimiento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] ActividadFortalecimiento actividadFortalecimiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actividadFortalecimiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actividadFortalecimiento);
        }

        // GET: ActividadFortalecimiento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActividadFortalecimiento actividadFortalecimiento = db.ActividadFortalecimientoes.Find(id);
            if (actividadFortalecimiento == null)
            {
                return HttpNotFound();
            }
            return View(actividadFortalecimiento);
        }

        // POST: ActividadFortalecimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActividadFortalecimiento actividadFortalecimiento = db.ActividadFortalecimientoes.Find(id);
            db.ActividadFortalecimientoes.Remove(actividadFortalecimiento);
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
