using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpDigital.ServerManagement;


namespace ExpDigital.Controllers
{
    public class RespaldoController : Controller
    {
        Models.sp_ServerBackUpsQuerry_Result actual;
        // GET: Respaldo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuRespaldo()
        {
            try
            {
                using (Models.ExpedienteDigitalEntities db = new Models.ExpedienteDigitalEntities())
                {
                    var listaRespaldos = db.sp_ServerBackUpsQuerry().ToList();
                    return View(listaRespaldos);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        public ActionResult Detalles(int id)
        {
            try
            {
                using (Models.ExpedienteDigitalEntities db = new Models.ExpedienteDigitalEntities())
                {

                    var listaRespaldos = db.sp_ServerBackUpsQuerry().ToList();
                    foreach (var item in listaRespaldos)
                    {
                        if (item.backup_set_id == id)
                        {
                            actual = item;
                            return View(item);
                        }
                    }
                    return MenuRespaldo();
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return MenuRespaldo();
            }
        }
        [HttpPost]
        public ActionResult Detalles(string path)
        {
            try
            {
                string[] partes = path.Split('\\');
                ManejoBackUp mbu = new ManejoBackUp();
                mbu.RestoreDB(partes[partes.Length - 1]);
                ViewBag.restauracionExitosa = "Restauracion exitosa";
                return Detalles(actual.backup_set_id);
            }
            catch (Exception e)
            {
                ViewBag.errorRestauracion = "Error : No se pudo usar el respaldo" + e;
                return View();
            }
        }
        public ActionResult CreateRespaldo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRespaldo(string filename)
        {
            try
            {
                ManejoBackUp mbu = new ManejoBackUp();
                mbu.CreateBackUp(filename);
                ViewBag.nuevoRespaldo = "Backup creado" + filename;
                return MenuRespaldo();
            }
            catch (Exception e)
            {
                ViewBag.errorRespaldo = "Error : No se pudo crear el respaldo " + e;
                return View();
            }
        }
    }
}