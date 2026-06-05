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
    public partial class frmEditTestType : Form
    {
        private clsTestType _TestType;

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, bool RefreshPeopleList);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public frmEditTestType(int TestTypeID)
        {
            InitializeComponent();
            _InitializeFormFields(TestTypeID);
        }

        private void _InitializeFormFields(int TestTypeID)
        {
            _TestType = clsTestType.Find(TestTypeID);

            lblTestTypeID.Text = TestTypeID.ToString();
            tbTestTypeTitle.Text = _TestType.TestTypeTitle;
            rtbTestTypeDescription.Text = _TestType.TestTypeDescription;
            tbTestTypeFees.Text = _TestType.TestTypeFees.ToString();
        }

        // -----------------------------------
        // 
        //    custom written logic/methods
        //
        // -----------------------------------

        private void _FillTestTypeInfo()
        {
            _TestType.TestTypeID = _TestType.TestTypeID;
            _TestType.TestTypeTitle = tbTestTypeTitle.Text;
            _TestType.TestTypeDescription = rtbTestTypeDescription.Text;
            _TestType.TestTypeFees = float.Parse(tbTestTypeFees.Text);
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
            _FillTestTypeInfo();

            if (_TestType.Save())
            {
                MessageBox.Show("saved Successfully!", "Saving Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Failed to Save Test Type Info.", "Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Trigger the event to send data back to previous forms
            bool refreshTestTypesList = true;
            if (DataBack != null)
                DataBack.Invoke(this, refreshTestTypesList);
        }

        private void tbTestTypeFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allow only numerical input and dots (floating point)
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.Equals(e.KeyChar, '.'))
            {
                e.Handled = true;
            }
        }
    }
}
