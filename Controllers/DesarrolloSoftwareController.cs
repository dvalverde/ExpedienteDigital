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
using System.Transactions;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.IO;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ExpDigital.Controllers
{
    public class DesarrolloSoftwareController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: DesarrolloSoftware
        public ActionResult Index()
        {
            return View(db.DesarrolloSoftwares.ToList());
        }

        // GET: DesarrolloSoftware/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesarrolloSoftware desarrolloSoftware = db.DesarrolloSoftwares.Find(id);
            if (desarrolloSoftware == null)
            {
                return HttpNotFound();
            }
            return View(desarrolloSoftware);
        }

        // GET: DesarrolloSoftware/Create
        public ActionResult CreateDesarrolloSoftware()
        {
            return View();
        }

        // POST: DesarrolloSoftware/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDesarrolloSoftware([Bind(Include = "ID,nombre,numeroAutores, autores, autorXDesarrolloSoftware")] DesarolloSoftwareAutor desarrolloSoftwareAutor, HttpPostedFileBase upload)
        {
            try
            {
                DesarrolloSoftware software = new DesarrolloSoftware();
                software.nombre = desarrolloSoftwareAutor.nombre;
                software.numeroAutores = desarrolloSoftwareAutor.numeroAutores;
                db.DesarrolloSoftwares.Add(software);
                db.SaveChanges();

                PersonaXDesarrolloSoftware personaSoftware = new PersonaXDesarrolloSoftware();
                personaSoftware.id_persona = Int32.Parse(Session["ID"].ToString());
                personaSoftware.id_desarrollo_software = software.ID;
                db.PersonaXDesarrolloSoftwares.Add(personaSoftware);
                db.SaveChanges();

                for (int i = 0; i < desarrolloSoftwareAutor.numeroAutores; i++)
                {
                    Autor autor = new Autor();
                    autor.nombre = desarrolloSoftwareAutor.autores[i].nombre;
                    autor.correoElectronico = desarrolloSoftwareAutor.autores[i].correoElectronico;
                    db.Autors.Add(autor);
                    db.SaveChanges();
                    AutorXDesarrolloSoftware autorXdesarrollo = new AutorXDesarrolloSoftware();
                    autorXdesarrollo.id_desarrollo_software = software.ID;
                    autorXdesarrollo.id_autor = autor.ID;
                    autorXdesarrollo.distribucionAutoria = Convert.ToDecimal(desarrolloSoftwareAutor.autorXDesarrolloSoftware.ElementAt(i).distribucionAutoria);
                    db.AutorXDesarrolloSoftwares.Add(autorXdesarrollo);
                    db.SaveChanges();
                }
                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertDesarrolloSoftwareDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, software.ID);
                }
                ViewBag.softAgregado = desarrolloSoftwareAutor.nombre;
                return View();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ViewBag.errorsoft = "Error: " + dbEx;
                        Trace.TraceInformation("Property: {0} Error: {1}",
                        validationError.PropertyName,
                        validationError.ErrorMessage);
                    }
                }
                return View();
            }
        }


        public static void InsertDesarrolloSoftwareDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_desarrollo_software)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoDesarrolloSoftware](FileID,FileName,tipo,id_desarrollo_software)
	    VALUES (@FileID,@FileName,@tipo,@id_desarrollo_software);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
        FROM DocumentoDesarrolloSoftware
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
                        cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value = tipo;
                        cmd.Parameters.Add("@id_desarrollo_software", SqlDbType.Int).Value = id_desarrollo_software;
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

        // GET: DesarrolloSoftware/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesarrolloSoftware desarrolloSoftware = db.DesarrolloSoftwares.Find(id);
            if (desarrolloSoftware == null)
            {
                return HttpNotFound();
            }
            return View(desarrolloSoftware);
        }

        // POST: DesarrolloSoftware/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,numeroAutores")] DesarrolloSoftware desarrolloSoftware)
        {
            if (ModelState.IsValid)
            {
                db.Entry(desarrolloSoftware).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(desarrolloSoftware);
        }

        // GET: DesarrolloSoftware/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesarrolloSoftware desarrolloSoftware = db.DesarrolloSoftwares.Find(id);
            if (desarrolloSoftware == null)
            {
                return HttpNotFound();
            }
            return View(desarrolloSoftware);
        }

        // POST: DesarrolloSoftware/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DesarrolloSoftware desarrolloSoftware = db.DesarrolloSoftwares.Find(id);
            db.DesarrolloSoftwares.Remove(desarrolloSoftware);
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
