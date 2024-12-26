using System;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessLayer
{
    public class clsClientsDataAccess
    {
        public static bool GetClientInfoByID(ref int ID, ref string FirstName, ref string LastName, ref string Email,
            ref string Phone, ref int PinCode, ref int Balance, ref int UserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "SELECT * FROM Clients " +
                         "  Where AccNumber = @AccNumber";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AccNumber", ID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    FirstName = (string)reader["FirstName"];
                    LastName = (string)reader["LastName"];
                    Email = (string)reader["Email"];
                    Phone = (string)reader["Phone"];
                    PinCode = (int)reader["PinCode"];
                    Balance = (int)reader["Balance"];
                    UserID = (int)reader["UserID"];

                   
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


        public static int AddnewClient(int AccNumber , string FirstName, string LastName, string Email,
             string Phone, int PinCode, double Balance, int UserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string Query = "INSERT INTO Clients(AccNumber ,   FirstName ,LastName,Email  ,Phone,PinCode, Balance , UserID)" +
                " VALUES (@AccNumber , @FirstName ,@LastName,@Email ,@Phone ,@PinCode,@Balance, @UserID)"
                + "Select SCOPE_IDENTITY(); ";


            //"SELECT SCOPE_IDENTITY();

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@AccNumber", AccNumber);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@PinCode", PinCode);
            command.Parameters.AddWithValue("@Balance", Balance);
            command.Parameters.AddWithValue("@UserID", UserID);
           
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    AccNumber = InsertedID;
                }

            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return AccNumber;

        }

        public static bool UpdateClient(int AccNumber, string FirstName, string LastName, string Email,
             string Phone, int PinCode, double Balance, int UserID)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string Query = " Update  Clients "
                            + "set FirstName = @FirstName,"
                            + "LastName = @LastName,"
                            + "Email = @Email,"
                            + "Phone = @Phone,"
                            + "PinCode = @PinCode,"
                            + "Balance = @Balance,"
                            + "UserID = @UserID"
                            + " where AccNumber = @AccNumber ";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@AccNumber", AccNumber);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@PinCode", PinCode);
            command.Parameters.AddWithValue("@Balance", Balance);
            command.Parameters.AddWithValue("@UserID", UserID);
            

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

        public static bool DeleteClient(int ID)
        {
            int RowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "DELETE FROM Clients" +
                             " WHERE AccNumber = @AccNumber";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AccNumber", ID);

            try
            {
                connection.Open();
                RowsAffected =  command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                Console.WriteLine("EArror " + ex.Message);
            }

            finally
            {
                connection.Close();
            }
            return (RowsAffected>0);

            //return RowsAffected;
        }


        public static DataTable GetAllClients()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "Select * From Clients";

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


        public static DataTable GetAccNumberOfClients()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "Select AccNumber From Clients";

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


        public static DataTable ClientsDataTable = new DataTable();
        public static DataView ClientsDataView = new DataView();


        public static DataView GetAllClientsDesc()
        {
            ClientsDataTable = GetAllClients();

            ClientsDataView = ClientsDataTable.DefaultView;

            ClientsDataView.Sort = "AccNumber Desc";
            return ClientsDataView;
        }

        public static DataView GetAllClientsAsc()
        {
            ClientsDataTable = GetAllClients();

            ClientsDataView = ClientsDataTable.DefaultView;

            ClientsDataView.Sort = "AccNumber Asc";

            return ClientsDataView;
        }

        public static DataView FindClient(int ID)
        {
            ClientsDataTable = GetAllClients();

            ClientsDataView = ClientsDataTable.DefaultView;

            ClientsDataView.RowFilter = "AccNumber = '"+ID + "'";

            return ClientsDataView;
        }

        public static DataTable ClientLogTable = new DataTable();
        public static DataView ClientLogViewTable = new DataView();
        public static int TableLogCreationCount = 0;
        private static DataTable _ClientLogTable()
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
            return ClientLogTable;
        }

        public static DataView GetAllClientsSender()
        {
            ClientsDataTable = GetAllClients();

            ClientsDataView = ClientsDataTable.DefaultView;

            ClientsDataView.Sort = "AccNumber Asc";

            return ClientsDataView;
        }
        public static DataView FindSenderClient(int ID)
        {
            ClientLogTable = _ClientLogTable();
            ClientLogViewTable = ClientLogTable.DefaultView;
            ClientLogViewTable.RowFilter = "S.AccN = '" + ID + "'";

            return ClientLogViewTable;
        }
        
        public static int CountClient()
        {
            ClientsDataTable = GetAllClients();

            ClientsDataView = ClientsDataTable.DefaultView;

           return ClientsDataView.Count;

        }

        // Other Method for IsClientExist
        public static bool IsClientExist2(int ID)
        {
            ClientsDataTable = GetAllClients();

            ClientsDataView = ClientsDataTable.DefaultView;

            ClientsDataView.RowFilter = "AccNumber = '" + ID +"'";

            return (ClientsDataView.Count != 0);
        }

        public static bool IsClientExist(int ID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "SELECT Found=1 FROM Clients " +
                         "  Where AccNumber = @AccNumber";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AccNumber", ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();

                // My second Solution
                //object result = command.ExecuteScalar(); 

                //if (result != null)
                //{
                //    isFound = true;
                //}
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

        public static int GetTotallBalance()
        {
            int TotallBalance = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "Select sum(Balance) From Clients";


            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString() , out int MyTotall))
                {
                    TotallBalance = MyTotall;
                }
                
            }

            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return TotallBalance;
        }
    }

}
