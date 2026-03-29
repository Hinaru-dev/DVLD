using DVLD_Data_Access;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Logic
{
    public class clsUser
    {
        enum enMode { AddNewMode = 0, UpdateMode = 1 };
        enMode Mode;

        public int PersonID { get; set; }
        public int UserID { get; set; }
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

        clsUser(int PersonID, int UserID, string Personname, string Password, bool isActive)
        {
            this.PersonID = UserID;
            this.UserID = UserID;
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


            bool isFound = clsUserData.GetUserInfoByID(UserID, ref UserID, ref Username, ref Password, ref isActive);


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


            bool isfound = clsUserData.GetUserInfoByUsername(Username, ref UserID, ref UserID, ref Password, ref isActive);


            if (isfound)
                return new clsUser(UserID, PersonID, Username, Password, isActive);

            else
            return null;
        }

        public static bool ValidateLoginInfo(String Username, string password)
        {
            clsUser user = clsUser.Find(Username);

            if (user != null)
                if (password == user.Password)
                    return true;

            return false;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewUser())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
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

        public static DataTable getAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public static clsUser getNewUserObject()
        {
            return new clsUser();
        }

        public bool Delete(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        public static string getUserFullname(int personID)
        {
            clsPerson Person = clsPerson.Find(personID);
            return Person.FullName();
        }

        public static bool isPersonAUser(int personID)
        {
            return clsUserData.IsPersonAUser(personID);
        }
    
        public static bool isUserExist(string username)
        {
            return (clsUserData.IsUserExist(username));
        }
    }
}