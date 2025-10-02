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
    public partial class ctrlUserCard : UserControl
    {
        clsUser _User;

        private int _UserID = -1;

        public int UserID
        {
            get { return UserID; }
        }

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.Find(UserID);
            if (_User == null)
            {
                _ResetUserInfo();
                MessageBox.Show("No User with User id = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillUserInfo();
        }

        public void LoadUserInfo(string Username)
        {
            _User = clsUser.Find(Username);
            if (_User == null)
            {
                _ResetUserInfo();
                MessageBox.Show("No User with username = " + Username, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillUserInfo();
        }

        private void _FillUserInfo()
        {
            _UserID = _User.UserID;
            lblUserID.Text = _User.UserID.ToString();
            lblUsername.Text = _User.Username;

            lblIsActive.Text = _User.isActive ? "Yes" : "NO";
            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
        }

        private void _ResetUserInfo()
        {
            _UserID = -1;
            lblUserID.Text = "???";
            lblUsername.Text = "???";
            lblIsActive.Text = "???";

            ctrlPersonCard1.LoadPersonInfo(_UserID);  
        }
    }
}
