using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Data_Access;

namespace DVLD_Business_Logic
{
    public class clsUser
    {
        enum enMode { AddNewMode = 0, UpdateMode = 1 };
        enMode Mode;

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool isActive { get; set; }

        clsUser()
        {
            UserID = -1;
            PersonID = -1;
            Username = string.Empty;
            Password = string.Empty;
            isActive = true;
        }

        clsUser(int UserID, int PersonID, string Username, string Password, bool isActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.Username = Username;
            this.Password = Password;
            this.isActive = isActive;
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.Username, this.Password, this.isActive);

            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.Username, this.Password, this.isActive);
        }

        public static clsUser Find(int UserID)
        {
            int PersonID = -1;
            string Username = "", Password = "";
            bool isActive = true;


            bool isFound = clsUserData.GetUserInfoByID(UserID, ref PersonID, ref Username, ref Password, ref isActive);


            if (isFound)
                return new clsUser(UserID, PersonID, Username, Password, isActive);

            else
            return null;
        }

        public static clsUser Find(string Username)
        {
            int UserID = -1, PersonID = -1;
            string Password = "";
            bool isActive = true;


            bool isfound = clsUserData.GetUserInfoByUsername(Username, ref UserID, ref PersonID, ref Password, ref isActive);


            if (isfound)
                return new clsUser(UserID, PersonID, Username, Password, isActive);

            else
            return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewUser())
                        return true;
                    else
                        return false;

                case enMode.UpdateMode:
                    if (_UpdateUser())
                        return true;
                    else
                        return false;

                default:
                    return false;
            }
        }
    }
}