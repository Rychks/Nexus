using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Nexus.Clases
{
    public class QA
    {
        public string insert_pno_vencimiento(string cwid, string last_name, string fist_name, string super_last_name, string super_first_name,
        string item_ID, string requiered, string departamento)
        {
            var msg = "";
            try
            {
                DataTable dt = new DataTable();
                var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("insert_pno_vencimiento", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@cwid", SqlDbType.VarChar).Value = cwid;
                    cmd.Parameters.Add("@last_name", SqlDbType.VarChar).Value = last_name;
                    cmd.Parameters.Add("@fist_name", SqlDbType.VarChar).Value = fist_name;
                    cmd.Parameters.Add("@super_last_name", SqlDbType.VarChar).Value = super_last_name;
                    cmd.Parameters.Add("@super_first_name", SqlDbType.VarChar).Value = super_first_name;
                    cmd.Parameters.Add("@item_ID", SqlDbType.VarChar).Value = item_ID;
                    cmd.Parameters.Add("@requiered", SqlDbType.VarChar).Value = requiered == "" || requiered == null ? (object)DBNull.Value : Convert.ToDateTime(requiered).ToString("yyyyMMdd");
                    cmd.Parameters.Add("@departamento", SqlDbType.VarChar).Value = departamento;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() != "saved")
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
        public int get_TotalPag_pno_vencimiento(string cwid, int PageSize)
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
                sql.CommandText = "select dbo.get_TotalPag_pno_vencimiento(" + (cwid == "" || cwid == null ? (object)DBNull.Value + "null" : ("'" + cwid + "'")) +
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
        public DataTable get_pno_vencimiento_table(string cwid, int PageIndex, int PageSize)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString);
            var msg = "";
            DataTable dt = new DataTable();
            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("get_pno_vencimiento_table", con);
                    cmd.Parameters.Add("@cwid", SqlDbType.VarChar).Value = cwid == "" || cwid == null ? (object)DBNull.Value : cwid;
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
        public string insert_capasV2(string client,string deviation_number,string id_capa,string start_date,string deadline,string end_date,string activity_name,
            string state,string user,string user_id,string efectivenes,string justification,string type,string mail,string department, string area)
        {
            var msg = "";
            try
            {
                DataTable dt = new DataTable();
                var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("insert_capasV2", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@client", SqlDbType.VarChar).Value = client == "" || client == null || client == "#" ? (object)DBNull.Value : client;
                    cmd.Parameters.Add("@deviation_number", SqlDbType.VarChar).Value = deviation_number == "" || deviation_number == null || deviation_number == "#" ? (object)DBNull.Value : deviation_number;
                    cmd.Parameters.Add("@id_capa", SqlDbType.VarChar).Value = id_capa == "" || id_capa == null || id_capa == "#" ? (object)DBNull.Value : id_capa;
                    cmd.Parameters.Add("@start_date", SqlDbType.VarChar).Value = start_date == "" || start_date == null || start_date == "#" ? (object)DBNull.Value : Convert.ToDateTime(start_date).ToString("yyyyMMdd");
                    cmd.Parameters.Add("@deadline", SqlDbType.VarChar).Value = deadline == "" || deadline == null || deadline == "#" ? (object)DBNull.Value : Convert.ToDateTime(deadline).ToString("yyyyMMdd");
                    cmd.Parameters.Add("@end_date", SqlDbType.VarChar).Value = end_date == "" || end_date == null || end_date == "#" ? (object)DBNull.Value : Convert.ToDateTime(end_date).ToString("yyyyMMdd");
                    cmd.Parameters.Add("@activity_name", SqlDbType.VarChar).Value = activity_name == "" || activity_name == null || activity_name == "#" ? (object)DBNull.Value : activity_name;
                    cmd.Parameters.Add("@state", SqlDbType.VarChar).Value = state == "" || state == null || state == "#" ? (object)DBNull.Value : state;
                    cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = user == "" || user == null || user == "#" ? (object)DBNull.Value : user;
                    cmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = user_id == "" || user_id == null || user_id == "#" ? (object)DBNull.Value : user_id;
                    cmd.Parameters.Add("@efectivenes", SqlDbType.VarChar).Value = efectivenes == "" || efectivenes == null || efectivenes == "#" ? (object)DBNull.Value : efectivenes;
                    cmd.Parameters.Add("@justification", SqlDbType.VarChar).Value = justification == "" || justification == null || justification == "#" ? (object)DBNull.Value : justification;
                    cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = type == "" || type == null || type == "#" ? (object)DBNull.Value : type;
                    cmd.Parameters.Add("@mail", SqlDbType.VarChar).Value = mail == "" || mail == null || mail == "#" ? (object)DBNull.Value : mail;
                    cmd.Parameters.Add("@department", SqlDbType.VarChar).Value = department == "" || department == null || department == "#" ? (object)DBNull.Value : department;
                    cmd.Parameters.Add("@area", SqlDbType.VarChar).Value = area == "" || area == null || area == "#" ? (object)DBNull.Value : area;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() != "saved")
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

        public string INSERT_TABLE_CAPAS(DataTable TabCapas)
        {
            var msg = "";
            try
            {
                DataTable dt = new DataTable();
                var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("INSERT_TABLE_CAPAS", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TabCapas", TabCapas);
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
    }
}