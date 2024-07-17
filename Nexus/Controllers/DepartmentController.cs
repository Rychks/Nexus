using Nexus.Clases;
using Nexus.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexus.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        Departments departments = new Departments();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult get_department_list(int Status)
        {
            List<ListModel> list = new List<ListModel>();
            try
            {
                DataTable datos = departments.get_department_list(Status);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new ListModel
                    {
                        ID = Convert.ToInt32(data["id_department"]),
                        Opcion = data["name_department"].ToString(),
                    });
                }
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_department_department_code(string id_department)
        {
            return Json(departments.get_department_department_code(id_department), JsonRequestBehavior.AllowGet);
        }
    }
}