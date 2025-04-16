using DentalCare.Report;
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

namespace DentalCare
{
    public partial class FormHome : Form
    {
        public FormHome()
        {
            InitializeComponent();
            reset_all();

            lbl_amount.Text = "0";
            txt_docName.Focus();

            btn_update.Enabled = false;
            btn_delete.Enabled = false;
        }
        private void responsive()
        {
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            this.Location = new Point(0, 0);
            this.Size = new Size(w, h);
        }

        private void FormHome_Load(object sender, EventArgs e)
        {
         //   responsive();   
            // TODO: This line of code loads data into the 'databaseDataSet1.entry' table. You can move, or remove it, as needed.
            this.entryTableAdapter1.Fill(this.databaseDataSet1.entry);
            // TODO: This line of code loads data into the 'databaseDataSet1.entry' table. You can move, or remove it, as needed.
            this.entryTableAdapter1.Fill(this.databaseDataSet1.entry);
            // TODO: This line of code loads data into the 'databaseDataSet.entry' table. You can move, or remove it, as needed.
            //this.entryTableAdapter.Fill(this.databaseDataSet.entry);
            suggest_name();
            suggest_contact();
            get_id();

        }
        public void suggest_name()
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
            txt_docName.AutoCompleteMode = AutoCompleteMode.Suggest;
            txt_docName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_docName.AutoCompleteCustomSource = autotext;
            txt_search.AutoCompleteMode = AutoCompleteMode.Suggest;
            txt_search.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_search.AutoCompleteCustomSource = autotext;
            con.Close();
        }
        public void suggest_contact()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
            string sqlquery = "select opd_number from entry";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, con);
            con.Open();
            SqlDataReader sdr = sqlcomm.ExecuteReader();
            AutoCompleteStringCollection autotext = new AutoCompleteStringCollection();
            while (sdr.Read())
            {
                autotext.Add(sdr.GetString(0));
            }
            txt_contact.AutoCompleteMode = AutoCompleteMode.Suggest;
            txt_contact.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_contact.AutoCompleteCustomSource = autotext;

            con.Close();
        }
        private void txt_id_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            FormLogin fl = new FormLogin();
            fl.Show();
            

            //Application.Exit();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            add_case_in_database();
            reset_all();
            suggest_name();
            suggest_contact();
        }
        private void add_case_in_database()
        {
            string doc_name, patient_name, work;
            doc_name = txt_docName.Text;
            patient_name = txt_patientName.Text;
            work = txt_work.Text;
            string doc_contact = txt_contact.Text;

            if (txt_docName.Text != string.Empty && txt_patientName.Text != string.Empty && txt_work.Text != string.Empty && txt_price.Text != string.Empty)
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
                SqlCommand cmd = new SqlCommand("INSERT INTO entry(date,doctor_name,opd_number,patient_name,work,price) values('" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" + doc_name + "','" + doc_contact + "','" + patient_name + "','" + work + "','" + Convert.ToInt32(txt_price.Text) + "') ", con);

                con.Open();
                cmd.ExecuteNonQuery();
                DialogResult result = MessageBox.Show("Save sucessfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataIntoGridview();
                //new Report.FormPrintData(txt_id.Text).Show();


            }
            else
            {
                MessageBox.Show("Please fill all the required fileds", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void LoadDataIntoGridview()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();



            con.Open();
            string query = "select * from [entry] order by bill_id asc";
            SqlDataAdapter sqldata = new SqlDataAdapter(query, con);
            DataTable dtbl = new DataTable();
            sqldata.Fill(dtbl);
            dataGridView1.DataSource = dtbl;

            dataGridView1.Update();
        }
        public void get_id()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
            string sqlquery = "select MAX(bill_id+1) from entry";
            SqlCommand command = new SqlCommand(sqlquery, con);
            con.Open();
            SqlDataReader dr = command.ExecuteReader();

            if (dr.Read())
            {
                String val = dr[0].ToString();
                txt_id.Text = val.ToString();

            }
            con.Close();
        }
        private void reset_all()
        {
            txt_contact.Clear();
            txt_id.Clear();
            dateTimePicker1.ResetText();
            txt_docName.Clear();
            txt_patientName.Clear();
            txt_work.Clear();
            txt_price.Clear();
            txt_search.Clear();
            date_from.ResetText();
            date_to.ResetText();
            lbl_amount.Text = "0";
            btn_search.Enabled = true;
            btn_update.Enabled = false;
            btn_delete.Enabled = false;
            btn_save.Enabled = true;
            btn_search.Enabled = true;
            LoadDataIntoGridview();
            get_id();

        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            search();
        }
        private void search_date()
        {

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
            string query = "select * from entry where doctor_name like '%" + txt_search.Text + "%' and date between '" + date_from.Value.ToString("yyyy-MM-dd") + "' and '" + date_to.Value.ToString("yyyy-MM-dd") + "' order by date desc";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable data = new DataTable();


            sda.Fill(data);
            dataGridView1.DataSource = data;

            con.Close();
        }
        private void search()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
            string query = "select * from [dbo].[entry] where doctor_name like '%" + txt_search.Text + "%' order by date desc";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable data = new DataTable();


            sda.Fill(data);
            dataGridView1.DataSource = data;
            con.Close();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            search_date();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                lbl_amount.Text = Convert.ToString(double.Parse(lbl_amount.Text) + double.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString()));
            }

            btn_search.Enabled = false;
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Do you want to update this data?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
                    string query = "update [entry] set date='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',doctor_name='" + txt_docName.Text + "',opd_number='" + txt_contact.Text + "',patient_name='" + txt_patientName.Text + "',work='" + txt_work.Text + "',price='" + txt_price.Text + "' where bill_id='" + txt_id.Text + "'";

                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();


                    sda.Fill(data);
                    dataGridView1.DataSource = data;
                    con.Close();
                    LoadDataIntoGridview();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to delete this entry?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
                string query = "DELETE FROM dbo.[entry] WHERE bill_id='" + txt_id.Text + "'";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable data = new DataTable();


                sda.Fill(data);
                dataGridView1.DataSource = data;
                con.Close();
                LoadDataIntoGridview();
                reset_all();
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            reset_all();
        }

        private void txt_search_Click(object sender, EventArgs e)
        {
            btn_search.Enabled = true;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                txt_id.Text = row.Cells[0].Value.ToString();
                dateTimePicker1.Text = row.Cells[1].Value.ToString();
                txt_docName.Text = row.Cells[2].Value.ToString();
                txt_contact.Text = row.Cells[3].Value.ToString();
                txt_patientName.Text = row.Cells[4].Value.ToString();
                txt_work.Text = row.Cells[5].Value.ToString();
                txt_price.Text = row.Cells[6].Value.ToString();

            }

            btn_update.Enabled = true;
            btn_delete.Enabled = true;
            btn_save.Enabled = false;
        }

        private void btn_users_Click(object sender, EventArgs e)
        {
            FormUsers f1 = new FormUsers();
            f1.ShowDialog();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            FormPrintReport formPrintReport = new FormPrintReport();
            formPrintReport.Show();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                txt_id.Text = row.Cells[0].Value.ToString();
                new Report.FormPrintData(txt_id.Text).Show();
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       
    }
}
