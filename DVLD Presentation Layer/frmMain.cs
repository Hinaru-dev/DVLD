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
    public partial class frmMain : Form
    {        
        // Declare a delegate
        public delegate void DataBackEventHandler();

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public frmMain()
        {
            InitializeComponent();
        }

        // ---------------------------------
        //         custom functions
        // ---------------------------------

        private void ShowUserDetails(int UserID)
        {
            frmUserCardDetails frmUserCardDetails = new frmUserCardDetails(UserID);
            frmUserCardDetails.ShowDialog();
        }

        private void changeUserPassword(int UserID)
        {
            frmChangePassword frmChangePassword = new frmChangePassword(UserID);
            frmChangePassword.ShowDialog();
        }

        // ---------------------------------
        //         controls' events
        // ---------------------------------

        private void tsmiPeople_Click(object sender, EventArgs e)
        {
            Form frmManagePeople1 = new frmManagePeople();
            frmManagePeople1.ShowDialog();
        }

        private void tsmiUsers_Click(object sender, EventArgs e)
        {
            Form frmManageUsers = new frmManageUsers();
            frmManageUsers.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Trigger the event to send data back to Login Form
            if (DataBack != null)
                DataBack.Invoke();
        }

        private void tsmiSignOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmiCurrentUserInfo_Click(object sender, EventArgs e)
        {
            ShowUserDetails(clsGlobalUser.CurrentUser.UserID);
        }

        private void tsmiChangePassword_Click(object sender, EventArgs e)
        {
            changeUserPassword(clsGlobalUser.CurrentUser.UserID);
        }

        private void tsmiManageApplicationTypes_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frmManageApplicationTypes = new frmManageApplicationTypes();
            frmManageApplicationTypes.ShowDialog();
        }

        private void tsmiManageTestTypes_Click(object sender, EventArgs e)
        {
            frmManageTestTypes frmManageTestTypes = new frmManageTestTypes();
            frmManageTestTypes.ShowDialog();
        }
    }
}
