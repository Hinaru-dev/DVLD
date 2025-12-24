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
    public partial class FrmAddEditPersonInfo : Form
    {
        public FrmAddEditPersonInfo()
        {
            InitializeComponent();
        }

        // -----------------------------------
        // 
        //    custom written logic/methods
        //
        // -----------------------------------

        private void RestrictPersonAgePicker()
        {
            // restrict Person Age date time picker code here

            double DaysIn4Years = 1461; // (365 * 3 + 366)
            double LegalAgeInDays = Math.Ceiling(4.5 * DaysIn4Years); // 18 years = 4.5 * 4 years = 6575days 

            dtpDateOfBirth.MaxDate = DateTime.Now.Subtract(TimeSpan.FromDays(LegalAgeInDays));
        }


        // -----------------------------------
        //
        //  controls and events logic/methods
        //
        // -----------------------------------

        private void btnSave_Click(object sender, EventArgs e)
        {
            // saving logic code here
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // if there any logic before close add it here
                // close form code here
            this.Dispose();
        }

        private void lnklblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // setting image code here
        }
    }
}
