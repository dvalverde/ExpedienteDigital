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
    public class MembresiaConsejoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: MembresiaConsejo
        public ActionResult Index()
        {
            return View(db.MembresiaConsejos.ToList());
        }

        // GET: MembresiaConsejo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembresiaConsejo membresiaConsejo = db.MembresiaConsejos.Find(id);
            if (membresiaConsejo == null)
            {
                return HttpNotFound();
            }
            return View(membresiaConsejo);
        }

        // GET: MembresiaConsejo/Create
        public ActionResult CreateMembresiaConsejo()
        {
            return View();
        }

        // POST: MembresiaConsejo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMembresiaConsejo([Bind(Include = "ID,nombre, tipo")] MembresiaConsejo membresiaConsejo, HttpPostedFileBase upload)
        {
            try
            {

                db.MembresiaConsejos.Add(membresiaConsejo);
                db.SaveChanges();
                //return RedirectToAction("Index");

                PersonaXMembresiaConsejo personaX = new PersonaXMembresiaConsejo();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_membresia = membresiaConsejo.ID;
                db.PersonaXMembresiaConsejos.Add(personaX);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertLibroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, membresiaConsejo.ID);
                    //Path.GetFullPath(upload.FileName);
                }

                //return RedirectToAction("Index");

                ViewBag.membresiaConsejoAgregado = membresiaConsejo.nombre;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorMembresiaConsejo = "Error : No se pudo agregar " + e;
                return View();
            }
        }

        public static void InsertLibroDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_membresia)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoMembresiaConsejos](FileID,FileName,id_membresia)
	VALUES (@FileID,@FileName,@id_membresia);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoMembresiaConsejos
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
                        cmd.Parameters.Add("@id_membresia", SqlDbType.Int).Value = id_membresia;
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

        // GET: MembresiaConsejo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembresiaConsejo membresiaConsejo = db.MembresiaConsejos.Find(id);
            if (membresiaConsejo == null)
            {
                return HttpNotFound();
            }
            return View(membresiaConsejo);
        }

        // POST: MembresiaConsejo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,tipo")] MembresiaConsejo membresiaConsejo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(membresiaConsejo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(membresiaConsejo);
        }

        // GET: MembresiaConsejo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembresiaConsejo membresiaConsejo = db.MembresiaConsejos.Find(id);
            if (membresiaConsejo == null)
            {
                return HttpNotFound();
            }
            return View(membresiaConsejo);
        }

        // POST: MembresiaConsejo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MembresiaConsejo membresiaConsejo = db.MembresiaConsejos.Find(id);
            db.MembresiaConsejos.Remove(membresiaConsejo);
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
