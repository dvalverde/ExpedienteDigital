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
    public class ArticuloController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();
        // GET: Articulo
        public ActionResult Index()
        {
            var articuloes = db.Articuloes.Include(a => a.Pai);
            return View(articuloes.ToList());
        }

        // GET: Articulo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulo articulo = db.Articuloes.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            return View(articulo);
        }

        // GET: Articulo/Create
        public ActionResult CreateArticulo()
        {
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre");
            return View();
        }

        // POST: Articulo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateArticulo([Bind(Include = "ID,titulo,numeroAutores,anno,revista,id_pais,consejoEditorial, autores, autorXArticulos")] ArticuloAutor articuloAutor, HttpPostedFileBase upload)
        {
            //ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre");
            try
            {
                Articulo articulo = new Articulo();
                articulo.titulo = articuloAutor.titulo;
                articulo.numeroAutores = articuloAutor.numeroAutores;
                articulo.anno = articuloAutor.anno;
                articulo.revista = articuloAutor.revista;
                articulo.id_pais = articuloAutor.id_pais;
                articulo.consejoEditorial = articuloAutor.consejoEditorial;
                db.Articuloes.Add(articulo);
                db.SaveChanges();

                PersonaXArticulo personaArticulo = new PersonaXArticulo();
                personaArticulo.id_persona = Int32.Parse(Session["ID"].ToString());
                personaArticulo.id_articulo = articulo.ID;
                db.PersonaXArticuloes.Add(personaArticulo);
                db.SaveChanges();

                for (int i = 0; i < articuloAutor.numeroAutores; i++)
                {
                    Autor autor = new Autor();
                    autor.nombre = articuloAutor.autores[i].nombre;
                    autor.correoElectronico = articuloAutor.autores[i].correoElectronico;
                    db.Autors.Add(autor);
                    db.SaveChanges();
                    AutorXArticulo autorXarticulo = new AutorXArticulo();
                    autorXarticulo.id_articulo = articulo.ID;
                    autorXarticulo.id_autor = autor.ID;
                    autorXarticulo.distribucionAutoria = Convert.ToDecimal(articuloAutor.autorXArticulos.ElementAt(i).distribucionAutoria);
                    db.AutorXArticuloes.Add(autorXarticulo);
                    db.SaveChanges();
                }


                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertLibroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, articulo.ID);
                    //Path.GetFullPath(upload.FileName);
                }

                //return RedirectToAction("Index");

                ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre", articuloAutor.id_pais);
                ViewBag.artAgregado = articuloAutor.titulo;
                //return View(articuloAutor);
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorArt = "Error : No se pudo agregar el artículo " + e;
                return View();
            }

        }

        public static void InsertLibroDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_articulo)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoArticulo](FileID,FileName,tipo,id_articulo)
	VALUES (@FileID,@FileName,@tipo,@id_articulo);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoArticulo
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
                        cmd.Parameters.Add("@id_articulo", SqlDbType.Int).Value = id_articulo;
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
        //[HttpPost]
        /*public ActionResult SaveArticulo(string nombre, string correo, decimal distribucion)
        {
            Autor autor = new Autor();
            AutorXArticulo autorxarticulo = new AutorXArticulo();
            autor.nombre = nombre;
            autor.correoElectronico = correo;
            db.Autors.Add(autor);
            db.SaveChanges();
            //string queryAutor = "Select ID from Autor WHERE nombre = '" + nombre + "' and correoElectronico = '" + correo + "';";
            //var sequenceQueryResult = db.Database.SqlQuery<string>(queryAutor).FirstOrDefault();
            autorxarticulo.id_autor = (from s in db.Autors
                                              where s.nombre == nombre
                                              select s.ID).FirstOrDefault();
            autorxarticulo.id_articulo = 4;
            autorxarticulo.distribucionAutoria = distribucion;
            try
            {
                db.AutorXArticuloes.Add(autorxarticulo);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            return View();
        }*/

        // GET: Articulo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulo articulo = db.Articuloes.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre", articulo.id_pais);
            return View(articulo);
        }

        // POST: Articulo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,titulo,numeroAutores,anno,revista,id_pais,consejoEditorial")] Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre", articulo.id_pais);
            return View(articulo);
        }

        // GET: Articulo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulo articulo = db.Articuloes.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            return View(articulo);
        }

        // POST: Articulo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Articulo articulo = db.Articuloes.Find(id);
            db.Articuloes.Remove(articulo);
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
