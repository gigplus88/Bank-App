using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class clsUsersDataAccess
    {
        public static bool GetUserInfoByID(ref int UserID, ref string UserName)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString);

            string query = "SELECT UserName FROM Users " +
                         "  Where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    UserName = (string)reader["UserName"];

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
        public static bool GetUserInfoByUserName(ref int UserID, ref string UserName)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString);

            string query = "SELECT UserID FROM Users " +
                         "  Where UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

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
        public static bool GetUserPasswordByUserName(ref string UserName, ref string Password)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString);

            string query = "SELECT Password FROM Users " +
                         "  Where UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    Password = (string)reader["Password"];

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
        public static bool GetUsertInfoByUserName(ref int UserID, ref string UserName, ref string FirstName, ref string LastName, ref string Email,
         ref string Phone, ref int Password, ref int Permissions)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "SELECT * FROM Users " +
                         "  Where UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

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
                    Password = (int)reader["Password"];
                    Permissions = (int)reader["Permissions"];
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
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "Select * From Users";

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

        public static DataTable GetUserNameAndPasswordUser(string UserName, string Password)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString);

            string query = "Select UserName , Password From Users " +
                           "where UserName = @UserName and Password = @Password";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);


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

        public static int GetUserID(string UserName)
        {
            int UserID = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "Select UserID From Users " +
                           "where UserName = @UserName ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);


            try
            {
                connection.Open();
                 object result = command.ExecuteScalar();
            

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }

            }

            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return UserID;
        }
        public static bool IsUserWithpasswordtExist(string UserName, int Password)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "SELECT Found=1 FROM Users " +
                         "  Where UserName = @UserName and  Password = @Password";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();

                // My Solution
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

        public static DataTable UsersDataTable = new DataTable();
        public static DataView UsersDataView = new DataView();

        public static DataView FindUser(string UserName)
        {
            UsersDataTable = GetAllUsers();

            UsersDataView = UsersDataTable.DefaultView;

            UsersDataView.RowFilter = "UserName = '" + UserName + "'";

            return UsersDataView;
        }
        public static int CountUser()
        {
            UsersDataTable = GetAllUsers();

            UsersDataView = UsersDataTable.DefaultView;

            return UsersDataView.Count;

        }

        public static DataView UserByIDDesc()
        {
            UsersDataTable = GetAllUsers();

            UsersDataView = UsersDataTable.DefaultView;

            UsersDataView.Sort = "UserID Desc ";

            return UsersDataView;
        }
        public static DataView UserByIDAsc()
        {
            UsersDataTable = GetAllUsers();

            UsersDataView = UsersDataTable.DefaultView;

            UsersDataView.Sort = "UserID Asc ";

            return UsersDataView;
        }
        public static bool IsUserExist(string UserName)
        {
            UsersDataTable = GetAllUsers();

            UsersDataView = UsersDataTable.DefaultView;

            UsersDataView.RowFilter = "UserName = '" + UserName + "'";

            return (UsersDataView.Count != 0);
        }

        public static string AddNewUser( int UserID ,string UserName , string FirstName, string LastName, string Email,
         string Phone, int Password, int Permissions)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);
            UserID = 12;
            string  Query = "INSERT INTO Users( UserID , UserName  ,FirstName ,LastName, Email  , Phone, Password, Permissions )" +
                " VALUES (@UserID , @UserName, @FirstName ,@LastName,@Email ,@Phone ,@Password,@Permissions)"
                + "Select SCOPE_IDENTITY(); ";


            //"SELECT SCOPE_IDENTITY();

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@Permissions", Permissions);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null  && !int.TryParse(result.ToString(), out int InsertedID))
                {
                    UserName = result.ToString();
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

            return UserName;

        }
        public static bool UpdateUser( int UserID , string UserName, string FirstName, string LastName, string Email,
         string Phone, int Password, int Permissions)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string Query = " Update  Users "
                            + "set FirstName = @FirstName,"
                            + "LastName = @LastName,"
                            + "Email = @Email,"
                            + "Phone = @Phone,"
                            + "Password = @Password,"
                            + "Permissions = @Permissions"
                            + " where UserName = @UserName ";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@Permissions", Permissions);


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

        public static DataTable GetUserNameOfUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "Select UserName From Users";

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
        public static bool DeleteUser(string UserName)
        {
            int RowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionUserString);

            string query = "DELETE FROM Users" +
                             " WHERE UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                RowsAffected =  command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }

            finally
            {
                connection.Close();
            }
            return (RowsAffected>0);

            //return RowsAffected;
        }



    }
}
