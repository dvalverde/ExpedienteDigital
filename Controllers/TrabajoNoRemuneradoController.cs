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
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Drawing;
using System.Transactions;

namespace ExpDigital.Controllers
{
    public class TrabajoNoRemuneradoController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: TrabajoNoRemunerado
        public ActionResult Index()
        {
            return View(db.TrabajoNoRemuneradoes.ToList());
        }

        // GET: TrabajoNoRemunerado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrabajoNoRemunerado trabajoNoRemunerado = db.TrabajoNoRemuneradoes.Find(id);
            if (trabajoNoRemunerado == null)
            {
                return HttpNotFound();
            }
            return View(trabajoNoRemunerado);
        }

        // GET: TrabajoNoRemunerado/Create
        public ActionResult CreateTrabajoNoRemunerado()
        {
            return View();
        }

        // POST: TrabajoNoRemunerado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTrabajoNoRemunerado([Bind(Include = "ID,nombre,periodoLectivo")] TrabajoNoRemunerado trabajoNoRemunerado, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TrabajoNoRemunerado trabajo = new TrabajoNoRemunerado();
                    trabajo.nombre = trabajoNoRemunerado.nombre;
                    trabajo.periodoLectivo = trabajoNoRemunerado.periodoLectivo;
                    db.TrabajoNoRemuneradoes.Add(trabajo);
                    db.SaveChanges();

                    PersonaXTrabajoNoRemunerado personaTrabajo = new PersonaXTrabajoNoRemunerado();
                    personaTrabajo.id_persona = Int32.Parse(Session["ID"].ToString());
                    personaTrabajo.id_trabajo = trabajo.ID;
                    db.PersonaXTrabajoNoRemuneradoes.Add(personaTrabajo);
                    db.SaveChanges();

                    if (upload != null)
                    {
                        upload.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName));
                        string fullPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), upload.FileName);
                        InsertTrabajoDoc(System.Guid.NewGuid(), System.IO.Path.GetFileName(upload.FileName), fullPath, 0, trabajo.ID);
                        //Path.GetFullPath(upload.FileName);
                    }
                    ViewBag.trAgregado = trabajo.nombre;
                    return View();
                }
                catch (Exception e)
                {
                    ViewBag.errortrab = "Error : No se pudo agregar el libro ";
                    return View();
                }
            }

            return View(trabajoNoRemunerado);
        }
        
        //PARA GUARDAR DOCUMENTOS PESADOS 
        public static void InsertTrabajoDoc(System.Guid FileID, string FileName, string filecol, int tipo, int id_trabajo)
        {
            const string InsertTSql = @"
        INSERT INTO [DocumentoTrabajoNoRemunerado](FileID,FileName,id_trabajo)
	VALUES (@FileID,@FileName,@id_trabajo);
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
          FROM DocumentoTrabajoNoRemunerado
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
                        cmd.Parameters.Add("@id_trabajo", SqlDbType.Int).Value = id_trabajo;
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

        // GET: TrabajoNoRemunerado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrabajoNoRemunerado trabajoNoRemunerado = db.TrabajoNoRemuneradoes.Find(id);
            if (trabajoNoRemunerado == null)
            {
                return HttpNotFound();
            }
            return View(trabajoNoRemunerado);
        }

        // POST: TrabajoNoRemunerado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nombre,periodoLectivo")] TrabajoNoRemunerado trabajoNoRemunerado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trabajoNoRemunerado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trabajoNoRemunerado);
        }

        // GET: TrabajoNoRemunerado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrabajoNoRemunerado trabajoNoRemunerado = db.TrabajoNoRemuneradoes.Find(id);
            if (trabajoNoRemunerado == null)
            {
                return HttpNotFound();
            }
            return View(trabajoNoRemunerado);
        }

        // POST: TrabajoNoRemunerado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrabajoNoRemunerado trabajoNoRemunerado = db.TrabajoNoRemuneradoes.Find(id);
            db.TrabajoNoRemuneradoes.Remove(trabajoNoRemunerado);
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
