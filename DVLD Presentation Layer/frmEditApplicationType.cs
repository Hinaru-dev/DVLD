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
    public partial class frmEditApplicationType : Form
    {
        private clsApplicationType _ApplicationType;

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, bool RefreshPeopleList);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public frmEditApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _InitializeFormFields(ApplicationTypeID);
        }

        private void _InitializeFormFields(int ApplicationTypeID)
        {
            _ApplicationType = clsApplicationType.Find(ApplicationTypeID);

            lblApplicationTypeID.Text = ApplicationTypeID.ToString();
            tbApplicationTypeTitle.Text = _ApplicationType.ApplicationTypeTitle;
            tbApplicationTypeFees.Text = _ApplicationType.ApplicationTypeFees.ToString();
        }

        // -----------------------------------
        // 
        //    custom written logic/methods
        //
        // -----------------------------------

        private void _FillApplicationTypeInfo()
        {
            _ApplicationType.ApplicationTypeID = _ApplicationType.ApplicationTypeID;
            _ApplicationType.ApplicationTypeTitle = tbApplicationTypeTitle.Text;
            _ApplicationType.ApplicationTypeFees = float.Parse(tbApplicationTypeFees.Text);
        }

        // -----------------------------------
        //
        //  controls and events logic/methods
        //
        // -----------------------------------

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _FillApplicationTypeInfo();

            if (_ApplicationType.Save())
            { 
                MessageBox.Show("saved Successfully!", "Saving Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Failed to Save ApplicationType Info.", "Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Trigger the event to send data back to previous forms
            bool refreshApplicationTypesList = true;
            if (DataBack != null)
                DataBack.Invoke(this, refreshApplicationTypesList);
        }

        private void tbApplicationTypeFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allow only numerical input and dots (floating point)
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.Equals(e.KeyChar, '.'))
            {
                e.Handled = true;
            }
        }
    }
}