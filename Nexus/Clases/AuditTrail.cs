using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Nexus.Clases
{
    public class AuditTrail
    {
        public string insert_AuditTrail(string CWID, string Accion, string Anterior, string Actual, string Justificacion)
        {
            var msg = "";
            try
            {
                DataTable dt = new DataTable();
                var constr = ConfigurationManager.ConnectionStrings["BD_Base"].ConnectionString;
                using (var conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("insert_AuditTrail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Cwid", SqlDbType.VarChar).Value = CWID;
                    cmd.Parameters.Add("@Accion", SqlDbType.VarChar).Value = Accion;
                    cmd.Parameters.Add("@Anterior", SqlDbType.VarChar).Value = Anterior;
                    cmd.Parameters.Add("@Actual", SqlDbType.VarChar).Value = Actual;
                    cmd.Parameters.Add("@Justificacion", SqlDbType.VarChar).Value = Justificacion;
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