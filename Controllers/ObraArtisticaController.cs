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
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Transactions;
using System.Data.Entity.Validation;

namespace ExpDigital.Controllers
{
    public class ObraArtisticaController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ObraArtistica
        public ActionResult Index()
        {
            return View(db.ObraArtisticas.ToList());
        }

        // GET: ObraArtistica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraArtistica obraArtistica = db.ObraArtisticas.Find(id);
            if (obraArtistica == null)
            {
                return HttpNotFound();
            }
            return View(obraArtistica);
        }

        // GET: ObraArtistica/createObraArtistica
        public ActionResult createObraArtistica()
        {
            return View();
        }

        // POST: ObraArtistica/createObraArtistica
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public static void InsertObraArte(System.Guid FileID, string FileName, string filecol, int tipo, int id_obra_artistica)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoObraArtistica](FileID,FileName,tipo,id_obra_artistica)
	VALUES (@FileID,@FileName,@tipo,@id_obra_artistica);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoObraArtistica
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
                        cmd.Parameters.Add("@tipo", SqlDbType.Int).Value = tipo;
                        cmd.Parameters.Add("@id_obra_artistica", SqlDbType.Int).Value = id_obra_artistica;
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
        public ActionResult createObraArtistica([Bind(Include = "ID,nombre,numeroAutores,autores,autorXObraArtis")] ObraArteAutor arteAutor, HttpPostedFileBase upload, HttpPostedFileBase uploadCert)
        {
            try
            {
                ObraArtistica obraArte = new ObraArtistica();
                obraArte.nombre = arteAutor.nombre;
                obraArte.numeroAutores = arteAutor.numeroAutores;
                db.ObraArtisticas.Add(obraArte);
                db.SaveChanges();

                PersonaXObraArtistica personaX = new PersonaXObraArtistica();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_obra_artistica = obraArte.ID;
                db.PersonaXObraArtisticas.Add(personaX);
                db.SaveChanges();

                for (int i = 0; i < arteAutor.numeroAutores; i++)
                {
                    Autor autor = new Autor();
                    autor.nombre = arteAutor.autores.ElementAt(i).nombre;
                    autor.correoElectronico = arteAutor.autores.ElementAt(i).correoElectronico;
                    db.Autors.Add(autor);
                    db.SaveChanges();
                    AutorXObraArtistica autorXobraArte = new AutorXObraArtistica();
                    autorXobraArte.id_obra_artistica = obraArte.ID;
                    autorXobraArte.id_autor = autor.ID;
                    autorXobraArte.distribucionAutoria = Convert.ToDecimal(arteAutor.autorXObraArtis.ElementAt(i).distribucionAutoria);
                    db.AutorXObraArtisticas.Add(autorXobraArte);
                    db.SaveChanges();
                }

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertObraArte(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, obraArte.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                if (uploadCert != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), uploadCert.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), uploadCert.FileName);
                    InsertObraArte(System.Guid.NewGuid(), System.IO.Path.GetFileName(uploadCert.FileName), fullPath, 1, obraArte.ID);
                    //Path.GetFullPath(upload.FileName);
                }

                ViewBag.obraAgregado = obraArte.nombre;
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

        // GET: ObraArtistica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraArtistica obraArtistica = db.ObraArtisticas.Find(id);
            if (obraArtistica == null)
            {
                return HttpNotFound();
            }
            return View(obraArtistica);
        }

        // POST: ObraArtistica/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,numeroAutores")] ObraArtistica obraArtistica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obraArtistica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obraArtistica);
        }

        // GET: ObraArtistica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraArtistica obraArtistica = db.ObraArtisticas.Find(id);
            if (obraArtistica == null)
            {
                return HttpNotFound();
            }
            return View(obraArtistica);
        }

        // POST: ObraArtistica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ObraArtistica obraArtistica = db.ObraArtisticas.Find(id);
            db.ObraArtisticas.Remove(obraArtistica);
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
