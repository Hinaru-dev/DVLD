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
        
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            // filling people list
            dgvPeopleList.DataSource = clsPerson.getAllPeople();

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
                enFilter ColumnIndex = (enFilter)(cbFilter.SelectedIndex - 1);

                // if this column value is numerical:
                if (ColumnIndex == enFilter.personid || ColumnIndex == enFilter.nationalitycountryid || ColumnIndex == enFilter.gendor)
                    bs.Filter = ColumnIndex.ToString() + " = " + tbFilter.Text;

                // else if its string:
                else
                    bs.Filter = ColumnIndex.ToString() + " Like '%" + tbFilter.Text + "%'";

                dgvPeopleList.DataSource = bs.DataSource;
            }

            OnPeopleListUpdated(sender);
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            enFilter ColumnIndex = (enFilter)(cbFilter.SelectedIndex - 1);
            
            // if this column value is numerical:
            if (ColumnIndex == enFilter.personid || ColumnIndex == enFilter.gendor)
            {
                if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        // --------------------
        //   custom functions 
        // --------------------

        private void RefreshPeopleList()
        {
            // refill people's list from db
            dgvPeopleList.DataSource = clsPerson.getAllPeople();
        }
        
        private void UpdateRecordsCount()
        {
            lblRecordsCount.Text = dgvPeopleList.Rows.Count.ToString();
        }

        private void OnPeopleListUpdated(object sender, bool needListRefresh = false)
        {
            if (needListRefresh)
                RefreshPeopleList();
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

        // --------------------------------
        // 
        //  right clicked on people's list
        //
        // --------------------------------
        
        private void tsmiShowDetails_Click(object sender, EventArgs e)
        {
            // show details code here
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
            // delete person code here
        }

        private void tsmiSendEmail_Click(object sender, EventArgs e)
        {
            // send email code here
        }

        private void tsmiPhoneCall_Click(object sender, EventArgs e)
        {
            // phone call selected person code here
        }
    }
}
