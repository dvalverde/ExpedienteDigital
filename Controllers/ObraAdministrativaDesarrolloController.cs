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
    public class ObraAdministrativaDesarrolloController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ObraAdministrativaDesarrollo
        public ActionResult Index()
        {
            var obraAdministrativaDesarrolloes = db.ObraAdministrativaDesarrolloes.Include(o => o.TipoObraAdmi);
            return View(obraAdministrativaDesarrolloes.ToList());
        }

        // GET: ObraAdministrativaDesarrollo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraAdministrativaDesarrollo obraAdministrativaDesarrollo = db.ObraAdministrativaDesarrolloes.Find(id);
            if (obraAdministrativaDesarrollo == null)
            {
                return HttpNotFound();
            }
            return View(obraAdministrativaDesarrollo);
        }

        // GET: ObraAdministrativaDesarrollo/Create
        public ActionResult CreateObraAdmin()
        {
            ViewBag.id_tipo = new SelectList(db.TipoObraAdmis, "ID", "nombre");
            return View();
        }

        // POST: ObraAdministrativaDesarrollo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public static void InsertObraAdminDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_obra_administrativa)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoObraAdministrativa](FileID,FileName,tipo,id_obra_administrativa)
	VALUES (@FileID,@FileName,@tipo,@id_obra_administrativa);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoObraAdministrativa
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
                        cmd.Parameters.Add("@id_obra_administrativa", SqlDbType.Int).Value = id_obra_administrativa;
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
        public ActionResult CreateObraAdmin([Bind(Include = "ID,titulo,numeroAutores,id_tipo,autores, autorXObrasAdmin")] ObraAdminAutor obraAutor, HttpPostedFileBase upload, HttpPostedFileBase uploadCert)
        {
            try
            {
                ObraAdministrativaDesarrollo obraAdmin = new ObraAdministrativaDesarrollo();
            obraAdmin.titulo = obraAutor.titulo;
            obraAdmin.numeroAutores = obraAutor.numeroAutores;
            obraAdmin.id_tipo = obraAutor.id_tipo;
            db.ObraAdministrativaDesarrolloes.Add(obraAdmin);
            db.SaveChanges();

                PersonaXObraAdministrativaDesarrollo personaX = new PersonaXObraAdministrativaDesarrollo();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_obra_administrativa = obraAdmin.ID;
                db.PersonaXObraAdministrativaDesarrolloes.Add(personaX);
                db.SaveChanges();

                for (int i = 0; i < obraAutor.numeroAutores; i++)
            {
                Autor autor = new Autor();
                autor.nombre = obraAutor.autores.ElementAt(i).nombre;
                autor.correoElectronico = obraAutor.autores.ElementAt(i).correoElectronico;
                db.Autors.Add(autor);
                db.SaveChanges();
                AutorXObraAdministrativaDesarrollo autorXobraAdmi = new AutorXObraAdministrativaDesarrollo();
                autorXobraAdmi.id_obra_administrativa = obraAdmin.ID;
                autorXobraAdmi.id_autor = autor.ID;
                autorXobraAdmi.distribucionAutoria = Convert.ToDecimal(obraAutor.autorXObrasAdmin.ElementAt(i).distribucionAutoria);
                db.AutorXObraAdministrativaDesarrolloes.Add(autorXobraAdmi);
                db.SaveChanges();
            }

            if (upload != null)
            {
                upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                InsertObraAdminDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, obraAdmin.ID);
                //Path.GetFullPath(upload.FileName);
            }
            if (uploadCert != null)
            {
                upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), uploadCert.FileName));
                string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), uploadCert.FileName);
                InsertObraAdminDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(uploadCert.FileName), fullPath, 1, obraAdmin.ID);
                //Path.GetFullPath(upload.FileName);
            }
            ViewBag.obraAgregado = obraAutor.titulo;
            ViewBag.id_tipo = new SelectList(db.TipoObraAdmis, "ID", "nombre", obraAutor.id_tipo);
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

        // GET: ObraAdministrativaDesarrollo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraAdministrativaDesarrollo obraAdministrativaDesarrollo = db.ObraAdministrativaDesarrolloes.Find(id);
            if (obraAdministrativaDesarrollo == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_tipo = new SelectList(db.TipoObraAdmis, "ID", "nombre", obraAdministrativaDesarrollo.id_tipo);
            return View(obraAdministrativaDesarrollo);
        }

        // POST: ObraAdministrativaDesarrollo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,titulo,numeroAutores,id_tipo")] ObraAdministrativaDesarrollo obraAdministrativaDesarrollo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obraAdministrativaDesarrollo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_tipo = new SelectList(db.TipoObraAdmis, "ID", "nombre", obraAdministrativaDesarrollo.id_tipo);
            return View(obraAdministrativaDesarrollo);
        }

        // GET: ObraAdministrativaDesarrollo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraAdministrativaDesarrollo obraAdministrativaDesarrollo = db.ObraAdministrativaDesarrolloes.Find(id);
            if (obraAdministrativaDesarrollo == null)
            {
                return HttpNotFound();
            }
            return View(obraAdministrativaDesarrollo);
        }

        // POST: ObraAdministrativaDesarrollo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ObraAdministrativaDesarrollo obraAdministrativaDesarrollo = db.ObraAdministrativaDesarrolloes.Find(id);
            db.ObraAdministrativaDesarrolloes.Remove(obraAdministrativaDesarrollo);
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
