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
using ExpDigital.ViewModels;

namespace ExpDigital.Controllers
{
    public class ObraDidacticaController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ObraDidactica
        public ActionResult Index()
        {
            return View(db.ObraDidacticas.ToList());
        }

        // GET: ObraDidactica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraDidactica obraDidactica = db.ObraDidacticas.Find(id);
            if (obraDidactica == null)
            {
                return HttpNotFound();
            }
            return View(obraDidactica);
        }

        // GET: ObraDidactica/createObraDidactica
        public ActionResult createObraDidactica()
        {
            return View();
        }

        // POST: ObraDidactica/createObraDidactica
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createObraDidactica([Bind(Include = "ID, nombre, numeroAutores, autores, autorXObraDidactica")] ObraDidacticaAutor obraDidacticaAutor, HttpPostedFileBase upload)
        {
            try
            {
                ObraDidactica didactica = new ObraDidactica();
                didactica.nombre = obraDidacticaAutor.nombre;
                didactica.numeroAutores = obraDidacticaAutor.numeroAutores;
                db.ObraDidacticas.Add(didactica);
                db.SaveChanges();

                PersonaXObraDidactica personaDidactica = new PersonaXObraDidactica();
                personaDidactica.id_persona = Int32.Parse(Session["ID"].ToString());
                personaDidactica.id_obra_didactica = didactica.ID;
                db.PersonaXObraDidacticas.Add(personaDidactica);
                db.SaveChanges();

                for (int i = 0; i < obraDidacticaAutor.numeroAutores; i++)
                {
                    Autor autor = new Autor();
                    autor.nombre = obraDidacticaAutor.autores[i].nombre;
                    autor.correoElectronico = obraDidacticaAutor.autores[i].correoElectronico;
                    db.Autors.Add(autor);
                    db.SaveChanges();
                    AutorXObraDidactica autorXdidactica = new AutorXObraDidactica();
                    autorXdidactica.id_obra_didactica = didactica.ID;
                    autorXdidactica.id_autor = autor.ID;
                    db.AutorXObraDidacticas.Add(autorXdidactica);
                    db.SaveChanges();
                }

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertObraDidacticaDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, didactica.ID);
                }

                ViewBag.obraAgregado = obraDidacticaAutor.nombre;
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

        public static void InsertObraDidacticaDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_obra_didactica)
        {
            const string InsertTSql = @"
            INSERT INTO [DocumentoObraDidactica](FileID,FileName,tipo,id_obra_didactica)
	        VALUES (@FileID,@FileName,@tipo,@id_obra_didactica);
            SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
            FROM DocumentoObraDidactica
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
                        cmd.Parameters.Add("@id_obra_didactica", SqlDbType.Int).Value = id_obra_didactica;
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


        // GET: ObraDidactica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraDidactica obraDidactica = db.ObraDidacticas.Find(id);
            if (obraDidactica == null)
            {
                return HttpNotFound();
            }
            return View(obraDidactica);
        }

        // POST: ObraDidactica/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,numeroAutores")] ObraDidactica obraDidactica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obraDidactica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obraDidactica);
        }

        // GET: ObraDidactica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraDidactica obraDidactica = db.ObraDidacticas.Find(id);
            if (obraDidactica == null)
            {
                return HttpNotFound();
            }
            return View(obraDidactica);
        }

        // POST: ObraDidactica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ObraDidactica obraDidactica = db.ObraDidacticas.Find(id);
            db.ObraDidacticas.Remove(obraDidactica);
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
