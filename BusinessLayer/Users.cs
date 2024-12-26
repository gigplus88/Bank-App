using DataAccessLayer;
using System.Data;


namespace BusinessLayer
{
    public class clsUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Password { get; set; }
        public int Permission { get; set; }
        public enum enMode
        {
            AddNew = 0,
            Update = 1
        };
        public enMode Mode = enMode.AddNew;

        public clsUser()
        {
            this.UserName = "";
            this.LastName = "";
            this.Email = "";
            this.Phone = "";
            this.Password = 0;
            this.Permission = 0;
            Mode = enMode.AddNew;
        }

        private clsUser(int UserID, string UserName, string FirstName, string LastName, string Email, string Phone, int Password, int Permission)
        {
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Password = Password;
            this.Permission = Permission;
            Mode = enMode.Update;

        }

        private clsUser(string UserName, int Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }
        //public static clsUser Find(int UserID)
        //{
        //    string UserName = "";

        //    if (clsUsersDataAccess.GetUserInfoByID(ref ID, ref UserName))
        //    {
        //        return new clsUser(ID, UserName);
        //    }

        //    else
        //    {
        //        return null;
        //    }
        //}

        public static clsUser Find(string UserName)
        {
            int UserID = 0 , Password = 0 , Permission= 0;
            string FirstName = "", LastName = "", Email = "", Phone = "";

            if (clsUsersDataAccess.GetUsertInfoByUserName(ref UserID, ref UserName, ref FirstName, ref LastName, ref Email, ref Phone,
                ref Password, ref Permission))
            {
                return new clsUser( UserID,  UserName,  FirstName,  LastName,  Email,  Phone,  Password,  Permission);
            }

            else
            {
                return null;
            }
        }

        //public static clsUser FindPasswordByUserName(string UserName)
        //{
        //    string Password = "";

        //    if (clsUsersDataAccess.GetUserPasswordByUserName(ref UserName, ref Password))
        //    {
        //        return new clsUser(UserName, Password);

        //    }

        //    else
        //    {
        //        return null;
        //    }
        //}

        public static DataTable GetAllUsers()
        {
            return clsUsersDataAccess.GetAllUsers();
        }

        public static DataTable GetUserNameAndPasswordUsers(string UserName, string Password)
        {
            return clsUsersDataAccess.GetUserNameAndPasswordUser(UserName, Password);
        }


        public static int GetUserID(string UserName)
        {
            return clsUsersDataAccess.GetUserID(UserName);
        }

        public static bool IsUserWithPasswordExist(string UserName, int Password)
        {
            return clsUsersDataAccess.IsUserWithpasswordtExist(UserName, Password);
        }


        public static DataView FindUser(string UserName)
        {
            return clsUsersDataAccess.FindUser(UserName);
        }

        public static int CountUser()
        {
            return clsUsersDataAccess.CountUser();

        }

        public static DataView UserByIDAsc()
        {
            return clsUsersDataAccess.UserByIDAsc();
        }

        public static DataView UserByIDDesc()
        {
            return clsUsersDataAccess.UserByIDDesc();
        }

        public static bool IsUserExist(string UserName)
        {
            return clsUsersDataAccess.IsUserExist(UserName);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    _UpdateUser();
                    return true;
            }
            return false;
        }
        private bool _AddNewUser()
        {
            return this.UserName ==clsUsersDataAccess.AddNewUser( this.UserID ,this.UserName,this.FirstName, this.LastName, this.Email, this.Phone,
                this.Password, this.Permission);
        }
        private bool _UpdateUser()
        {
            return clsUsersDataAccess.UpdateUser(this.UserID, this.UserName, this.FirstName, this.LastName, this.Email, this.Phone,
                this.Password, this.Permission);
        }

        public static DataTable GetUserNameOfUsers()
        {
            return clsUsersDataAccess.GetUserNameOfUsers();
        }
        public static bool DeleteUser(string UserName)
        {
            return (clsUsersDataAccess.DeleteUser(UserName));

        }
    }
}
