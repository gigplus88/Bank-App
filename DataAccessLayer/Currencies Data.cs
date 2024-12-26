using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Policy;

namespace DataAccessLayer
{
    public class clsCurrenciesData
    {
        public static DataTable GetAllCurrencies()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "Select * From MondialCurrency";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }

            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;
        }


        public static DataTable CurrenciesDataTable = new DataTable();
        public static DataView CurrenciesDataView = new DataView();
        public static DataView GetCurrenciesWithCode(string Code)         
        {
            CurrenciesDataTable = GetAllCurrencies();

            CurrenciesDataView = CurrenciesDataTable.DefaultView;

            CurrenciesDataView.RowFilter = "[CurrencyCode] = '" + Code + "'";

            return CurrenciesDataView;
        }

        public static DataView GetCurrenciesWithCountry(string Country)
        {
            CurrenciesDataTable = GetAllCurrencies();

            CurrenciesDataView = CurrenciesDataTable.DefaultView;

            CurrenciesDataView.RowFilter = "[Country] = '" + Country + "'";

            return CurrenciesDataView;
        }
        public static DataView GetAllCurrenciesAsc()
        {
            CurrenciesDataTable = GetAllCurrencies();

            CurrenciesDataView = CurrenciesDataTable.DefaultView;

            CurrenciesDataView.Sort = "Country Asc";

            return CurrenciesDataView;
        }
        public static DataView GetAllCurrenciesDesc()
        {
            CurrenciesDataTable = GetAllCurrencies();

            CurrenciesDataView = CurrenciesDataTable.DefaultView;

            CurrenciesDataView.Sort = "Country Desc";

            return CurrenciesDataView;
        }

        public static bool FindCountry(string Country)
        {

            CurrenciesDataTable = GetAllCurrencies();

            CurrenciesDataView = CurrenciesDataTable.DefaultView;

            CurrenciesDataView.RowFilter = "[Country] = '" + Country + "'";

            return (CurrenciesDataView.Count == 1);
        }
        public static bool FindCode(string Code)
        {

            CurrenciesDataTable = GetAllCurrencies();

            CurrenciesDataView = CurrenciesDataTable.DefaultView;

            CurrenciesDataView.RowFilter = "[CurrencyCode] = '" + Code + "'";

            return (CurrenciesDataView.Count == 1);
        }

        public static bool GetCurrenciesInfoByCode(ref string Country , ref string Code, ref string Name  ,ref double Rate )
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "SELECT * FROM MondialCurrency " +
                         "  Where CurrencyCode = @Code";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Code", Code);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    Country = (string)reader["Country"];
                    Name = (string)reader["CurrencyName"];
                    Rate = Convert.ToDouble(reader["RateInUSD"]);
                    
                }
                else
                {
                    isFound = false;
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                isFound = false;
                //Console.WriteLine("Error: " + ex.Message);
            }

            finally
            {

                connection.Close();

            }

            return isFound;

        }
        public static DataTable GetAllCurrenciesCode()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "Select CurrencyCode From MondialCurrency";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }

            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static bool UpdateCurrencyRate(string Country,  double Rate)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string Query = " Update  MondialCurrency "
                            + "set RateInUSD = @Rate"
                            + " where Country = @Country ";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@Rate", Rate);
            command.Parameters.AddWithValue("@Country", Country);

            try
            {
                connection.Open();

                RowsAffected = command.ExecuteNonQuery();

            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (RowsAffected>0);
        }
    }
}
