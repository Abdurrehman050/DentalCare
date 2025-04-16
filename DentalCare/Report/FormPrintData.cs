using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DentalCare.Report
{
    public partial class FormPrintData : Form
    {
        string name;
        public FormPrintData(string _name)
        {
            InitializeComponent();
            this.name = _name;
        }

        private void FormPrintData_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();

            SqlDataAdapter da = new SqlDataAdapter("Select * from entry where bill_id='" + name + "'", con);
            DataSet1 ds = new DataSet1();
            da.Fill(ds, "entry");
            ReportDataSource dataSource = new ReportDataSource("DataSet1", ds.Tables[0]);

            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(dataSource);
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


            this.reportViewer1.RefreshReport();
            
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
