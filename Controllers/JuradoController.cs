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
    public class JuradoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: Jurado
        public ActionResult Index()
        {
            return View(db.Juradoes.ToList());
        }

        // GET: Jurado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jurado jurado = db.Juradoes.Find(id);
            if (jurado == null)
            {
                return HttpNotFound();
            }
            return View(jurado);
        }

        // GET: Jurado/Create
        public ActionResult CreateJurado()
        {
            return View();
        }

        // POST: Jurado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJurado([Bind(Include = "ID,nombre")] Jurado jurado, HttpPostedFileBase upload)
        {
            try
            {

                Jurado jura = new Jurado();
                jura.nombre = jurado.nombre;
                db.Juradoes.Add(jura);
                db.SaveChanges();

                PersonaXJurado personaJurado = new PersonaXJurado();
                personaJurado.id_persona = Int32.Parse(Session["ID"].ToString());
                personaJurado.id_jurado = jurado.ID;
                db.PersonaXJuradoes.Add(personaJurado);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertJuraDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, jura.ID);
                    //Path.GetFllPath(upload.FileName);
                }

                ViewBag.jurAgregad = jurado.nombre;
                return View(jurado);
            }
            catch (Exception e) { 
                ViewBag.errorjura = "Error : No se pudo agregar el jurado ";
                return View();
            }
            }

        public static void InsertJuraDoc(System.Guid FileID, string FileName, string filecol,  int id_jurado)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoJurado](FileID,FileName,id_jurado)
	VALUES (@FileID,@FileName,@id_jurado);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoJurado
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
                        cmd.Parameters.Add("@id_jurado", SqlDbType.Int).Value = id_jurado;
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

        // GET: Jurado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jurado jurado = db.Juradoes.Find(id);
            if (jurado == null)
            {
                return HttpNotFound();
            }
            return View(jurado);
        }

        // POST: Jurado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] Jurado jurado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jurado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jurado);
        }

        // GET: Jurado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jurado jurado = db.Juradoes.Find(id);
            if (jurado == null)
            {
                return HttpNotFound();
            }
            return View(jurado);
        }

        // POST: Jurado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Jurado jurado = db.Juradoes.Find(id);
            db.Juradoes.Remove(jurado);
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
