using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Client
{
    public partial class FrmManageClient : Form
    {
        public string UserName;
        public FrmManageClient(string UserName)
        {
            this.MaximizeBox = false;
            this.UserName = UserName;
            InitializeComponent();
            _RefreshData();
        }

        private void FrmManageClient_Load(object sender, EventArgs e)
        {
            dgvClientsData.DataSource = clsClient.GetAllClients();

        }
        private void _RefreshData()
        {
            lblDate1.Text = DateTime.UtcNow.ToString();
            lblUserName.Text = UserName;
            lblClientNumberFound.Text = clsClient.CountClient().ToString();


            lblDate2.Text = DateTime.UtcNow.ToString();
            lblUserName2.Text = UserName;
            Removingcount=0;

            lbldateTime3.Text = DateTime.UtcNow.ToString();
            lblUserName3.Text = UserName;
        }

        private void rbDesc_CheckedChanged_1(object sender, EventArgs e)
        {
            dgvClientsData.DataSource = clsClient.ClientsByAccNumberDesc();

        }

        private void rbAsc_CheckedChanged_1(object sender, EventArgs e)
        {
            dgvClientsData.DataSource = clsClient.ClientsByAccNumbeAsc();

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            _RefreshData();

        }

        private void txtSearchAccNumber_TextChanged_1(object sender, EventArgs e)
        {
            string input1 = txtSearchAccNumber.Text.Trim();


            if (!string.IsNullOrEmpty(input1) && int.TryParse(input1, out int Result))
            {
               
                dgvClientsData.DataSource = clsClient.FindClient(int.Parse(input1));
                lblClientNumberFound.Text =  clsClient.FindClient(int.Parse(input1)).Count.ToString();
                
            }
            else if (string.IsNullOrEmpty(input1))
            {
                dgvClientsData.DataSource = clsClient.GetAllClients();
                lblClientNumberFound.Text = clsClient.CountClient().ToString();
            }
            else if (!int.TryParse(input1, out int Result1))
            {
                dgvClientsData.DataSource = null;
                lblClientNumberFound.Text = "0";
            }

        }

       



        private void tabPage2_Click(object sender, EventArgs e)
        {
            _RefreshData();

        }
       
        public string MessageForNullNumber = "You must enter a numbers";
        public string MessageForNulWords = "You must enter a Words";
        public int Percent = 0 , Removingcount= 0;
        public bool FalseInput = false , InputEntered = false , InputRemoved = false;
 
        public void ContextErrorProvider(TextBox TextBoxType , string Message, CancelEventArgs e )
        {
            e.Cancel = true;
            epFindAccNumber.SetError(TextBoxType, Message);
        }

       private void ValidatingForNumbers(TextBox textBox , int ProgressValue, CancelEventArgs e)
        {
            string input = textBox.Text.Trim();
            bool Isvalide = int.TryParse(textBox.Text, out int result);

            if (Isvalide && !string.IsNullOrEmpty(input) && InputEntered == false)
            {
                FalseInput = false;
                //InputEntered = false;
                _ChangeProgressValue(textBox, input, MessageForNullNumber, e, ProgressValue);
                //InputEntered = false;
            }
            else if(!Isvalide && !string.IsNullOrEmpty(input))
            {
                ContextErrorProvider(textBox, "You must enter a numbers", e);
                FalseInput = true;
            }
            else
            {
                InputRemoved = true;
                _ChangeProgressValue(textBox, input, MessageForNullNumber, e, ProgressValue);
                ContextErrorProvider(textBox, "You must enter a numbers", e);
            }
        }
        private void ValidatingForWords(TextBox textBox, int ProgressValue, CancelEventArgs e)
        {

            string input = textBox.Text.Trim();
            bool Isvalide = int.TryParse(textBox.Text, out int result);

            if (Isvalide == false && !string.IsNullOrEmpty(input) && InputEntered == false)
            {
                FalseInput = false;
                //InputEntered = true;
                _ChangeProgressValue(textBox, input, MessageForNulWords, e, 12);
                //InputEntered = false;

            }
            else if (Isvalide && !string.IsNullOrEmpty(input))
            {
                ContextErrorProvider(textBox, "You must enter a words", e);
                FalseInput = true;
            }
            else
            {
                InputRemoved = true;
                _ChangeProgressValue(textBox, input, MessageForNulWords, e, 12);
                ContextErrorProvider(textBox, "You must enter a words", e);
            }
        }
        private void txtAccNumber_Validating(object sender, CancelEventArgs e)
        {
            string input = txtAccNumber.Text.Trim();
            bool Isvalide = int.TryParse(input, out int result);
         
            if (clsClient.IsClientExist(Convert.ToInt16(result)) && !string.IsNullOrEmpty(input))
            {
                ContextErrorProvider(txtAccNumber, "This Client  Exist , Please enter another", e);
                InputEntered =true;
                return;
            }
            InputEntered =false;
            ValidatingForNumbers(txtAccNumber, 30, e);


        }
        private void _ChangeProgressValue(TextBox TextBoxType, string input, string Message, CancelEventArgs e, int ProgressValue)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (FalseInput == true)
                {
                    return;
                }
                progressBar1.Value += ProgressValue;
                epFindAccNumber.SetError(TextBoxType, "");
                Percent += ProgressValue;
            }

            if (string.IsNullOrEmpty(input) && FalseInput == true)
            {
                if (progressBar1.Value >= ProgressValue && InputRemoved == true && InputEntered ==false)
                {
                    progressBar1.Value -= ProgressValue;
                    Percent -=ProgressValue;

                }
                //ContextErrorProvider(TextBoxType, Message, e);
            }
            lblPercent.Text = Percent.ToString();
        }

        private void txtPinCode_Validating(object sender, CancelEventArgs e)
        {
            ValidatingForNumbers(txtPinCode, 12, e);
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            ValidatingForWords(txtFirstName, 12, e);
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            ValidatingForWords(txtLastName, 12, e);
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
            // ValidatingForWords(txtEmail, 12, e);
            EmailValidation( e);


        }
        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            ValidatingForNumbers(txtPhone, 12, e);
        }

        private void nudBalance_Validating(object sender, CancelEventArgs e)
        {
            //string input = nudBalance.Text.Trim();
            //if (nudBalance.Value > 0)
            //{
            //    progressBar1.Value += 10 ;
            //    progressBar1.PerformStep();
            //    Percent += 10;
            //}

            //if (nudBalance.Value == 0)
            //{
            //    if (progressBar1.Value >= 10)
            //    {
            //        progressBar1.Value -= 10;
            //        Percent -=10;

            //    }
            //}

            //lblPercent.Text = Percent.ToString();
        }
         
       

        private void _ClearDataForAddPage()
        {
            txtAccNumber.Clear();
            txtPhone.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPinCode.Clear();
            nudBalance.Value = 0;
            Percent = 0;
            lblPercent.Text = "0";
            progressBar1.Value = 0;
        }

        private void _ClearDataForUpdatePage()
        {
            cbAccNumbers.Text = "";
            tbPhoneUp.Clear();
            tbFirstNameUp.Clear();
            tbLastNamUp.Clear();
            tbEmailUp.Clear();
            tbPinCodeUp.Clear();
            nudBalanceUp.Value = 0;
        }
        public void RefreshDataClient()
        {
            dgvClientsData.DataSource = clsClient.GetAllClients();

        }
        private void btnAddNewClient_Click(object sender, EventArgs e)
        {
            clsClient Client = new clsClient();

            Client.ID = int.Parse(txtAccNumber.Text);
            Client.FirstName = txtFirstName.Text;
            Client.LastName = txtLastName.Text;
            Client.Email = txtEmail.Text;
            Client.PinCode = int.Parse(txtPinCode.Text);
            Client.Phone = txtPhone.Text;
            Client.Balance = Convert.ToInt16(nudBalance.Value);
            Client.UserID = clsUser.GetUserID(this.UserName);

           
            if (MessageBox.Show("Are you sure to add this Client", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (Client.Save())
                {
                    MessageBox.Show("Your Client Added Successfullly", "Adding Client",MessageBoxButtons.OK ,  MessageBoxIcon.Information);
                    _ClearDataForAddPage();
                    RefreshDataClient();
                }
            }
           
        }

        public DataTable ClientsDataTable = new DataTable();
        public DataView ClientsDataView = new DataView();
        enum enMFindMode { BeforeFill , AfterFill };
        enMFindMode FindMode;


        private void btnUpdateClient_Click(object sender, EventArgs e)
        {
            _FillBoxes();
            RefreshDataClient();
        }
        public clsClient Client = new clsClient();

        private void cbAccNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAccNumbers.SelectedItem != null && int.TryParse(cbAccNumbers.SelectedItem.ToString(), out int accountNumber))
            {
               
                  Client = clsClient.FindClientWithFilling(accountNumber);
                
                  FindMode = enMFindMode.BeforeFill;

                  _FillBoxes();
            }
            else
            {
                MessageBox.Show("You aren t Items");
            }


        }

        private void _FillBoxes()
        {
            
            //Client = clsClient.FindClientWithFilling(int.Parse(cbAccNumbers.SelectedValue.ToString()));

            if (FindMode == enMFindMode.BeforeFill )
            {
                tbFirstNameUp.Text = Client.FirstName;
                tbLastNamUp.Text = Client.LastName;
                tbEmailUp.Text = Client.Email;
                tbPhoneUp.Text = Client.Phone;
                tbPinCodeUp.Text = Client.PinCode.ToString();
                nudBalanceUp.Value = ((decimal)Client.Balance);
                FindMode = enMFindMode.AfterFill;
            }

            else
            {
                Client.FirstName = tbFirstNameUp.Text;
                Client.LastName = tbLastNamUp.Text;
                Client.Email = tbEmailUp.Text;
                Client.Phone = tbPhoneUp.Text;
                Client.PinCode = int.Parse(tbPinCodeUp.Text);
                Client.Balance = Convert.ToInt16(nudBalanceUp.Value);
                if (MessageBox.Show("Are you sure to Update this Client", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (Client.Save())
                    {
                        MessageBox.Show("Your Client Updated Successfullly", "Updating Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _ClearDataForUpdatePage();
                    }
                }

            }
        }
        private void cbAccNumbers_Click(object sender, EventArgs e)
        {
           ClientsDataView = clsClient.GetAccNumberOfClients().DefaultView;

           cbAccNumbers.Items.Clear();

           for (int i = 0; i < ClientsDataView.Count; i++)
           {
               cbAccNumbers.Items.Add(ClientsDataView[i][0]);
           }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            dgvClientsData.DataSource = clsClient.GetAllClients();

        }

        private void DeletetoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (clsClient.DeleteClient((int)dgvClientsData.CurrentCell.Value))
            {
                MessageBox.Show("Your Client is deleting", "Delete Client" , MessageBoxButtons.OK ,MessageBoxIcon.Information);
                dgvClientsData.DataSource = clsClient.GetAllClients();
                lblClientNumberFound.Text = clsClient.CountClient().ToString();
            }
        }

      

        private void DeposittoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmTransactions frm = new FrmTransactions((int)dgvClientsData.CurrentCell.Value);
            frm.FillDepositPage();
            frm.ShowDialog();
            RefreshDataClient();

        }


        private void tabPage3_Click(object sender, EventArgs e)
        {
            _RefreshData();

        }

        private void WithdrawtoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FrmTransactions frm = new FrmTransactions((int)dgvClientsData.CurrentCell.Value);
            frm.FillWithrawPage();
            //frm.tpWithraw.Select();
            //frm.tpWithraw_Click(sender, e);
            frm.ShowDialog();
            //frm.tpWithraw.SelectedTab = frm.tpWithraw;
            RefreshDataClient();

        }
    }
}
