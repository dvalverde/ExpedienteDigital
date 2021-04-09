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
using System.Data.Entity.Validation;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Transactions;

namespace ExpDigital.Controllers
{
    public class MembresiaOrganismoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: MembresiaOrganismo
        public ActionResult Index()
        {
            return View(db.MembresiaOrganismos.ToList());
        }

        // GET: MembresiaOrganismo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembresiaOrganismo membresiaOrganismo = db.MembresiaOrganismos.Find(id);
            if (membresiaOrganismo == null)
            {
                return HttpNotFound();
            }
            return View(membresiaOrganismo);
        }

        // GET: MembresiaOrganismo/Create
        public ActionResult CreateMembresiaOrganismo()
        {
            return View();
        }

        // POST: MembresiaOrganismo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMembresiaOrganismo([Bind(Include = "ID,nombre")] MembresiaOrganismo membresiaOrganismo, HttpPostedFileBase upload)
        {
            try
            {
                db.MembresiaOrganismos.Add(membresiaOrganismo);
                db.SaveChanges();

                PersonaXMembresiaOrganismo personaOrganismo = new PersonaXMembresiaOrganismo();
                personaOrganismo.id_persona = Int32.Parse(Session["ID"].ToString());
                personaOrganismo.id_membresia = membresiaOrganismo.ID;
                db.PersonaXMembresiaOrganismos.Add(personaOrganismo);
                db.SaveChanges();


                if (upload != null)
                {
                    upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                    string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                    InsertCursoDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, membresiaOrganismo.ID);

                }

                ViewBag.organismoAgregado = membresiaOrganismo.nombre;
                return View();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ViewBag.errorOrganismo = "Error: " + dbEx;
                        Trace.TraceInformation("Property: {0} Error: {1}",
                        validationError.PropertyName,
                        validationError.ErrorMessage);
                    }
                }
                return View();
            }
        }

        public static void InsertCursoDoc(System.Guid FileID, string FileName, string filecol, int id_membresia)
        {
            const string InsertTSql = @"
            INSERT INTO [DocumentoMembresiaOrganismos](FileID,FileName,id_membresia)
	        VALUES (@FileID,@FileName,@id_membresia);
            SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
            FROM   DocumentoMembresiaOrganismos
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
                        cmd.Parameters.Add("@id_membresia", SqlDbType.Int).Value = id_membresia;
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

        // GET: MembresiaOrganismo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembresiaOrganismo membresiaOrganismo = db.MembresiaOrganismos.Find(id);
            if (membresiaOrganismo == null)
            {
                return HttpNotFound();
            }
            return View(membresiaOrganismo);
        }

        // POST: MembresiaOrganismo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre")] MembresiaOrganismo membresiaOrganismo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(membresiaOrganismo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(membresiaOrganismo);
        }

        // GET: MembresiaOrganismo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembresiaOrganismo membresiaOrganismo = db.MembresiaOrganismos.Find(id);
            if (membresiaOrganismo == null)
            {
                return HttpNotFound();
            }
            return View(membresiaOrganismo);
        }

        // POST: MembresiaOrganismo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MembresiaOrganismo membresiaOrganismo = db.MembresiaOrganismos.Find(id);
            db.MembresiaOrganismos.Remove(membresiaOrganismo);
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
