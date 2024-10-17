using Microsoft.SqlServer.Server;
using Nexus.Clases;
using Nexus.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace Nexus.Controllers
{
    public class DashboardTemplatesController : Controller
    {
        DashboardTemplates templates = new DashboardTemplates();
        NoficationModel noti = new NoficationModel();
        AuditTrail audit = new AuditTrail();
        Access access = new Access();
        // GET: DashboardTemplates
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult verify_owner(string id_template)
        {
            string BYTOST = HttpContext.User.Identity.Name;
            return Json(access.verify_owner_template(BYTOST, id_template), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_template_dashboard_list(string id_template)
        {
            List<DashboardModel> list = new List<DashboardModel>();
            try
            {
                string BYTOST = HttpContext.User.Identity.Name;
                DataTable datos = templates.get_template_dashboard_list(id_template);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new DashboardModel
                    {
                        link = data["link"].ToString(),
                        id_dashboard = Convert.ToInt32(data["id_dashboard"]),
                        title = data["title"].ToString(),
                        type = data["id_dashboard_type"].ToString(),
                        previus_image = data["previus_image"].ToString(),
                        guia = data["guia"].ToString()
                    });
                }
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_TotalPag_detail_user_templates(int NumRegistros = 4)
        {
            string BYTOST = HttpContext.User.Identity.Name;
            return Json(templates.get_TotalPag_detail_user_templates(BYTOST,NumRegistros), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_detail_user_templates_table(int Index = 0, int NumRegistros = 4)
        {
            List<TemplatesModel> list = new List<TemplatesModel>();
            try
            {
                string BYTOST = HttpContext.User.Identity.Name;
                DataTable datos = templates.get_detail_user_templates_table(BYTOST, Index, NumRegistros);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new TemplatesModel
                    {
                        RowNumber = Convert.ToInt32(data["RowNumber"]),
                        id_detail = Convert.ToInt32(data["ID"]),
                        id_template = Convert.ToInt32(data["id_template"]),
                        id_user = Convert.ToInt32(data["id_user"]),
                        name_template = data["name_template"].ToString(),
                    });
                }
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_dashboards_templates_public_list(int Status)
        {
            List<ListModel> list = new List<ListModel>();
            try
            {
                string BYTOST = HttpContext.User.Identity.Name;
                DataTable datos = templates.get_dashboards_templates_public_list(BYTOST);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new ListModel
                    {
                        ID = Convert.ToInt32(data["id_template"]),
                        Opcion = data["name_template"].ToString(),
                    });
                }
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult insert_dashboards_templates(string name,string is_private)
        {
            try
            {
                string BYTOST = HttpContext.User.Identity.Name;

                string datos = templates.insert_dashboards_templates(name, is_private, BYTOST);
                int last_id = templates.get_dashboard_templates_last_id();
                templates.insert_detail_user_templates(BYTOST, last_id.ToString());
                if (datos == "saved")
                {
                    noti.Message = "Template creado correctamente";
                    noti.Type = "success";
                    noti.Id = last_id.ToString();
                    audit.insert_AuditTrail(BYTOST, "I", "N/A", "Creación de template" + " " + name, "N/A");
                }
                else
                {
                    noti.Message = "Se produjo un error inesperado al tratar de crear el template";
                    noti.Type = "warning";
                    noti.Error = datos;
                }
            }
            catch (Exception e)
            {
                noti.Message = "Se produjo un error inesperado al tratar de crear el template";
                noti.Type = "warning";
                noti.Error = e.Message;
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(noti, JsonRequestBehavior.AllowGet);
        }
        public JsonResult insert_detail_user_templates(string id_template)
        {
            try
            {
                string BYTOST = HttpContext.User.Identity.Name;

                string datos = templates.insert_detail_user_templates(BYTOST,id_template);
                if (datos == "saved")
                {
                    noti.Message = "Template agregado correctamente";
                    noti.Type = "success";
                }
                else
                {
                    noti.Message = "Se produjo un error inesperado al tratar de agregar el template";
                    noti.Type = "warning";
                    noti.Error = datos;
                }
            }
            catch (Exception e)
            {
                noti.Message = "Se produjo un error inesperado al tratar de agregar el template";
                noti.Type = "warning";
                noti.Error = e.Message;
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(noti, JsonRequestBehavior.AllowGet);
        }
        public JsonResult delete_detail_user_templates(string id_template)
        {
            try
            {
                string BYTOST = HttpContext.User.Identity.Name;

                string datos = templates.delete_detail_user_templates(BYTOST, id_template);
                if (datos == "saved")
                {
                    noti.Message = "Template omitido correctamente";
                    noti.Type = "success";
                }
                else
                {
                    noti.Message = "Se produjo un error inesperado al tratar de omitido el template";
                    noti.Type = "warning";
                    noti.Error = datos;
                }
            }
            catch (Exception e)
            {
                noti.Message = "Se produjo un error inesperado al tratar de omitido el template";
                noti.Type = "warning";
                noti.Error = e.Message;
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(noti, JsonRequestBehavior.AllowGet);
        }
        public JsonResult insert_detail_dashboard_templates(string id_template, string id_dashboard)
        {
            try
            {
                string BYTOST = HttpContext.User.Identity.Name;

                string datos = templates.insert_detail_dashboard_templates(id_template,id_dashboard);
                if (datos == "saved")
                {
                    noti.Message = "Dashboard agregado correctamente";
                    noti.Type = "success";
                }
                else
                {
                    noti.Message = "Se produjo un error inesperado al tratar de agregar el dashboard";
                    noti.Type = "warning";
                    noti.Error = datos;
                }
            }
            catch (Exception e)
            {
                noti.Message = "Se produjo un error inesperado al tratar de agregar el dashboard";
                noti.Type = "warning";
                noti.Error = e.Message;
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(noti, JsonRequestBehavior.AllowGet);
        }

        public JsonResult delete_detail_dashboard_templates(string id_template, string id_dashboard)
        {
            try
            {
                string BYTOST = HttpContext.User.Identity.Name;

                string datos = templates.delete_detail_dashboard_templates(id_template, id_dashboard);
                if (datos == "saved")
                {
                    noti.Message = "Dashboard oimitido correctamente";
                    noti.Type = "success";
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