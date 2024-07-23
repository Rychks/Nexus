using DocumentFormat.OpenXml.Spreadsheet;
using Nexus.Clases;
using Nexus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexus.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        // GET: Settings
        Settings settings = new Settings();
        Clases.Users users = new Clases.Users();
        Dasboards dasboards = new Dasboards();
        NoficationModel noti = new NoficationModel();
        AuditTrail audit = new AuditTrail();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult get_dashboard_list()
        {
            int Id_user = users.get_user_id_by_cwid(HttpContext.User.Identity.Name);
                List <DashboardModel> list = new List<DashboardModel>();
            try
            {
                DataTable datos = settings.get_user_settings_dashboards_list(Id_user);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new DashboardModel
                    {
                        link = data["link"].ToString(),
                        id_dashboard = Convert.ToInt32(data["id_dashboard"]),
                        title = data["title"].ToString(),
                        type = data["id_dashboard_type"].ToString(),
                        previus_image = data["previus_image"].ToString(),
                    });
                }
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult insert_user_settings(string id_dashboard)
        {
            try
            {
                string BYTOST = HttpContext.User.Identity.Name;
                
                string datos = settings.insert_user_settings(BYTOST, id_dashboard);
                if (datos == "saved")
                {
                    noti.Message = "Dashboard añadido a su lista";
                    noti.Type = "success";
                    audit.insert_AuditTrail(BYTOST, "I", "N/A", "Se agrega dashboard", "N/A");
                }
                else
                {
                    noti.Message = "Se produjo un error inesperado al tratar de añadir el dashboard";
                    noti.Type = "warning";
                    noti.Error = datos;
                }
            }
            catch (Exception e)
            {
                noti.Message = "Se produjo un error inesperado al tratar de añador el dashboard";
                noti.Type = "warning";
                noti.Error = e.Message;
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(noti, JsonRequestBehavior.AllowGet);
        }
        public JsonResult delete_user_settings(string id_dashboard)
        {
            try
            {
                string BYTOST = HttpContext.User.Identity.Name;

                string datos = settings.delete_user_settings(BYTOST, id_dashboard);
                if (datos == "saved")
                {
                    noti.Message = "Dashboard omitido de su lista";
                    noti.Type = "success";
                    audit.insert_AuditTrail(BYTOST, "I", "N/A", "Se agrega dashboard", "N/A");
                }
                else
                {
                    noti.Message = "Se produjo un error inesperado al tratar de omitir el dashboard";
                    noti.Type = "warning";
                    noti.Error = datos;
                }
            }
            catch (Exception e)
            {
                noti.Message = "Se produjo un error inesperado al tratar de omitir el dashboard";
                noti.Type = "warning";
                noti.Error = e.Message;
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(noti, JsonRequestBehavior.AllowGet);
        }
    }
}