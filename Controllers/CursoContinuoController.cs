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
    public class CursoContinuoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: CursoContinuo
        public ActionResult Index()
        {
            return View(db.CursoContinuos.ToList());
        }

        // GET: CursoContinuo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CursoContinuo cursoContinuo = db.CursoContinuos.Find(id);
            if (cursoContinuo == null)
            {
                return HttpNotFound();
            }
            return View(cursoContinuo);
        }

        // GET: CursoContinuo/Create
        public ActionResult CreateCursoContinuo()
        {
            return View();
        }

        // POST: CursoContinuo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        public static void InsertCursoDoc(System.Guid FileID, string FileName, string filecol, int id_curso)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoCursoContinuo](FileID,FileName,id_curso)
	VALUES (@FileID,@FileName,@id_curso);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoCursoContinuo
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
                        cmd.Parameters.Add("@id_curso", SqlDbType.Int).Value = id_curso;
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
        public ActionResult CreateCursoContinuo([Bind(Include = "ID,nombre,anno")] CursoContinuo cursoContinuo, HttpPostedFileBase upload)
        {
            try
            {
                db.CursoContinuos.Add(cursoContinuo);
                db.SaveChanges();

                PersonaXCursoContinuo personaX = new PersonaXCursoContinuo();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_curso = cursoContinuo.ID;
                db.PersonaXCursoContinuos.Add(personaX);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertCursoDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, cursoContinuo.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                ViewBag.obraAgregado = cursoContinuo.nombre;
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

        // GET: CursoContinuo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CursoContinuo cursoContinuo = db.CursoContinuos.Find(id);
            if (cursoContinuo == null)
            {
                return HttpNotFound();
            }
            return View(cursoContinuo);
        }

        // POST: CursoContinuo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,anno")] CursoContinuo cursoContinuo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cursoContinuo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cursoContinuo);
        }

        // GET: CursoContinuo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CursoContinuo cursoContinuo = db.CursoContinuos.Find(id);
            if (cursoContinuo == null)
            {
                return HttpNotFound();
            }
            return View(cursoContinuo);
        }

        // POST: CursoContinuo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CursoContinuo cursoContinuo = db.CursoContinuos.Find(id);
            db.CursoContinuos.Remove(cursoContinuo);
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
