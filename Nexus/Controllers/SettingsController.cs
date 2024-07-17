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
    }
}