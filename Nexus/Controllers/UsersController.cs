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
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        Users user = new Users();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult get_TotalPag_users(string cwid, string fullname, string department, string is_enable, int NumRegistros = 100)
        {
            return Json(user.get_TotalPag_users(cwid, fullname, department, is_enable, NumRegistros), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_users_table(string cwid, string fullname, string department, string is_enable, int Index = 0, int NumRegistros = 100)
        {
            List<UserModel> list = new List<UserModel>();
            try
            {
                DataTable datos = user.get_users_table(cwid, fullname, department, is_enable, Index, NumRegistros);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new UserModel
                    {
                        RowNumber = Convert.ToInt32(data["RowNumber"]),
                        id_user = Convert.ToInt32(data["ID"]),
                        is_enable = Convert.ToInt32(data["is_enable"]),
                        title = data["title"].ToString(),
                        cwid = data["cwid"].ToString().ToUpper(),
                        department = data["department"].ToString(),
                        email = data["email"].ToString(),
                        fullname = data["fullname"].ToString()
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