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
    public class ProyectoInvestigacionsController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ProyectoInvestigacions
        public ActionResult Index()
        {
            return View(db.ProyectoInvestigacions.ToList());
        }

        // GET: ProyectoInvestigacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProyectoInvestigacion proyectoInvestigacion = db.ProyectoInvestigacions.Find(id);
            if (proyectoInvestigacion == null)
            {
                return HttpNotFound();
            }
            return View(proyectoInvestigacion);
        }

        // GET: ProyectoInvestigacions/Create
        public ActionResult CreateProyectoInvestigacion()
        {
            return View();
        }

        // POST: ProyectoInvestigacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProyectoInvestigacion([Bind(Include = "ID,nombre,medioDivulgacion")] ProyectoInvestigacion proyectoInvestigacion, HttpPostedFileBase upload, HttpPostedFileBase upload1)
        {
            try
            {
                db.ProyectoInvestigacions.Add(proyectoInvestigacion);
                db.SaveChanges();

                PersonaXProyectoInvestigacion personaX = new PersonaXProyectoInvestigacion();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_proyecto_investigacion = proyectoInvestigacion.ID;
                db.PersonaXProyectoInvestigacions.Add(personaX);
                db.SaveChanges();
                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertLibroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, proyectoInvestigacion.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                if (upload1 != null)
                {
                    upload1.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload1.FileName));
                    string fullPath1 = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload1.FileName);
                    InsertLibroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload1.FileName), fullPath1, 1, proyectoInvestigacion.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                ViewBag.proyectoInvestigacionAgregado = proyectoInvestigacion.nombre;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorProyectoInvestigacion = "Error : No se pudo agregar " + e;
                return View();
            }
        }

        public static void InsertLibroDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_proyecto_investigacion)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoProyectoInvestigacion](FileID,FileName,tipo,id_proyecto_investigacion)
	VALUES (@FileID,@FileName,@tipo,@id_proyecto_investigacion);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoProyectoInvestigacion
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
                        cmd.Parameters.Add("@tipo", SqlDbType.Int).Value = tipo; //VERIFICAR QUE TENGA TIPO , SI NO SE BORRA 
                        cmd.Parameters.Add("@id_proyecto_investigacion", SqlDbType.Int).Value = id_proyecto_investigacion;
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

        // GET: ProyectoInvestigacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProyectoInvestigacion proyectoInvestigacion = db.ProyectoInvestigacions.Find(id);
            if (proyectoInvestigacion == null)
            {
                return HttpNotFound();
            }
            return View(proyectoInvestigacion);
        }

        // POST: ProyectoInvestigacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,medioDivulgacion")] ProyectoInvestigacion proyectoInvestigacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyectoInvestigacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proyectoInvestigacion);
        }

        // GET: ProyectoInvestigacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProyectoInvestigacion proyectoInvestigacion = db.ProyectoInvestigacions.Find(id);
            if (proyectoInvestigacion == null)
            {
                return HttpNotFound();
            }
            return View(proyectoInvestigacion);
        }

        // POST: ProyectoInvestigacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProyectoInvestigacion proyectoInvestigacion = db.ProyectoInvestigacions.Find(id);
            db.ProyectoInvestigacions.Remove(proyectoInvestigacion);
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
