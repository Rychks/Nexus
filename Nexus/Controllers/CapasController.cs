using ClosedXML.Excel;
using Nexus.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexus.Controllers
{
    [Authorize]
    public class CapasController : Controller
    {
        // GET: Capas
        NoficationModel noti = new NoficationModel();
        Capas capas = new Capas();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult import_capas(HttpPostedFileBase archivo, string hoja)
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
                    string number = fila["Number"].ToString();
                    string type = fila["Type"].ToString();
                    string fecha1 = fila["Start date"].ToString();
                    string start_date = Convert.ToDateTime(fila["Start date"]).ToString("yyyyMMdd"); //DateTime.ParseExact(fila["Start date"].ToString(), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture); 
                    string entry_date = Convert.ToDateTime(fila["Entry date (CET)"]).ToString("yyyyMMdd");
                    string short_description = fila["Short description"].ToString();
                    string deliverables = fila["CAPA deliverables"].ToString();
                    string client = fila["Client"].ToString();
                    string deadline_final_aprobal = Convert.ToDateTime(fila["Deadline for final approval"]).ToString("yyyyMMdd");
                    string workflow_status = fila["Workflow status"].ToString();
                    string success = fila["Success"].ToString();
                    string result = fila["Result"].ToString();
                    string remark = fila["Remark"].ToString();
                    string finishdate = fila["Workflow-Finishdate"].ToString(); 
                    string origin = fila["Origin"].ToString();
                    int number_extensions = Convert.ToInt32(fila["Number of due date extension requests"]);
                    string effectiveness_check = fila["Effectiveness check required?"].ToString();
                    string justification = fila["Justification for no effectivness check required"].ToString();
                    string implementation_date = fila["Date of implementation"].ToString();
                    string planned_date = Convert.ToDateTime(fila["Planned implementation date"]).ToString("yyyyMMdd");
                    string v_et = fila["V/ET"].ToString();
                    string evento = fila["EVENTO"].ToString();

                    if (!string.IsNullOrEmpty(number))
                    {
                        string guardado = capas.insert_capas(number,type,start_date,entry_date,short_description,deliverables,client,deadline_final_aprobal,workflow_status,success,
                            result, remark, finishdate, origin, number_extensions, effectiveness_check, justification, implementation_date, planned_date,v_et,evento);
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
        public JsonResult get_TotalPag_capas(string number, int NumRegistros = 100)
        {
            return Json(capas.get_TotalPag_capas(number, NumRegistros), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_capas_table(string number, int Index = 0, int NumRegistros = 100)
        {
            List<CapaModule> list = new List<CapaModule>();
            try
            {
                DataTable datos = capas.get_capas_table(number, Index, NumRegistros);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new CapaModule
                    {
                        RowNumber = Convert.ToInt32(data["RowNumber"]),
                        ID = Convert.ToInt32(data["ID"]),
                        number = data["number"].ToString(),
                        type = data["type"].ToString(),
                        start_date = validaFecha(data["start_date"].ToString()),
                        entry_date = validaFecha(data["entry_date"].ToString()),
                        short_description = data["short_description"].ToString(),
                        deliverables = data["deliverables"].ToString(),
                        client = data["client"].ToString(),
                        deadline_final_aprobal = validaFecha(data["deadline_final_aprobal"].ToString()),
                        workflow_status = data["workflow_status"].ToString(),
                        success = data["success"].ToString(),
                        result = data["result"].ToString(),
                        remark = data["remark"].ToString(),
                        finishdate = validaFecha(data["finishdate"].ToString()),
                        origin = data["origin"].ToString(),
                        number_extensions = data["number_extensions"].ToString(),
                        effectiveness_check = data["effectiveness_check"].ToString(),
                        justification = data["justification"].ToString(),
                        implementation_date = validaFecha(data["implementation_date"].ToString()),
                        planned_date = validaFecha(data["planned_date"].ToString()),
                        v_et = data["v_et"].ToString(),
                        evento = data["evento"].ToString(),                       
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