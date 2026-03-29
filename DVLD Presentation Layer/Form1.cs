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
    public partial class Form1 : Form
    {        
        // Declare a delegate
        public delegate void DataBackEventHandler();

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public Form1()
        {
            InitializeComponent();
        }

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
    }
}
