using ExpDigital.Filters;
using ExpDigital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpDigital.Controllers
{
    public class AgregarUsuarioController : Controller
    {

        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();
        // GET: AgregarUsuarios
        public ActionResult CreateAgregarUsuario()
        {
            return View();
        }

        // POST: Persona/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAgregarUsuario([Bind(Include = "ID,correoElectronico,password")] Persona persona)
        {
            try
            {
                Persona per = new Persona();
                per.correoElectronico = persona.correoElectronico;
                per.password = Encriptar.GetSHA256(persona.password);
                db.Personas.Add(per);
                db.SaveChanges();

                ViewBag.usuarioAgregado = persona.correoElectronico;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorUsuario = "Error : No se pudo agregar el artículo " + e;
                return View();
            }

        }

    }
}