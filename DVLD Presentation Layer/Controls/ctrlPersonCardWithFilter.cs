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
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void btnFindPerson_Click(object sender, EventArgs e)
        {
            switch (cbFindPersonFilter.SelectedIndex)
            {
                case 0:
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(tbFindPerson.Text));
                    return;
                case 1:
                    ctrlPersonCard1.LoadPersonInfo(tbFindPerson.Text);
                    return;
                default:
                    return;
            }
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            // open adding person Form
        }
    }
}
