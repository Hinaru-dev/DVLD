using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business_Logic;

namespace DVLD_Presentation_Layer
{
    public partial class FrmAddEditPersonInfo : Form
    {
        private clsPerson Person;
        public enum enMode { AddNew = 0, Update = 1 }
        private enum enGender { Male = 0, Female = 1 }
        private enum enImageTag { MaleImage = 0, FemaleImage = 1, PersonImage = 2 }


        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, bool RefreshPeopleList);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        private enMode Mode;

        private void InitializeForm()
        {
            InitializeComponent();

            loadCountriesList();
            
            RestrictPersonAgePicker();
        }

        public FrmAddEditPersonInfo()
        {
            InitializeForm();
            
            Mode = enMode.AddNew;

            tbEmail.Text = string.Empty;
            tbFirstName.Text = string.Empty;
            tbSecondName.Text = string.Empty;
            tbThirdName.Text = string.Empty;
            tbLastName.Text = string.Empty;
            tbNationalNo.Text = string.Empty;
            tbPhone.Text = string.Empty;
            rtbAddess.Text = string.Empty;
        }

        public FrmAddEditPersonInfo(int PersonID)
        {
            InitializeForm();

            Mode = enMode.Update;
            Person = clsPerson.Find(PersonID);

            if (Person != null)
            {
                lblPersonID.Text = Person.PersonID.ToString();
                tbFirstName.Text = Person.FirstName;
                tbSecondName.Text = Person.SecondName;
                tbThirdName.Text = Person.ThirdName;
                tbLastName.Text = Person.LastName;
                tbNationalNo.Text = Person.NationalNo;
                tbEmail.Text = Person.Email;
                tbPhone.Text = Person.Phone;
                rtbAddess.Text = Person.Address;
                dtpDateOfBirth.Value = Person.DateOfBirth;
                cbCountry.SelectedValue = Person.NationalityCountryID;

                if ((enGender)Person.Gender == enGender.Male)
                {
                    rbMale.Checked = true;
                }

                if ((enGender)Person.Gender == enGender.Female)
                {
                    rbFemale.Checked = true;
                    // since female isn't default gender image update it
                    rbGenderChangedProtocol();
                }

                if (Person.ImagePath != string.Empty && Person.ImagePath != null)
                {
                    pbPersonImage.ImageLocation = Person.ImagePath;
                    pbPersonImage.Tag = enImageTag.PersonImage.ToString();
                }
            }
            else
            {
                MessageBox.Show("No Person Found With that ID.", "Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Dispose();
            }
        }

        // -----------------------------------
        // 
        //    custom written logic/methods
        //
        // -----------------------------------

        private void loadCountriesList()
        {
            cbCountry.DataSource = clsCountry.getAllCountriesList();
            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "CountryID";
        }

        private DateTime getDateRestrictedByLegalAge()
        {
                     double DaysIn4Years = 1461; // (365 * 3 + 366)
            double LegalAgeInDays = Math.Ceiling(4.5 * DaysIn4Years); // 18 years = 4.5 * 4 years = 6575days

            return DateTime.Now.Subtract(TimeSpan.FromDays(LegalAgeInDays));
        }

        private void RestrictPersonAgePicker()
        { 
            dtpDateOfBirth.MaxDate = getDateRestrictedByLegalAge();
        }

        private void InputAlreadyExistsError(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            
            if(clsPerson.Find(tb.Text) != null)
            {
                e.Cancel = true;
                tb.Focus();
                epInputAlreadyExistsError.SetError(tb, tb.Tag + " Already exists, Please pick another one!");
            }
            else
            {
                e.Cancel=false;
                epInputAlreadyExistsError.SetError(tb, "");
            }
        }

        private bool isValidInput()
        {
            bool isValidInput = true;
            
            // in case there is already exist national no input
            if (epInputAlreadyExistsError.GetError(tbNationalNo) != string.Empty)
                return isValidInput = false;

            // non-nullable fields validation:
            // like non nullable fields to not be empty
            if ( tbFirstName.Text  == string.Empty 
              || tbLastName.Text   == string.Empty 
              || tbNationalNo.Text == string.Empty 
              || tbPhone.Text      == string.Empty 
              || rtbAddess.Text    == string.Empty
              || !(rbMale.Checked || rbFemale.Checked)
              || cbCountry.Text == string.Empty)
                return isValidInput = false;

            return isValidInput;
        }

        // copy person picture to a local folder then return its path
        private string CopyPersonImageBeforeSaving()
        {
            // guid naming and copy-saving feature should be done on dataAccessLevel i believe
            if (pbPersonImage.Tag == null)
                return Person.ImagePath;

            if (pbPersonImage.Tag.ToString() == enImageTag.PersonImage.ToString())
            {
                string SavingFolderPath = @"c\DVLD_Pictures\PeopleImages\";

                // if image exists in saving folder then quit
                string CurrentImagePath = System.IO.Path.GetDirectoryName(pbPersonImage.ImageLocation) + @"\";
                if (CurrentImagePath == SavingFolderPath)
                    return pbPersonImage.ImageLocation;

                // create saving folder in disk if not existant
                // createDirectory() checks if exists before creation
                // also parent folders if needed
                System.IO.Directory.CreateDirectory(SavingFolderPath);

                string DestinationImageName = SavingFolderPath + Guid.NewGuid().ToString() + System.IO.Path.GetExtension(pbPersonImage.ImageLocation);
                System.IO.File.Copy(pbPersonImage.ImageLocation, DestinationImageName);

                return DestinationImageName;
            }

            return Person.ImagePath;
        }

        private void FillPersonInfo()
        {
            Person.FirstName = tbFirstName.Text;
            Person.SecondName = tbSecondName.Text == string.Empty ? null : tbSecondName.Text;
            Person.ThirdName = tbThirdName.Text == string.Empty ? null : tbThirdName.Text;
            Person.LastName = tbLastName.Text;
            Person.NationalNo = tbNationalNo.Text;
            Person.Email = tbEmail.Text == string.Empty ? null : tbEmail.Text;
            Person.Phone = tbPhone.Text;
            Person.Address = rtbAddess.Text;
            Person.DateOfBirth = dtpDateOfBirth.Value;
            Person.NationalityCountryID = (int)cbCountry.SelectedValue;

            if (rbMale.Checked)
                Person.Gender = (byte)enGender.Male;

            else // if (rbFemale.Checked)
                Person.Gender = (byte)enGender.Female;

            Person.ImagePath = CopyPersonImageBeforeSaving();
        }

        private void rbGenderChangedProtocol()
        {
            if (pbPersonImage.Tag == null)
                pbPersonImage.Tag = string.Empty;

            if (pbPersonImage.Tag.ToString() == enImageTag.PersonImage.ToString())
                return;

            if (rbMale.Checked && pbPersonImage.Tag.ToString() != enImageTag.MaleImage.ToString())
            {
                pbPersonImage.Image = Properties.Resources.Male_512;
                pbPersonImage.Tag = enImageTag.MaleImage.ToString();
            }

            else if (rbFemale.Checked && pbPersonImage.Tag.ToString() != enImageTag.FemaleImage.ToString())
            {
                pbPersonImage.Image = Properties.Resources.Female_512;
                pbPersonImage.Tag = enImageTag.FemaleImage.ToString();
            }
        }

        
        private void SendDataBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // -----------------------------------
        //
        //  controls and events logic/methods
        //
        // -----------------------------------

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Mode == enMode.AddNew)
                Person = clsPerson.getNewPersonObject();

            if (isValidInput())
            {
                FillPersonInfo();

                if (Person.Save())
                {
                    if (Mode == enMode.AddNew) 
                        Mode = enMode.Update;
                    lblPersonID.Text = Person.PersonID.ToString();
                    MessageBox.Show("saved Successfully!", "Saving Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Failed to Save Person Info.", "Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
                MessageBox.Show("Please Make Sure to fill needed Input Fields Properly Before Saving", "Invalid Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);

            
            // Trigger the event to send data back to Form1
            bool refreshPeopleList = true;
            if (DataBack != null)
                DataBack.Invoke(this, refreshPeopleList);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void lnklblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ofdPersonImage.InitialDirectory = @"C:\";
            ofdPersonImage.Title = "Select Image to Set";
            ofdPersonImage.DefaultExt = "jpeg";
            ofdPersonImage.Filter = "BMP Files (*.BMP;*.DIB,*.RLE)|*.bmp;*.dib;*.rle" 
                + "|JPEG Files (*.JPG;*.JPEG;*.JPE;*.JFIF)|*.jpg;*.jpeg;*.jpe;*.jfif"
                + "|PNG Files (*.PNG)|*.png"
                + "|SVG Files (*.SVG)|*.svg";
            ofdPersonImage.FilterIndex = 2;

            if (ofdPersonImage.ShowDialog() == DialogResult.OK)
            {
                pbPersonImage.ImageLocation = ofdPersonImage.FileName;
                pbPersonImage.Tag = enImageTag.PersonImage.ToString();
            }
        }

        private void tbNationalNo_Validating(object sender, CancelEventArgs e)
        {
            InputAlreadyExistsError(sender, e);
        }

        private void rbMale_Click(object sender, EventArgs e)
        {

            rbGenderChangedProtocol();
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            rbGenderChangedProtocol();
        }
    }
}