using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Data_Access;

namespace DVLD_Business_Logic
{
    public class clsPerson
    {
        enum enMode { AddNewMode=0, UpdateMode=1 };
        enMode Mode;

        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName()
        {
            return FirstName + " " + SecondName + " " + ThirdName + " " + LastName;
        }
        public string NationalNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email{set; get;}
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        clsPerson()
        {
            PersonID    = -1;
            NationalNo  = string.Empty;
            FirstName   = string.Empty;
            SecondName  = string.Empty;
            ThirdName   = string.Empty;
            LastName    = string.Empty;
            DateOfBirth = DateTime.Now;
            Gender      = 'M';
            Address     = string.Empty;
            Phone       = string.Empty;
            Email       = string.Empty;
            NationalityCountryID = -1;
            ImagePath   = string.Empty;
        }

        clsPerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string lastName, DateTime DateOfBirth, char Gender, string address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = lastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.NationalNo, this.DateOfBirth, this.Gender, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
        
            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.NationalNo, this.DateOfBirth, this.Gender, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
        }

        public static clsPerson Find(int PersonID)
        {
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "",Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            char Gender = '\0';
            int NationalityCountryID = -1;


            bool isFound = clsPersonData.GetPersonInfoByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);


            if (isFound)
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);

            else
                return null;
        }

        public static clsPerson Find(string NationalNo)
        {

            int PersonID = -1, NationalityCountryID = -1;
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            char Gender = '\0';


            bool isFound = clsPersonData.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);


            if (isFound)
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);

            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
	        {
		        case enMode.AddNewMode:
                    if (_AddNewPerson())
                        return true;
                    else
                        return false;

                case enMode.UpdateMode:
                   if (_UpdatePerson())
                        return true;
                    else
                        return false;
                
                default:
                    return false;
	        }
        }
    }
}
