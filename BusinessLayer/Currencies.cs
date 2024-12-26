using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsCurrencies
    {
        public string Country { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
       
        public clsCurrencies() 
        {
            Country = string.Empty;
            Code = string.Empty;
            Name = string.Empty;
            Rate = 0;
        }

        private clsCurrencies(string Country, string Code, string Name, double Rate)
        {
            this.Country=Country;
            this.Code=Code;
            this.Name=Name;
            this.Rate=Rate;
        }

        public static DataTable GetAlCurrencies()
        {
            return clsCurrenciesData.GetAllCurrencies();
        }

        public static DataView GetCurrenciesWithCode(string Code)
        {
            return clsCurrenciesData.GetCurrenciesWithCode(Code);
        }

        public static DataView GetCurrenciesWithCountry(string Country)
        {
            return clsCurrenciesData.GetCurrenciesWithCountry(Country);
        }
        public static DataView GetAllCurrenciesAsc()
        {
            return clsCurrenciesData.GetAllCurrenciesAsc();
        }
        public static DataView GetAllCurrenciesDesc()
        {
            return clsCurrenciesData.GetAllCurrenciesDesc();
        }
        public static bool FindCountry(string Country)
        {
            return clsCurrenciesData.FindCountry(Country);
        }
        public static bool FindCode(string Code)
        {
            return clsCurrenciesData.FindCode(Code);
        }
        public static clsCurrencies Find(string Code)
        {
            string Country = "" , Name = "";
            double Rate = 0;

            if (clsCurrenciesData.GetCurrenciesInfoByCode(ref  Country, ref  Code, ref  Name, ref  Rate))
            {
                return new clsCurrencies(Country,  Code,  Name,  Rate);
            }

            else
            {
                return null;
            }
        }

        public static DataTable GetAllCurrenciesCode()
        {
            return clsCurrenciesData.GetAllCurrenciesCode();
        }

        private bool _UpdateRate()
        {
            return clsCurrenciesData.UpdateCurrencyRate(this.Country , this.Rate);
        }
        public bool  Save()
        {
            return _UpdateRate();
        }
    }
}
