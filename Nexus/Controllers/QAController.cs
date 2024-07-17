using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office.Word;
using Newtonsoft.Json;
using Nexus.Clases;
using Nexus.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Interop;

namespace Nexus.Controllers
{
    [Authorize]
    public class QAController : Controller
    {
        // GET: QA
        QA qa = new QA();
        Users users = new Users();
        NoficationModel noti = new NoficationModel();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PNO_Vencimiento()
        {
            return View();
        }
        public JsonResult import_pno_vencimiento(HttpPostedFileBase archivo, string hoja)
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

                    string cwid = fila["CWID"].ToString();
                    string last_name = fila["Last Name"].ToString();
                    string fist_name = fila["First Name"].ToString();
                    string super_last_name = fila["Super Last Name"].ToString();
                    string super_first_name = fila["Super First Name"].ToString();
                    string item_ID = fila["Item ID"].ToString();
                    string requiered = fila["Required"].ToString();
                    string departamento = fila[7].ToString();


                    if (!string.IsNullOrEmpty(cwid))
                    {
                        string guardado = qa.insert_pno_vencimiento(cwid, last_name, fist_name, super_last_name, super_first_name, item_ID, requiered, departamento);
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
        public JsonResult get_TotalPag_pno_vencimiento(string cwid, int NumRegistros = 100)
        {
            return Json(qa.get_TotalPag_pno_vencimiento(cwid, NumRegistros), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_pno_vencimiento_table(string number, int Index = 0, int NumRegistros = 100)
        {
            List<Pno_vencimientoModel> list = new List<Pno_vencimientoModel>();
            try
            {
                DataTable datos = qa.get_pno_vencimiento_table(number, Index, NumRegistros);
                foreach (DataRow data in datos.Rows)
                {
                    list.Add(new Pno_vencimientoModel
                    {
                        RowNumber = Convert.ToInt32(data["RowNumber"]),
                        ID = Convert.ToInt32(data["ID"]),
                        cwid = data["cwid"].ToString(),
                        last_name = data["last_name"].ToString(),
                        fist_name = data["fist_name"].ToString(),
                        super_last_name = data["super_last_name"].ToString(),
                        super_first_name = data["super_first_name"].ToString(),
                        item_ID = data["item_ID"].ToString(),
                        requiered = validaFecha(data["requiered"].ToString()),
                        departamento = data["departamento"].ToString(),
                    });
                }
            }
            catch (Exception e)
            {
                Clases.ErrorLogger.Registrar(this, e.ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult import_capas(HttpPostedFileBase archivo, string hoja)
        {
            string msg = "";
            DataTable TabCapas = new DataTable();
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

                
                TabCapas.Columns.Add(new DataColumn("id", typeof(int)));
                TabCapas.Columns.Add(new DataColumn("client", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("deviation_number", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("id_capa", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("activity_start_date", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("workflow_deadline", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("activity_end_date", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("activity_name", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("activity_instance_state", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("recommended_user_id", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("capa_efectivenes", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("capa_justification", typeof(string)));
                TabCapas.Columns.Add(new DataColumn("deviation_type", typeof(string)));
                foreach (DataRow fila in DatosExcel.Rows)
                {
                    string client = fila["Client for Reporting"].ToString();
                    string deviation_number = fila["Deviation Number"].ToString();
                    string Id_capa = fila["CAPA ID Reference Ob"].ToString();
                    string start_date = fila["Activity Start Date"].ToString();
                    string deadline = fila["Worklow Deadline Dat"].ToString();
                    string end_date = fila["Activity End Date"].ToString();
                    string activity_name = fila["ACTIVITYNAME"].ToString();
                    string state = fila["Activity Instance State"].ToString();
                    string user_id = fila["RECOMMENDEDUSERID"].ToString();
                    string efectivenes = fila["CAPA- Effectivenes c"].ToString();
                    string justification = fila["CAPA- Justification for effectiveness check required"].ToString();
                    string type = fila["Deviation type Devia"].ToString();


                    if (!string.IsNullOrEmpty(Id_capa) || user_id != "#" || user_id != "")
                    {
                        if (deviation_number != "**initDevNumber**" && Id_capa != "**initCapaNumber**")
                        {
                            client = client == "" || client == null || client == "#" ? null : client;
                            deviation_number = deviation_number == "" || deviation_number == null || deviation_number == "#" ? null : deviation_number;
                            Id_capa = Id_capa == "" || Id_capa == null || Id_capa == "#" ? null : Id_capa;
                            start_date = start_date == "" || start_date == null || start_date == "#" ? null : start_date;
                            deadline = deadline == "" || deadline == null || deadline == "#" ? null : deadline;
                            end_date = end_date == "" || end_date == null || end_date == "#" ? null : end_date;
                            activity_name = activity_name == "" || activity_name == null || activity_name == "#" ? null : activity_name;
                            state = state == "" || state == null || state == "#" ? null : state;
                            user_id = user_id == "" || user_id == null || user_id == "#" ? null : user_id;
                            efectivenes = efectivenes == "" || efectivenes == null || efectivenes == "#" ? null : efectivenes;
                            justification = justification == "" || justification == null || justification == "#" ? null : justification;
                            type = type == "" || type == null || type == "#" ? null : type;

                            TabCapas.Rows.Add(1,client, deviation_number, Id_capa, start_date, deadline, end_date, activity_name, state, user_id, efectivenes, justification, type);
                            
                        }
                    }
                }
                qa.INSERT_TABLE_CAPAS(TabCapas);
                Directory.Delete(path, true);
                registrarUsuarios(TabCapas);
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
        private void registrarUsuarios(DataTable Usuarios)
        {
            int numDatos = Usuarios.Rows.Count;
            int datosRegistrados = 1;
            foreach (DataRow fila in Usuarios.Rows)
            {

                string deviation_number = fila["deviation_number"].ToString();
                string Id_capa = fila["id_capa"].ToString();             
                string user_id = fila["recommended_user_id"].ToString();

                if (!string.IsNullOrEmpty(Id_capa) || user_id != "#" || user_id != "")
                {
                    if (deviation_number != "**initDevNumber**")
                    {
                        try
                        {
                            ProfileUser user = new ProfileUser(user_id);
                            string guardado = users.insert_users(user.nombre, user_id, user.apellido, user.NombreCompleto, user.correo, user.company, user.department, user.title, user.title);
                            if (guardado == "saved")
                            {
                                datosRegistrados++;
                            }
                        }
                        catch(Exception e)
                        {

                        }
                        
                    }

                }
            }
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