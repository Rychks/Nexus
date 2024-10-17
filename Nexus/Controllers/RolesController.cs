using DocumentFormat.OpenXml.Spreadsheet;
using Nexus.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexus.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        // GET: Roles
        Access access = new Access();
        Roles rol = new Roles();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult verificarAcceso(int funcionId)
        {
            string cwidUsuario = HttpContext.User.Identity.Name;
            int id_rol = rol.get_id_rol_by_cwid(cwidUsuario);

            return Json(access.verify_access(id_rol.ToString(), funcionId), JsonRequestBehavior.AllowGet);
        }
    }
}