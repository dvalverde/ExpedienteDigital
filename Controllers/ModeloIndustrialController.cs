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
    public class ModeloIndustrialController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: ModeloIndustrial
        public ActionResult Index()
        {
            return View(db.ModeloIndustrials.ToList());
        }

        // GET: ModeloIndustrial/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModeloIndustrial modeloIndustrial = db.ModeloIndustrials.Find(id);
            if (modeloIndustrial == null)
            {
                return HttpNotFound();
            }
            return View(modeloIndustrial);
        }

        // GET: ModeloIndustrial/Create
        public ActionResult CreateModelo()
        {
            return View();
        }

        // POST: ModeloIndustrial/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModelo([Bind(Include = "ID,nombre")] ModeloIndustrial modeloIndustrial, HttpPostedFileBase upload)
        {
           try
            {
                ModeloIndustrial modelo = new ModeloIndustrial();
                modelo.nombre = modeloIndustrial.nombre;
                db.ModeloIndustrials.Add(modelo);
                db.SaveChanges();

                PersonaXModeloIndustrial personaCapacitacion = new PersonaXModeloIndustrial();
                personaCapacitacion.id_persona = Int32.Parse(Session["ID"].ToString());
                personaCapacitacion.id_modelo = modelo.ID;
                db.PersonaXModeloIndustrials.Add(personaCapacitacion);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertModelDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, modelo.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                ViewBag.modelAgrega = modelo.nombre;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errormode = "Error : No se pudo agregar el modelo";
                return View();
            }
        }
        public static void InsertModelDoc(System.Guid FileID, string FileName, string filecol, int id_modelo)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoModeloIndustrial](FileID,FileName,id_modelo)
	    VALUES (@FileID,@FileName,@id_modelo);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoModeloIndustrial
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
                        cmd.Parameters.Add("@id_modelo", SqlDbType.Int).Value = id_modelo;
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

        // GET: ModeloIndustrial/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModeloIndustrial modeloIndustrial = db.ModeloIndustrials.Find(id);
            if (modeloIndustrial == null)
            {
                return HttpNotFound();
            }
            return View(modeloIndustrial);
        }

        // POST: ModeloIndustrial/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] ModeloIndustrial modeloIndustrial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modeloIndustrial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modeloIndustrial);
        }

        // GET: ModeloIndustrial/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModeloIndustrial modeloIndustrial = db.ModeloIndustrials.Find(id);
            if (modeloIndustrial == null)
            {
                return HttpNotFound();
            }
            return View(modeloIndustrial);
        }

        // POST: ModeloIndustrial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModeloIndustrial modeloIndustrial = db.ModeloIndustrials.Find(id);
            db.ModeloIndustrials.Remove(modeloIndustrial);
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
