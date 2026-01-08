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
    public partial class frmManagePeople : Form
    {
        private enum enFilter { None = -1, personid=0, nationalno=1, firstname=2, secondname=3, thirdname=4, lastname=5, nationalitycountryid=6, gendor=7, phone=8, email=9 };
        private enum enGendor { Male=0, Female=1 };
        
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            PopulatePeopleList();
            UpdateRecordsCount();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            AddNewPerson();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedItem.ToString() == "None")
                tbFilter.Visible = false;
            else
                tbFilter.Visible = true;
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dgvPeopleList.DataSource;

            if (tbFilter.Text == "")
            {
                bs.Filter = string.Empty;
                dgvPeopleList.DataSource = bs.DataSource;
            }

            else
            {
                enFilter ColumnToBeFiltered = (enFilter)(cbFilter.SelectedIndex - 1);

                // if this column value is numerical:
                if (ColumnToBeFiltered == enFilter.personid || ColumnToBeFiltered == enFilter.nationalitycountryid)
                    bs.Filter = ColumnToBeFiltered.ToString() + " = " + tbFilter.Text;

                else if (ColumnToBeFiltered == enFilter.gendor)
                    bs.Filter = ColumnToBeFiltered.ToString() + " Like '" + tbFilter.Text + "%'";

                // else if its string:
                else
                    bs.Filter = ColumnToBeFiltered.ToString() + " Like '%" + tbFilter.Text + "%'";

                dgvPeopleList.DataSource = bs.DataSource;
            }

            OnPeopleListUpdated(sender);
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            enFilter ColumnIndex = (enFilter)(cbFilter.SelectedIndex - 1);
            
            // if this column value is numerical:
            // allow only numerical input
            if (ColumnIndex == enFilter.personid)
            {
                if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            // prevent numerical input for gender filter
            if (ColumnIndex == enFilter.personid)
            {
                if (!char.IsLetter(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

        }

        // --------------------
        //   custom functions 
        // --------------------
        private void CorrectGenderValues(ref DataTable PeopleList)
        {
            // Get the original position (ordinal) of the column
            int originalPosition = PeopleList.Columns["Gendor"].Ordinal;

            // Add a temporary column with the NEW data type
            PeopleList.Columns.Add("Gender_Temp", typeof(String));
            int LastPosition = PeopleList.Columns["Gender_Temp"].Ordinal;

            // BeginLoadData / EndLoadData: Use these methods to turn off index maintenance and constraints while you are updating. This significantly speeds up bulk changes. (when have thousands of records)
            PeopleList.BeginLoadData();
            try
            {
                // Loop through all rows to convert the values
                foreach (DataRow row in PeopleList.Rows)
                {
                    enGendor genderValue = row[originalPosition].ToString() == "1" ? enGendor.Female : enGendor.Male;
                    row[LastPosition] = genderValue.ToString();
                }
            }
            finally
            {
                PeopleList.EndLoadData();
            }

            // Remove the old column
            PeopleList.Columns.Remove("Gendor");

            // Rename the temp column back to the original name
            PeopleList.Columns["Gender_Temp"].ColumnName = "Gendor";

            // Move the column back to its original "middle" position
            PeopleList.Columns["Gendor"].SetOrdinal(originalPosition);
        }

        private void PopulatePeopleList()
        {
            // Filling/Refill people's list from db
            //dgvPeopleList.DataSource = clsPerson.getAllPeople();
            DataTable PeopleList = clsPerson.getAllPeople();

            CorrectGenderValues(ref PeopleList);

            dgvPeopleList.DataSource = PeopleList;
        }
        
        private void UpdateRecordsCount()
        {
            lblRecordsCount.Text = dgvPeopleList.Rows.Count.ToString();
        }

        private void OnPeopleListUpdated(object sender, bool needListRefresh = false)
        {
            if (needListRefresh)
                PopulatePeopleList();
            UpdateRecordsCount();
        }

        private void AddNewPerson()
        {
            FrmAddEditPersonInfo frmAddEditPersonInfo = new FrmAddEditPersonInfo();
            frmAddEditPersonInfo.DataBack += OnPeopleListUpdated;
            frmAddEditPersonInfo.ShowDialog();
        }

        private void EditPerson(int PersonID)
        {
            FrmAddEditPersonInfo frmAddEditPersonInfo = new FrmAddEditPersonInfo(PersonID);
            frmAddEditPersonInfo.DataBack += OnPeopleListUpdated;
            frmAddEditPersonInfo.ShowDialog();
        }

        private bool isDeletionConfirmed(int PersonID)
        {
            string PersonFullName = dgvPeopleList.SelectedRows[0].Cells["FirstName"].Value + " " + dgvPeopleList.SelectedRows[0].Cells["LastName"].Value;

            if (MessageBox.Show("Are you sure you want to Delete "+ PersonFullName + " with ID:" + PersonID + "?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                return true;
            }

            return false;
        }

        private void deletePerson(int PersonID)
        {
            if (isDeletionConfirmed(PersonID))
            {
                clsPerson Person = clsPerson.Find(PersonID);

                if (Person.Delete(PersonID))
                {
                    MessageBox.Show("Deleted Successfully!", "Deletion Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    bool RefreshPeopleListFlag = true;
                    OnPeopleListUpdated(tsmiDelete, RefreshPeopleListFlag);
                }
                else
                    MessageBox.Show("Failed to Delete Person Info.", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowPersonDetails(int PersonID)
        {
            frmPersonCardDetails frmPersonCardDetails = new frmPersonCardDetails(PersonID);
            frmPersonCardDetails.DataBack += OnPeopleListUpdated;
            frmPersonCardDetails.ShowDialog();
        }

        private void NotReadyFeatureMessage()
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // --------------------------------
        // 
        //  right clicked on people's list
        //
        // --------------------------------
        
        private void tsmiShowDetails_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeopleList.SelectedRows[0].Cells["PersonID"].Value;
            ShowPersonDetails(PersonID);
        }

        private void tsmiAddNewPerson_Click(object sender, EventArgs e)
        {
            AddNewPerson();
        }

        private void tsmiEdit_Click(object sender, EventArgs e)
        {
            if (dgvPeopleList.SelectedRows.Count == 1)
            {
                DataGridViewRow SelectedRow = dgvPeopleList.SelectedRows[0];
                EditPerson((int)SelectedRow.Cells["personid"].Value);
            }
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeopleList.SelectedRows[0].Cells["PersonID"].Value;
            deletePerson(PersonID);
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
    }
}
