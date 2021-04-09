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
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Drawing;
using System.Transactions;

namespace ExpDigital.Controllers
{
    public class PremioNacionalController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: PremioNacional
        public ActionResult Index()
        {
            return View(db.PremioNacionals.ToList());
        }

        // GET: PremioNacional/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PremioNacional premioNacional = db.PremioNacionals.Find(id);
            if (premioNacional == null)
            {
                return HttpNotFound();
            }
            return View(premioNacional);
        }

        // GET: PremioNacional/Create
        public ActionResult CreatePremioNacional()
        {
            return View();
        }

        // POST: PremioNacional/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePremioNacional([Bind(Include = "ID,nombre,anno")] PremioNacional premioNacional, HttpPostedFileBase upload)
        {

            try
            {
                PremioNacional premio = new PremioNacional();
                premio.nombre = premioNacional.nombre;
                premio.anno = premioNacional.anno;
                db.PremioNacionals.Add(premio);
                db.SaveChanges();

                PersonaXPremioNacional personaPremio = new PersonaXPremioNacional();
                personaPremio.id_persona = Int32.Parse(Session["ID"].ToString());
                personaPremio.id_premio = premio.ID;
                db.PersonaXPremioNacionals.Add(personaPremio);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertPremioDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, premio.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                ViewBag.prAgregado = premioNacional.nombre;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorpr = "Error : No se pudo agregar el premio ";
                return View();
            }


        }

        //PARA GUARDAR DOCUMENTOS PESADOS 
        public static void InsertPremioDoc(System.Guid FileID, string FileName, string filecol, int id_premio)
        {
            const string InsertTSql = @"
            INSERT INTO [DocumentoPremioNacional](FileID,FileName,id_premio)
	        VALUES (@FileID,@FileName,@id_premio);
            SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
            FROM DocumentoPremioNacional
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
                        cmd.Parameters.Add("@id_premio", SqlDbType.Int).Value = id_premio;
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


        // GET: PremioNacional/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PremioNacional premioNacional = db.PremioNacionals.Find(id);
            if (premioNacional == null)
            {
                return HttpNotFound();
            }
            return View(premioNacional);
        }

        // POST: PremioNacional/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,anno")] PremioNacional premioNacional)
        {
            if (ModelState.IsValid)
            {
                db.Entry(premioNacional).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(premioNacional);
        }

        // GET: PremioNacional/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PremioNacional premioNacional = db.PremioNacionals.Find(id);
            if (premioNacional == null)
            {
                return HttpNotFound();
            }
            return View(premioNacional);
        }

        // POST: PremioNacional/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PremioNacional premioNacional = db.PremioNacionals.Find(id);
            db.PremioNacionals.Remove(premioNacional);
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
