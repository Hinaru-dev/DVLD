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
    public partial class frmUserCardDetails : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, bool RefreshPeopleList);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public frmUserCardDetails(int UserID)
        {
            InitializeComponent();

            ctrlUserCard1.LoadUserInfo(UserID);
            //subscribe to card's people update event 
            ctrlUserCard1.DataBack += OnPeopleListUpdated;
        }

        private void OnPeopleListUpdated(object sender, bool needListRefresh = false)
        {
            // Trigger the event to send data back to target previous form
            bool refreshPeopleList = needListRefresh;
            if (DataBack != null)
                DataBack.Invoke(this, refreshPeopleList);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
