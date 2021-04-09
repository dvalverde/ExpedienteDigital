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
    public class ProyectoGraduacionGalardonadoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ProyectoGraduacionGalardonado
        public ActionResult Index()
        {
            return View(db.ProyectoGraduacionGalardonadoes.ToList());
        }

        // GET: ProyectoGraduacionGalardonado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProyectoGraduacionGalardonado proyectoGraduacionGalardonado = db.ProyectoGraduacionGalardonadoes.Find(id);
            if (proyectoGraduacionGalardonado == null)
            {
                return HttpNotFound();
            }
            return View(proyectoGraduacionGalardonado);
        }

        // GET: ProyectoGraduacionGalardonado/Create
        public ActionResult CreateProyecto()
        {
            return View();
        }

        // POST: ProyectoGraduacionGalardonado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProyecto([Bind(Include = "ID,nombre")] ProyectoGraduacionGalardonado proyectoGraduacionGalardonado, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProyectoGraduacionGalardonado proyecto = new ProyectoGraduacionGalardonado();
                    proyecto.nombre = proyectoGraduacionGalardonado.nombre;
                    db.ProyectoGraduacionGalardonadoes.Add(proyecto);
                    db.SaveChanges();

                    PersonaXProyectoGraduacion personaProy = new PersonaXProyectoGraduacion();
                    personaProy.id_persona = Int32.Parse(Session["ID"].ToString());
                    personaProy.id_proyecto_graduacion = proyecto.ID;
                    db.PersonaXProyectoGraduacions.Add(personaProy);
                    db.SaveChanges();


                    if (upload != null)
                    {
                        upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                        string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                        InsertProyectoDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, proyecto.ID);
                        //Path.GetFullPath(upload.FileName);
                    }
                    ViewBag.proyAgregado = proyecto.nombre;
                    return View();
                }
                catch (Exception e)
                {
                    ViewBag.errorproy = "Error : No se pudo agregar el libro ";
                    return View();
                }
            }

            return View(proyectoGraduacionGalardonado);
        }
        public static void InsertProyectoDoc(System.Guid FileID, string FileName, string filecol, int tipo,int id_proyecto)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoProyectoGraduacion](FileID,FileName,id_proyecto)
	VALUES (@FileID,@FileName,@id_proyecto);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoProyectoGraduacion
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
                        //cmd.Parameters.Add("@tipo", SqlDbType.Int).Value = tipo; //VERIFICAR QUE TENGA TIPO , SI NO SE BORRA 
                        cmd.Parameters.Add("@id_proyecto", SqlDbType.Int).Value = id_proyecto;
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

        // GET: ProyectoGraduacionGalardonado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProyectoGraduacionGalardonado proyectoGraduacionGalardonado = db.ProyectoGraduacionGalardonadoes.Find(id);
            if (proyectoGraduacionGalardonado == null)
            {
                return HttpNotFound();
            }
            return View(proyectoGraduacionGalardonado);
        }

        // POST: ProyectoGraduacionGalardonado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] ProyectoGraduacionGalardonado proyectoGraduacionGalardonado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyectoGraduacionGalardonado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proyectoGraduacionGalardonado);
        }

        // GET: ProyectoGraduacionGalardonado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProyectoGraduacionGalardonado proyectoGraduacionGalardonado = db.ProyectoGraduacionGalardonadoes.Find(id);
            if (proyectoGraduacionGalardonado == null)
            {
                return HttpNotFound();
            }
            return View(proyectoGraduacionGalardonado);
        }

        // POST: ProyectoGraduacionGalardonado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProyectoGraduacionGalardonado proyectoGraduacionGalardonado = db.ProyectoGraduacionGalardonadoes.Find(id);
            db.ProyectoGraduacionGalardonadoes.Remove(proyectoGraduacionGalardonado);
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
