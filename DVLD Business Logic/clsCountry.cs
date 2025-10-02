using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Data_Access;

namespace DVLD_Business_Logic
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        
        clsCountry()
        {
            CountryID    = -1;
            CountryName  = string.Empty;
        }

        clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }

        public static clsCountry Find(int CountryID)
        {
            string CountryName = "";

            bool isFound = clsCountryData.GetCountryInfoByID(CountryID, ref CountryName);

            if (isFound)
                return new clsCountry(CountryID, CountryName);

            else
                return null;
        }

        public static clsCountry Find(string CountryName)
        {

            int CountryID = -1;

            bool isFound = clsCountryData.GetCountryInfoByCountryName(CountryName, ref CountryID);

            
            if (isFound)
                return new clsCountry(CountryID, CountryName);

            else
                return null;
        }
    }
}
