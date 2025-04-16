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

namespace DentalCare.Report
{
    public partial class FormPrintReport : Form
    {
        public FormPrintReport()
        {
            InitializeComponent();
        }
    private void responsive()
        {
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            this.Location = new Point(0, 0);
            this.Size = new Size(w, h);
        }

        private void FormPrintReport_Load(object sender, EventArgs e)
        {
            //responsive();
            suggest();
        }
        public void suggest()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
            string sqlquery = "select doctor_name from entry";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, con);
            con.Open();
            SqlDataReader sdr = sqlcomm.ExecuteReader();
            AutoCompleteStringCollection autotext = new AutoCompleteStringCollection();
            while (sdr.Read())
            {
                autotext.Add(sdr.GetString(0));
            }
            txt_search.AutoCompleteMode = AutoCompleteMode.Suggest;
            txt_search.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_search.AutoCompleteCustomSource = autotext;
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
            SqlDataAdapter da = new SqlDataAdapter("select * from entry where doctor_name like '%" + txt_search.Text + "%' and date between '" + date_from.Value.ToString("yyyy-MM-dd") + "' and '" + date_to.Value.ToString("yyyy-MM-dd") + "' order by date desc", con);
            DataSet1 ds = new DataSet1();
            da.Fill(ds, "entry");
            ReportDataSource dataSource = new ReportDataSource("DataSet1", ds.Tables[0]);

            ReportParameterCollection reportParameters = new ReportParameterCollection();
            reportParameters.Add(new ReportParameter("fromdate", date_from.Value.ToString()));
            reportParameters.Add(new ReportParameter("todate", date_to.Value.ToString()));

            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(dataSource);
            this.reportViewer1.LocalReport.SetParameters(reportParameters);
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }
    }
}
