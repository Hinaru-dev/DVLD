using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business_Logic;

namespace DVLD_Presentation_Layer
{
    public partial class frmLogin : Form
    {
        // ---------------------
        //  Global Declarations
        // ---------------------

        string _Delimeter = "|#|#|";

        // --------------------
        //    Class Heading
        // --------------------

        public frmLogin()
        {
            InitializeComponent();

            // load last logged info from remember me file if data exists
            _loadSavedLoginInfo();
        }


        // --------------------
        //   Custom Functions
        // --------------------

        private void _loadSavedLoginInfo()
        {
            string filePath = @"c\Login.txt";

            if (File.Exists(filePath))
            {
                string LoginString = File.ReadAllText(filePath);

                if (string.IsNullOrEmpty(LoginString))
                {
                    MessageBox.Show("Login Info is Null or empty!!", "TemporaryError Loading remembered Login Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string[] LoginInfo = LoginString.Split(new string[] { _Delimeter }, StringSplitOptions.None);

                tbUsername.Text = LoginInfo[0];
                tbPassword.Text = LoginInfo[1];
                cbRememberMe.Checked = true;
            }
        }

        private bool _isValidUserLogin()
        {
            return clsUser.ValidateLoginInfo(tbUsername.Text, tbPassword.Text);
        }

        // no boolean return here since this file is a cache file that won't show exception when trying to save in it
        private void _SaveLoginInfo()
        {
            string filePath = @"c\Login.txt";
            string LoginInfo = tbUsername.Text + _Delimeter + tbPassword.Text;

            // This creates the file if missing OR wipes it if it exists
            File.WriteAllText(filePath, LoginInfo);
        }

        private void _EraseLoginInfoCache()
        {
            string filePath = @"c\Login.txt";
            
            if (File.Exists(filePath))
                 // Opening the file with FileMode.Truncate immediately clears its contents.
                // The 'using' statement ensures the FileStream is closed and disposed of
                // immediately after opening, making the truncation permanent.
                using (FileStream fs = new FileStream(filePath, FileMode.Truncate)) {}

        }

        private void MainFormClosed()
        {
            this.Visible = true;
        }

        // --------------------
        //    System Events
        // --------------------

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (_isValidUserLogin())
            {
                this.Visible = false;
                Form1 MainForm = new Form1();
                MainForm.DataBack += MainFormClosed;
                MainForm.Show();

                if (cbRememberMe.Checked)
                    _SaveLoginInfo();
                else
                    _EraseLoginInfoCache();
            }
            else
            {
                // maybe should do someothing if validation wrong or limit trials count
                MessageBox.Show("Invalid Credentials", "Login Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // UseDelegate to know that main form is disposed then make this frmLogin visible against
    }
}
