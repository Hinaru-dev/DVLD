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
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        // --------------------------------
        //        custom functions 
        // --------------------------------

        private void PopulateTestTypesList()
        {
            DataTable TestTypesList = clsTestType.GetAllTestTypes();

            dgvTestTypesList.DataSource = TestTypesList;
        }

        private void UpdateRecordsCount()
        {
            lblRecordsCount.Text = dgvTestTypesList.Rows.Count.ToString();
        }
        private void OnTestTypesListUpdated(object sender, bool needListRefresh = false)
        {
            if (needListRefresh)
                PopulateTestTypesList();
        }

        private void EditTestType(int TestTypeID)
        {
            frmEditTestType frmEditTestType = new frmEditTestType(TestTypeID);
            frmEditTestType.DataBack += OnTestTypesListUpdated;
            frmEditTestType.ShowDialog();
        }


        // --------------------------------
        //        events and controls
        // --------------------------------

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            PopulateTestTypesList();
            UpdateRecordsCount();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // --------------------------------
        //  right clicked on TestTypes's list
        // --------------------------------

        private void tsmiEditTestType_Click(object sender, EventArgs e)
        {
            if (dgvTestTypesList.SelectedRows.Count == 1)
            {
                DataGridViewRow SelectedRow = dgvTestTypesList.SelectedRows[0];
                EditTestType((int)SelectedRow.Cells["TestTypeID"].Value);
            }
        }
    }
}
