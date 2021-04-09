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
    public class GradoAcademicoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: GradoAcademico
        public ActionResult Index()
        {
            return View(db.GradoAcademicoes.ToList());
        }

        // GET: GradoAcademico/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradoAcademico gradoAcademico = db.GradoAcademicoes.Find(id);
            if (gradoAcademico == null)
            {
                return HttpNotFound();
            }
            return View(gradoAcademico);
        }

        // GET: GradoAcademico/Create
        public ActionResult CreateGrado()
        {
            return View();
        }

        // POST: GradoAcademico/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGrado([Bind(Include = "ID,nombre")] GradoAcademico gradoAcademico, HttpPostedFileBase upload)
        {
            try
            {
                GradoAcademico grado = new GradoAcademico();
                grado.nombre = gradoAcademico.nombre;
                db.GradoAcademicoes.Add(grado);
                db.SaveChanges();

                PersonaXGradoAcademico personaGrado = new PersonaXGradoAcademico();
                personaGrado.id_persona = Int32.Parse(Session["ID"].ToString());
                personaGrado.id_grado = grado.ID;
                db.PersonaXGradoAcademicoes.Add(personaGrado);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertGradoDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, grado.ID);
                    //Path.GetFllPath(upload.FileName);
                }
                ViewBag.gradoAgregado = grado.nombre;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorGrado = "Error : No se pudo agregar el grado academico";
                return View();
            }

        }


        public static void InsertGradoDoc(System.Guid FileID, string FileName, string filecol, int id_grado)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoGradoAcademico](FileID,FileName,id_grado)
	VALUES (@FileID,@FileName,@id_grado);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoGradoAcademico
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
                        cmd.Parameters.Add("@id_grado", SqlDbType.Int).Value = id_grado;
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


        // GET: GradoAcademico/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradoAcademico gradoAcademico = db.GradoAcademicoes.Find(id);
            if (gradoAcademico == null)
            {
                return HttpNotFound();
            }
            return View(gradoAcademico);
        }

        // POST: GradoAcademico/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] GradoAcademico gradoAcademico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gradoAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gradoAcademico);
        }

        // GET: GradoAcademico/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradoAcademico gradoAcademico = db.GradoAcademicoes.Find(id);
            if (gradoAcademico == null)
            {
                return HttpNotFound();
            }
            return View(gradoAcademico);
        }

        // POST: GradoAcademico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GradoAcademico gradoAcademico = db.GradoAcademicoes.Find(id);
            db.GradoAcademicoes.Remove(gradoAcademico);
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
