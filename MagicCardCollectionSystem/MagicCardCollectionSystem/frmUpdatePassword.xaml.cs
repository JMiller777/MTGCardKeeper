using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataTransferObjects;
using LogicLayer;

namespace MagicCardCollectionSystem
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class frmUpdatePassword : Window
    {
        private IUserManager _userManager;
        private User _user;

        public frmUpdatePassword(IUserManager userManager, User user)
        {
            _userManager = userManager;
            _user = user;

            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            
            

            //********* TEST VALUES *********
            pwdOldPassword.Password = "newuser";
            pwdNewPassword.Password = "P@ssw0rd";
            pwdRetypedPassword.Password = "P@ssw0rd";


            var oldpassword = pwdOldPassword.Password;
            var newPassword = pwdNewPassword.Password;
            var retypedPassword = pwdRetypedPassword.Password;

            /* ********************** Beginning of Commenting for testing purposes ********************** 

            // got an old password?
            if (oldpassword == "")
            {
                MessageBox.Show("Please enter your current password.");
                clearPasswordBoxes();
                return;
            }

            // new password meet requirements?
            if (newPassword.Length < 6)
            {
                MessageBox.Show("New Password is invalid. Try again.");
                clearPasswordBoxes();
                return;
            }

            // make sure the new password and retyped password match
            if (newPassword != retypedPassword)
            {
                MessageBox.Show("Your new password and retyped password do not match.");
                clearPasswordBoxes();
                return;
            }
             * 
            End of test commenting */
            
            
            // update the password
            try
            {
                _user = _userManager.UpdatePassword(_user, oldpassword, newPassword);
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message, "Update Failed.");
            }

            // if the dialog completed successfully: message
            this.DialogResult = true;
        }

        private void clearPasswordBoxes()
        {
            pwdNewPassword.Password = "";
            pwdRetypedPassword.Password = "";

            if (_user.Roles[0].RoleID == "New User")
            {
                pwdNewPassword.Focus();
            }
            // For existing users
            else
            {
                pwdOldPassword.Password = "";
                pwdOldPassword.Focus();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.btnSubmit.IsDefault = true;
            if (_user.Roles[0].RoleID == "New User")
            {
                this.tblkMessage.Text = "Welcome " + _user.Collector.FirstName + ". As a new user, you must choose a permanent password in order to continue.";
                this.pwdOldPassword.Password = "newuser";
                this.pwdOldPassword.IsEnabled = false;
            }
            //clearPasswordBoxes();
        }
    }
}
