using BusinessLayer;
using System;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Bank_Client
{
    public partial class FrmManageUsers : Form
    {
        private string _UserName;
        private int Permissions = 0;
        private int PermissionsUp = 0;
        private DataTable UsersLogDataTable = new DataTable();
        private DataView UsersLogDataView = new DataView();


        enum enPermissions { All = -1 , ManageClients = 1 , ClientsTransactions = 2 ,ManageUsers = 8 , CurrencyExChange = 16 }
        enPermissions permission;
        public FrmManageUsers(string UserName , DataTable ClientLogTable)
        {
            this.MaximizeBox = false;
            this._UserName = UserName;
            this.UsersLogDataTable = ClientLogTable;
            InitializeComponent();
            _RefreshData();
        }
        private void _RefreshData()
        {
            // Show Users
            dgvUsersData.DataSource = clsUser.GetAllUsers();
            lblDate1.Text = DateTime.UtcNow.ToString();
            lblUserName.Text = _UserName;
            lblClientNumberFound.Text = clsUser.CountUser().ToString();

            //Add New User
            lblUserName2.Text = _UserName;
            lblDate2.Text = DateTime.UtcNow.ToString();
            chbClientsTransactions.Enabled = false;
            chbManageUsers.Enabled = false;
            chbCurrencyExchange.Enabled = false;
            chbManageClients.Enabled = false;

            //Update User
            lblUserName3.Text = _UserName;
            lblDate3.Text = DateTime.UtcNow.ToString();

            // Users Log
            dgvUsersDataLog.DataSource = GetAllUsersLog();
            lblClientNumberFound2.Text =  GetAllUsersLog().Count.ToString();
            lblDate4.Text = DateTime.UtcNow.ToString();
            lblUserName4.Text = _UserName;

        }
        private void rbDesc_CheckedChanged(object sender, EventArgs e)
        {
            dgvUsersData.DataSource = clsUser.UserByIDDesc();

        }

        private void rbAsc_CheckedChanged(object sender, EventArgs e)
        {
            dgvUsersData.DataSource = clsUser.UserByIDAsc();

        }

        private void tpShowUser_Click(object sender, EventArgs e)
        {
            dgvUsersData.DataSource = clsUser.GetAllUsers();
            lblDate1.Text = DateTime.UtcNow.ToString();
            lblUserName.Text = _UserName;
        }


        public string MessageForNullNumbers= "You must enter a UserName";
        public string MessageForNulWords = "You must enter a Words";
        public int Percent = 0;
        public bool FalseInput = false, InputEntered = false, InputRemoved = false;


        public void ContextErrorProvider(TextBox TextBoxType, string Message, CancelEventArgs e)
        {
            e.Cancel = true;
            epFindUserName.SetError(TextBoxType, Message);
        }
        private void ValidatingForNumbers(TextBox textBox, int ProgressValue, CancelEventArgs e)
        {
            string input = textBox.Text.Trim();
            bool Isvalide = int.TryParse(textBox.Text, out int result);

            if (Isvalide && !string.IsNullOrEmpty(input))
            {
                FalseInput = false;
                InputEntered = true;
                _ChangeProgressValue(textBox, input, MessageForNullNumbers, e, ProgressValue);
                InputEntered = false;
            }
            else if (!Isvalide && !string.IsNullOrEmpty(input))
            {
                ContextErrorProvider(textBox, "You must enter a numbers", e);

            }
            else
            {
                InputRemoved = true;
                _ChangeProgressValue(textBox, input, MessageForNullNumbers, e, ProgressValue);
                ContextErrorProvider(textBox, "You must enter a numbers", e);
            }
        }
        private void ValidatingForWords(TextBox textBox, int ProgressValue, CancelEventArgs e)
        {

            string input = textBox.Text.Trim();
            bool Isvalide = int.TryParse(textBox.Text, out int result);

            if (!Isvalide  && !string.IsNullOrEmpty(input) )
            {
                FalseInput = false;
                InputEntered = true;
                _ChangeProgressValue(textBox, input, MessageForNulWords, e, ProgressValue);
                InputEntered = false;

            }
            else if (Isvalide && !string.IsNullOrEmpty(input))
            {
                ContextErrorProvider(textBox, "You must enter a words", e);

            }
            else
            {
                InputRemoved = true;
                _ChangeProgressValue(textBox, input, MessageForNulWords, e, ProgressValue);
                ContextErrorProvider(textBox, "You must enter a words", e);
            }
        }
        private void _ChangeProgressValue(TextBox TextBoxType, string input, string Message, CancelEventArgs e, int ProgressValue)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (FalseInput == true)
                {
                    return;
                }
                epFindUserName.SetError(TextBoxType, "");
                Percent += ProgressValue;
            }

            if (string.IsNullOrEmpty(input))
            {
                if (progressBar1.Value >= ProgressValue && InputRemoved == true && InputEntered ==true)
                {
                    progressBar1.Value -= ProgressValue;
                    Percent -=ProgressValue;

                }
                //ContextErrorProvider(TextBoxType, Message, e);
            }
            lblPercent.Text = Percent.ToString();
        }
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            string input = txtUserName.Text.Trim();
            bool Isvalide = int.TryParse(input, out int result);

            if (!Isvalide && clsUser.IsUserExist(input))
            {
                ContextErrorProvider(txtUserName, "This User  Exist , Please enter another", e);
                InputEntered =true;
                return;
            }
            ValidatingForWords(txtUserName, 30, e);
        }
        public bool ValidateEmail(string Email)
        {
            string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(Email, EmailPattern);
        }
        public void EmailValidation(CancelEventArgs e)
        {
            string Email = txtEmail.Text.Trim();

            if (!ValidateEmail(Email) && !string.IsNullOrEmpty(Email))
            {
                ContextErrorProvider(txtEmail, "Invalid Email Address Format", e);
            }
            else if (string.IsNullOrEmpty(Email))
            {
                ContextErrorProvider(txtEmail, "Required", e);
            }
            else
            {
                ContextErrorProvider(txtEmail, "", e);
                e.Cancel = false;
            }

        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            //ValidatingForWords(txtEmail, 12, e);
            EmailValidation(e);
        } 

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            ValidatingForWords(txtFirstName, 12, e);
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            ValidatingForWords(txtLastName, 12, e);

        }
        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            ValidatingForNumbers(txtPhone, 12, e);

        }

        private void txtPinCode_Validating(object sender, CancelEventArgs e)
        {
            ValidatingForNumbers(txtPassword, 12, e);

        }
        private void tpAddUser_Click(object sender, EventArgs e)
        {
            _ClearDataForAddPage();
            lblUserName2.Text = _UserName;
            lblDate2.Text = DateTime.UtcNow.ToString();
        }

        private void rbYes_CheckedChanged(object sender, EventArgs e)
        {
            Permissions = (int)enPermissions.All;
            ValidateCheckBoxForFalse();
            if (progressBar1.Value == 100)
            {
                return;
            }
           

        }
        private void rbNo_CheckedChanged(object sender, EventArgs e)
        {

            ValidateCheckBoxForTrue();
            if (progressBar1.Value == 100)
            {
                return;
            }
            Percent += 10;
            lblPercent.Text = Percent.ToString();
        }

        public void ValidateCheckBoxForFalse()
        {
            chbManageClients.Enabled = false;
            chbClientsTransactions.Enabled = false;
            chbManageUsers.Enabled = false ;
            chbCurrencyExchange.Enabled = false;
        }
        public void ValidateCheckBoxForTrue()
        {
            chbManageClients.Enabled = true;
            chbClientsTransactions.Enabled = true;
            chbManageUsers.Enabled = true;
            chbCurrencyExchange.Enabled = true;
        }


        public int CheckboxPermissions()
        {
            Permissions = 0;
            if (chbManageClients.Checked )
            {
                Permissions += (int)enPermissions.ManageClients;
               
            }
            if (chbClientsTransactions.Checked)
            {
                
                Permissions += (int)enPermissions.ClientsTransactions;
                
            }
            if (chbManageUsers.Checked)
            {
                Permissions += (int)enPermissions.ManageUsers;
            }
            if (chbCurrencyExchange.Checked)
            {
                Permissions += (int)enPermissions.CurrencyExChange;
            }
            return Permissions;
        }

        private void _ClearDataForAddPage()
        {
            txtUserName.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            Percent = 0;
            lblPercent.Text = "0";
            progressBar1.Value = 0;


            rbYes.Checked = false;
            rbNo.Checked = false;
            chbClientsTransactions.Enabled = false;
            chbManageUsers.Enabled = false;
            chbCurrencyExchange.Enabled = false;
            chbManageClients.Enabled = false;
        }

        private void _ClearDataForUpdatePage()
        {
            cbUserName.Text = "";
            tbPassword2.Clear();
            tbFirstName2.Clear();
            tbLastName2.Clear();
            tbEmail2.Clear();
            tbPhone2.Clear();
            tbPassword2.Clear();

            rbYes2.Checked = false;
            rbNo2.Checked = false;
            chbManageClients2.Checked = false;
            chbClientsTransactions2.Checked = false;
            chbManageUsers2.Checked = false;
            chbCurrencyExchange2.Checked = false;
        }
        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            clsUser User = new clsUser();

           // Client.UserID = clsUser.GetUserID(this.UserName);

            User.UserName = txtUserName.Text;
            User.Email = txtEmail.Text;
            User.FirstName  = txtFirstName.Text;
            User.LastName = txtLastName.Text;
            User.Phone = txtPhone.Text;
            User.Password = int.Parse(txtPassword.Text);
            User.Permission = Permissions;

            if (MessageBox.Show("Are you sure to add this User", "Warning", MessageBoxButtons.YesNo ) == DialogResult.Yes)
            {
                if (User.Save())
                {
                    MessageBox.Show("Your Client Added Successfully", "Adding Client", MessageBoxButtons.OK , MessageBoxIcon.Information);
                    dgvUsersDataLog.DataSource = clsUser.GetAllUsers();
                    _ClearDataForAddPage();
                }
            }
        }

        

        private void txtSearchAccNumber_DragLeave(object sender, EventArgs e)
        {
            string input1 = txtSearchUserName.Text.Trim();


            if (!int.TryParse(input1, out int result))
            {
                dgvUsersData.DataSource = clsUser.FindUser(input1);
                lblClientNumberFound.Text = clsUser.FindUser(input1).Count.ToString();
            }

            else if (string.IsNullOrEmpty(input1))
            {
                dgvUsersData.DataSource = clsUser.GetAllUsers();
                lblClientNumberFound.Text = clsUser.CountUser().ToString();
            }
        }

       

        private void txtSearchAccNumber_TextChanged(object sender, EventArgs e)
       {
            string input1 = txtSearchUserName.Text.Trim();


            if (!string.IsNullOrEmpty(input1))
            {
                dgvUsersData.DataSource = clsUser.FindUser(input1);
                lblClientNumberFound.Text = clsUser.FindUser(input1).Count.ToString();
            }

            else if (string.IsNullOrEmpty(input1))
            {
                dgvUsersData.DataSource = clsUser.GetAllUsers();
                lblClientNumberFound.Text = clsUser.CountUser().ToString();
            }


        }
        private void chbManageClients_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxPermissions();
        }

        private void chbClientsTransactions_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxPermissions();
        }

        private void chbManageUsers_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxPermissions();
        }

        private void chbCurrencyExchange_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxPermissions();
        }

        public DataTable UsersDataTable = new DataTable();
        public DataView UsersDataView = new DataView();
        enMFindMode FindMode;
        clsUser User = new clsUser();
        enum enMFindMode { BeforeFill, AfterFill };

        private void rbYes2_CheckedChanged(object sender, EventArgs e)
        {
            PermissionsUp = (int)enPermissions.All;
            ValidateCheckBoxForFalseUp();
        }
        private void rbNo2_CheckedChanged(object sender, EventArgs e)
        {
            ValidateCheckBoxForTrueUp();
        }
        public int CheckboxPermissionsUp()
        {
            rbYes.Checked = false;
            PermissionsUp = 0;
            if (chbManageClients2.Checked)
            {
                PermissionsUp += (int)enPermissions.ManageClients;

            }
            if (chbClientsTransactions2.Checked)
            {

                PermissionsUp += (int)enPermissions.ClientsTransactions;

            }
            if (chbManageUsers2.Checked)
            {
                PermissionsUp += (int)enPermissions.ManageUsers;
            }
            if (chbCurrencyExchange2.Checked)
            {
                PermissionsUp += (int)enPermissions.CurrencyExChange;
            }
            return PermissionsUp;
        }

       
        private void cbUserName_Click(object sender, EventArgs e)
        {
            UsersDataView = clsUser.GetUserNameOfUsers().DefaultView;

            cbUserName.Items.Clear();

            for (int i = 0; i < UsersDataView.Count; i++)
            {
                cbUserName.Items.Add(UsersDataView[i][0]);
            }
        }

      
        private void cbAccNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbUserName.SelectedItem != null && !int.TryParse(cbUserName.SelectedItem.ToString(), out int Username))
            {

                User = clsUser.Find(cbUserName.SelectedItem.ToString());

                FindMode = enMFindMode.BeforeFill;

                _FillBoxes();
            }
            else
            {
                MessageBox.Show("You aren t Items");
            }
        }


      

        private void FillPermissions2()
        {
            // Reset all checkboxes
            rbYes2.Checked = false;
            rbNo2.Checked = false;
            chbManageClients2.Checked = false;
            chbClientsTransactions2.Checked = false;
            chbManageUsers2.Checked = false;
            chbCurrencyExchange2.Checked = false;

            if (User.Permission == -1)
            {
                rbYes2.Checked = true;
                chbManageClients2.Checked = true;
                chbClientsTransactions2.Checked = true;
                chbManageUsers2.Checked = true;
                chbCurrencyExchange2.Checked = true;
                return;
            }
            rbNo2.Checked = true;

            // Use bitwise operations to check each permission flag
            if ((User.Permission & 1) != 0) // Permission 1: Manage Clients
                chbManageClients2.Checked = true;
            
            if ((User.Permission & 2) != 0) // Permission 2: Clients Transactions
                chbClientsTransactions2.Checked = true;
            
            if ((User.Permission & 8) != 0) // Permission 8: Manage Users
                chbManageUsers2.Checked = true;
            
            if ((User.Permission & 16) != 0) // Permission 16: Currency Exchange
            chbCurrencyExchange2.Checked = true;
        }

        private void _FillBoxes()
        {

            //Client = clsClient.FindClientWithFilling(int.Parse(cbAccNumbers.SelectedValue.ToString()));

            if (FindMode == enMFindMode.BeforeFill)
            {
                tbFirstName2.Text = User.FirstName;
                tbLastName2.Text = User.LastName;
                tbEmail2.Text = User.Email;
                tbPhone2.Text = User.Phone;
                tbPassword2.Text = User.Password.ToString();
                FillPermissions2();
                FindMode = enMFindMode.AfterFill;
            }

            else
            {
               User.FirstName = tbFirstName2.Text;
               User.LastName = tbLastName2.Text;
               User.Email = tbEmail2.Text;
               User.Phone = tbPhone2.Text;
               User.Password = int.Parse(tbPassword2.Text);
               User.Permission = CheckboxPermissionsUp();
                if (MessageBox.Show("Are you sure to Update this User", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (User.Save())
                    {
                        MessageBox.Show("Your User Updated Successfully", "Updating User", MessageBoxButtons.OK, MessageBoxIcon.Information) ;
                        dgvUsersDataLog.DataSource = clsUser.GetAllUsers();
                        _ClearDataForUpdatePage();
                    }
                }

            }
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            _FillBoxes();
        }

        private void tpUpdateUser_Click(object sender, EventArgs e)
        {
            lblUserName3.Text = _UserName;
            lblDate3.Text = DateTime.UtcNow.ToString();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure to Delete {(string)dgvUsersData.CurrentCell.Value} " , "Confirm" , MessageBoxButtons.OKCancel ) == DialogResult.OK)
            {
                if (clsUser.DeleteUser((string)dgvUsersData.CurrentCell.Value))
                {
                    MessageBox.Show("Your Client is deleting", "Delete Client " , MessageBoxButtons.OK , MessageBoxIcon.Information);

                }
            }
        }
        public void ValidateCheckBoxForFalseUp()
        {
            chbManageClients2.Enabled = false;
            chbClientsTransactions2.Enabled = false;
            chbManageUsers2.Enabled = false;
            chbCurrencyExchange2.Enabled = false;
        }

        private void FrmManageUsers_Load(object sender, EventArgs e)
        {
            dgvUsersDataLog.DataSource = clsUser.GetAllUsers();
        }

        public DataView GetUsersLog(string UserName)
        {
            UsersLogDataView = UsersLogDataTable.DefaultView;

            UsersLogDataView.RowFilter = "[User Name] = '" + UserName + "'";

            return UsersLogDataView;
        }

        public DataView GetAllUsersLog()
        {
            UsersLogDataView = UsersLogDataTable.DefaultView;

            UsersLogDataView.RowFilter = "";

            return UsersLogDataView;
        }
        private void txtSearchAccNumber2_TextChanged(object sender, EventArgs e)
        {

            string input1 = txtSearchUserName2.Text.Trim();


            if (!string.IsNullOrEmpty(input1))
            {
                dgvUsersDataLog.DataSource = GetUsersLog(input1);
                lblClientNumberFound2.Text = GetUsersLog(input1).Count.ToString();
            }

            else if (string.IsNullOrEmpty(input1))
            {
                dgvUsersDataLog.DataSource = GetAllUsersLog();
                lblClientNumberFound2.Text = GetAllUsersLog().Count.ToString();
            }

        }

        public DataView GetUserLogAsc()
        {
            UsersLogDataView = UsersLogDataTable.DefaultView;

            UsersLogDataView.Sort = "User Name Asc";

            return UsersLogDataView;
        }
        public DataView GetUserLogDesc()
        {
            UsersLogDataView = UsersLogDataTable.DefaultView;

            UsersLogDataView.Sort = "User Name Desc";

            return UsersLogDataView;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dgvUsersDataLog.DataSource = GetUserLogAsc();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dgvUsersDataLog.DataSource = GetUserLogDesc();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        public void ValidateCheckBoxForTrueUp()
        {
            chbManageClients2.Enabled = true;
            chbClientsTransactions2.Enabled = true;
            chbManageUsers2.Enabled = true;
            chbCurrencyExchange2.Enabled = true;
        }

        private void tpLoginRegister_Click(object sender, EventArgs e)
        {
            dgvUsersDataLog.DataSource = GetAllUsersLog();
            lblClientNumberFound2.Text =  GetAllUsersLog().Count.ToString();

        }


    }
}
