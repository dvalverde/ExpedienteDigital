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
using System.IO;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Transactions;
using System.Data.Entity.Validation;

namespace ExpDigital.Controllers
{
    public class DisenoIndustrialController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: DisenoIndustrial
        public ActionResult Index()
        {
            return View(db.DisenoIndustrials.ToList());
        }

        // GET: DisenoIndustrial/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisenoIndustrial disenoIndustrial = db.DisenoIndustrials.Find(id);
            if (disenoIndustrial == null)
            {
                return HttpNotFound();
            }
            return View(disenoIndustrial);
        }

        // GET: DisenoIndustrial/Create
        public ActionResult CreateDisenoIndustrial()
        {
            return View();
        }

        // POST: DisenoIndustrial/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        public static void InsertDisenoDoc(System.Guid FileID, string FileName, string filecol, int id_diseno)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoDisenoIndustrial](FileID,FileName,id_diseno)
	VALUES (@FileID,@FileName,@id_diseno);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoDisenoIndustrial
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
                        cmd.Parameters.Add("@id_diseno", SqlDbType.Int).Value = id_diseno;
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
        public ActionResult CreateDisenoIndustrial([Bind(Include = "ID,nombre")] DisenoIndustrial disenoIndustrial, HttpPostedFileBase upload)
        {
            try
            {
                db.DisenoIndustrials.Add(disenoIndustrial);
                db.SaveChanges();

                PersonaXDisenoIndustrial personaX = new PersonaXDisenoIndustrial();
                personaX.id_persona = Int32.Parse(Session["ID"].ToString());
                personaX.id_diseno = disenoIndustrial.ID;
                db.PersonaXDisenoIndustrials.Add(personaX);
                db.SaveChanges();

                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertDisenoDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, disenoIndustrial.ID);
                    //Path.GetFullPath(upload.FileName);
                }
                ViewBag.obraAgregado = disenoIndustrial.nombre;
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

        // GET: DisenoIndustrial/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisenoIndustrial disenoIndustrial = db.DisenoIndustrials.Find(id);
            if (disenoIndustrial == null)
            {
                return HttpNotFound();
            }
            return View(disenoIndustrial);
        }

        // POST: DisenoIndustrial/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] DisenoIndustrial disenoIndustrial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disenoIndustrial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(disenoIndustrial);
        }

        // GET: DisenoIndustrial/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisenoIndustrial disenoIndustrial = db.DisenoIndustrials.Find(id);
            if (disenoIndustrial == null)
            {
                return HttpNotFound();
            }
            return View(disenoIndustrial);
        }

        // POST: DisenoIndustrial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DisenoIndustrial disenoIndustrial = db.DisenoIndustrials.Find(id);
            db.DisenoIndustrials.Remove(disenoIndustrial);
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
