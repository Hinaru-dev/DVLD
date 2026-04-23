using DVLD_Business_Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Presentation_Layer.FrmAddEditPersonInfo;

namespace DVLD_Presentation_Layer
{
    public partial class frmAddEditUserInfo : Form
    {
        private clsUser User;
        public enum enMode { AddNew = 0, Update = 1 }

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, bool RefreshPeopleList);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        private enMode Mode;

        private void _subToUserPersonalInfoChangeEvent()
        {
            ctrlPersonCardWithFilter1.DataBack += _OnUsersPersonalInformationUpdate;
        }

        private void _SetFormLabel()
        {
            this.Text = "Update User";
            lblAddEditPersonInfo.Text = "Update User";
        }

        private void _LoadUserPersonalInfo()
        {
            // fill user's personal info
            ctrlPersonCardWithFilter1.FillPersonInfo(User.PersonID);
            ctrlPersonCardWithFilter1.disableFilter();
        }

        private void _SetLoginFields(int userID =-1)
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    tbUsername.Text = string.Empty;
                    tbPassword.Text = string.Empty;
                    tbConfirmPassword.Text = string.Empty;
                    cbIsActive.Checked = true;
                    break;

                case enMode.Update:
                    lblUserID.Text = userID.ToString();
                    tbUsername.Text = User.Username;
                    tbPassword.Text = User.Password;
                    tbConfirmPassword.Text = User.Password;
                    cbIsActive.Checked = User.isActive;
                    break;

                default:
                    break;
            }
        }

        private void _LockPasswordFields()
        {
            tbPassword.Enabled = false;
            tbConfirmPassword.Enabled = false;
        }

        public frmAddEditUserInfo()
        {
            InitializeComponent();
            _subToUserPersonalInfoChangeEvent();

            Mode = enMode.AddNew;

            _SetLoginFields();
        }

        public frmAddEditUserInfo(int userID)
        {
            InitializeComponent();
            _subToUserPersonalInfoChangeEvent();

            Mode = enMode.Update;
            User = clsUser.Find(userID);

            if (User != null)
            {
                _SetFormLabel();
                _SetLoginFields(userID);
                _LoadUserPersonalInfo();
                _LockPasswordFields();
            }
            else
            {
                MessageBox.Show("No User exist With that user ID.", "Unknown User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Dispose();
            }
        }

        // -----------------------------------
        // 
        //    custom written logic/methods
        //
        // -----------------------------------

        private bool isPersonExist()
        {
            return ctrlPersonCardWithFilter1.PersonID != -1;
        }

        private bool isPersonAUser()
        {
            return clsUser.isPersonAUser(ctrlPersonCardWithFilter1.PersonID);
        }

        private void _InputAlreadyExistsError(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (clsUser.isUserExist(tb.Text))
            {
                //tb.Focus();
                ErrorProvider.SetError(tb, tb.Text + " Already exists, Please pick another one!");
            }
            else
            {
                ErrorProvider.SetError(tb, "");
            }
        }

        private void _EmptyTextBoxError(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text == string.Empty)
            {
                //tb.Focus();
                epEmptyInputField.SetError(tb, tb.Tag.ToString() + " cannot be blank");
            }
            else
            {
                epEmptyInputField.SetError(tb, "");
            }
        }

        private void _PasswordsNotMatchingError(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text != tbPassword.Text)
            {
                //tb.Focus();
                ErrorProvider.SetError(tb, "Password confirmation does not match password. Try again");
            }
            else
            {
                ErrorProvider.SetError(tb, "");
            }
        }

        private bool _isValidInput()
        {
            bool isValidInput = true;

            // non-nullable fields validation:
            // like non nullable fields to not be empty
            if (epEmptyInputField.GetError(tbUsername) != string.Empty)
                return isValidInput = false;

            if (epEmptyInputField.GetError(tbPassword) != string.Empty)
                return isValidInput = false;

            if (epEmptyInputField.GetError(tbConfirmPassword) != string.Empty)
                return isValidInput = false;

            // in case there is already exist username input
            // or password confirmation mismatch
            if (ErrorProvider.GetError(tbUsername) != string.Empty)
                return isValidInput = false;

            if (ErrorProvider.GetError(tbConfirmPassword) != string.Empty)
                return isValidInput = false;

            // new accounts should be active ones
            if (Mode == enMode.AddNew && !cbIsActive.Checked)
                return isValidInput = false;

            return isValidInput;
        }

        private void _FillUserInfo()
        {
            User.Username = tbUsername.Text;
            User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            User.Password = tbPassword.Text;
            User.isActive = cbIsActive.Checked;
        }

        private void _OnUsersPersonalInformationUpdate(object sender, bool usersListneedrefresh=false)
        {
            // Trigger the event to send data back to previous forms
            bool refreshUsersList = usersListneedrefresh;
            if (DataBack != null)
                DataBack.Invoke(this, refreshUsersList);
        }

        // -----------------------------------
        //
        //  controls and events logic/methods
        //
        // -----------------------------------

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!isPersonExist())
                MessageBox.Show("Select a Person before creating a User Account!", "Select a Person First", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // if person exist then:
            else
            {
                if (isPersonAUser())
                    MessageBox.Show("Selected Person Already has a User, choose another person", "Select Another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);

                else
                    tcPerson.SelectedTab = tpLoginInfo;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Mode == enMode.AddNew)
            {
                User = clsUser.getNewUserObject();

                // mew accounts should be active:
                // won't rely on client input
                // force it and prevent wrong filling input field message ambiguity
                if (!cbIsActive.Checked)
                    if(MessageBox.Show("new user account must be active, isActive is now checked!", "Invalid account creation parameter \"isActive\"", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                        cbIsActive.Checked = true;
            }

            if (_isValidInput())
            {
                _FillUserInfo();

                if (User.Save())
                {
                    if (Mode == enMode.AddNew)
                        Mode = enMode.Update;

                    lblUserID.Text = User.UserID.ToString();

                    MessageBox.Show("saved Successfully!", "Saving Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Failed to Save User Info.", "Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Trigger the event to send data back to previous forms
                bool refreshUsersList = true;
                if (DataBack != null)
                    DataBack.Invoke(this, refreshUsersList);
            }

            else
                MessageBox.Show("Please Make Sure to fill needed Input Fields Properly Before Saving", "Invalid Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void tbUsername_Validating(object sender, CancelEventArgs e)
        {
            _EmptyTextBoxError(sender, e);
            _InputAlreadyExistsError(sender, e);
        }

        private void tbPassword_Validating(object sender, CancelEventArgs e)
        {
            _EmptyTextBoxError(sender, e);
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            _EmptyTextBoxError(sender, e);
            _PasswordsNotMatchingError(sender, e);
        }

        private void tpLoginInfo_Enter(object sender, EventArgs e)
        {
            // don't allow creating user account in case of:
            // no person selected
            // person already a user
            if (!isPersonExist() || (Mode == enMode.AddNew && isPersonAUser()))
            {
                MessageBox.Show("Select a Person with no user account before creating a new User Account!", "Select a Person First", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // back to person selection tab page
                tcPerson.SelectedTab = tpPersonalInfo;
            }
        }
    }
}
