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
            get { return _UserID; }
        }

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

        public ctrlUserCard()
        {
            InitializeComponent();
            subToPersonInfoUpdate();
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

            // sending userid (-1) to get invalid personid and reset personcard
            ctrlPersonCard1.ResetPersonInfo();  
        }
    }
}
