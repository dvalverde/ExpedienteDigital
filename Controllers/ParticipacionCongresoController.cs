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
    public class ParticipacionCongresoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ParticipacionCongreso
        public ActionResult Index()
        {
            return View(db.ParticipacionCongresoes.ToList());
        }

        // GET: ParticipacionCongreso/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipacionCongreso participacionCongreso = db.ParticipacionCongresoes.Find(id);
            if (participacionCongreso == null)
            {
                return HttpNotFound();
            }
            return View(participacionCongreso);
        }

        // GET: ParticipacionCongreso/Create
        public ActionResult CreateParticipacionCongreso()
        {
            return View();
        }

        // POST: ParticipacionCongreso/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateParticipacionCongreso([Bind(Include = "ID,nombre")] ParticipacionCongreso participacionCongreso, HttpPostedFileBase upload)
        {
            try
            {
                db.ParticipacionCongresoes.Add(participacionCongreso);
                db.SaveChanges();

                PersonaXParticipacionCongreso personaCongreso = new PersonaXParticipacionCongreso();
                personaCongreso.id_persona = Int32.Parse(Session["ID"].ToString());
                personaCongreso.id_participacion = participacionCongreso.ID;
                db.PersonaXParticipacionCongresoes.Add(personaCongreso);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertParticipacionCongresoaDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, participacionCongreso.ID);
                }

                ViewBag.congresoAgregado = participacionCongreso.nombre;
                return View();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ViewBag.errorCongreso = "Error: " + dbEx;
                        Trace.TraceInformation("Property: {0} Error: {1}",
                        validationError.PropertyName,
                        validationError.ErrorMessage);
                    }
                }
                return View();
            }
        }

        public static void InsertParticipacionCongresoaDoc(System.Guid FileID, string FileName, string filecol, int id_participacion)
        {
            const string InsertTSql = @"
            INSERT INTO [DocumentoParticipacionCongreso](FileID,FileName,id_participacion)
	        VALUES (@FileID,@FileName,@id_participacion);
            SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
            FROM DocumentoParticipacionCongreso
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
                        cmd.Parameters.Add("@id_participacion", SqlDbType.Int).Value = id_participacion;
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


        // GET: ParticipacionCongreso/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipacionCongreso participacionCongreso = db.ParticipacionCongresoes.Find(id);
            if (participacionCongreso == null)
            {
                return HttpNotFound();
            }
            return View(participacionCongreso);
        }

        // POST: ParticipacionCongreso/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] ParticipacionCongreso participacionCongreso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(participacionCongreso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(participacionCongreso);
        }

        // GET: ParticipacionCongreso/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipacionCongreso participacionCongreso = db.ParticipacionCongresoes.Find(id);
            if (participacionCongreso == null)
            {
                return HttpNotFound();
            }
            return View(participacionCongreso);
        }

        // POST: ParticipacionCongreso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParticipacionCongreso participacionCongreso = db.ParticipacionCongresoes.Find(id);
            db.ParticipacionCongresoes.Remove(participacionCongreso);
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
