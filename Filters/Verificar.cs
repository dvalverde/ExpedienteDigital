using ExpDigital.Controllers;
using ExpDigital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpDigital.Filters
{
    public class Verificar: ActionFilterAttribute
    {
        private Persona oUsuario;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try {
                base.OnActionExecuting(filterContext);
                oUsuario = (Persona)HttpContext.Current.Session["User"];
                if (oUsuario == null) {
                    if (filterContext.Controller is LoginController == false) {
                        filterContext.HttpContext.Response.Redirect("/Login/Login");
                    }
                }

            }
            catch (Exception e) {
                filterContext.Result = new RedirectResult("~/Login/Login");
            }
            
        }
    }
}