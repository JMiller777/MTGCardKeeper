using DataTransferObjects;
using LogicLayer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicCardCollectionSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _user = null;
        private IUserManager _userManager = new UserManager();        
        private ICardManager _cardManager = new CardManager();
        private List<Card> _cardList = new List<Card>();

        private const int MIN_PASSWORD_LENGTH = 5;
        private const int MIN_USERNAME_LENGTH = 8;
        private const int MAX_USERNAME_LENGTH = 100;

        public MainWindow()
        {
            InitializeComponent();
            AppData.ImagePath = AppDomain.CurrentDomain.BaseDirectory + @"images\";
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (_user != null) // someone is logged in and we need to log them out
            {
                logout();
                return;
            }

            // accept the input
            var username = txtUserName.Text;
            var password = txtPassword.Password;

            // check for missing or invalid data
            if (username.Length < MIN_USERNAME_LENGTH ||
                username.Length > MAX_USERNAME_LENGTH)
            {
                MessageBox.Show("Invalid Username", "Login Failed.",
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                clearLogin();
                return;
            }
            if (password.Length < MIN_PASSWORD_LENGTH)
            {
                MessageBox.Show("Invalid Password", "Login Failed.",
                    MessageBoxButton.OK, MessageBoxImage.Hand);
                clearLogin();
                return;
            }

            // before checking for the user token, we need a try block
            try
            {
                _user = _userManager.AuthenticateUser(username, password);
                if (_user.Roles.Count == 0)
                {
                    // check if user is authorized
                    _user = null;

                    MessageBox.Show("You have not been assigned any roles. \nYou will be logged out. \nContact your supervisor.",
                        "Unauthorized Employee", MessageBoxButton.OK, MessageBoxImage.Stop);

                    clearLogin();

                    return;
                }

                // user is now logged in
                var message = "Welcome back, " + _user.Collector.FirstName + ". You are logged in as: ";
                foreach (var r in _user.Roles)
                {
                    message += r.RoleID + "    ";
                }

                showUserTabs();
                statusMain.Items[0] = message;

                clearLogin();
                txtPassword.Visibility = Visibility.Hidden;
                txtUserName.Visibility = Visibility.Hidden;
                lblPassword.Visibility = Visibility.Hidden;
                lblUsername.Visibility = Visibility.Hidden;

                // prevent accidental logouts by moving the focus away from log button
                this.btnLogin.IsDefault = false;
                btnLogin.Content = "Log Out";

                // check for expired password
                if (_user.PasswordMustBeChanged)
                {
                    changePassword();
                }
            }
            catch (Exception ex) // nowhere to throw an exception at the presentation layer
            {
                string message = ex.Message;

                if (ex.InnerException != null)
                {
                    message += "\n\n" + ex.InnerException.Message;
                }
                MessageBox.Show(message, "Login Failed.",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);

                clearLogin();
                return;
            }
        }

        private void logout()
        {
            _user = null;
            // reenable login controls
            txtPassword.Visibility = Visibility.Visible;
            txtUserName.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            lblUsername.Visibility = Visibility.Visible;
            btnLogin.Content = "Log In";
            clearLogin();
            statusMain.Items[0] = "You are not logged in";
            hideAllTabs();
        }

        private void changePassword()
        {
            // we need to get the user's new password
            var passwordChangeWindow = new frmUpdatePassword(_userManager, _user);
            var result = passwordChangeWindow.ShowDialog(); // dialog box. cannot interact with main window until closed

            if (result == true)
            {
                if (_user.Roles[0].RoleID == "New User")
                {
                    // log out
                    logout();
                    MessageBox.Show("You have been logged out. \nYou must log in again \nwith your new password to continue.", "Re-Login required.");
                    return;
                }
                // not new user, so log them out and back in
                else
                {
                    MessageBox.Show("Password Updated.");
                }
            }
            else
            {
                logout();
                MessageBox.Show("Password change canceled. You are not logged out.");

            }
        }
        private void hideAllTabs()
        {
            foreach (var tab in tabsetMain.Items)
            {
                // collaps the tabs
                ((TabItem)tab).Visibility = Visibility.Collapsed;

                // hide tabset
                //tabsetMain.Visibility = Visibility.Visible;
            }
            tabsetMain.Visibility = Visibility.Hidden;
        }

        private void showUserTabs()
        {
            //this.lblScreenCover.Visibility = Visibility.Hidden;
            tabsetMain.Visibility = Visibility.Hidden;
            foreach (var r in _user.Roles)
            {
                switch (r.RoleID)
                {
                    case "Admin":
                        foreach (var tab in tabsetMain.Items)
                        {
                            ((TabItem)tab).Visibility = Visibility.Visible;
                        }
                        tabCardCollection.IsSelected = true;
                        break;
                    case "Collector":
                        tabCollector.Visibility = Visibility.Visible;
                        tabCollector.IsSelected = true;
                        break;
                    case "Maintenance":
                        tabMaintenance.Visibility = Visibility.Visible;
                        tabMaintenance.IsSelected = true;
                        break;
                    case "Viewer":
                        tabViewer.Visibility = Visibility.Visible;
                        tabViewer.IsSelected = true;
                        break;
                }
            }
            tabsetMain.Visibility = Visibility.Visible;
        }

        //********************** CHANGE BACK TO REGULAR VALUES AFTER TESTING IS COMPLETE********************** 
        private void clearLogin()
        {
            this.btnLogin.IsDefault = true;

            //txtUserName.Text = ""; <------- Correct version. Test value directly below **************
            txtUserName.Text = "jen@magic.com";

            //txtPassword.Password = ""; <------- Correct version. Test value directly below **************
            txtPassword.Password = "P@ssw0rd";
            txtUserName.Focus();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
            this.btnLogin.IsDefault = true;
            hideAllTabs();
            refreshCardList();
        }

        private void refreshCardList(bool active = true)
        {
            dgCardCollection.ItemsSource = null;
            dgCardCollection.Items.Clear();

            try
            {
                _cardList = _cardManager.RetrieveCardList();
                dgCardCollection.ItemsSource = _cardList;
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message, "Data Retrieval Error!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void tabCardCollection_GotFocus(object sender, RoutedEventArgs e)
        {
            dgCardCollection.ItemsSource = _cardList;
        }

        private void btnCardDetails_Click(object sender, RoutedEventArgs e)
        {
            Card item = null;
            CardDetail cdDetail = null;
            if (this.dgCardCollection.SelectedItems.Count > 0)
            {
                item = (Card)this.dgCardCollection.SelectedItem;
                try
                {
                    cdDetail = _cardManager.RetrieveCardDetail(item);
                    //var cardItem = (Card)this.dgCardCollection.SelectedItem;
                    var frmDetails = new frmCardDetails(_cardManager, cdDetail, CardDetailMode.View);
                    var result = frmDetails.ShowDialog();
                    if (result == true)
                    {
                        refreshCardList();
                        dgCardCollection.ItemsSource = _cardList;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Oh no..."); 
                }
            }
            else
            {
                MessageBox.Show("Please make a selection.");
            }
        }

        private void dgCardCollection_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var cardItem = (Card)this.dgCardCollection.SelectedItem;

            // we need to pass the selected item to the _equipmentManager method
            var cardDetail = _cardManager.RetrieveCardDetail(cardItem);

            var frmDetails = new frmCardDetails(_cardManager, cardDetail, CardDetailMode.View);
            var result = frmDetails.ShowDialog();
            if (result == true)
            {
                refreshCardList();
                dgCardCollection.ItemsSource = _cardList;
            }
        }

        private void btnEditCard_Click(object sender, RoutedEventArgs e)
        {
            Card item = null;
            CardDetail cdDetail = null;
            if (this.dgCardCollection.SelectedItems.Count > 0)
            {
                item = (Card)this.dgCardCollection.SelectedItem;
                try
                {
                    cdDetail = _cardManager.RetrieveCardDetail(item);
                    var detailForm = new frmCardDetails(_cardManager, cdDetail, CardDetailMode.Edit);
                    var results = detailForm.ShowDialog();
                    if (results == true)
                    {
                        refreshCardList();
                        dgCardCollection.ItemsSource = _cardList;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Oh no...");
                }
            }
            else
            {
                MessageBox.Show("Please select a card to edit.");
            }
        }
            
            // make any selections?
        //    if (this.dgCardCollection.SelectedItems.Count == 0)
        //    {
        //        MessageBox.Show("You haven't selected anything.");
        //        return;
        //    }
        //    // pass selected card item to card manager method for details
        //    var cardItem = (Card)this.dgCardCollection.SelectedItem;
        //    var cardDetail = _cardManager.RetrieveCardDetail(cardItem);

        //    var frmDetails = new frmCardDetails(_cardManager, cardDetail, CardDetailMode.Edit);
        //    var result = frmDetails.ShowDialog();
        //    if (result == true)
        //    {
        //        dgCardCollection.ItemsSource = _cardList;
        //        refreshCardList();
        //    }
        //}

        private void btnAddCard_Click(object sender, RoutedEventArgs e)
        {
            var frmDetails = new frmCardDetails(_cardManager);
            var result = frmDetails.ShowDialog();
            if (result == true)
            {
                dgCardCollection.ItemsSource = _cardList;
                refreshCardCollection();
            }
        }

        private void refreshCardCollection()
        {
            dgCardCollection.ItemsSource = null;
            dgCardCollection.Items.Clear();
            refreshCardList();
            dgCardCollection.ItemsSource = _cardList;
        }

        private void tabViewer_GotFocus(object sender, RoutedEventArgs e)
        {
            dgCardView.ItemsSource = _cardList;
        }

        private void btnDeactivateCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.dgCardCollection.SelectedItems.Count > 0)
                {
                    if (((Card)this.dgCardCollection.SelectedItem).Active == false)
                    {
                        MessageBox.Show("This Card is already inactive.");
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Are you sure you want to Deactivate Card " +
                            ((Card)this.dgCardCollection.SelectedItem).Name + "?", "Deactivation Warning", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.No)
                        {
                            return;
                        }
                        _cardManager.DeactivateCardByID(((Card)this.dgCardCollection.SelectedItem).CardID);
                        refreshCardCollection();
                    }
                }
                else
                {
                    MessageBox.Show("You must select a card.");
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Deactivation Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
