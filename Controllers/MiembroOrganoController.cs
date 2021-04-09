using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpDigital.Models;
using ExpDigital.ViewModels;
using System.IO;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Transactions;
using System.Data.Entity.Validation;

namespace ExpDigital.Controllers
{
    public class MiembroOrganoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: MiembroOrgano
        public ActionResult Index()
        {
            return View(db.MiembroOrganos.ToList());
        }

        // GET: MiembroOrgano/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MiembroOrgano miembroOrgano = db.MiembroOrganos.Find(id);
            if (miembroOrgano == null)
            {
                return HttpNotFound();
            }
            return View(miembroOrgano);
        }

        // GET: MiembroOrgano/Create
        public ActionResult CreateMiembroOrgano()
        {
            return View();
        }

        // POST: MiembroOrgano/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        public static void InsertMiembroDoc(System.Guid FileID, string FileName, string filecol, int id_miembro)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoMiembroOrganos](FileID,FileName,id_miembro)
	VALUES (@FileID,@FileName,@id_miembro);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoMiembroOrganos
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
                        cmd.Parameters.Add("@id_miembro", SqlDbType.Int).Value = id_miembro;
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

        protected void Page_Load(int ID)
        {
            string ConnStr = System.Configuration.ConfigurationManager.
        ConnectionStrings["ExpedienteDigitalEntities"].ConnectionString;
            int indice = ConnStr.IndexOf("data source");
            ConnStr = ConnStr.Substring(indice, ConnStr.Length - 2 - indice);
            const string SelectTSql = @"
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
         FROM DocumentoMiembroOrganos d
         INNER JOIN MiembroOrganos t ON d.id_miembro = t.ID
         WHERE t.ID = @ID ";
            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string serverPath;
                    byte[] serverTxn;
                    using (SqlCommand cmd = new SqlCommand(SelectTSql, conn))
                    {
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            serverPath = rdr.GetSqlString(0).Value;
                            serverTxn = rdr.GetSqlBinary(1).Value;
                            rdr.Close();
                        }
                    }
                    this.StreamPhotoImage(serverPath, serverTxn);
                }
                ts.Complete();
            }
        }

        private void StreamPhotoImage(string serverPath, byte[] serverTxn)
        {
            const int BlockSize = 1024 * 512;
            const string PDFContentType = "application/pdf";
            using (SqlFileStream sfs =
              new SqlFileStream(serverPath, serverTxn, FileAccess.Read))
            {
                byte[] buffer = new byte[BlockSize];
                int bytesRead;
                Response.BufferOutput = false;
                Response.ContentType = PDFContentType;
                while ((bytesRead = sfs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Response.OutputStream.Write(buffer, 0, bytesRead);
                    Response.Flush();
                }
                sfs.Close();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMiembroOrgano([Bind(Include = "ID,nombre")] MiembroOrgano miembroOrgano, HttpPostedFileBase upload)
        {
            try
            {
                db.MiembroOrganos.Add(miembroOrgano);
                db.SaveChanges();

                PersonaXMiembroOrgano personaX = new PersonaXMiembroOrgano();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_miembro = miembroOrgano.ID;
                db.PersonaXMiembroOrganos.Add(personaX);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertMiembroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, miembroOrgano.ID);
                    Page_Load(miembroOrgano.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                ViewBag.obraAgregado = miembroOrgano.nombre;
                return View();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ViewBag.errorObra = "Error: " + dbEx;
                        Trace.TraceInformation("Property: {0} Error: {1}",
                        validationError.PropertyName,
                        validationError.ErrorMessage);
                    }
                }
                return View();
            }
        }

        // GET: MiembroOrgano/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MiembroOrgano miembroOrgano = db.MiembroOrganos.Find(id);
            if (miembroOrgano == null)
            {
                return HttpNotFound();
            }
            return View(miembroOrgano);
        }

        // POST: MiembroOrgano/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] MiembroOrgano miembroOrgano)
        {
            if (ModelState.IsValid)
            {
                db.Entry(miembroOrgano).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(miembroOrgano);
        }

        // GET: MiembroOrgano/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MiembroOrgano miembroOrgano = db.MiembroOrganos.Find(id);
            if (miembroOrgano == null)
            {
                return HttpNotFound();
            }
            return View(miembroOrgano);
        }

        // POST: MiembroOrgano/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MiembroOrgano miembroOrgano = db.MiembroOrganos.Find(id);
            db.MiembroOrganos.Remove(miembroOrgano);
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