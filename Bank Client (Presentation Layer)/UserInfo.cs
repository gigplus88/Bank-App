using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Client
{
    public partial class UserInfo : UserControl
    {
        public string Username;
        
        public UserInfo(string Username)
        {
            InitializeComponent();
            this.Username = Username;
        }

        public UserInfo()
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UserInfo_Load(object sender, EventArgs e)
        {
            lblDate1.Text = DateTime.UtcNow.ToString();
            lblUserNameUC.Text = Username;

        }
    }
}
