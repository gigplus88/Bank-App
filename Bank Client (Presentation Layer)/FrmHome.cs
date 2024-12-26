using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Bank_Client.FrmLogin;

namespace Bank_Client
{
    public partial class FrmHome : Form
    {
        public enum enPermissions { All = -1, ManageClients = 1, ClientsTransactions = 2, ManageUsers = 8, CurrencyExChange = 16 }
        enPermissions Permission;
        private clsUser User = new clsUser();
        private DataTable UsersLogTable = new DataTable();
        

        private string _UserName;
        private int _Password;

        public FrmHome(string UserName , int Password , DataTable clientLogTable)
        {
            this._UserName = UserName;
            this._Password = Password;
            this.MaximizeBox = false;
            InitializeComponent();
            this.UsersLogTable=clientLogTable;
           
        }

        private void _Load()
        {
            lblUserName.Text = _UserName;
            lblDay.Text =   DateTime.UtcNow.ToString();
            Permissions();
        }

        private void FrmHome_Load(object sender, EventArgs e)
        {
            _Load();
        }
        private bool CheckAccessPermission(enPermissions Permission)
        {
            User =  clsUser.Find(_UserName);

            if (User == null)
                return false;

            if (User.Permission == (int)enPermissions.All)
                return true;

            if (((int)Permission & User.Permission) == (int)Permission)
                return true;
            else
                return false;

        }

        private void Permissions()
        {
            if (!CheckAccessPermission(enPermissions.ManageClients))
            {
                btnManageClients.Visible = false;
            }
            if (!CheckAccessPermission(enPermissions.ClientsTransactions))
            {
                btnClientsTransactions.Visible = false;
            }
            if (!CheckAccessPermission(enPermissions.ManageUsers))
            {
                btnManageUsers.Visible = false;
            }
            if (!CheckAccessPermission(enPermissions.CurrencyExChange))
            {
                btnCurrencyExchange.Visible = false;
            }
        }
        private void btnManageClients_Click(object sender, EventArgs e)
        {
           
            FrmManageClient frm = new FrmManageClient(_UserName);
            frm.ShowDialog();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmTransactions frm = new FrmTransactions(_UserName);
            frm.ShowDialog();
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            FrmManageUsers frm = new FrmManageUsers(_UserName , UsersLogTable);
            frm.ShowDialog();
        }

        private void btnCurrencyExchange_Click(object sender, EventArgs e)
        {
            FrmCurrencyExchange frm = new FrmCurrencyExchange(_UserName);
            frm.ShowDialog();
        }
    }
}
