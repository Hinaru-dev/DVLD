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

        private void _InitializeForm()
        {
            InitializeComponent();

            _loadCountriesList();
            
            _RestrictPersonAgePicker();
        }

        public FrmAddEditPersonInfo()
        {
            _InitializeForm();
            
            Mode = enMode.AddNew;

            tbEmail.Text = string.Empty;
            tbFirstName.Text = string.Empty;
            tbSecondName.Text = string.Empty;
            tbThirdName.Text = string.Empty;
            tbLastName.Text = string.Empty;
            tbNationalNo.Text = string.Empty;
            tbPhone.Text = string.Empty;
            tbAddress.Text = string.Empty;
        }

        public FrmAddEditPersonInfo(int PersonID)
        {
            _InitializeForm();

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
                tbAddress.Text = Person.Address;
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
                    _rbGenderChangedProtocol();
                }

                if (Person.ImagePath != string.Empty && Person.ImagePath != null)
                {
                    pbPersonImage.ImageLocation = Person.ImagePath;
                    pbPersonImage.Tag = enImageTag.PersonImage.ToString();
                    lnklblRemove.Visible = true;

                    // flag image as original to be ready in case of delete
                    lnklblRemove.Tag = string.Empty;
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

        private void _loadCountriesList()
        {
            cbCountry.DataSource = clsCountry.getAllCountriesList();
            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "CountryID";
        }

        private DateTime _getDateRestrictedByLegalAge()
        {
            double DaysIn4Years = 1461; // (365 * 3 + 366)
            double LegalAgeInDays = Math.Ceiling(4.5 * DaysIn4Years); // 18 years = 4.5 * 4 years = 6575days

            return DateTime.Now.Subtract(TimeSpan.FromDays(LegalAgeInDays));
        }

        private void _RestrictPersonAgePicker()
        { 
            dtpDateOfBirth.MaxDate = _getDateRestrictedByLegalAge();
        }

        private void _InputAlreadyExistsError(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            
            if(clsPerson.Find(tb.Text) != null)
            {
                e.Cancel = true;
                tb.Focus();
                ErrorProvider.SetError(tb, tb.Text + " Already exists, Please pick another one!");
            }
            else
            {
                e.Cancel=false;
                ErrorProvider.SetError(tb, "");
            }
        }

        private void _EmptyTextBoxError(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text == string.Empty)
            {
                e.Cancel = true;
                tb.Focus();
                ErrorProvider.SetError(tb, "Required Field, Please fill your information!");
            }
            else
            {
                e.Cancel = false;
                ErrorProvider.SetError(tb, "");
            }
        }

        private void _InvalidEmailFormatError(CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(tbEmail.Text.Trim()))
                return;

            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(tbEmail.Text.Trim());
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                tbEmail.Focus();
                ErrorProvider.SetError(tbEmail, "Invalid Email format, please enter your right email!\n" + ex.Message);
                return;
            }

            // if no exception catched then valid email format
            e.Cancel = false;
            ErrorProvider.SetError(tbEmail, "");
        }

        private bool _isValidInput()
        {
            bool isValidInput = true;

            // non-nullable fields validation:
            // like non nullable fields to not be empty
            if (ErrorProvider.GetError(tbFirstName) != string.Empty)
                return isValidInput = false;
            
            if (ErrorProvider.GetError(tbLastName) != string.Empty)
                return isValidInput = false;

            // in case there is already exist nationalNo input
            // or if it's empty (non-nullable field) 
            if (ErrorProvider.GetError(tbNationalNo) != string.Empty)
                return isValidInput = false;

            if (ErrorProvider.GetError(tbPhone) != string.Empty)
                return isValidInput = false;
            
            if (ErrorProvider.GetError(tbAddress) != string.Empty)
                return isValidInput = false;

            if (!(rbMale.Checked || rbFemale.Checked))
            {
                MessageBox.Show("Please pick your gender first", "No Gender Choice");
                return isValidInput = false;
            }

            if (cbCountry.Text == string.Empty)
                return isValidInput = false;

            return isValidInput;
        }

        // copy person picture to a local folder then return its path
        private string _CopyPersonImageBeforeSaving()
        {
            // guid naming and copy-saving feature should be done on dataAccessLevel i believe
            
            // prevent null exception when comparing tag to enImageTag
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

        private void _FillPersonInfo()
        {
            Person.FirstName = tbFirstName.Text;
            Person.SecondName = tbSecondName.Text == string.Empty ? null : tbSecondName.Text;
            Person.ThirdName = tbThirdName.Text == string.Empty ? null : tbThirdName.Text;
            Person.LastName = tbLastName.Text;
            Person.NationalNo = tbNationalNo.Text;
            Person.Email = tbEmail.Text == string.Empty ? null : tbEmail.Text;
            Person.Phone = tbPhone.Text;
            Person.Address = tbAddress.Text;
            Person.DateOfBirth = dtpDateOfBirth.Value;
            Person.NationalityCountryID = (int)cbCountry.SelectedValue;

            if (rbMale.Checked)
                Person.Gender = (byte)enGender.Male;

            else // if (rbFemale.Checked)
                Person.Gender = (byte)enGender.Female;


            // if original image is flaged for deletion
            if (lnklblRemove.Tag != null)
                // only delete if person saves Changes or reset the image by another 
                _DeleteImage(lnklblRemove.Tag.ToString());

            Person.ImagePath = _CopyPersonImageBeforeSaving();
        }

        private void _rbGenderChangedProtocol()
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

        private void _DeleteImage(string ImagePath)
        {
            if (System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
                Person.ImagePath = null;
                lnklblRemove.Tag = null;
            }
        }

        private void _ResetImage(bool HideRemoveLinkLabel = true)
        {
            // only flag original image here to be deleted by saving image paths
            if (lnklblRemove.Tag.ToString() == string.Empty)
                lnklblRemove.Tag = pbPersonImage.ImageLocation;

            // Hide lnklblRemove
            if (HideRemoveLinkLabel)
            {
                lnklblRemove.Visible = false;
                // Reset pbImage Tag To gendered & image to proper default one
                pbPersonImage.Tag = string.Empty;
                _rbGenderChangedProtocol();
            }
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

            if (_isValidInput())
            {
                _FillPersonInfo();

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
                if (pbPersonImage.Tag.ToString() == enImageTag.PersonImage.ToString())
                    _ResetImage(false); // _ResetImage(bool HideRemoveLinkLabel);

                pbPersonImage.ImageLocation = ofdPersonImage.FileName;
                pbPersonImage.Tag = enImageTag.PersonImage.ToString();
                lnklblRemove.Visible = true;
            }
        }

        private void tbNationalNo_Validating(object sender, CancelEventArgs e)
        {
            _InputAlreadyExistsError(sender, e);
            _EmptyTextBoxError(sender, e);
        }

        private void rbMale_Click(object sender, EventArgs e)
        {

            _rbGenderChangedProtocol();
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            _rbGenderChangedProtocol();
        }

        private void lnklblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ResetImage();
        }

        private void tbFirstName_Validating(object sender, CancelEventArgs e)
        {
            _EmptyTextBoxError(sender, e);
        }

        private void tbLastName_Validating(object sender, CancelEventArgs e)
        {
            _EmptyTextBoxError(sender, e);
        }

        private void tbPhone_Validating(object sender, CancelEventArgs e)
        {
            _EmptyTextBoxError(sender, e);
        }

        private void tbAddress_Validating(object sender, CancelEventArgs e)
        {
            _EmptyTextBoxError(sender, e);
        }

        private void tbEmail_Validating(object sender, CancelEventArgs e)
        {
            // validate email form
            _InvalidEmailFormatError(e);
        }
    }
}