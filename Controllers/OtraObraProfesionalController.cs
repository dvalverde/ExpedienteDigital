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
    public class OtraObraProfesionalController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: OtraObraProfesional
        public ActionResult Index()
        {
            var otraObraProfesionals = db.OtraObraProfesionals.Include(o => o.TipoObraProfesional);
            return View(otraObraProfesionals.ToList());
        }

        // GET: OtraObraProfesional/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtraObraProfesional otraObraProfesional = db.OtraObraProfesionals.Find(id);
            if (otraObraProfesional == null)
            {
                return HttpNotFound();
            }
            return View(otraObraProfesional);
        }

        // GET: OtraObraProfesional/Create
        public ActionResult CreateObraProfesional()
        {
            ViewBag.id_tipo = new SelectList(db.TipoObraProfesionals, "ID", "nombre");
            return View();
        }

        // POST: OtraObraProfesional/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateObraProfesional([Bind(Include = "ID,titulo,numeroAutores,id_tipo, autores, autorXOtraObraProfesionals")] OtraObraAutor otraObraAutor, HttpPostedFileBase upload, HttpPostedFileBase upload1)
        {
            try
            {
                OtraObraProfesional otraObraProfesional1 = new OtraObraProfesional();
                otraObraProfesional1.titulo = otraObraAutor.titulo;
                otraObraProfesional1.numeroAutores = otraObraAutor.numeroAutores;
                otraObraProfesional1.id_tipo = otraObraAutor.id_tipo;
                db.OtraObraProfesionals.Add(otraObraProfesional1);
                db.SaveChanges();

                PersonaXOtraObraProfesional personaOtraObra = new PersonaXOtraObraProfesional();
                personaOtraObra.id_persona = Int32.Parse(Session["ID"].ToString());
                personaOtraObra.id_obra_profesional = otraObraProfesional1.ID;
                db.PersonaXOtraObraProfesionals.Add(personaOtraObra);
                db.SaveChanges();

                for (int i = 0; i < otraObraAutor.numeroAutores; i++)
                {
                    Autor autor = new Autor();
                    autor.nombre = otraObraAutor.autores[i].nombre;
                    autor.correoElectronico = otraObraAutor.autores[i].correoElectronico;
                    db.Autors.Add(autor);
                    db.SaveChanges();
                    AutorXOtraObraProfesional autorXOtraObraProfesional = new AutorXOtraObraProfesional();
                    autorXOtraObraProfesional.id_obra_profesional = otraObraProfesional1.ID;
                    autorXOtraObraProfesional.id_autor = autor.ID;
                    autorXOtraObraProfesional.distribucionAutoria = Convert.ToDecimal(otraObraAutor.autorXOtraObraProfesionals.ElementAt(i).distribucionAutoria);
                    db.AutorXOtraObraProfesionals.Add(autorXOtraObraProfesional);
                    db.SaveChanges();
                }
                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertLibroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, otraObraProfesional1.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                if (upload1 != null)
                {
                    upload1.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload1.FileName));
                    string fullPath1 = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload1.FileName);
                    InsertLibroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload1.FileName), fullPath1, 1, otraObraProfesional1.ID);
                    //Path.GetFullPath(upload.FileName);
                }

                //return RedirectToAction("Index");

                ViewBag.id_tipo = new SelectList(db.TipoObraProfesionals, "ID", "nombre", otraObraAutor.id_tipo);
                ViewBag.otraObraAgregado = otraObraAutor.titulo;
                return View();
                //return View(articuloAutor);
                //return View();
            }
            catch (Exception e)
            {
                ViewBag.errorOtraObra = "Error : No se pudo agregar ";
                return View();
            }
        }

        public static void InsertLibroDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_obra_profesional)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoOtraObraProfesional](FileID,FileName,tipo,id_obra_profesional)
	VALUES (@FileID,@FileName,@tipo,@id_obra_profesional);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoOtraObraProfesional
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
                        cmd.Parameters.Add("@id_obra_profesional", SqlDbType.Int).Value = id_obra_profesional;
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

        // GET: OtraObraProfesional/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtraObraProfesional otraObraProfesional = db.OtraObraProfesionals.Find(id);
            if (otraObraProfesional == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_tipo = new SelectList(db.TipoObraProfesionals, "ID", "nombre", otraObraProfesional.id_tipo);
            return View(otraObraProfesional);
        }

        // POST: OtraObraProfesional/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,titulo,numeroAutores,id_tipo")] OtraObraProfesional otraObraProfesional)
        {
            if (ModelState.IsValid)
            {
                db.Entry(otraObraProfesional).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_tipo = new SelectList(db.TipoObraProfesionals, "ID", "nombre", otraObraProfesional.id_tipo);
            return View(otraObraProfesional);
        }

        // GET: OtraObraProfesional/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtraObraProfesional otraObraProfesional = db.OtraObraProfesionals.Find(id);
            if (otraObraProfesional == null)
            {
                return HttpNotFound();
            }
            return View(otraObraProfesional);
        }

        // POST: OtraObraProfesional/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OtraObraProfesional otraObraProfesional = db.OtraObraProfesionals.Find(id);
            db.OtraObraProfesionals.Remove(otraObraProfesional);
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
