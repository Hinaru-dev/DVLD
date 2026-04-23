using DVLD_Business_Logic;
using DVLD_Presentation_Layer.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation_Layer
{
    public partial class frmChangePassword : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, bool RefreshPeopleList);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();

            ctrlUserCard1.LoadUserInfo(UserID);
            //subscribe to card's people update event 
            ctrlUserCard1.DataBack += OnPeopleListUpdated;
        }

        private void OnPeopleListUpdated(object sender, bool needListRefresh = false)
        {
            // Trigger the event to send data back to target previous form
            bool refreshPeopleList = needListRefresh;
            if (DataBack != null)
                DataBack.Invoke(this, refreshPeopleList);
        }

        private void _PasswordsNotMatchingError(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text != tbNewPassword.Text)
            {
                //tb.Focus();
                ErrorProvider.SetError(tb, "Password confirmation does not match password. Try again");
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

        private bool _isValidInput(string Username)
        {
            bool isValidInput = true;

            // non-nullable fields validation:
            if (epEmptyInputField.GetError(tbCurrentPassword) != string.Empty)
                return isValidInput = false;

            if (epEmptyInputField.GetError(tbNewPassword) != string.Empty)
                return isValidInput = false;

            if (epEmptyInputField.GetError(tbConfirmPassword) != string.Empty)
                return isValidInput = false;

            // password confirmation mismatch validation
            if (ErrorProvider.GetError(tbConfirmPassword) != string.Empty)
                return isValidInput = false;

            // validate Current Password
            if (!clsUser.isValidLoginInfo(Username, tbCurrentPassword.Text))
                return isValidInput = false;

            return isValidInput;
        }

        private void _FillUserNewPassword(ref clsUser User)
        {
            User.Password = tbNewPassword.Text;
        }

        // -------------------------
        //          events
        // -------------------------

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsUser User = clsUser.Find(ctrlUserCard1.UserID);

            if (_isValidInput(User.Username))
            {
                _FillUserNewPassword(ref User);

                if (User.Save())
                {
                    MessageBox.Show("password saved Successfully!", "Saving Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Failed to Save new password.", "Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
                MessageBox.Show("Please Make Sure to fill needed Input Fields Properly Before Saving", "Invalid Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            _PasswordsNotMatchingError(sender, e);
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
