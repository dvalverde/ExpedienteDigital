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
    public class ConocimientoSoftwareController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ConocimientoSoftware
        public ActionResult Index()
        {
            return View(db.ConocimientoSoftwares.ToList());
        }

        // GET: ConocimientoSoftware/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConocimientoSoftware conocimientoSoftware = db.ConocimientoSoftwares.Find(id);
            if (conocimientoSoftware == null)
            {
                return HttpNotFound();
            }
            return View(conocimientoSoftware);
        }

        // GET: ConocimientoSoftware/Create
        public ActionResult CreateConocimientoSoftware()
        {
            return View();
        }

        // POST: ConocimientoSoftware/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConocimientoSoftware([Bind(Include = "ID,nombre, tipoSoftware")] ConocimientoSoftware conocimientoSoftware, HttpPostedFileBase upload)
        {
            try
            {
                db.ConocimientoSoftwares.Add(conocimientoSoftware);
                db.SaveChanges();

                PersonaXConocimientoSoftware personaSoftware = new PersonaXConocimientoSoftware();
                personaSoftware.id_persona = Int32.Parse(Session["ID"].ToString());
                personaSoftware.id_software = conocimientoSoftware.ID;
                db.PersonaXConocimientoSoftwares.Add(personaSoftware);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertObraDidacticaDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, conocimientoSoftware.ID);
                }

                ViewBag.conocimientoAgregado = conocimientoSoftware.nombre;
                return View();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ViewBag.errorConocimiento = "Error: " + dbEx;
                        Trace.TraceInformation("Property: {0} Error: {1}",
                        validationError.PropertyName,
                        validationError.ErrorMessage);
                    }
                }
                return View();
            }
        }

        public static void InsertObraDidacticaDoc(System.Guid FileID, string FileName, string filecol, int id_software)
        {
            const string InsertTSql = @"
            INSERT INTO [DocumentoConocimientoSoftware](FileID,FileName,id_software)
	        VALUES (@FileID,@FileName,@id_software);
            SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
            FROM DocumentoConocimientoSoftware
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
                        cmd.Parameters.Add("@id_software", SqlDbType.Int).Value = id_software;
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


        // GET: ConocimientoSoftware/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConocimientoSoftware conocimientoSoftware = db.ConocimientoSoftwares.Find(id);
            if (conocimientoSoftware == null)
            {
                return HttpNotFound();
            }
            return View(conocimientoSoftware);
        }

        // POST: ConocimientoSoftware/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] ConocimientoSoftware conocimientoSoftware)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conocimientoSoftware).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(conocimientoSoftware);
        }

        // GET: ConocimientoSoftware/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConocimientoSoftware conocimientoSoftware = db.ConocimientoSoftwares.Find(id);
            if (conocimientoSoftware == null)
            {
                return HttpNotFound();
            }
            return View(conocimientoSoftware);
        }

        // POST: ConocimientoSoftware/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConocimientoSoftware conocimientoSoftware = db.ConocimientoSoftwares.Find(id);
            db.ConocimientoSoftwares.Remove(conocimientoSoftware);
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
