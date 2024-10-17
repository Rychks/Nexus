using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using System.DirectoryServices;

namespace Nexus.Clases
{
    public class Users
    {
        Departments depto = new Departments();
        public int get_TotalPag_users(string cwid, string fullname, string department, string is_enable, int PageSize)
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
                sql.CommandText = "select dbo.get_TotalPag_users(" + (cwid == "" || cwid == null ? (object)DBNull.Value + "null" : ("'" + cwid + "'")) +
                    "," + (fullname == "" || fullname == null ? (object)DBNull.Value + "null" : ("'" + fullname + "'")) +
                    "," + (department == "" || department == null ? (object)DBNull.Value + "null" : ("'" + depto.get_department_department_code(department) + "'")) +
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
        public DataTable get_users_table(string cwid, string fullname, string department, string is_enable, int PageIndex, int PageSize)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString);
            var msg = "";
            DataTable dt = new DataTable();
            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("get_users_table", con);
                    cmd.Parameters.Add("@cwid", SqlDbType.VarChar).Value = cwid == "" || cwid == null ? (object)DBNull.Value : cwid;
                    cmd.Parameters.Add("@fullname", SqlDbType.VarChar).Value = fullname == "" || fullname == null ? (object)DBNull.Value : fullname;
                    cmd.Parameters.Add("@department", SqlDbType.VarChar).Value = department == "" || department == null ? (object)DBNull.Value : depto.get_department_department_code(department);
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
        public string get_user_department_code(string cwid)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString);
            string msg = "";
            try
            {
                System.Data.SqlClient.SqlDataReader reader;
                System.Data.SqlClient.SqlCommand sql;
                con.Open();
                sql = new System.Data.SqlClient.SqlCommand();
                sql.CommandText = "select dbo.get_user_department_code('" + cwid + "');";
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
        public int get_user_id_by_cwid(string CWID)
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
                sql.CommandText = "select dbo.get_user_id_by_cwid('" + CWID + "');";
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
        public string get_user_fullname(string cwid)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString);
            string msg = "";
            try
            {
                System.Data.SqlClient.SqlDataReader reader;
                System.Data.SqlClient.SqlCommand sql;
                con.Open();
                sql = new System.Data.SqlClient.SqlCommand();
                sql.CommandText = "select dbo.get_user_fullname('" + cwid + "');";
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
        public bool autenticate_user(string CWID, string Password)
        {
            bool res = false;
            try
            {
                string path = "LDAP://ad-pt.intranet.cnb";
                DirectoryEntry de = new DirectoryEntry(path, CWID, Password, AuthenticationTypes.Secure);
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.FindOne();
                res = true;
            }
            catch (Exception e)
            {
                res = false;
                if (!(e is DirectoryServicesCOMException))
                {
                    ErrorLogger.Registrar(this, e.ToString());
                }
            }
            return res;
        }
        public int validate_user(string CWID)
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
                sql.CommandText = "select dbo.validate_user('" + CWID + "');";
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
        public string insert_users(string name, string cwid, string lastname, string fullname, string email, string company, string department, string area, string title)
        {
            
            var msg = "";
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    DataTable dt = new DataTable();
                    var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
                    using (var conn = new SqlConnection(constr))
                    {
                        SqlCommand cmd = new SqlCommand("insert_users", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name == "" || name == null || name == "#" ? (object)DBNull.Value : name;
                        cmd.Parameters.Add("@cwid", SqlDbType.VarChar).Value = cwid == "" || cwid == null || cwid == "#" ? (object)DBNull.Value : cwid; ;
                        cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = lastname == "" || lastname == null || lastname == "#" ? (object)DBNull.Value : lastname;
                        cmd.Parameters.Add("@fullname", SqlDbType.VarChar).Value = fullname == "" || fullname == null || fullname == "#" ? (object)DBNull.Value : fullname;
                        cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email == "" || email == null || email == "#" ? (object)DBNull.Value : email;
                        cmd.Parameters.Add("@company", SqlDbType.VarChar).Value = company == "" || company == null || company == "#" ? (object)DBNull.Value : company;
                        cmd.Parameters.Add("@department", SqlDbType.VarChar).Value = department == "" || department == null || department == "#" ? (object)DBNull.Value : department;
                        cmd.Parameters.Add("@area", SqlDbType.VarChar).Value = area == "" || area == null || area == "#" ? (object)DBNull.Value : area;
                        cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = title == "" || title == null || title == "#" ? (object)DBNull.Value : title;
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
                {
                    msg = string.Empty;
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