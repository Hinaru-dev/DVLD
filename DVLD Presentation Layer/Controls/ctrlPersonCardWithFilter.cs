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

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
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
    }
}
