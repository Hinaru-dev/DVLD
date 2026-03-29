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
                tbFilter.Visible = false;
            else
                tbFilter.Visible = true;
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
            if (ColumnIndex == enFilter.UserID || ColumnIndex == enFilter.PersonID)
            {
                if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            // prevent numerical input for gender filter
            if (ColumnIndex == enFilter.PersonID)
            {
                if (!char.IsLetter(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void cbFilter_Validating(object sender, CancelEventArgs e)
        {
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
            //frmAddEditUserInfo.DataBack += OnPeopleListUpdated;
            frmAddEditUserInfo.ShowDialog();
        }

    }
}
