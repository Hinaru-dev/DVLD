using DVLD_Data_Access;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Logic
{
    public class clsApplicationType
    {
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public float ApplicationTypeFees { get; set; }
        
        public clsApplicationType()
        {
            ApplicationTypeID = -1;
            ApplicationTypeTitle = string.Empty;
            ApplicationTypeFees = -1;
        }

        clsApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float ApplicationTypeFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.ApplicationTypeFees = ApplicationTypeFees;
        }

        public static clsApplicationType Find(int ApplicationTypeID)
        {
            string ApplicationTypeTitle = "";
            float ApplicationTypeFees = -1;


            bool isFound = clsApplicationTypeData.GetApplicationTypeInfoByID(ApplicationTypeID, ref ApplicationTypeTitle, ref ApplicationTypeFees);


            if (isFound)
                return new clsApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationTypeFees);

            else
                return null;
        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.Update(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationTypeFees);
        }

        public bool Save()
        {
            if (_UpdateApplicationType())
                return true;
            else
                return false;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();
        }
    }
}
