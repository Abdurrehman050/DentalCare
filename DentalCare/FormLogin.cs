using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace DentalCare
{

    public partial class FormLogin : Form
    {

        public FormLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DentalCare.Properties.Settings.DatabaseConnectionString1"].ToString();
                con.Open();

                string query = "select * from [user] where user_name='" + txtusername.Text.Trim() + "'and user_password='" + txtpassword.Text.Trim() + "'";
                SqlDataAdapter sdfa = new SqlDataAdapter(query, con);
                DataTable dtbl = new DataTable();
                sdfa.Fill(dtbl);

                if (dtbl.Rows.Count == 1)
                {
                    FormHome f1 = new FormHome();
                    this.Hide();
                    f1.Show();
                }
                else
                {
                    txtpassword.Clear();
                    MessageBox.Show("Please Enter Correct User and Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
