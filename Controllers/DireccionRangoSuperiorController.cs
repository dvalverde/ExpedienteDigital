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
    public class DireccionRangoSuperiorController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: DireccionRangoSuperior
        public ActionResult Index()
        {
            return View(db.DireccionRangoSuperiors.ToList());
        }

        // GET: DireccionRangoSuperior/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DireccionRangoSuperior direccionRangoSuperior = db.DireccionRangoSuperiors.Find(id);
            if (direccionRangoSuperior == null)
            {
                return HttpNotFound();
            }
            return View(direccionRangoSuperior);
        }

        // GET: DireccionRangoSuperior/Create
        public ActionResult CreateRango()
        {
            return View();
        }

        // POST: DireccionRangoSuperior/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRango([Bind(Include = "ID,nombre")] DireccionRangoSuperior direccionRangoSuperior, HttpPostedFileBase upload)
        {
            try
            {
                DireccionRangoSuperior direccion = new DireccionRangoSuperior();
                direccion.nombre = direccionRangoSuperior.nombre;
                db.DireccionRangoSuperiors.Add(direccion);
                db.SaveChanges();

                PersonaXDireccionRangoSuperior personaDireccion = new PersonaXDireccionRangoSuperior();
                personaDireccion.id_persona = Int32.Parse(Session["ID"].ToString());
                personaDireccion.id_direccion = direccion.ID;
                db.PersonaXDireccionRangoSuperiors.Add(personaDireccion);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertDireDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, direccion.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                ViewBag.rangoAgrega = direccion.nombre;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorrango = "Error : No se pudo agregar el Rango Superior ";
                return View();
            }

        }

        public static void InsertDireDoc(System.Guid FileID, string FileName, string filecol, int id_direccion)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoDireccionRangoSuperior](FileID,FileName,id_direccion)
	VALUES (@FileID,@FileName,@id_direccion);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoDireccionRangoSuperior
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
                        cmd.Parameters.Add("@id_direccion", SqlDbType.Int).Value = id_direccion;
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


        // GET: DireccionRangoSuperior/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DireccionRangoSuperior direccionRangoSuperior = db.DireccionRangoSuperiors.Find(id);
            if (direccionRangoSuperior == null)
            {
                return HttpNotFound();
            }
            return View(direccionRangoSuperior);
        }

        // POST: DireccionRangoSuperior/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] DireccionRangoSuperior direccionRangoSuperior)
        {
            if (ModelState.IsValid)
            {
                db.Entry(direccionRangoSuperior).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(direccionRangoSuperior);
        }

        // GET: DireccionRangoSuperior/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DireccionRangoSuperior direccionRangoSuperior = db.DireccionRangoSuperiors.Find(id);
            if (direccionRangoSuperior == null)
            {
                return HttpNotFound();
            }
            return View(direccionRangoSuperior);
        }

        // POST: DireccionRangoSuperior/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DireccionRangoSuperior direccionRangoSuperior = db.DireccionRangoSuperiors.Find(id);
            db.DireccionRangoSuperiors.Remove(direccionRangoSuperior);
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
