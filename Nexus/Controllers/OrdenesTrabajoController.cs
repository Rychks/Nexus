using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Nexus.Clases;
using Nexus.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Web;
using System.Web.Mvc;

namespace Nexus.Controllers
{
    [Authorize]
    public class OrdenesTrabajoController : Controller
    {
        // GET: OrdenesTrabajo
        OrdenesTrabajo OrdenesTrabajo = new OrdenesTrabajo();
        NoficationModel noti = new NoficationModel();
        public ActionResult Index()
        {
            return View();
        }
        public DataTable GetDataTableFromExcel(string filename, string sheetname = "", bool hasHeader = true)
        {
            string rutaArchivo = Server.MapPath("/Atach/") + filename;
            using (var workbook = new XLWorkbook(rutaArchivo))
            {
                IXLWorksheet worksheet;
                if (string.IsNullOrEmpty(sheetname))
                    worksheet = workbook.Worksheets.First();
                else
                    worksheet = workbook.Worksheets.FirstOrDefault(x => x.Name == sheetname);

                var rangeRowFirst = worksheet.FirstRowUsed().RowNumber();
                var rangeRowLast = worksheet.LastRowUsed().RowNumber();
                var rangeColFirst = worksheet.FirstColumnUsed().ColumnNumber();
                var rangeColLast = worksheet.LastColumnUsed().ColumnNumber();

                DataTable tbl = new DataTable();

                for (int col = rangeColFirst; col <= rangeColLast; col++)
                    tbl.Columns.Add(hasHeader ? worksheet.FirstRowUsed().Cell(col).Value.ToString() : $"Column {col}");

                //Logger("started creating datatable");

                rangeRowFirst = rangeRowFirst + (hasHeader ? 1 : 0);
                var colCount = rangeColLast;
                for (int rowNum = rangeRowFirst; rowNum <= rangeRowLast; rowNum++)
                {
                    List<string> colValues = new List<string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        colValues.Add(worksheet.Row(rowNum).Cell(col).Value.ToString());
                    }
                    tbl.Rows.Add(colValues.ToArray());
                }

                //Logger("finished creating datatable");
                return tbl;
            }
        }
        public JsonResult get_TotalPag_ordenes_trabajo(string orden, int NumRegistros = 100)
        {
            return Json(OrdenesTrabajo.get_TotalPag_ordenes_trabajo(orden, NumRegistros), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_ordenes_trabajo_table(string orden, int Index = 0, int NumRegistros = 100)
        {
            List<OrdenesTrabajoModel> list = new List<OrdenesTrabajoModel>();
            try
            {
                DataTable datos = OrdenesTrabajo.get_ordenes_trabajo_table(orden, Index, NumRegistros);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new OrdenesTrabajoModel
                    {
                        RowNumber = Convert.ToInt32(data["RowNumber"]),
                        ID = Convert.ToInt32(data["ID"]),
                        orden = data["orden"].ToString(),
                        aviso = data["aviso"].ToString(),
                        ubicacion_tecnica = data["ubicacion_tecnica"].ToString(),
                        denominacion = data["denominacion"].ToString(),
                        texto_breve = data["texto_breve"].ToString(),
                        status_sistema = data["status_sistema"].ToString(),
                        autor = data["autor"].ToString(),
                        modificado_por = data["modificado_por"].ToString(),
                    });
                }
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult import_ordenes_trabajo(HttpPostedFileBase archivo, string hoja)
        {
            string msg = "";
            DataTable TabOrdenes = new DataTable();
            DataTable DatosExcel = new DataTable();
            try
            {
                string date = DateTime.Now.ToString("ddMMyyhhmmssff");
                string path = Server.MapPath("~/Atach/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                string filename = Guid.NewGuid() + Path.GetExtension(archivo.FileName);
                string filepath = "/Atach/" + filename;
                archivo.SaveAs(Path.Combine(Server.MapPath("/Atach"), filename));
                DatosExcel = GetDataTableFromExcel(filename);


                TabOrdenes.Columns.Add(new DataColumn("id_orden", typeof(int)));
                TabOrdenes.Columns.Add(new DataColumn("orden", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("aviso", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("ubicacion_tecnica", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("denominacion", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("texto_breve", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("status_sistema", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("autor", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("modificado_por", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("prioridad", typeof(int)));
                TabOrdenes.Columns.Add(new DataColumn("fecha_ini", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("fecha_fin", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("fin_programado", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("fecha_inicio_real", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("hora_fin_real", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("fecha_modificacion", typeof(string)));
                TabOrdenes.Columns.Add(new DataColumn("prioridad_text", typeof(string)));

                foreach (DataRow fila in DatosExcel.Rows)
                {

                    string orden = fila["Orden"].ToString();
                    string aviso = fila["Aviso"].ToString();
                    string ubicacion_tecnica = fila["Ubicac.técnica"].ToString();
                    string denominacion = fila["Denominación"].ToString();
                    string texto_breve = fila["Texto breve"].ToString();
                    string status_sistema = fila["Status sistema"].ToString();
                    string autor = fila["Autor"].ToString();
                    string modificado_por = fila["Modificado por"].ToString();
                    string cl_actividad = ""; //fila["Cl.actividad PM"].ToString();
                    string prioridad = fila["Prioridad"].ToString();
                    string fecha_fin = fila["Fe.fin extrema"].ToString();
                    string fin_programado = fila["Fin programado"].ToString();                    
                    string fecha_inicio_real = fila["Fe.inicio real"].ToString();
                    string hora_fin_real = fila["Fin real (hora)"].ToString();
                    string fecha_ini = fila["Fe.inic.extrema"].ToString();
                    string fecha_modificacion = fila["Fecha modific."].ToString();

                    if (!string.IsNullOrEmpty(orden))
                    {
                        if(hora_fin_real == "24:00:00") { hora_fin_real = "00:00:00"; }
                        aviso = aviso == "" || aviso == null ? null : aviso;
                        ubicacion_tecnica = ubicacion_tecnica == "" || ubicacion_tecnica == null ? "" : ubicacion_tecnica.Trim();
                        denominacion = denominacion == "" || denominacion == null ? "" : denominacion;
                        texto_breve = texto_breve == "" || texto_breve == null ? "" : texto_breve;
                        status_sistema = status_sistema == "" || status_sistema == null ? "" : status_sistema;
                        autor = autor == "" || autor == null ? "" : autor.Trim();
                        modificado_por = modificado_por == "" || modificado_por == null ? null : modificado_por.Trim();
                        prioridad = prioridad == "" || prioridad == null ? null : prioridad;
                        fecha_ini = fecha_ini == "" || fecha_ini == null ? null : Convert.ToDateTime(fecha_ini).ToString("yyyyMMdd");
                        fecha_fin = fecha_fin == "" || fecha_fin == null ? null : Convert.ToDateTime(fecha_fin).ToString("yyyyMMdd");
                        fin_programado = fin_programado == "" || fin_programado == null ? null : Convert.ToDateTime(fin_programado).ToString("yyyyMMdd");
                        fecha_inicio_real = fecha_inicio_real == "" || fecha_inicio_real == null ? null : Convert.ToDateTime(fecha_inicio_real).ToString("yyyyMMdd");
                        hora_fin_real = hora_fin_real == "" || hora_fin_real == null ? null : Convert.ToDateTime(hora_fin_real).ToString("HH:mm:ss");
                        fecha_modificacion = fecha_modificacion == "" || fecha_modificacion == null ? null : Convert.ToDateTime(fecha_modificacion).ToString("yyyyMMdd");


                        TabOrdenes.Rows.Add(1, orden, aviso, ubicacion_tecnica, denominacion, texto_breve, status_sistema, autor, modificado_por, prioridad, fecha_ini,
                            fecha_fin, fin_programado, fecha_inicio_real, hora_fin_real, fecha_modificacion, null);
                    }
                }

                OrdenesTrabajo.insert_ordenes_trabajo(TabOrdenes);
            }
            catch (Exception e)
            {
                noti.Message = "An error occurred while trying to save the Information";
                noti.Type = "warning";
                noti.Error = e.Message;
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(noti, JsonRequestBehavior.AllowGet);
        }

    }
}