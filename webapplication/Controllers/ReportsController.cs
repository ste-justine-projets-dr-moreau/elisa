using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Clinic.BackEnd.Context;
using WebApplication.Utility;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;




namespace WebApplication.Controllers
{
    public class ReportController : Controller
    {
        #region Declarations

        // private ClinicContext db = new ClinicContext();

        #endregion

        // GET: Report

        public void ExportCSV(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["ClinicContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT  * FROM vw_Elisa"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);

                            //Build the CSV file data as a Comma separated string.
                            //string csv = string.Empty;

                            StringBuilder sb = new StringBuilder();

                            foreach (DataColumn column in dt.Columns)
                            {
                                //Add the Header row for CSV file.
                                //csv += column.ColumnName + ',';
                                sb.Append(column.ColumnName + ',');
                            }

                            //Add new line.
                            //csv += "\r\n";

                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    //Add the Data rows.
                                    sb.Append(row[column.ColumnName].ToString().Replace(",", ";") + ',');
                                    //csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';

                                }

                                //Add new line.
                                //csv += "\r\n";
                                sb.Append("\r\n");
                            }

                            //Download the CSV file.
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv");
                            Response.Charset = "";
                            Response.ContentType = "application/text";
                            Response.Output.Write(sb.ToString());
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
        }
    }
}