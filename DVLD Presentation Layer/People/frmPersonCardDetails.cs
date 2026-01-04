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
    public partial class frmPersonCardDetails : Form
    {
        // no need to make personID variable but potato pc stack overflowed when calling 
        // ctrlPersonCard.PersonID
        private int _PersonID;
        
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, bool RefreshPeopleList);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public frmPersonCardDetails(int PersonID)
        {
            _PersonID = PersonID;

            InitializeComponent();

            ctrlPersonCard1.LoadPersonInfo(_PersonID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        private void OnPeopleListUpdated(object sender, bool needListRefresh = false)
        {
            // Trigger the event to send data back to Form1
            bool refreshPeopleList = true;
            if (DataBack != null)
                DataBack.Invoke(this, refreshPeopleList);   
        }

        private void lnklblEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmAddEditPersonInfo edit = new FrmAddEditPersonInfo(_PersonID);
            edit.DataBack += OnPeopleListUpdated;
            edit.ShowDialog();
        }
    }
}
