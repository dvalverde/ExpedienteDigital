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
    public class OrganizacionEventoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: OrganizacionEvento
        public ActionResult Index()
        {
            return View(db.OrganizacionEventoes.ToList());
        }

        // GET: OrganizacionEvento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizacionEvento organizacionEvento = db.OrganizacionEventoes.Find(id);
            if (organizacionEvento == null)
            {
                return HttpNotFound();
            }
            return View(organizacionEvento);
        }

        // GET: OrganizacionEvento/Create
        public ActionResult CreateOrganizacionEvento()
        {
            return View();
        }

        // POST: OrganizacionEvento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrganizacionEvento([Bind(Include = "ID,nombre,anno")] OrganizacionEvento organizacionEvento, HttpPostedFileBase upload)
        {
            try
            {
                db.OrganizacionEventoes.Add(organizacionEvento);
                db.SaveChanges();

                PersonaXOrganizacionEvento personaX = new PersonaXOrganizacionEvento();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_evento = organizacionEvento.ID;
                db.PersonaXOrganizacionEventoes.Add(personaX);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertLibroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, organizacionEvento.ID);
                    //Path.GetFullPath(upload.FileName);
                }

                //return RedirectToAction("Index");

                ViewBag.organizacionEventoAgregado = organizacionEvento.nombre;
                return View();
            }

            catch (Exception e)
            {
                ViewBag.errorOrganizacionEvento = "Error : No se pudo agregar " + e;
                return View();
            }
        }

        public static void InsertLibroDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_evento)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoOrganizacionEvento](FileID,FileName,id_evento)
	VALUES (@FileID,@FileName,@id_evento);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoOrganizacionEvento
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
                        cmd.Parameters.Add("@id_evento", SqlDbType.Int).Value = id_evento;
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

        // GET: OrganizacionEvento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizacionEvento organizacionEvento = db.OrganizacionEventoes.Find(id);
            if (organizacionEvento == null)
            {
                return HttpNotFound();
            }
            return View(organizacionEvento);
        }

        // POST: OrganizacionEvento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,anno")] OrganizacionEvento organizacionEvento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organizacionEvento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organizacionEvento);
        }

        // GET: OrganizacionEvento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizacionEvento organizacionEvento = db.OrganizacionEventoes.Find(id);
            if (organizacionEvento == null)
            {
                return HttpNotFound();
            }
            return View(organizacionEvento);
        }

        // POST: OrganizacionEvento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrganizacionEvento organizacionEvento = db.OrganizacionEventoes.Find(id);
            db.OrganizacionEventoes.Remove(organizacionEvento);
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
