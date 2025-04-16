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
    public partial class FormUsers : Form
    {
        public FormUsers()
        {
            InitializeComponent();
            btn_update.Enabled = false;
            btn_delete.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadDataIntoGridview()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();



            con.Open();
            string query = "select * from [user]";
            SqlDataAdapter sqldata = new SqlDataAdapter(query, con);
            DataTable dtbl = new DataTable();
            sqldata.Fill(dtbl);
            dataGridView1.DataSource = dtbl;

            dataGridView1.Update();
        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            string username = txt_username.Text;
            string password = txt_password.Text;
            if (txt_username.Text != string.Empty && txt_password.Text != string.Empty)
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
                SqlCommand cmd = new SqlCommand("INSERT INTO [user](user_name,user_password) values('" + username + "','" + password + "') ", con);

                con.Open();
                cmd.ExecuteNonQuery();
                DialogResult result = MessageBox.Show("Save sucessfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataIntoGridview();

            }
            else
            {
                MessageBox.Show("Please fill all the required fileds", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            reset_all();
        }

        private void FormUsers_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.user' table. You can move, or remove it, as needed.
            this.userTableAdapter.Fill(this.databaseDataSet.user);
            // TODO: This line of code loads data into the 'databaseDataSet.user' table. You can move, or remove it, as needed.
            this.userTableAdapter.Fill(this.databaseDataSet.user);

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
                    string query = "update [user] set user_name='" + txt_username.Text + "',user_password='" + txt_password.Text + "' where user_id='" + txt_id.Text + "'";

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                txt_id.Text = row.Cells[0].Value.ToString();
                txt_username.Text = row.Cells[1].Value.ToString();
                txt_password.Text = row.Cells[2].Value.ToString();
                

            }

            btn_update.Enabled = true;
            btn_delete.Enabled = true;
            btn_save.Enabled = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to delete this entry?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
                string query = "DELETE FROM dbo.[user] WHERE user_id='" + txt_id.Text + "'";

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
        private void reset_all()
        {
            txt_id.Clear();
            txt_password.Clear();
            txt_username.Clear();
        }
    }
}
