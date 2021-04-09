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
    public class ColaboracionEventoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ColaboracionEvento
        public ActionResult Index()
        {
            return View(db.ColaboracionEventoes.ToList());
        }

        // GET: ColaboracionEvento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ColaboracionEvento colaboracionEvento = db.ColaboracionEventoes.Find(id);
            if (colaboracionEvento == null)
            {
                return HttpNotFound();
            }
            return View(colaboracionEvento);
        }

        // GET: ColaboracionEvento/Create
        public ActionResult CreateColaboracionEvento()
        {
            return View();
        }

        // POST: ColaboracionEvento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        public static void InsertColabEDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_colaboracion)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoColaboracionEvento](FileID,FileName,tipo,id_colaboracion)
	VALUES (@FileID,@FileName,@tipo,@id_colaboracion);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoColaboracionEvento
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
                        cmd.Parameters.Add("@tipo", SqlDbType.Int).Value = tipo;
                        cmd.Parameters.Add("@id_colaboracion", SqlDbType.Int).Value = id_colaboracion;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateColaboracionEvento([Bind(Include = "ID,nombreEvento,nombreEntidad")] ColaboracionEvento colaboracionEvento, HttpPostedFileBase upload, HttpPostedFileBase uploadCert)
        {
            try
            {
                db.ColaboracionEventoes.Add(colaboracionEvento);
                db.SaveChanges();

                PersonaXColaboracionEvento personaX = new PersonaXColaboracionEvento();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_colaboracion = colaboracionEvento.ID;
                db.PersonaXColaboracionEventoes.Add(personaX);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertColabEDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, colaboracionEvento.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                if (uploadCert != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), uploadCert.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), uploadCert.FileName);
                    InsertColabEDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(uploadCert.FileName), fullPath, 1, colaboracionEvento.ID);
                    //Path.GetFullPath(upload.FileName);
                }

                ViewBag.obraAgregado = colaboracionEvento.nombreEvento;
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

        // GET: ColaboracionEvento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ColaboracionEvento colaboracionEvento = db.ColaboracionEventoes.Find(id);
            if (colaboracionEvento == null)
            {
                return HttpNotFound();
            }
            return View(colaboracionEvento);
        }

        // POST: ColaboracionEvento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombreEvento,nombreEntidad")] ColaboracionEvento colaboracionEvento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(colaboracionEvento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(colaboracionEvento);
        }

        // GET: ColaboracionEvento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ColaboracionEvento colaboracionEvento = db.ColaboracionEventoes.Find(id);
            if (colaboracionEvento == null)
            {
                return HttpNotFound();
            }
            return View(colaboracionEvento);
        }

        // POST: ColaboracionEvento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ColaboracionEvento colaboracionEvento = db.ColaboracionEventoes.Find(id);
            db.ColaboracionEventoes.Remove(colaboracionEvento);
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
