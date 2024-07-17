using ClosedXML.Excel;
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
    public class EquipmentsController : Controller
    {
        // GET: Equipments
        Equipments equipments = new Equipments();
        Users users = new Users();
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
                //var colCount = rangeColLast - rangeColFirst;
                var colCount = rangeColLast - rangeColFirst;
                for (int rowNum = rangeRowFirst; rowNum <= rangeRowLast; rowNum++)
                {
                    List<string> colValues = new List<string>();
                    for (int col = 1; col <= rangeColLast; col++)
                    {
                        colValues.Add(worksheet.Row(rowNum).Cell(col).Value.ToString());
                    }
                    tbl.Rows.Add(colValues.ToArray());
                }

                //Logger("finished creating datatable");
                return tbl;
            }
        }
        public JsonResult import_equipments(HttpPostedFileBase archivo, string hoja)
        {
            string msg = "";
            DataTable TabEquipments = new DataTable();
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

                TabEquipments.Columns.Add(new DataColumn("id_equipment", typeof(int)));
                TabEquipments.Columns.Add(new DataColumn("equipment", typeof(string)));
                TabEquipments.Columns.Add(new DataColumn("department", typeof(string)));
                TabEquipments.Columns.Add(new DataColumn("akz", typeof(string)));
                TabEquipments.Columns.Add(new DataColumn("is_enable", typeof(int)));

                foreach (DataRow fila in DatosExcel.Rows)
                {
                    string equipment = fila["Equipo"].ToString();
                    string department = fila["Departamento"].ToString();
                    string akz = fila["AKZ"].ToString();

                    if (!string.IsNullOrEmpty(equipment))
                    {
                        equipment = equipment == "" || equipment == null || equipment == "#" ? null : equipment;
                        department = department == "" || department == null || department == "#" ? null : department;
                        akz = akz == "" || akz == null || akz == "#" ? null : akz;

                        TabEquipments.Rows.Add(1, equipment, department, akz,1);
                    }
                }
                equipments.insert_equipments(TabEquipments);
                Directory.Delete(path, true);
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