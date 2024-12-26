using BusinessLayer;
using System;
using System.Data;
using System.Windows.Forms;
using Humanizer;


namespace Bank_Client
{
    public partial class FrmTransactions : Form
    {
        private string _UserName;
        private int _AccNumber;
        private byte TableLogCreationCount = 0;

     
        public FrmTransactions(string UserName)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this._UserName = UserName;
            _RefreshData();
            _ClientLogTable();
        }

        public FrmTransactions(int AccNumber)
        {
            InitializeComponent();
            this._AccNumber = AccNumber;
            _ClientLogTable();
        }
        public clsClient Client = new clsClient();
        public clsClient Client1 = new clsClient();

        
        public DataView ClientsDataView = new DataView();
        public void FillCbAccNumber2()
        {
            cbAccNumber2.Items.Clear();
            ClientsDataView = clsClient.GetAccNumberOfClients().DefaultView;


            for (int i = 0; i < ClientsDataView.Count; i++)
            {
                cbAccNumber2.Items.Add(ClientsDataView[i][0]);
            }

        }
        private TabControl tabControl;

        public void FillWithrawPage()
        {
            FillCbAccNumber2();
            lblCurrentBalance2.Visible = true;
            lblBalanceWord2.Visible = true;
            lblDollarChar2.Visible = true;
            
            Client = clsClient.FindClientWithFilling(_AccNumber);
            cbAccNumber2.Text = _AccNumber.ToString();
            lblCurrentBalance2.Text = Client.Balance.ToString();
            tpWithraw.Show();
        }
        public void GetAccNumbers(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            ClientsDataView = clsClient.GetAccNumberOfClients().DefaultView;


            for (int i = 0; i < ClientsDataView.Count; i++)
            {
                comboBox.Items.Add(ClientsDataView[i][0]);
            }
        }

        public void FillCbAccNumber()
        {
            cbAccNumbers.Items.Clear();
            ClientsDataView = clsClient.GetAccNumberOfClients().DefaultView;


            for (int i = 0; i < ClientsDataView.Count; i++)
            {
                cbAccNumbers.Items.Add(ClientsDataView[i][0]);
            }

        }
        public void FillDepositPage()
        {
            FillCbAccNumber();

            lblCurrentBalance.Visible = true;
            lblBalanceWord.Visible = true;
            lblDollarChar.Visible = true;

            Client = clsClient.FindClientWithFilling(_AccNumber);
            cbAccNumbers.Text = _AccNumber.ToString();
            lblCurrentBalance.Text = Client.Balance.ToString();

        }
       
        private void cbAccNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAccNumbers.SelectedItem != null && int.TryParse(cbAccNumbers.SelectedItem.ToString(), out int accountNumber))
            {
                lblCurrentBalance.Visible = true;
                lblBalanceWord.Visible = true;
                lblDollarChar.Visible = true;

                Client = clsClient.FindClientWithFilling(accountNumber);

                lblCurrentBalance.Text = Client.Balance.ToString();

            }
            else
            {
                MessageBox.Show("You aren t  any Item");
            }
        }

       
        private void cbAccNumbers_Click(object sender, EventArgs e)
        {
            FillCbAccNumber();
        }
        private void btnDeposit_Click(object sender, EventArgs e)
        {
            Client.Balance += (int)nudBalance.Value;

            if (Client.Save())
            {
                if (MessageBox.Show("Are you sure to Perform this Transaction", "Confirm") == DialogResult.OK)
                {
                    Console.Beep();
                    MessageBox.Show($"Amount has been deposited [{nudBalance.Value}] successfully ");
                    _ClearDepositData();

                    
                   
                }
            }
        }

        private void _ClearDepositData()
        {
            // Deposit
            lblCurrentBalance.Visible = false;
            cbAccNumbers.Text = "";
            lblBalanceWord.Visible = false;
            lblDollarChar.Visible = false;
            nudBalance.Value = 0;

        }

        private void _ClearWithrawData()
        {
            // Deposit
            lblCurrentBalance2.Visible = false;
            cbAccNumber2.Text = "";
            lblBalanceWord2.Visible = false;
            lblDollarChar2.Visible = false;
            nudBalance2.Value = 0;

        }

        void _ClearTransferData()
        {
            cbSenderAccNumber.Text = "";
            cbRecipientAccNumber.Text = "";
            lblSenderCurrentBalance.Visible = false;
            lblRecipientCurrentBalance.Visible = false;
            lblSenderBalance.Visible = false;
            lblRecipientBalance.Visible = false;
            nudBalance3.Value = 0;
            btnTransfer.Enabled = false;
        }

        private void _RefreshData()
        {
            // For Deposit
            _ClearDepositData();
            lblUserName.Text = _UserName;
            lblDate1.Text = DateTime.UtcNow.ToString();

            // For Withraw
            _ClearWithrawData();
            lblUserName2.Text = _UserName;
            lblDate2.Text = DateTime.UtcNow.ToString();

            //For Total Balances
            _TotalBalances();
            lblUserName3.Text = _UserName;
            lblDate3.Text = DateTime.UtcNow.ToString();
            dgvClientsData.DataSource = clsClient.GetAllClients();

            //For Transfer
            _ClearTransferData();
            lblUserName4.Text = _UserName;
            lblDate4.Text = DateTime.UtcNow.ToString();

            //For Transfer Log
            dgvClientsData2.DataSource = GetAllClientsSender();
            lblUserName5.Text = _UserName;
            lblDate5.Text = DateTime.UtcNow.ToString();


        }




        private void FrmTransactions_Load(object sender, EventArgs e)
        {
            //_RefreshData();
        }

        public void AbilityWithrawInfo(Label label)
        {
            btnWithraw.Enabled = true;
            lblCurrentBalance2.Visible = true;
            lblBalanceWord2.Visible = true;
            lblDollarChar2.Visible = true;
        }
        private void cbAccNumber2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cbAccNumber2.SelectedItem != null && int.TryParse(cbAccNumber2.SelectedItem.ToString(), out int accountNumber))
            {
                Client = clsClient.FindClientWithFilling(accountNumber);

                lblCurrentBalance2.Visible = true;
                lblBalanceWord2.Visible = true;
                lblDollarChar2.Visible = true;

                if (Client.Balance == 0)
                {
                    MessageBox.Show("Empty Balance", "Info",MessageBoxButtons.OK);
                   btnWithraw.Enabled = false;
                }
                else
                {
                    btnWithraw.Enabled = true;
                }

                lblCurrentBalance2.Text = Client.Balance.ToString();

            }
            else
            {
                MessageBox.Show("You aren t  any Item");
            }
        }

        private void cbAccNumber2_Click(object sender, EventArgs e)
        {
            cbAccNumber2.Items.Clear();
            GetAccNumbers(cbAccNumber2);
        }

        private void tbTransactions_Click(object sender, EventArgs e)
        {
            _RefreshData();
        }

        
        private void btnWithraw_Click(object sender, EventArgs e)
        {
            Client.Balance -= (int)nudBalance2.Value;

            if (Client.Save())
            {
                if (MessageBox.Show("Are you sure to Perform this Transaction", "Confirm") == DialogResult.OK)
                {
                    Console.Beep();
                    MessageBox.Show($"Amount has been Withrawed [{nudBalance2.Value}] succesfully " , "Info" , MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    _ClearWithrawData();
                }

            }
        }

   
        private void nudBalance2_Click(object sender, EventArgs e)
        {
            if (nudBalance2.Value > Client.Balance)
            {
                nudBalance2.Value = Client.Balance;
            }
        }

        private void nudBalance2_MouseClick(object sender, MouseEventArgs e)
        {
            if (nudBalance2.Value > Client.Balance)
            {
                nudBalance2.Value = Client.Balance;
            }
        }

        private void nudBalance2_ValueChanged(object sender, EventArgs e)
        {
            if (nudBalance2.Value > Client.Balance)
            {
                nudBalance2.Value = Client.Balance;
            }
        }

        public string BalanceToString = clsClient.TotalBalances().ToWords(new System.Globalization.CultureInfo("en"));
        private void _TotalBalances()
        {

            lblTotalBalances.Text = "( $ " + clsClient.TotalBalances().ToString() + ")" ;

            lblBalancesToWords.Text = BalanceToString + " Dollar(s)";

        }

        private void lblBalancesToWords_Click(object sender, EventArgs e)
        {

        }

        private void txtSearchAccNumber_TextChanged(object sender, EventArgs e)
        {

            string input1 = txtSearchAccNumber.Text.Trim();


            if (int.TryParse(input1, out int result))
            {
                dgvClientsData.DataSource = clsClient.FindClient(result);
                lblClientNumberFound.Text = clsClient.FindClient(result).Count.ToString();
            }

            else if (string.IsNullOrEmpty(input1))
            {
                dgvClientsData.DataSource = clsClient.GetAllClients();
                lblClientNumberFound.Text = clsClient.CountClient().ToString();

            }

            else
            {
                MessageBox.Show("You must enter a numbers");

            }
        }

        private void rbDesc_CheckedChanged(object sender, EventArgs e)
        {
            dgvClientsData.DataSource = clsClient.ClientsByAccNumberDesc();

        }

        private void rbAsc_CheckedChanged(object sender, EventArgs e)
        {
            dgvClientsData.DataSource = clsClient.ClientsByAccNumbeAsc();

        }


        int SenderAccNumber = 0;
        private void cbSenderAccNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cbSenderAccNumber.SelectedItem != null && int.TryParse(cbSenderAccNumber.SelectedItem.ToString(), out int accountNumber))
            {
                SenderAccNumber = accountNumber;
                Client = clsClient.FindClientWithFilling(accountNumber);
                lblSenderCurrentBalance.Visible = true;
                lblSenderBalance.Visible = true;

                if (Client.Balance == 0)
                {
                    MessageBox.Show("Empty Balance", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    btnTransfer.Enabled = false;
                }
                else
                {
                    btnTransfer.Enabled = true;
                }

                lblSenderBalance.Text = Client.Balance.ToString();
            }
            else
            {
                MessageBox.Show("You aren t  any Item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbRecipientAccNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRecipientAccNumber.SelectedItem != null && int.TryParse(cbRecipientAccNumber.SelectedItem.ToString(), out int accountNumber))
            {
                Client1 = clsClient.FindClientWithFilling(accountNumber);

                lblRecipientCurrentBalance.Visible = true;
                lblRecipientBalance.Visible = true;              

                lblRecipientBalance.Text = Client1.Balance.ToString();

            }
            else
            {
                MessageBox.Show("You aren t  any Item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbSenderAccNumber_Click(object sender, EventArgs e)
        {
            lblSenderCurrentBalance.Visible = false;
            cbSenderAccNumber.Items.Clear();
            GetAccNumbers(cbSenderAccNumber);
        }


        private void cbRecipientAccNumber_Click(object sender, EventArgs e)
        {

            ClientsDataView = clsClient.GetAccNumberOfClients().DefaultView;
            cbRecipientAccNumber.Items.Clear();

            for (int i = 0; i < ClientsDataView.Count; i++)
            {
                if ((int)ClientsDataView[i][0] == SenderAccNumber)
                {
                    continue;
                }
                cbRecipientAccNumber.Items.Add(ClientsDataView[i][0]);
            }
            
        }

       

        DataTable ClientLogTable = new DataTable();
        DataView ClientLogViewTable = new DataView();


        private void _ClientLogTable()
        {
            TableLogCreationCount++;

            if (TableLogCreationCount == 1)
            {
                ClientLogTable.Columns.Add("Date", typeof(DateTime));
                ClientLogTable.Columns.Add("S.AccN", typeof(int));
                ClientLogTable.Columns.Add("R.AccN", typeof(int));
                ClientLogTable.Columns.Add("Amount", typeof(int));
                ClientLogTable.Columns.Add("S.Balance", typeof(int));
                ClientLogTable.Columns.Add("R.Balance", typeof(int));
                ClientLogTable.Columns.Add("User Name", typeof(string));

            }

        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            Client = clsClient.FindClientWithFilling(int.Parse(cbSenderAccNumber.Text));
            Client1 = clsClient.FindClientWithFilling(int.Parse(cbRecipientAccNumber.Text));


            Client.Balance -= (int)nudBalance3.Value;

            Client1.Balance += (int)nudBalance3.Value;

            if (Client.Save())
            {
                if (Client1.Save())
                {
                    if (MessageBox.Show("Are you sure to Perform this Transaction", "Confirm" , MessageBoxButtons.OKCancel) == DialogResult.OK )
                    {
                        MessageBox.Show($"Amount has been Transfered from [{Client.FirstName}]  to  [{Client1.FirstName}] succesfully ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        /*clsClient.GetAllClientsSender.Rows */
                        ClientLogTable.Rows.Add(DateTime.UtcNow, Client.ID, Client1.ID, (int)nudBalance3.Value, Client.Balance, Client1.Balance, _UserName);

                        _ClearTransferData();
                    }
                }
            }

        }

        private void nudBalance3_ValueChanged(object sender, EventArgs e)
        {
            if (nudBalance3.Value > Client.Balance)
            {
                nudBalance3.Value = Client.Balance;
            }
        }

        private void nudBalance3_Scroll(object sender, ScrollEventArgs e)
        {
            if (nudBalance3.Value > Client.Balance)
            {
                nudBalance3.Value = Client.Balance;
            }
        }

        private void nudBalance3_Click(object sender, EventArgs e)
        {
            if (nudBalance3.Value > Client.Balance)
            {
                nudBalance3.Value = Client.Balance;
            }
        }

        private void lblClientNumberFound_Click(object sender, EventArgs e)
        {

        }

        public  DataView GetAllClientsSender()
        {
            ClientLogViewTable = ClientLogTable.DefaultView;
            ClientLogViewTable.RowFilter = "";

            return ClientLogViewTable;
        }

        public DataView GetClientsSender(int AccN )
        {
            ClientLogViewTable = ClientLogTable.DefaultView;

            ClientLogViewTable.RowFilter = "S.AccN = "+ AccN;

            return ClientLogViewTable;
        }
        private void txtSearchAccNumber2_TextChanged(object sender, EventArgs e)
        {
            string input1 = txtSearchAccNumber2.Text.Trim();


            if (int.TryParse(input1, out int result))
            {
                dgvClientsData2.DataSource = GetClientsSender(result)  /*clsClient.FindSenderClient(result)*/;
                lblClientNumberFound2.Text =/* clsClient.FindSenderClient(result)*/GetClientsSender(result).Count.ToString();
            }

            if (string.IsNullOrEmpty(txtSearchAccNumber2.Text))
            {
                dgvClientsData2.DataSource = GetAllClientsSender();
                lblClientNumberFound2.Text = GetAllClientsSender().Count.ToString();
            }
        }

        public DataView GetClientsSenderAsc()
        {
            ClientLogViewTable = ClientLogTable.DefaultView;

            ClientLogViewTable.Sort = "S.AccN Asc";

            return ClientLogViewTable;
        }
        public DataView GetClientsSenderDesc()
        {
            ClientLogViewTable = ClientLogTable.DefaultView;

            ClientLogViewTable.Sort = "S.AccN Desc";

            return ClientLogViewTable;
        }

        private void rbDesc2_CheckedChanged(object sender, EventArgs e)
        {
            dgvClientsData2.DataSource =  GetClientsSenderDesc();

        }

        private void rbAsc2_CheckedChanged(object sender, EventArgs e)
        {
            dgvClientsData2.DataSource = GetClientsSenderAsc();

        }

        private void tbDeposit_Click(object sender, EventArgs e)
        {

        }

        public void tpWithraw_Click(object sender, EventArgs e)
        {
            tpWithraw.Select();
        }

        private void tpTotalBalances_Click(object sender, EventArgs e)
        {
            dgvClientsData2.DataSource = GetAllClientsSender();
            lblClientNumberFound2.Text = GetAllClientsSender().Count.ToString();
        }

        private void lblBalanceWord_Click(object sender, EventArgs e)
        {

        }

        private void tpTransferLog_Click(object sender, EventArgs e)
        {

        }

        private void tbTransactions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    
}
