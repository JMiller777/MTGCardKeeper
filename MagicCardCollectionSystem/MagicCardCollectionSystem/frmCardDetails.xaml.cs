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
    /// Interaction logic for frmCardDetails.xaml
    /// </summary>
    public partial class frmCardDetails : Window
    {
        private ICardManager _cardManager = new CardManager();
        private CardDetail _cardDetail;
        private CardDetailMode _mode;

        private List<Edition> _editionList;
        private List<Rarity> _rarityList;
        private List<CardColor> _colorList;
        private List<DataTransferObjects.Type> _typeList;

        // edit mode
        public frmCardDetails(ICardManager cdMgr, CardDetail cdDetail, CardDetailMode mode)
        {
            _cardManager = cdMgr;
            _cardDetail = cdDetail;
            _mode = mode;
            InitializeComponent();
        }

        // add mode
        public frmCardDetails(ICardManager cdMgr)
        {
            _cardManager = cdMgr;
            // _cardDetail = null;
            _mode = CardDetailMode.Add;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _colorList = _cardManager.RetrieveColorList();
                _typeList = _cardManager.RetrieveTypeList();
                _editionList = _cardManager.RetrieveEditionList();
                _rarityList = _cardManager.RetrieveRarityList();

                this.cboColor.ItemsSource = _colorList;
                this.cboEdition.ItemsSource = _editionList;
                this.cboRarity.ItemsSource = _rarityList;
                this.cboType.ItemsSource = _typeList;
            }
            catch
            {
                MessageBox.Show("Error loading one or more option lists.");
            }
            if (_cardDetail != null)
            {
                try
                {
                    this.imgCard.Source = new BitmapImage(new Uri(AppData.ImagePath + _cardDetail.ImgFileName.ImgFileNameID));

                    foreach (var i in cboRarity.Items)
                    {
                        if (((Rarity)i).ToString() == _cardDetail.Card.RarityID)
                        {
                            cboRarity.SelectedItem = i;
                            break;
                        }
                    }

                    foreach (var i in cboType.Items)
                    {
                        if (((DataTransferObjects.Type)i).ToString() == _cardDetail.Card.TypeID)
                        {
                            cboType.SelectedItem = i;
                            break;
                        }
                    }

                    foreach (var i in cboColor.Items)
                    {
                        if (((CardColor)i).ToString() == _cardDetail.Card.ColorID)
                        {
                            cboColor.SelectedItem = i;
                            break;
                        }
                    }

                    foreach (var i in cboEdition.Items)
                    {
                        if (((Edition)i).ToString() == _cardDetail.Card.EditionID)
                        {
                            cboEdition.SelectedItem = i;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Card Access Problem\n\n" + ex.InnerException.Message);
                }
            }
            switch (_mode)
            {
                case CardDetailMode.View:
                    setupViewMode();
                    break;
                case CardDetailMode.Edit:
                    setupEditMode();
                    break;
                case CardDetailMode.Add:
                    setupAddMode();
                    break;
                default:
                    break;
            }
        }

        private void setComboBoxes(bool enabled = false)
        {
            this.cboEdition.Text = _cardDetail.Card.EditionID;
            this.cboEdition.IsEnabled = enabled;

            this.cboRarity.Text = _cardDetail.Card.RarityID;
            this.cboRarity.IsEnabled = enabled;

            this.cboColor.Text = _cardDetail.Card.ColorID;
            this.cboColor.IsEnabled = enabled;

            this.cboType.Text = _cardDetail.Card.TypeID;
            this.cboType.IsEnabled = enabled;
        }

        private void populateControls()
        {
            this.txtName.Text = _cardDetail.Card.Name.ToString();
            this.txtImgFileName.Text = _cardDetail.ImgFileName.ImgFileNameID.ToString();
            this.txtCardText.Text = _cardDetail.Card.CardText;
            if (_cardDetail.Card.Active == true)
            {
                this.chbxActive.IsChecked = true;
            }
            if (_cardDetail.Card.IsFoil == true)
            {
                this.chbxFoil.IsChecked = true;
            }
        }

        private void setupViewMode()
        {
            this.btnSaveEdit.Content = "Edit";
            this.Title = "View Card Details";
            populateControls();
            setInputs(readOnly: true);
        }
        private void setupEditMode()
        {
            this.btnSaveEdit.Content = "Save";
            this.Title = "Edit a Card";
            setInputs(readOnly: false);
            this.cboRarity.IsEnabled = true;
            this.chbxFoil.IsEnabled = true;
            this.chbxActive.IsEnabled = true;
            populateControls();
        }
        private void setupAddMode()
        {
            this.btnSaveEdit.Content = "Save";
            this.Title = "Add a New Card to the Collection";
            setInputs(readOnly: false);
            this.cboRarity.IsEnabled = true;
            this.chbxFoil.IsEnabled = true;
            this.chbxActive.IsChecked = true;
            this.chbxActive.IsEnabled = false;
        }
        private void setInputs(bool readOnly = true)
        {
            this.txtName.IsReadOnly = readOnly;
            this.cboEdition.IsReadOnly = readOnly;
            this.cboRarity.IsEnabled = readOnly;
            this.cboType.IsReadOnly = readOnly;
            this.cboColor.IsReadOnly = readOnly;
            this.txtImgFileName.IsReadOnly = readOnly;
            this.txtCardText.IsReadOnly = readOnly;
            this.chbxFoil.IsEnabled = readOnly;
            this.chbxActive.IsEnabled = readOnly;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_mode == CardDetailMode.View) // if we're in View Mode we need to get to edit mode
            {
                _mode = CardDetailMode.Edit;
                setupEditMode();
                return;
            }
            var card = new Card();

            switch (_mode)
            {
                case CardDetailMode.Add:
                    if(captureCard(card) == false)
                    {
                        return;
                    }

                    try
                    {
                        if (_cardManager.AddCard(card))
                        {
                            this.DialogResult = true;
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case CardDetailMode.Edit:
                    if(captureCard(card) == false)
                    {
                        return;
                    }
                    card.CardID = _cardDetail.Card.CardID;
                    var oldCard = _cardDetail.Card;
                    try
                    {
                        if(_cardManager.EditCard(card, oldCard))
                        {
                            this.DialogResult = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case CardDetailMode.View:
                    break;
                default:
                    break;
            }
        }

        private bool captureCard(Card card)
        {
            if (this.cboColor.SelectedItem == null)
            {
                MessageBox.Show("You must select a Color");
                return false;
            }
            else
            {
                card.ColorID = ((CardColor)cboColor.SelectedItem).ColorID;
            }
            if (this.cboEdition.SelectedItem == null)
            {
                MessageBox.Show("You must select an Edition");
                return false;
            }
            else
            {
                card.EditionID = ((Edition)cboEdition.SelectedItem).EditionID;
            }
            if (this.cboRarity.SelectedItem == null)
            {
                MessageBox.Show("You must select a card Rarity");
                return false;
            }
            else
            {
                card.RarityID = ((Rarity)cboRarity.SelectedItem).RarityID;
            }
            if (this.cboType.SelectedItem == null)
            {
                MessageBox.Show("You must select a card Type");
                return false;
            }
            else
            {
                card.TypeID = ((DataTransferObjects.Type)cboType.SelectedItem).TypeID;
            }
            if(this.txtCardText.Text == "")
            {
                MessageBox.Show("You must enter card Text.");
                return false;
            }
            else
            {
                card.CardText = txtCardText.Text;
            }
            if(this.txtImgFileName.Text == "")
            {
                MessageBox.Show("You must enter an Image File Name.");
                return false;
            }
            else
            {
                card.ImgFileName = txtImgFileName.Text;
            }
            if(this.txtName.Text == "")
            {
                MessageBox.Show("You must enter a card Name.");
                return false;
            }
            else
            {
                card.Name = txtName.Text;
            }
            card.Active = (bool)this.chbxActive.IsChecked;
            card.IsFoil = (bool)this.chbxFoil.IsChecked;
            return true;
        }
    }
}
