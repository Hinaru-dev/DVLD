using DVLD_Business_Logic;
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
    public partial class frmManageUsers : Form
    {

        private enum enFilter { None = -1, UserID = 0, PersonID = 1, FullName, UserName = 3, isActive = 4 };
        private enum enIsActiveFilter { All = -1, No = 0, Yes = 1 };

        public frmManageUsers()
        {
            InitializeComponent();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            PopulateUsersList();
            UpdateRecordsCount();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            AddNewUser();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedItem.ToString() == enFilter.None.ToString())
            {
                tbFilter.Visible = false;
                cbIsActiveFilter.Visible = false;
            }
            else if (cbFilter.SelectedItem.ToString() == enFilter.isActive.ToString())
            {
                tbFilter.Visible = false;
                cbIsActiveFilter.Visible = true;
            }
            else
            {
                cbIsActiveFilter.Visible = false;
                tbFilter.Visible = true;
            }
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dgvUsersList.DataSource;

            if (tbFilter.Text == "")
            {
                bs.Filter = string.Empty;
            }

            else
            {
                enFilter ColumnToBeFiltered = (enFilter)(cbFilter.SelectedIndex - 1);

                // if this column value is numerical:
                if (ColumnToBeFiltered == enFilter.PersonID || ColumnToBeFiltered == enFilter.UserID)
                    bs.Filter = ColumnToBeFiltered.ToString() + " = " + tbFilter.Text;

                // in case of ColumnToBeFiltered == enFilter.isActive
                    // this event won't be invoked due to tbFilter is invisible

                // else if its string:
                else
                    bs.Filter = ColumnToBeFiltered.ToString() + " Like '%" + tbFilter.Text + "%'";
            }

            dgvUsersList.DataSource = bs.DataSource;
            OnUsersListUpdated(sender);
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            enFilter ColumnIndex = (enFilter)(cbFilter.SelectedIndex - 1);

            // if this column value is numerical:
            // allow only numerical input
            if (ColumnIndex == enFilter.UserID || ColumnIndex == enFilter.PersonID || ColumnIndex == enFilter.isActive)
            {
                if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            // only allow chars, spaces, control (eg:backspace) input for fullname
            if (ColumnIndex == enFilter.FullName)
            {
                if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            // prevent space input for username filter
            if (ColumnIndex == enFilter.UserName)
            {
                if (char.IsWhiteSpace(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void cbIsActiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dgvUsersList.DataSource;

            if (cbIsActiveFilter.SelectedIndex-1 == (int)enIsActiveFilter.All)
            {
                bs.Filter = string.Empty;
            }

            else
            {
                // get int value to be inserted in filter query (No=0=false, Yes=1=true)
                byte cbIsActiveFilterValue = cbIsActiveFilter.SelectedItem.ToString() == enIsActiveFilter.Yes.ToString() ? (byte)enIsActiveFilter.Yes : (byte)enIsActiveFilter.No;

                // filter users accordingly
                bs.Filter = cbFilter.SelectedItem.ToString() + " = " + cbIsActiveFilterValue.ToString();

            }

            dgvUsersList.DataSource = bs.DataSource;
            OnUsersListUpdated(sender);
        }

        private void cbFilter_Validating(object sender, CancelEventArgs e)
        {
            // in case of wrong user input in Filter combo box
            if (!cbFilter.Items.Contains(cbFilter.Text))
            {
                cbFilter.Text = enFilter.None.ToString();
                cbFilter_SelectedIndexChanged(sender, EventArgs.Empty);
            }
        }

        // --------------------
        //   custom functions 
        // --------------------

        private void AlterUsersList(ref DataTable UsersList)
        {
            // modify userlist datatable to show proper new format:
            // userid, personid, fullname, username, is active
            // current is userid, personid, username, password, isactive(bit)

            // Get the original position (ordinal) of the column
            // it should be inserted right before username
            int originalPosition = UsersList.Columns["Username"].Ordinal;

            // Add a temporary column with the NEW data type
            UsersList.Columns.Add("Fullname", typeof(String));
            int LastPosition = UsersList.Columns["Fullname"].Ordinal;

            // BeginLoadData / EndLoadData: Use these methods to turn off index maintenance and constraints while you are updating. This significantly speeds up bulk changes. (when have thousands of records)
            UsersList.BeginLoadData();
            try
            {
                // Loop through all rows to convert the values
                foreach (DataRow row in UsersList.Rows)
                {
                    string UserFullName = clsUser.getUserFullname((int)row["personid"]);
                    row[LastPosition] = UserFullName;
                }
            }
            finally
            {
                UsersList.EndLoadData();
            }

            // Remove the oassword column
            UsersList.Columns.Remove("password");

            // Move the column to its Proper "middle" position
            UsersList.Columns["Fullname"].SetOrdinal(originalPosition);
        }

        private void PopulateUsersList()
        {
            DataTable UsersList = clsUser.getAllUsers();

            // modify userlist datatable to show proper new format:
            // userid, personid, fullname, username, is active
            AlterUsersList(ref UsersList);

            dgvUsersList.DataSource = UsersList;
        }

        private void UpdateRecordsCount()
        {
            lblRecordsCount.Text = dgvUsersList.Rows.Count.ToString();
        }

        private void OnUsersListUpdated(object sender, bool needListRefresh = false)
        {
            if (needListRefresh)
                PopulateUsersList();
            UpdateRecordsCount();
        }

        private void AddNewUser()
        {
            frmAddEditUserInfo frmAddEditUserInfo = new frmAddEditUserInfo();
            frmAddEditUserInfo.DataBack += OnUsersListUpdated;
            frmAddEditUserInfo.ShowDialog();
        }

        private void EditUser(int UserID)
        {
            frmAddEditUserInfo frmAddEditUserInfo = new frmAddEditUserInfo(UserID);
            frmAddEditUserInfo.DataBack += OnUsersListUpdated;
            frmAddEditUserInfo.ShowDialog();
        }

        private bool isDeletionConfirmed(int UserID)
        {
            string UserFullName = dgvUsersList.SelectedRows[0].Cells["Fullname"].Value.ToString();

            if (MessageBox.Show("Are you sure you want to Delete " + UserFullName + " with ID:" + UserID + "?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                return true;
            }

            return false;
        }

        private void deleteUser(int UserID)
        {
            if (isDeletionConfirmed(UserID))
            {
                clsUser User = clsUser.Find(UserID);

                if (User.Delete(UserID))
                {
                    MessageBox.Show("Deleted Successfully!", "Deletion Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    bool RefreshUsersListFlag = true;
                    OnUsersListUpdated(tsmiDelete, RefreshUsersListFlag);
                }
                else
                    MessageBox.Show("Failed to Delete User Info.", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowUserDetails(int UserID)
        {
            frmUserCardDetails frmUserCardDetails = new frmUserCardDetails(UserID);
            frmUserCardDetails.DataBack += OnUsersListUpdated;
            frmUserCardDetails.ShowDialog();
        }

        private void changeUserPassword(int UserID)
        {
            frmChangePassword frmChangePassword = new frmChangePassword(UserID);
            frmChangePassword.ShowDialog();
        }

        private void NotReadyFeatureMessage()
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // --------------------------------
        // 
        //  right clicked on Users's list
        //
        // --------------------------------

        private void tsmiShowDetails_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsersList.SelectedRows[0].Cells["UserID"].Value;
            ShowUserDetails(UserID);
        }

        private void tsmiAddNewUser_Click(object sender, EventArgs e)
        {
            AddNewUser();
        }

        private void tsmiEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsersList.SelectedRows.Count == 1)
            {
                DataGridViewRow SelectedRow = dgvUsersList.SelectedRows[0];
                EditUser((int)SelectedRow.Cells["userid"].Value);
            }
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsersList.SelectedRows[0].Cells["UserID"].Value;
            deleteUser(UserID);
        }

        private void tsmiSendEmail_Click(object sender, EventArgs e)
        {
            // send email code will be here, god willing
            NotReadyFeatureMessage();
        }

        private void tsmiPhoneCall_Click(object sender, EventArgs e)
        {
            // phone call selected person code will be here, god willing
            NotReadyFeatureMessage();
        }

        private void tsmiChangePassword_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsersList.SelectedRows[0].Cells["UserID"].Value;
            changeUserPassword(UserID);
        }
    }
}
