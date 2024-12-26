using BusinessLayer;
using System;
using System.Data;
using System.IO.IsolatedStorage;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace Bank_Client
{
    public partial class FrmLogin : Form
    {

        public int UserEnterCount = 3;

        private string _UserName;
        private int _Password;

        public enum enPermissions { All = -1, ManageClients = 1, ClientsTransactions = 2, ManageUsers = 8, CurrencyExChange = 16 }
        enPermissions Permission;
        private clsUser User = new clsUser();

        public FrmLogin()
        {
            this.MaximizeBox = false;
            InitializeComponent();
        }
        
        private void _Login()
        {
           
            string UserName = txtUserName.Text;
            int Password = int.Parse(txtPassword.Text);
            if (!clsUser.IsUserWithPasswordExist(UserName, Password) && UserEnterCount > 0)
            {
                UserEnterCount--;
                lblInvalid.Text = "Invalid User Name Or Password";
                lblAttempts.Text = "You have "+(UserEnterCount) + " attempts before lock your account";

                lblAttempts.Visible = true;
                lblInvalid.Visible = true;

                if (UserEnterCount <= 0)
                {
                    btnLogin.Enabled = false;
                    lblAttempts.Text = "Sorry your account is blocked , contact your support for more information";
                    return;
                }
                txtUserName.Clear();
                txtPassword.Clear();
                txtUserName.Focus();
            }

            else
            {
                _UserName = UserName;
                _Password = Password;

                _ClientLogTable();
                User =  clsUser.Find(_UserName);
                UsersLogTable.Rows.Add(User.UserName, DateTime.UtcNow, User.FirstName, User.LastName, User.Email, User.Phone, User.Password, User.Permission);


                FrmHome frm = new FrmHome(UserName, Password, UsersLogTable);
                frm.ShowDialog();

            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            _Login();
        }
        DataTable UsersLogTable = new DataTable();
        
        public int TableLogCreationCount = 0;
        private void _ClientLogTable()
        {
            TableLogCreationCount++;

            if (TableLogCreationCount == 1)
            {
                UsersLogTable.Columns.Add("User Name", typeof(string));
                UsersLogTable.Columns.Add("Log Date", typeof(DateTime));
                UsersLogTable.Columns.Add("First Name", typeof(string));
                UsersLogTable.Columns.Add("Last Name", typeof(string));
                UsersLogTable.Columns.Add("Email", typeof(string));
                UsersLogTable.Columns.Add("Phone", typeof(string));
                UsersLogTable.Columns.Add("Password", typeof(int));
                UsersLogTable.Columns.Add("Permissions", typeof(int));


            }

        }
      
        private void _Load()
        {
            UserEnterCount = 3;
            lblDay.Text =   DateTime.UtcNow.ToString();
            txtUserName.Focus();
            lblAttempts.Visible = false;
            lblInvalid.Visible = false;
            btnLogin.Enabled = true;
            UserInfo userInfo = new UserInfo(_UserName);
        }
        private void FrmLogin_Load(object sender, EventArgs e)
        {
            _Load();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
        private void txtPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool Isvalide = int.TryParse(txtPassword.Text, out int result);
            if (!Isvalide )
            {
                e.Cancel = true;
                errorLoginProvider.SetError(txtPassword, "You must enter a numbers");

            }

            else
            {
                e.Cancel = false;
                errorLoginProvider.SetError(txtPassword, "");
            }
        }

        private void txtUserName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text) )
            {
                errorLoginProvider.SetError(txtUserName, "You must enter a value");
                e.Cancel = true;
                return;
            }
            else
            {
                e.Cancel = false;
                errorLoginProvider.SetError(txtUserName, "");
            }
        }
    }
}
