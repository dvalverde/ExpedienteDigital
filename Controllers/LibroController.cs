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
    public class LibroController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: Libro
        public ActionResult Index()
        {
            var libroes = db.Libroes.Include(l => l.Pai);
            return View(libroes.ToList());
        }

        // GET: Libro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libroes.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // GET: Libro/Create
        public ActionResult CreateLibro()
        {
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre");
            return View();
        }

        // POST: Libro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLibro([Bind(Include = "ID,titulo,anno,editorial,consejoEditorial,id_pais,capitulos,matrizAutores,matrizDistribucion,numeroCapitulos")] LibroCapituloAutor librocapautor, HttpPostedFileBase upload)
        {
            try
            {
                //agrego el libro para obtener su id
                Libro libro = new Libro();
                libro.anno = librocapautor.anno;
                libro.titulo = librocapautor.titulo;
                libro.editorial = librocapautor.editorial;
                libro.id_pais = librocapautor.id_pais;
                libro.consejoEditorial = librocapautor.consejoEditorial;
                db.Libroes.Add(libro);
                db.SaveChanges();

                //iterar sobre los capitulos
                Console.WriteLine("count ");
                for (int i = 0; i < librocapautor.numeroCapitulos; i++) //no es hasta 10 si no hasta el valor del select
                {
                    //AGREGO UN CAPITULO 
                    Capitulo cap = new Capitulo();
                    cap.titulo = librocapautor.capitulos[i];
                    cap.id_libro = libro.ID;
                    db.Capituloes.Add(cap);
                    db.SaveChanges();
                    //iterar sobre los autores de un capitulo
                    for (int j = 0; j < 10; j++)//HASTA EL MOMENTO HAY 10 AUTORES POSIBLES 
                    {
                        Console.WriteLine(librocapautor.matrizAutores[0][0].nombre);
                        if (librocapautor.matrizAutores[i][j].nombre != null)//pregunto si no esta vacio el nombre 
                        {
                            Console.WriteLine("count ");
                            //creo un autor
                            Autor autorsito = new Autor();
                            autorsito.nombre = librocapautor.matrizAutores[i][j].nombre;
                            autorsito.correoElectronico = librocapautor.matrizAutores[i][j].correoElectronico;
                            db.Autors.Add(autorsito);
                            db.SaveChanges();
                            AutorXCapitulo autoCap = new AutorXCapitulo();
                            autoCap.id_autor = autorsito.ID;
                            autoCap.id_capitulo = cap.ID;
                            autoCap.distribucionAutoria = Convert.ToDecimal(librocapautor.matrizDistribucion[i][j].distribucionAutoria);
                            db.AutorXCapituloes.Add(autoCap);
                            db.SaveChanges();
                        }
                    }
                }
                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertLibroDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, libro.ID);
                    //Path.GetFullPath(upload.FileName);
                }

                ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre", librocapautor.id_pais);
                ViewBag.libAgregado = librocapautor.titulo;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorlib = "Error : No se pudo agregar el libro ";
                return View();
            }
        }

        public static void InsertLibroDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_libro)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoLibro](FileID,FileName,tipo,id_libro)
	VALUES (@FileID,@FileName,@tipo,@id_libro);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoLibro
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
                        cmd.Parameters.Add("@id_libro", SqlDbType.Int).Value = id_libro;
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
        // GET: Libro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libroes.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre", libro.id_pais);
            return View(libro);
        }

        // POST: Libro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,titulo,anno,editorial,consejoEditorial,id_pais")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(libro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre", libro.id_pais);
            return View(libro);
        }

        // GET: Libro/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libroes.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // POST: Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Libro libro = db.Libroes.Find(id);
            db.Libroes.Remove(libro);
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