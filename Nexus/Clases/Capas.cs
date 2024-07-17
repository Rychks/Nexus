using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using System.Diagnostics;
using Nexus.Clases;
using System.Globalization;

namespace Nexus
{
    public class Capas
    {
        public string insert_capas(string number,string type,string start_date, string entry_date,string short_description,string deliverables,string client,string deadline_final_aprobal, string workflow_status,string success,string result,string remark, string finishdate, string origin,int number_extensions,string effectiveness_check,string justification,string implementation_date ,string  planned_date,string v_et,string evento)
        {
            var msg = "";
            try
            {
                DataTable dt = new DataTable();
                var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("insert_capas", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@number", SqlDbType.VarChar).Value = number;
                    cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = type;
                    cmd.Parameters.Add("@start_date", SqlDbType.VarChar).Value = start_date;
                    cmd.Parameters.Add("@entry_date", SqlDbType.VarChar).Value = entry_date;
                    cmd.Parameters.Add("@short_description", SqlDbType.VarChar).Value = short_description;
                    cmd.Parameters.Add("@deliverables", SqlDbType.VarChar).Value = deliverables;
                    cmd.Parameters.Add("@client", SqlDbType.VarChar).Value = client;
                    cmd.Parameters.Add("@deadline_final_aprobal", SqlDbType.VarChar).Value = deadline_final_aprobal;
                    cmd.Parameters.Add("@workflow_status", SqlDbType.VarChar).Value = workflow_status;
                    cmd.Parameters.Add("@success", SqlDbType.VarChar).Value = success;
                    cmd.Parameters.Add("@result", SqlDbType.VarChar).Value = result;
                    cmd.Parameters.Add("@remark", SqlDbType.VarChar).Value = remark;
                    cmd.Parameters.Add("@finishdate", SqlDbType.VarChar).Value = finishdate == "" || finishdate == null ? (object)DBNull.Value : Convert.ToDateTime(finishdate).ToString("yyyyMMdd");
                    cmd.Parameters.Add("@origin", SqlDbType.VarChar).Value = origin;
                    cmd.Parameters.Add("@number_extensions", SqlDbType.Int).Value = number_extensions;
                    cmd.Parameters.Add("@effectiveness_check", SqlDbType.VarChar).Value = effectiveness_check;
                    cmd.Parameters.Add("@justification", SqlDbType.VarChar).Value = justification;
                    cmd.Parameters.Add("@implementation_date", SqlDbType.VarChar).Value = implementation_date == "" || implementation_date == null ? (object)DBNull.Value : Convert.ToDateTime(implementation_date).ToString("yyyyMMdd"); ;
                    cmd.Parameters.Add("@planned_date", SqlDbType.VarChar).Value = planned_date;
                    cmd.Parameters.Add("@v_et", SqlDbType.VarChar).Value = v_et;
                    cmd.Parameters.Add("@evento", SqlDbType.VarChar).Value = evento;
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
        public int get_TotalPag_capas(string number, int PageSize)
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
                sql.CommandText = "select dbo.get_TotalPag_capas(" + (number == "" || number == null ? (object)DBNull.Value + "null" : ("'" + number + "'")) +
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
        public DataTable get_capas_table(string number,int PageIndex, int PageSize)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString);
            var msg = "";
            DataTable dt = new DataTable();
            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("get_capas_table", con);
                    cmd.Parameters.Add("@number", SqlDbType.VarChar).Value = number == "" || number == null ? (object)DBNull.Value : number;
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
    }
}

