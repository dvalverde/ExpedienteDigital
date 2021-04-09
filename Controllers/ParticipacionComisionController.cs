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
    public class ParticipacionComisionController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ParticipacionComision
        public ActionResult Index()
        {
            return View(db.ParticipacionComisions.ToList());
        }

        // GET: ParticipacionComision/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipacionComision participacionComision = db.ParticipacionComisions.Find(id);
            if (participacionComision == null)
            {
                return HttpNotFound();
            }
            return View(participacionComision);
        }

        // GET: ParticipacionComision/Create
        public ActionResult CreateParticipacion()
        {
            return View();
        }

        // POST: ParticipacionComision/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateParticipacion([Bind(Include = "ID,nombre")] ParticipacionComision participacionComision, HttpPostedFileBase upload)
        {
            try
            {
                ParticipacionComision participacion = new ParticipacionComision();
                participacion.nombre = participacionComision.nombre;
                db.ParticipacionComisions.Add(participacion);
                db.SaveChanges();

                PersonaXParticipacionComision personaComision = new PersonaXParticipacionComision();
                personaComision.id_persona = Int32.Parse(Session["ID"].ToString());
                personaComision.id_participacion = participacion.ID;
                db.PersonaXParticipacionComisions.Add(personaComision);
                db.SaveChanges();


                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertComiDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, participacion.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                ViewBag.comiAgrega = participacion.nombre;

                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorcomi = "Error : No se pudo agregar el modelo";
                return View();
            }
        }
        public static void InsertComiDoc(System.Guid FileID, string FileName, string filecol, int id_participacion)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoParticipacionComision](FileID,FileName,id_participacion)
	VALUES (@FileID,@FileName,@id_participacion);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoParticipacionComision
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

        // GET: ParticipacionComision/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipacionComision participacionComision = db.ParticipacionComisions.Find(id);
            if (participacionComision == null)
            {
                return HttpNotFound();
            }
            return View(participacionComision);
        }

        // POST: ParticipacionComision/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] ParticipacionComision participacionComision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(participacionComision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(participacionComision);
        }

        // GET: ParticipacionComision/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipacionComision participacionComision = db.ParticipacionComisions.Find(id);
            if (participacionComision == null)
            {
                return HttpNotFound();
            }
            return View(participacionComision);
        }

        // POST: ParticipacionComision/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParticipacionComision participacionComision = db.ParticipacionComisions.Find(id);
            db.ParticipacionComisions.Remove(participacionComision);
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
