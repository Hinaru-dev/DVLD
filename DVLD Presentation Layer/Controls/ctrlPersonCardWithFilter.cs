using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business_Logic;

namespace DVLD_Presentation_Layer.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        enum enfilter { PersonID = 0, NationalNo = 1}
        public int PersonID = -1;

        // -----------------------------------
        //  notify on person info card update
        // -----------------------------------

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, bool RefreshPeopleList);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        void OnPersonInfoUpdate(object sender, bool needListRefresh = false)
        {
            // Trigger the event to send data back to previous forms
            bool refreshPeopleList = needListRefresh;
            if (DataBack != null)
                DataBack.Invoke(this, refreshPeopleList);
        }

        private void subToPersonInfoUpdate()
        {
            ctrlPersonCard1.DataBack += OnPersonInfoUpdate;
        }

        // -----------------------------------
        // -----------------------------------

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
            subToPersonInfoUpdate();
        }

        private void btnFindPerson_Click(object sender, EventArgs e)
        {
            switch ((enfilter)cbFindPersonFilter.SelectedIndex)
            {
                case enfilter.PersonID:
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(tbFindPerson.Text));
                    break;
                case enfilter.NationalNo:
                    ctrlPersonCard1.LoadPersonInfo(tbFindPerson.Text);
                    break;
                default:
                    return;
            }

            PersonID = ctrlPersonCard1.PersonID;
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            AddNewPerson();
        }

        private void AddNewPerson()
        {
            FrmAddEditPersonInfo frmAddEditPersonInfo = new FrmAddEditPersonInfo();
            frmAddEditPersonInfo.ShowDialog();
        }

        public void FillPersonInfo(int personID)
        {
            cbFindPersonFilter.SelectedIndex = (int)enfilter.PersonID;
            tbFindPerson.Text = personID.ToString();
            btnFindPerson_Click(btnFindPerson, EventArgs.Empty);
        }

        public void disableFilter()
        {
            gbFilter.Enabled = false;
        }
    }
}
