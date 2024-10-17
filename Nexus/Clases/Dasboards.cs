using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace Nexus.Clases
{
    public class Dasboards
    {
        public DataTable get_Dashboard_info(int id_dashboard)
        {
            var msg = "";
            var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("get_Dashboard_info", conn);
                    cmd.Parameters.Add("@id_dashboard", SqlDbType.Int).Value = id_dashboard;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                if (dt.Columns[0].ToString() == "ErrorNumber")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        msg = "Error Number: " + row[0].ToString() + ", Severity: " + row[1].ToString() + ", State: " + row[2].ToString() +
                                ", Procedure: " + row[3].ToString() + " Line: " + row[4].ToString() + " Message: " + row[5].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.Registrar(this, e.ToString(), "SQL: " + msg);
            }
            return dt;
        }
        public DataTable get_list_dashboard_type(int is_enable)
        {
            var msg = "";
            var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("get_list_dashboard_type", conn);
                    cmd.Parameters.Add("@is_enable", SqlDbType.Int).Value = is_enable;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                if (dt.Columns[0].ToString() == "ErrorNumber")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        msg = "Error Number: " + row[0].ToString() + ", Severity: " + row[1].ToString() + ", State: " + row[2].ToString() +
                                ", Procedure: " + row[3].ToString() + " Line: " + row[4].ToString() + " Message: " + row[5].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.Registrar(this, e.ToString(), "SQL: " + msg);
            }
            return dt;
        }
        public DataTable get_dashboard_list(int is_enable)
        {
            var msg = "";
            var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("get_dashboard_list", conn);
                    cmd.Parameters.Add("@is_enable", SqlDbType.Int).Value = is_enable;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                if (dt.Columns[0].ToString() == "ErrorNumber")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        msg = "Error Number: " + row[0].ToString() + ", Severity: " + row[1].ToString() + ", State: " + row[2].ToString() +
                                ", Procedure: " + row[3].ToString() + " Line: " + row[4].ToString() + " Message: " + row[5].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.Registrar(this, e.ToString(), "SQL: " + msg);
            }
            return dt;
        }
        public int get_TotalPag_dashboards(string id_department, string title, string id_dashboard_type,string is_enable, int PageSize)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString);
            int res = 0;
            string msg = "";
            try
            {
                System.Data.SqlClient.SqlDataReader reader;
                System.Data.SqlClient.SqlCommand sql;
                con.Open();
                sql = new System.Data.SqlClient.SqlCommand();
                sql.CommandText = "select dbo.get_TotalPag_dashboards(" + (id_department == "" || id_department == null ? (object)DBNull.Value + "null" : ("'" + id_department + "'"))+
                    "," + (title == "" || title == null ? (object)DBNull.Value + "null" : ("'" + title + "'")) +
                    "," + (id_dashboard_type == "" || id_dashboard_type == null ? (object)DBNull.Value + "null" : ("'" + id_dashboard_type + "'")) +
                    "," + (is_enable == "" || is_enable == null ? (object)DBNull.Value + "null" : ("'" + is_enable + "'")) +
                    "," + PageSize + ");";
                sql.Connection = con;
                using (reader = sql.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (int.TryParse(reader[0].ToString(), out res))
                        {
                            res = Convert.ToInt32(reader[0]);
                        }
                        else
                        {
                            msg = "No devuelve un valor esperado. Revisar la función en la base de datos.";
                        }
                    }
                    else
                    {
                        msg = "Error en ExecuteReader. Revisar la función en la base de datos.";
                    }
                }
                con.Close();
            }
            catch (Exception e)
            {
                ErrorLogger.Registrar(this, e.ToString(), msg);
            }
            return res;
        }
        public DataTable get_table_dashboards(string id_department,string title,string id_dashboard_type,string is_enable, int PageIndex, int PageSize)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString);
            var msg = "";
            DataTable dt = new DataTable();
            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("get_table_dashboards", con);
                    cmd.Parameters.Add("@id_department", SqlDbType.Int).Value = id_department == "" || id_department == null ? (object)DBNull.Value : id_department;
                    cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = title == "" || title == null ? (object)DBNull.Value : title;
                    cmd.Parameters.Add("@id_dashboard_type", SqlDbType.Int).Value = id_dashboard_type == "" || id_dashboard_type == null ? (object)DBNull.Value : id_dashboard_type;
                    cmd.Parameters.Add("@is_enable", SqlDbType.Int).Value = is_enable == "" || is_enable == null ? (object)DBNull.Value : is_enable;
                    cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = PageIndex;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                if (dt.Columns[0].ToString() == "ErrorNumber")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        msg = "Error Number: " + row[0].ToString() + ", Severity: " + row[1].ToString() + ", State: " + row[2].ToString() +
                                ", Procedure: " + row[3].ToString() + " Line: " + row[4].ToString() + " Message: " + row[5].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.Registrar(this, e.ToString(), msg);
            }
            return dt;
        }
        public string upsert_dashboards(string id_dashboard, string link, string title, string id_department, string code_department, string is_enable, 
            string previus_image, string id_dashboard_type,string guia)
        {
            var msg = "";
            try
            {
                DataTable dt = new DataTable();
                var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("upsert_dashboards", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_dashboard", SqlDbType.Int).Value = id_dashboard == "" || id_dashboard == null ? (object)DBNull.Value : id_dashboard;
                    cmd.Parameters.Add("@link", SqlDbType.VarChar).Value = link;
                    cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = title;
                    cmd.Parameters.Add("@id_department", SqlDbType.Int).Value = id_department;
                    cmd.Parameters.Add("@code_deparment", SqlDbType.VarChar).Value = code_department;
                    cmd.Parameters.Add("@is_enable", SqlDbType.Int).Value = is_enable;
                    cmd.Parameters.Add("@previus_image", SqlDbType.VarChar).Value = previus_image;
                    cmd.Parameters.Add("@id_dashboard_type", SqlDbType.Int).Value = id_dashboard_type;
                    cmd.Parameters.Add("@guia", SqlDbType.VarChar).Value = guia;
 
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() != "guardado")
                    {
                        msg = "Error Number: " + row[0].ToString() + ", Severity: " + row[1].ToString() + ", State: " + row[2].ToString() +
                            ", Procedure: " + row[3].ToString() + " Line: " + row[4].ToString() + " Message: " + row[5].ToString();
                    }
                    else
                    {
                        msg = row[0].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.Registrar(this, e.ToString(), "SQL: " + msg);
            }
            return msg;
        }
        public string get_dashboard_prev_img(string id_dashboard)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString);
            string msg = "";
            try
            {
                System.Data.SqlClient.SqlDataReader reader;
                System.Data.SqlClient.SqlCommand sql;
                con.Open();
                sql = new System.Data.SqlClient.SqlCommand();
                sql.CommandText = "select dbo.get_dashboard_prev_img('" + id_dashboard + "');";
                sql.Connection = con;
                using (reader = sql.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        msg = reader[0].ToString();
                    }
                    else
                    {
                        msg = "Error en ExecuteReader. Revisar la función en la base de datos.";
                    }
                }
                con.Close();
            }
            catch (Exception e)
            {
                ErrorLogger.Registrar(this, e.ToString(), msg);
            }
            return msg;
        }
    }
}