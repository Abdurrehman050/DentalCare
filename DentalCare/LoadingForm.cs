using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DentalCare
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel2.Width += 3;
            if (panel2.Width >= 100)
            {
                panel2.BackColor = Color.Red;
            }
            if(panel2.Width >= 200)
            {
                panel2.BackColor = Color.Purple;
            }
            if(panel2.Width >= 300)
            {
                panel2.BackColor = Color.PowderBlue;
            }
            if(panel2.Width >= 400)
            {
                panel2.BackColor = Color.RosyBrown;
            }
            if(panel2.Width >= 500)
            {
                panel2.BackColor = Color.Aquamarine;
            }
            if(panel2.Width >= 600)
            {
                panel2.BackColor = Color.GreenYellow;
            }
            if (panel2.Width >= 700)
            {
                timer1.Stop();
                FormLogin fl = new FormLogin();
                fl.Show();
                this.Hide();
            }
        }
    }
}
