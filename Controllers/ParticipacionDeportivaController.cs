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
    public class ParticipacionDeportivaController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ParticipacionDeportiva
        public ActionResult Index()
        {
            return View(db.ParticipacionDeportivas.ToList());
        }

        // GET: ParticipacionDeportiva/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipacionDeportiva participacionDeportiva = db.ParticipacionDeportivas.Find(id);
            if (participacionDeportiva == null)
            {
                return HttpNotFound();
            }
            return View(participacionDeportiva);
        }

        // GET: ParticipacionDeportiva/Create
        public ActionResult CreateParticipacion()
        {
            return View();
        }

        // POST: ParticipacionDeportiva/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateParticipacion([Bind(Include = "ID,nombreEvento,nombreLugar")] ParticipacionDeportiva participacionDeportiva, HttpPostedFileBase upload)
        {
            try
            {

                ParticipacionDeportiva participacion = new ParticipacionDeportiva();
                participacion.nombreEvento = participacionDeportiva.nombreEvento;
                participacion.nombreLugar = participacionDeportiva.nombreLugar;
                
                db.ParticipacionDeportivas.Add(participacion);
                db.SaveChanges();

                PersonaXParticipacionDeportiva personaDeportiva = new PersonaXParticipacionDeportiva();
                personaDeportiva.id_persona = Int32.Parse(Session["ID"].ToString());
                personaDeportiva.id_participacion = participacion.ID;
                db.PersonaXParticipacionDeportivas.Add(personaDeportiva);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertPDeporDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, participacion.ID);
                    //Path.GetFllPath(upload.FileName);
                }
                ViewBag.parDeporAgre = participacion.nombreEvento;
                return View();
            }
            catch (Exception e) {
                ViewBag.errorPD = "Error : No se pudo agregar la participacion";
                return View();
            }

            
        }

        public static void InsertPDeporDoc(System.Guid FileID, string FileName, string filecol, int id_participacion)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoParticipacionDeportiva](FileID,FileName,id_participacion)
	VALUES (@FileID,@FileName,@id_participacion);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoParticipacionDeportiva
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

        // GET: ParticipacionDeportiva/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipacionDeportiva participacionDeportiva = db.ParticipacionDeportivas.Find(id);
            if (participacionDeportiva == null)
            {
                return HttpNotFound();
            }
            return View(participacionDeportiva);
        }

        // POST: ParticipacionDeportiva/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombreEvento,nombreLugar")] ParticipacionDeportiva participacionDeportiva)
        {
            if (ModelState.IsValid)
            {
                db.Entry(participacionDeportiva).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(participacionDeportiva);
        }

        // GET: ParticipacionDeportiva/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipacionDeportiva participacionDeportiva = db.ParticipacionDeportivas.Find(id);
            if (participacionDeportiva == null)
            {
                return HttpNotFound();
            }
            return View(participacionDeportiva);
        }

        // POST: ParticipacionDeportiva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParticipacionDeportiva participacionDeportiva = db.ParticipacionDeportivas.Find(id);
            db.ParticipacionDeportivas.Remove(participacionDeportiva);
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
