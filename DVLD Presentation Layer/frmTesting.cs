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
    public partial class frmTesting : Form
    {
        public frmTesting()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (rbUserID.Checked)
                ctrlUserCard1.LoadUserInfo(int.Parse(tbInput.Text));
            
            else // which means rbUsername is Checked	
                ctrlUserCard1.LoadUserInfo(tbInput.Text);

        }
    }
}
