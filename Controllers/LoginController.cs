using ExpDigital.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using ExpDigital.Models;
namespace ExpDigital.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logoff()
        {
            Session["User"] = null;
            Session["ID"] = null;
            Session["Email"] = null;
            Session["Nombre"] = null;
            Session["Apellido"] = null;
            Session["Rol"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(String User, String Pass) {
            try {
                using (Models.ExpedienteDigitalEntities db = new Models.ExpedienteDigitalEntities()) {

                    String pass = Encriptar.GetSHA256(Pass).Trim();

                    var oUser = (from d in db.Personas where d.correoElectronico == User.Trim() && d.password == pass.Trim() select d).FirstOrDefault();
                    if (oUser == null) {
                        ViewBag.Error = "Usuario o Contraseña Invalido";
                        return View();
                    }

                    Session["User"] = oUser;
                    Session["ID"] = oUser.ID;
                    Session["Email"] = oUser.correoElectronico;
                    Session["Nombre"] = oUser.nombre;
                    Session["Apellido"] = oUser.apellido1;
                    if(oUser.rol == 1)
                    {
                        Session["Rol"] = oUser.rol;
                    }
                    

                    var lista = (from d in db.Personas where d.correoElectronico != "algo imposible" select d).ToList();
                    var str = lista.ToString();
                }
                if (Session["Nombre"] == null)
                {
                    //si no tengo datos del usuario los tengo que enviar
                    return RedirectToAction("CreatePersona", "Persona");
                }
                else {
                  
                    return RedirectToAction("Index", "Home");
                }
                 
            }
            catch (Exception e) {
                ViewBag.Error = e.Message;
                return View();
            }

        }
    }
}