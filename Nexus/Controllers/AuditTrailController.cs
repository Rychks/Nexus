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
    public class AuditTrailController : Controller
    {
        // GET: AuditTrail
        AuditTrail audit = new AuditTrail();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult get_TotalPag_AuditTrail(string Fecha1, string Fecha2, string Usuario, string Accion, int Index = 0, int NumRegistros = 100)
        {
            int TotalPaginas = 0;
            try
            {
                TotalPaginas = audit.get_TotalPag_AuditTrail(Fecha1, Fecha2, Usuario, Accion, NumRegistros);
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(TotalPaginas);
        }

        public JsonResult get_audittrail_table(string Fecha1, string Fecha2, string Usuario, string Accion, int Index = 0, int NumRegistros = 100)
        {
            List<AuditTrailModel> list = new List<AuditTrailModel>();
            try
            {
                DataTable datos = audit.get_audittrail_table(Fecha1, Fecha2, Usuario, Accion, Index, NumRegistros);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new AuditTrailModel
                    {
                        RowNumber = data["RowNumber"].ToString(),
                        Fecha = data["Fecha"].ToString(),
                        Usuario = data["Usuario"].ToString(),
                        Accion = data["Accion"].ToString(),
                        Anterior = data["Anterior"].ToString(),
                        Actual = data["Actual"].ToString(),
                        Justificacion = data["Justificacion"].ToString()
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