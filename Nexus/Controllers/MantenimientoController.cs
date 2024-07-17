using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Nexus.Clases;
using Nexus.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexus.Controllers
{
    [Authorize]
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        Mantenimiento manto = new Mantenimiento();
        NoficationModel noti = new NoficationModel();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Avizos_Z1()
        {
            return View();
        }
        public ActionResult Gastos_Refacciones()
        {
            return View();
        }

        public JsonResult import_avisos_Z1(HttpPostedFileBase archivo, string hoja)
        {
            string msg = "";
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
                DataTable DatosExcel = GetDataTableFromExcel(filename);
                int numDatos = DatosExcel.Rows.Count;
                int datosRegistrados = 1;
                foreach (DataRow fila in DatosExcel.Rows)
                {

                    string centro_costos = fila["Centro coste"].ToString();
                    string ubicacion_tecnica = fila["Ubicac.técnica"].ToString();
                    string aviso = fila["Aviso"].ToString();
                    string fecha_aviso = fila["Fecha de aviso"].ToString();
                    string clase_aviso = fila["Clase de aviso"].ToString();
                    string descripcion = fila["Descripción"].ToString();
                    string orden = fila["Orden"].ToString();
                    string modificado_por = fila["Modificado por"].ToString();
                    string fecha_modificado = fila["Modificado el"].ToString();
                    string status_sistema = fila["Status sistema"].ToString();
                    string status_usuario = fila["Status usuario"].ToString();
                    string clase_trabajo = fila["Clase trabajo"].ToString();
                    string duracion_parada = fila["Duración parada"].ToString();
                    string prioridad = fila["Prioridad"].ToString();
                    string inicio_deseado = fila["Inicio deseado"].ToString();
                    string hora_inicio_averia = fila["Hora in.avería"].ToString();
                    string hora_fin_averia = fila["Hora fin avería"].ToString();
                    string hora_inicio_des = fila["Hora inic.des."].ToString();
                    string inicio_averia = fila["Inicio avería"].ToString();
                    string fin_averia = fila["Fin de avería"].ToString();
                    string denominacion = fila["Denominación"].ToString();
                    string equipo = fila["Equipo"].ToString();

                    if (!string.IsNullOrEmpty(aviso))
                    {
                        string guardado = manto.insert_avisos_z1(centro_costos, ubicacion_tecnica, aviso, fecha_aviso, clase_aviso, descripcion, orden, modificado_por, fecha_modificado,
                    status_sistema, status_usuario, clase_trabajo, duracion_parada, prioridad, inicio_deseado, hora_inicio_averia, hora_fin_averia, hora_inicio_des,
                    inicio_averia, fin_averia, denominacion, equipo);
                        if (guardado == "saved")
                        {
                            datosRegistrados++;
                        }
                        else
                        {
                            Clases.ErrorLogger.Registrar(this, guardado);
                            break;
                        }
                    }
                }
                if (datosRegistrados == numDatos) { msg = "guardado"; }

                if (msg == "guardado")
                {
                    noti.Message = "Information Saved Correctly";
                    noti.Type = "success";
                }
                else
                {
                    noti.Message = "An error occurred while trying to save the Information";
                    noti.Type = "warning";
                }
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
                var colCount = rangeColLast - rangeColFirst;
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
        public JsonResult get_TotalPag_avisos_z1(string number, int NumRegistros = 100)
        {
            return Json(manto.get_TotalPag_avisos_z1(number, NumRegistros), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_avisos_z1_table(string number, int Index = 0, int NumRegistros = 100)
        {
            List<Avisos_Z1Model> list = new List<Avisos_Z1Model>();
            try
            {
                DataTable datos = manto.get_avisos_z1_table(number, Index, NumRegistros);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new Avisos_Z1Model
                    {
                        RowNumber = Convert.ToInt32(data["RowNumber"]),
                        ID = Convert.ToInt32(data["ID"]),
                        centro_costos = data["centro_costos"].ToString(),
                        ubicacion_tecnica = data["ubicacion_tecnica"].ToString(),
                        aviso = data["aviso"].ToString(),
                        fecha_aviso = validaFecha(data["fecha_aviso"].ToString()),
                        clase_aviso = data["clase_aviso"].ToString(),
                        descripcion = data["descripcion"].ToString(),
                        orden = data["orden"].ToString(),
                        modificado_por = data["modificado_por"].ToString(),
                        status_sistema = data["status_sistema"].ToString(),
                        status_usuario = data["status_usuario"].ToString(),
                        clase_trabajo = data["clase_trabajo"].ToString(),
                        duracion_parada = data["duracion_parada"].ToString(),
                        prioridad = data["prioridad"].ToString()
                    });
                }
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private string validaFecha(string fecha)
        {
            string fechaAux = "";

            if (!string.IsNullOrEmpty(fecha))
            {
                fechaAux = Convert.ToDateTime(fecha).ToString("dd.MM.yyyy");
            }
            return fechaAux;
        }
    }
}