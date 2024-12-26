using BusinessLayer;
using System;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Bank_Client
{
    public partial class FrmCurrencyExchange : Form
    {
        private string _UserName;
        clsCurrencies Currency = new clsCurrencies();
        clsCurrencies Currency2 = new clsCurrencies();

        public DataTable CurrenciesDataTable = new DataTable();
        public DataView CurrenciesDataView = new DataView();
        public FrmCurrencyExchange(string UserName)
        {
            InitializeComponent();
            this._UserName = UserName;
            this.MaximizeBox = false;
        }

        private void FrmCurrencyExchange_Load(object sender, EventArgs e)
        {
            _RefreshData();
        }

        private void rbDesc_CheckedChanged(object sender, EventArgs e)
        {
            dgvCurrenciesData.DataSource = clsCurrencies.GetAllCurrenciesDesc();

        }

        private void rbAsc_CheckedChanged(object sender, EventArgs e)
        {
            dgvCurrenciesData.DataSource = clsCurrencies.GetAllCurrenciesAsc();

        }

        private void txtSearchUserName_TextChanged(object sender, EventArgs e)
        {
            string input1 = txtSearchUserName.Text.Trim();


            if (!string.IsNullOrEmpty(input1))
            {
                if (clsCurrencies.FindCountry(input1) )
                {
                    dgvCurrenciesData.DataSource = clsCurrencies.GetCurrenciesWithCountry(input1);
                    lblClientNumberFound.Text = clsCurrencies.GetCurrenciesWithCountry(input1).Count.ToString();
                    return;
                }
                else 
                {
                    dgvCurrenciesData.DataSource = clsCurrencies.GetCurrenciesWithCode(input1);
                    lblClientNumberFound.Text = clsCurrencies.GetCurrenciesWithCode(input1).Count.ToString();
                }
               
            }

            else if (string.IsNullOrEmpty(input1))
            {
                dgvCurrenciesData.DataSource =clsCurrencies.GetAlCurrencies();
                lblClientNumberFound.Text = clsCurrencies.GetAlCurrencies().ToString();
            }
        }

        private void _FillLabels()
        {
            lblCountry.Text = Currency.Country;
            lblCode.Text = Currency.Code;
            lblName.Text = Currency.Name;
            lblRate.Text = "(" +Currency.Rate.ToString() + "$)";
        }
        private void cbCurrencyCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCurrencyCode.SelectedItem != null && !int.TryParse(cbCurrencyCode.SelectedItem.ToString(), out int CurrencyCode))
            {

                Currency = clsCurrencies.Find(cbCurrencyCode.SelectedItem.ToString());

                _FillLabels();
            }
            else
            {
                MessageBox.Show("You aren t Items");
            }
        }

        private void cbCurrencyCode_Click(object sender, EventArgs e)
        {
            CurrenciesDataView = clsCurrencies.GetAllCurrenciesCode().DefaultView;

            cbCurrencyCode.Items.Clear();

            for (int i = 0; i < CurrenciesDataView.Count; i++)
            {
                cbCurrencyCode.Items.Add(CurrenciesDataView[i][0]);
            }
        }

       
        private void btnUpdateRate_Click(object sender, EventArgs e)
        {

            Currency.Rate =Convert.ToDouble(nbNewRate.Value);

            if (MessageBox.Show("Are you sure to Update this Currency ", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (Currency.Save())
                {
                    lblRate.Text = "(" +Currency.Rate.ToString() + "$)";
                    MessageBox.Show($" Currency {Currency.Code} Updated Successfully", "Updating Currency", MessageBoxButtons.OK);
                    dgvCurrenciesData.DataSource = clsCurrencies.GetAlCurrencies();

                }
            }
        }

       
        private void _RefreshData()
        {
            // Show Currencies
            dgvCurrenciesData.DataSource = clsCurrencies.GetAlCurrencies();
            lblClientNumberFound.Text = clsCurrencies.GetAlCurrencies().Rows.Count.ToString();
            lblUserName.Text = _UserName;
            lblDate.Text = DateTime.UtcNow.ToString();

            // Update Rate
            //clsCurrencies Currency = new clsCurrencies();
            Currency = clsCurrencies.Find("USD");

            lblUserName1.Text = _UserName;
            lblDate1.Text = DateTime.UtcNow.ToString();

            cbCurrencyCode.Text = Currency.Code;
            lblCountry.Text = Currency.Country;
            lblCode.Text = Currency.Code;
            lblName.Text = Currency.Name;
            lblRate.Text = "(" +Currency.Rate.ToString() + "$)";

            // Currency Calculator
            lblResult.Visible = false;
            lblUserName3.Text = _UserName;
            lblDate3.Text = DateTime.UtcNow.ToString();
        }

        double Result = 0;
        string CountryFrom = "";
        double BaseRate = 0;
        double SubRate = 0;
        double CrossRate = 0;


        private void _FillLabelsToConverting()
        {
            CountryFrom = Currency.Country;
            BaseRate = Currency.Rate;
            lblCountry2.Text = Currency.Country;
            lblCode2.Text = Currency.Code;
            lblName2.Text = Currency.Name;
            lblRate2.Text = "(" +Currency.Rate.ToString() + "$)";
        }
        private void _FillLabelsToReciepting()
        {
            SubRate = Currency2.Rate;
            lblCountry3.Text = Currency2.Country;
            lblCode3.Text = Currency2.Code;
            lblName3.Text = Currency2.Name;
            lblRate3.Text = "(" +Currency2.Rate.ToString() + "$)";
        }


        private double Conversion()
        {
            if (CountryFrom == "Ecuador")
            {
                Result =Math.Round(Convert.ToDouble(nudAmounttoExchange.Value) / SubRate);
            }
            else
            {
                CrossRate = BaseRate /  SubRate;
                Result = Math.Round(Convert.ToDouble(nudAmounttoExchange.Value) * CrossRate);

            }
            return Result;
        }

        private void cbConvertFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbConvertFrom.SelectedItem != null && !int.TryParse(cbConvertFrom.SelectedItem.ToString(), out int CurrencyCode))
            {

                Currency = clsCurrencies.Find(cbConvertFrom.SelectedItem.ToString());

                _FillLabelsToConverting();
            }
            else
            {
                MessageBox.Show("You aren t Items");
            }
        }

        private void cbConvertFrom_Click(object sender, EventArgs e)
        {
            CurrenciesDataView = clsCurrencies.GetAllCurrenciesCode().DefaultView;

            cbConvertFrom.Items.Clear();

            for (int i = 0; i < CurrenciesDataView.Count; i++)
            {
                cbConvertFrom.Items.Add(CurrenciesDataView[i][0]);
            }
        }

        private void cbConvertTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbConvertTo.SelectedItem != null && !int.TryParse(cbConvertTo.SelectedItem.ToString(), out int CurrencyCode))
            {

                Currency2 = clsCurrencies.Find(cbConvertTo.SelectedItem.ToString());

                _FillLabelsToReciepting();
            }
            else
            {
                MessageBox.Show("You aren t Items");
            }
        }

        private void cbConvertTo_Click(object sender, EventArgs e)
        {
            CurrenciesDataView = clsCurrencies.GetAllCurrenciesCode().DefaultView;

            cbConvertTo.Items.Clear();

            for (int i = 0; i < CurrenciesDataView.Count; i++)
            {
                cbConvertTo.Items.Add(CurrenciesDataView[i][0]);
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            lblResult.Visible = true;
            
            lblResult.Text = nudAmounttoExchange.Value + $" {Currency.Code} " + "=" + Conversion()+ $" {Currency2.Code}";
        }

        private void tpShowCurrencies_Click(object sender, EventArgs e)
        {
            dgvCurrenciesData.DataSource = clsCurrencies.GetAlCurrencies();
        }
    }
}
