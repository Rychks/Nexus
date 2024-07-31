using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace Nexus.Clases
{
    public class Mantenimiento
    {
        public string insert_avisos_z1(string centro_costos,string ubicacion_tecnica,string aviso, string fecha_aviso,string clase_aviso,
        string descripcion,string orden, string modificado_por,string fecha_modificado,string status_sistema,string status_usuario,
string clase_trabajo,string duracion_parada,string prioridad,string inicio_deseado,string hora_inicio_averia,string hora_fin_averia,
string hora_inicio_des,string inicio_averia,string fin_averia,string denominacion,string equipo)
        {
            var msg = "";
            try
            {
                DataTable dt = new DataTable();
                var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("insert_avisos_z1", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@centro_costos", SqlDbType.VarChar).Value = centro_costos;
                    cmd.Parameters.Add("@ubicacion_tecnica", SqlDbType.VarChar).Value = ubicacion_tecnica;
                    cmd.Parameters.Add("@aviso", SqlDbType.BigInt).Value = aviso;
                    cmd.Parameters.Add("@fecha_aviso", SqlDbType.VarChar).Value = fecha_aviso == "" || fecha_aviso == null ? (object) DBNull.Value : Convert.ToDateTime(fecha_aviso).ToString("yyyyMMdd");
                    cmd.Parameters.Add("@clase_aviso", SqlDbType.VarChar).Value = clase_aviso;
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = descripcion;
                    cmd.Parameters.Add("@orden", SqlDbType.BigInt).Value = orden == "" || orden == null ? (object) DBNull.Value : orden;
                    cmd.Parameters.Add("@modificado_por", SqlDbType.VarChar).Value = modificado_por;
                    cmd.Parameters.Add("@fecha_modificado", SqlDbType.VarChar).Value = fecha_modificado == "" || fecha_modificado == null ? (object) DBNull.Value : Convert.ToDateTime(fecha_modificado).ToString("yyyyMMdd");
                    cmd.Parameters.Add("@status_sistema", SqlDbType.VarChar).Value = status_sistema;
                    cmd.Parameters.Add("@status_usuario", SqlDbType.VarChar).Value = status_usuario;
                    cmd.Parameters.Add("@clase_trabajo", SqlDbType.VarChar).Value = clase_trabajo;
                    cmd.Parameters.Add("@duracion_parada", SqlDbType.Float).Value = duracion_parada == "" || duracion_parada == null ? (object)DBNull.Value : duracion_parada;
                    cmd.Parameters.Add("@prioridad", SqlDbType.VarChar).Value = prioridad;
                    cmd.Parameters.Add("@inicio_deseado", SqlDbType.VarChar).Value = inicio_deseado == "" || inicio_deseado == null ? (object)DBNull.Value : Convert.ToDateTime(inicio_deseado).ToString("yyyyMMdd");
                    cmd.Parameters.Add("@hora_inicio_averia", SqlDbType.VarChar).Value = hora_inicio_averia == "" || hora_inicio_averia == null ? (object)DBNull.Value : Convert.ToDateTime(hora_inicio_averia).ToString("T");
                    cmd.Parameters.Add("@hora_fin_averia", SqlDbType.VarChar).Value = hora_fin_averia == "" || hora_fin_averia == null ? (object)DBNull.Value : Convert.ToDateTime(hora_fin_averia).ToString("T");
                    cmd.Parameters.Add("@hora_inicio_des", SqlDbType.VarChar).Value = hora_inicio_des == "" || hora_inicio_des == null ? (object)DBNull.Value : Convert.ToDateTime(hora_inicio_des).ToString("T");
                    cmd.Parameters.Add("@inicio_averia", SqlDbType.VarChar).Value = inicio_averia == "" || inicio_averia == null ? (object)DBNull.Value : Convert.ToDateTime(inicio_averia).ToString("yyyyMMdd");
                    cmd.Parameters.Add("@fin_averia", SqlDbType.VarChar).Value = fin_averia == "" || fin_averia == null ? (object)DBNull.Value : Convert.ToDateTime(fin_averia).ToString("yyyyMMdd");
                    cmd.Parameters.Add("@denominacion", SqlDbType.VarChar).Value = denominacion;
                    cmd.Parameters.Add("@equipo", SqlDbType.BigInt).Value = equipo == "" || equipo == null ? (object)DBNull.Value : equipo;
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
        public int get_TotalPag_avisos_z1(string aviso, int PageSize)
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
                sql.CommandText = "select dbo.get_TotalPag_avisos_z1(" + (aviso == "" || aviso == null ? (object)DBNull.Value + "null" : ("'" + aviso + "'")) +
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
        public DataTable get_avisos_z1_table(string aviso, int PageIndex, int PageSize)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString);
            var msg = "";
            DataTable dt = new DataTable();
            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("get_avisos_z1_table", con);
                    cmd.Parameters.Add("@aviso", SqlDbType.VarChar).Value = aviso == "" || aviso == null ? (object)DBNull.Value : aviso;
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

        public string insert_GOALS_INDIC_PROD(DataTable TabGOALS_INDIC_PROD)
        {
            var msg = "";
            try
            {
                DataTable dt = new DataTable();
                var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("insert_GOALS_INDIC_PROD", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TabGOALS_INDIC_PROD", TabGOALS_INDIC_PROD);
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
    }
}