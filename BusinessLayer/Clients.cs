using DataAccessLayer;
using System;
using System.Data;

namespace BusinessLayer
{
    public class clsClient
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PinCode { get; set; }
        public int Balance { get; set; }
        public int UserID { get; set; }
        public enum enMode
        {
            AddNew = 0,
            Update = 1
        };
        public enMode Mode = enMode.AddNew;

        public clsClient()
        {
            this.ID = 0;
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.Phone = "";
            this.PinCode = 0;
            this.Balance = 0;
            this.UserID = 0;
            Mode = enMode.AddNew;

        }
        public DataTable ClientsDataTable = new DataTable();
        public DataView ClientsDataView = new DataView();


        public clsClient(int ID, string FirstName, string LastName, string Email, string Phone, int PinCode
            , int Balance, int UserID)
        {
            this.ID = ID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.PinCode = PinCode;
            this.Balance = Balance;
            this.UserID = UserID;
            Mode = enMode.Update;
        }
      

        private bool _UpdateClient()
        {
            return clsClientsDataAccess.UpdateClient(this.ID, this.FirstName, this.LastName, this.Email, this.Phone, this.PinCode,
                this.Balance, this.UserID);
        }
        public  bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddnewClient())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    _UpdateClient();
                    return true;
            }
            return false;
        }
        private bool _AddnewClient()
        {
            return this.ID ==clsClientsDataAccess.AddnewClient(this.ID, this.FirstName, this.LastName, this.Email, this.Phone, 
                this.PinCode, this.Balance, this.UserID);
       }
        
        public static bool DeleteClient(int ID)
        {
            return (clsClientsDataAccess.DeleteClient(ID));

        }

        public static DataTable GetAllClients()
        {
            return clsClientsDataAccess.GetAllClients();
        }


        public static DataView ClientsByAccNumberDesc()
        {
            return clsClientsDataAccess.GetAllClientsDesc();
        }
        public static DataView ClientsByAccNumbeAsc()
        {
            return clsClientsDataAccess.GetAllClientsAsc();
        }

        public static DataView FindClient(int ID)
        {
            return clsClientsDataAccess.FindClient(ID);
        }

        public static DataView FindSenderClient(int ID)
        {
            return clsClientsDataAccess.FindSenderClient(ID);

        }

        public static DataTable GetAllClientsSender()
        {
            return clsClientsDataAccess.GetAllClients();
        }
        public static clsClient FindClientWithFilling(int ID)
        {
            string FirstName = "", LastName = "", Email = "", Phone = "";
            int Balance = 0;
            int UserID =0 , PinCode = 0;

            if (clsClientsDataAccess.GetClientInfoByID(ref ID, ref FirstName, ref LastName, ref Email, ref Phone, ref PinCode,
                ref Balance, ref UserID))
            {
                return new clsClient(ID, FirstName, LastName, Email, Phone, PinCode,
                Balance, UserID);
            }

            else
            {
                return null;
            }
        }

        public static int CountClient()
        {
            return clsClientsDataAccess.CountClient();
        }

        public static bool IsClientExist(int ID)
        {
            return clsClientsDataAccess.IsClientExist2(ID);
        }

        public static DataTable GetAccNumberOfClients()
        {
            return clsClientsDataAccess.GetAccNumberOfClients();
        }

        public static int TotalBalances()
        {
            return clsClientsDataAccess.GetTotallBalance();
        }
    }


}
