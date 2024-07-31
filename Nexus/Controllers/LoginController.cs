using Nexus.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Nexus.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        Users users = new Users();
        AuditTrail auditTrail = new AuditTrail();
        [AllowAnonymous]
        public ActionResult Index(string Mensaje, string Tipo)
        {
            if (!string.IsNullOrEmpty(Mensaje))
            {
                ViewBag.Mensaje = Mensaje;
                ViewBag.Tipo = Tipo;
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult IniciarSesion(string BYTOST, string ZNACKA)
        {
            DateTime Registro = DateTime.Now;

            bool comprobar = users.autenticate_user(BYTOST, ZNACKA);
            if (comprobar)
            {
                int estatus = users.validate_user(BYTOST);
                if (estatus == 404)
                {
                    ProfileUser user = new ProfileUser(BYTOST);
                    string guardado = users.insert_users(user.nombre, BYTOST, user.apellido, user.NombreCompleto, user.correo, user.company, user.department, user.title, user.title);
                    FormsAuthentication.SetAuthCookie(BYTOST.ToUpper(), false);
                    auditTrail.insert_AuditTrail(BYTOST, "I", "N/A", "Login", "N/A");
                    return Redirect("~/Home/Index");
                }
                else if(estatus == 303)
                {
                    ViewBag.Mensaje = "The user has been deactivated";
                    ViewBag.Tipo = "warning";
                    auditTrail.insert_AuditTrail(BYTOST, "I", "N/A", "Failed Login Attempt", "The user has been deactivated");
                    return RedirectToAction("Index", new { Mensaje = ViewBag.Mensaje, Tipo = ViewBag.Tipo });
                }
                else
                {       
                    FormsAuthentication.SetAuthCookie(BYTOST.ToUpper(), false);
                    auditTrail.insert_AuditTrail(BYTOST, "I", "N/A", "Login", "N/A");
                    return Redirect("~/Home/Index");
                } 
            }
            else
            {
                ViewBag.Mensaje = "Wrong password";
                ViewBag.Tipo = "warning";
                auditTrail.insert_AuditTrail(BYTOST, "I", "N/A", "Failed Login Attempt", "Wrong password");
                return RedirectToAction("Index", new { Mensaje = ViewBag.Mensaje, Tipo = ViewBag.Tipo });
            }

            //int estatus = users.validate_user(BYTOST);
            //if (estatus == 200)
            //{
            //    bool comprobar = users.autenticate_user(BYTOST, ZNACKA);
            //    if (comprobar)
            //    {
            //        FormsAuthentication.SetAuthCookie(BYTOST.ToUpper(), false);
            //        auditTrail.insert_AuditTrail(BYTOST, "I", "N/A", "Login", "N/A");
            //        return Redirect("~/Home/Index");
            //    }
            //    else
            //    {
            //        ViewBag.Mensaje = "Wrong password";
            //        ViewBag.Tipo = "warning";
            //        auditTrail.insert_AuditTrail(BYTOST, "I", "N/A", "Failed Login Attempt", "Wrong password");
            //        return RedirectToAction("Index", new { Mensaje = ViewBag.Mensaje, Tipo = ViewBag.Tipo });
            //    }
            //}
            //else if (estatus == 204)
            //{
            //    ViewBag.Mensaje = "The user has been deactivated";
            //    ViewBag.Tipo = "warning";
            //    auditTrail.insert_AuditTrail(BYTOST, "I", "N/A", "Failed Login Attempt", "The user has been deactivated");
            //    return RedirectToAction("Index", new { Mensaje = ViewBag.Mensaje, Tipo = ViewBag.Tipo });
            //}
            //else if (estatus == 404)
            //{
            //    ProfileUser user = new ProfileUser(BYTOST);
            //    string guardado = users.insert_users(user.nombre, BYTOST, user.apellido, user.NombreCompleto, user.correo, user.company, user.department, user.title, user.title);

            //    ViewBag.Mensaje = "We have trouble finding the user. Please verify the user";
            //    ViewBag.Tipo = "danger";
            //    auditTrail.insert_AuditTrail(BYTOST, "I", "N/A", "Failed Login Attempt", "The user is not registered");
            //    return RedirectToAction("Index", new { Mensaje = ViewBag.Mensaje, Tipo = ViewBag.Tipo });
            //}
            //return RedirectToAction("Error", new { Mensaje = ViewBag.Mensaje, Tipo = ViewBag.Tipo });
        }
        public ActionResult Error(string Mensaje, string Tipo)
        {
            if (!string.IsNullOrEmpty(Mensaje))
            {
                ViewBag.Mensaje = Mensaje;
                ViewBag.Tipo = Tipo;
            }
            return View();
        }
        [Authorize]
        public void CerrarSesion()
        {
            DateTime Registro = DateTime.Now;
            string CWID = HttpContext.User.Identity.Name.ToUpper();
            auditTrail.insert_AuditTrail(CWID, "M", "Login", "Logout", "N/A");
            System.Web.Security.FormsAuthentication.SignOut();
            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        }
        [Authorize]
        public void CerrarSesionInactividad()
        {
            DateTime Registro = DateTime.Now;
            string CWID = HttpContext.User.Identity.Name.ToUpper();
            auditTrail.insert_AuditTrail(CWID, "M", "Login", "Logout", "Logging out due to inactivity");
            System.Web.Security.FormsAuthentication.SignOut();
            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        }
        public ActionResult Login_Index()
        {
            return View();
        }
    }
}