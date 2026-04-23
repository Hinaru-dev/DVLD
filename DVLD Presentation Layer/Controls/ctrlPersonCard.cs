using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business_Logic;

namespace DVLD_Presentation_Layer.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        clsPerson _Person;

        private int _PersonID = -1;
        
        public int PersonID
        {
            get { return _PersonID; }
        }

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, bool RefreshPeopleList);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if (_Person == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No Person with Pesron id = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillPersonInfo();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No Person with National No. = " + NationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillPersonInfo();
        }

        public void ResetPersonInfo()
        {
            _ResetPersonInfo();
        }

        private void _FillPersonInfo()
        {
            _PersonID = _Person.PersonID;
         
            lblPersonID.Text = _Person.PersonID.ToString();
            lblName.Text = _Person.FullName();
            lblNationalNo.Text = _Person.NationalNo;

            if (_Person.Gender == 0)
            {
                pbGender.Image = Properties.Resources.male;
                lblGender.Text = "Male";
            }
            else
            {
                pbGender.Image = Properties.Resources.male;
                lblGender.Text = "Female";
            }

            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _Person.Phone;

            lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;

            // set a default person photo before checking if there is a picture or not
                pbPersonPhoto.Image = Properties.Resources.personMan;

            if (_Person.ImagePath != string.Empty)
            {
                if (System.IO.File.Exists(_Person.ImagePath))
                {
                    pbPersonPhoto.Load(_Person.ImagePath);
                }   
            }
            else
            {
                if (_Person.ImagePath != string.Empty && _Person.ImagePath != null)
                    MessageBox.Show("Couldn't find this image: " + _Person.ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lnklblEditPersonInfo.Enabled = true;
        }

        private void _ResetPersonInfo()
        {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblName.Text = "[????]";
            lblNationalNo.Text = "[????]";
            pbGender.Image = Properties.Resources.genders;
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblAddress.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";
            pbPersonPhoto.Image = Properties.Resources.manQuestion;
            lnklblEditPersonInfo.Enabled = false;
        }
        private void OnPeopleListUpdated(object sender, bool needListRefresh = false)
        {
            // reload person data if needed
            if (needListRefresh)
                LoadPersonInfo(_PersonID);

            // Trigger the event to send data back to previous forms
            bool refreshPeopleList = needListRefresh;
            if (DataBack != null)
                DataBack.Invoke(this, refreshPeopleList);
        }

        private void lnklblEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmAddEditPersonInfo edit = new FrmAddEditPersonInfo(_PersonID);
            edit.DataBack += OnPeopleListUpdated;
            edit.ShowDialog();
        }
    }
}
