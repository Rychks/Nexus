using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Win32;
using Nexus.Clases;
using Nexus.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.Mvc;

namespace Nexus.Controllers
{
    [Authorize]
    public class DashboardsController : Controller
    {
        // GET: Dashboards
        Dasboards dasboards = new Dasboards();
        NoficationModel noti = new NoficationModel();
        AuditTrail audit = new AuditTrail();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult upsert_dashboard(string id_dashboard, string link, string title, string id_department, string code_department, string is_enable,
            HttpPostedFileBase img,HttpPostedFileBase guia, string id_dashboard_type)
        {
            try
            {
                string filename = "default.png";
                string guiaFile = string.Empty;
                string BYTOST = HttpContext.User.Identity.Name;
                if (!string.IsNullOrEmpty(id_dashboard) ) 
                {
                    filename = dasboards.get_dashboard_prev_img(id_dashboard);
                }
               
                if (img != null)
                {
                    //string filename = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    filename = img.FileName;
                    string tipo = Path.GetExtension(img.FileName);
                    //string filepath = "~/Assets/Adjuntos/img/Dashboards" + filename;
                    img.SaveAs(Path.Combine(Server.MapPath("/Assets/img/Dashboards/"), filename));                  
                }

                if (guia != null)
                {
                    //string filename = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    guiaFile = guia.FileName;
                    string tipo = Path.GetExtension(guia.FileName);
                    //string filepath = "~/Assets/Adjuntos/img/Dashboards" + filename;
                    guia.SaveAs(Path.Combine(Server.MapPath("/Assets/guia/Dashboards/"), guiaFile));
                }

                string datos = dasboards.upsert_dashboards(id_dashboard, link, title, id_department, code_department, is_enable, filename, id_dashboard_type, guiaFile);
                if (datos == "guardado")
                {
                    noti.Message = "Dashboard guardado correctamente";
                    noti.Type = "success";
                    audit.insert_AuditTrail(BYTOST, "I", "N/A", "Registro de Dashboard " + title, "N/A");
                }
                else
                {
                    noti.Message = "Se produjo un error inesperado al tratar de gusrdar el archivo";
                    noti.Type = "warning";
                    noti.Error = datos;
                }
            }
            catch (Exception e)
            {
                noti.Message = "Se produjo un error inesperado al tratar de gusrdar el archivo";
                noti.Type = "warning";
                noti.Error = e.Message;
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(noti, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_dashboard_list(int Status)
        {
            List<ListModel> list = new List<ListModel>();
            try
            {
                DataTable datos = dasboards.get_dashboard_list(Status);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new ListModel
                    {
                        ID = Convert.ToInt32(data["id_dashboard"]),
                        Opcion = data["title"].ToString(),
                    });
                }
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_list_dashboard_type(int Status)
        {
            List<ListModel> list = new List<ListModel>();
            try
            {
                DataTable datos = dasboards.get_list_dashboard_type(Status);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new ListModel
                    {
                        ID = Convert.ToInt32(data["id_dashboard_type"]),
                        Opcion = data["dashboard_type"].ToString(),
                    });
                }
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_TotalPag_dashboards(string id_department,string title,string id_dashboard_type,string is_enable, int NumRegistros = 100)
        {
            return Json(dasboards.get_TotalPag_dashboards(id_department,title,id_dashboard_type, is_enable, NumRegistros), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_table_dashboards(string id_department, string title, string id_dashboard_type,string is_enable, int Index = 0, int NumRegistros = 100)
        {
            List<DashboardModel> list = new List<DashboardModel>();
            try
            {
                DataTable datos = dasboards.get_table_dashboards(id_department,title,id_dashboard_type, is_enable, Index, NumRegistros);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new DashboardModel
                    {
                        RowNumber = Convert.ToInt32(data["RowNumber"]),
                        id_dashboard = Convert.ToInt32(data["ID"]),
                        is_enable = Convert.ToInt32(data["is_enable"]),
                        title = data["title"].ToString(),
                        dashboard_type = data["dashboard_type"].ToString(),
                        insert_time_stamp = data["insert_time_stamp"].ToString(),
                        update_time_stamp = data["update_time_stamp"].ToString(),
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

        public JsonResult get_Dashboard_info(int id_dashboard)
        {
            List<DashboardModel> list = new List<DashboardModel>();
            try
            {
                DataTable datos = dasboards.get_Dashboard_info(id_dashboard);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new DashboardModel
                    {
                        id_dashboard = Convert.ToInt32(data["ID"]),
                        title = data["title"].ToString(),
                        code_department = data["code_department"].ToString(),
                        is_enable = Convert.ToInt32(data["is_enable"]),
                        id_department = Convert.ToInt32(data["id_department"]),
                        id_dashboard_type = Convert.ToInt32(data["id_dashboard_type"]),
                        previus_image = data["previus_image"].ToString(),
                        link = data["link"].ToString(),
                        dashboard_type = data["dashboard_type"].ToString(),
                        insert_time_stamp = data["insert_time_stamp"].ToString(),
                        update_time_stamp = data["update_time_stamp"].ToString(),
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
    }
}