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
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        // --------------------------------
        //        custom functions 
        // --------------------------------

        private void PopulateApplicationTypesList()
        {
            DataTable ApplicationTypesList = clsApplicationType.GetAllApplicationTypes();

            dgvApplicationTypesList.DataSource = ApplicationTypesList;
        }

        private void UpdateRecordsCount()
        {
            lblRecordsCount.Text = dgvApplicationTypesList.Rows.Count.ToString();
        }
        private void OnApplicationTypesListUpdated(object sender, bool needListRefresh = false)
        {
            if (needListRefresh)
                PopulateApplicationTypesList();
        }

        private void EditApplicationType(int ApplicationTypeID)
        {
            frmEditApplicationType frmEditApplicationType = new frmEditApplicationType(ApplicationTypeID);
            frmEditApplicationType.DataBack += OnApplicationTypesListUpdated;
            frmEditApplicationType.ShowDialog();
        }


        // --------------------------------
        //        events and controls
        // --------------------------------

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            PopulateApplicationTypesList();
            UpdateRecordsCount();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        
        // --------------------------------
        //  right clicked on ApplicationTypes's list
        // --------------------------------

        private void tsmiEdit_Click(object sender, EventArgs e)
        {
            if (dgvApplicationTypesList.SelectedRows.Count == 1)
            {
                DataGridViewRow SelectedRow = dgvApplicationTypesList.SelectedRows[0];
                EditApplicationType((int)SelectedRow.Cells["ApplicationTypeID"].Value);
            }
        }
    }
}
