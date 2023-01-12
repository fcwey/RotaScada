using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfUI.Windows
{
    /// <summary>
    /// Win_Tnm_Furnace.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Tnm_Furnace : Window
    {
        public Win_Tnm_Furnace()
        {
            InitializeComponent();
            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);

            _furnaceService = new FurnaceManager(new EfFurnaceDal());
            _bolumService = new BolumManager(new EfBolumDal());
            LoadIslem();
            List<Bolum> bolums = _bolumService.GetList().Data;
            cbxBolum.ItemsSource = bolums;
            cbxBolum.DisplayMemberPath = "Name";
            cbxBolum.SelectedValuePath = "Id";
        }
        //mouse sürükle
        private MouseButton _button;

        //tooltip timer
        private Timer m_toolTipClosingTimer;

        readonly IFurnaceService _furnaceService;
        readonly IBolumService _bolumService;

        Bolum _selectedbolum;
        Furnace _selectedfurnace;


        ToolTip tp = new ToolTip();

        private void ToolTipClosingCallBack(object p_useless)
        {
            Dispatcher.Invoke(() =>
            {
                popup_uc.IsOpen = false;
            });
        }
        void LoadIslem()
        {
            // List<Islem> islemler = _islemService.GetList().Data;
            List<FurnaceDto> furnaces = _furnaceService.GetListDto().Data;
            lvMGrup.ItemsSource = furnaces;
           
        }
        void SetUpdateField()
        {
            tbxFurnaceName.Text = _selectedfurnace.Name;
            tbxMaxTemp.Text = _selectedfurnace.Derece.ToString();
            cbxBolum.SelectedValue = _selectedfurnace.BolumId;
            LabelKaydet.Content = "Güncelle";
        }
        void ClearField()
        {
            tbxFurnaceName.Text = string.Empty;
            tbxMaxTemp.Text = string.Empty;
            cbxBolum.SelectedIndex = -1;
            _selectedfurnace = null;
            LabelKaydet.Content = "Ekle";
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        void CommandMessage(bool success, string message)
        {

            popup_uc.PlacementTarget = Message;
            popup_uc.Placement = PlacementMode.Left;
            popup_uc.IsOpen = true;
            if (success)
            {
                string str = System.IO.Directory.GetCurrentDirectory();
                string imagePath = str + "\\icon\\success.png";

                Header.PopupText.Text = message;
                Header.PopupText.Foreground = new SolidColorBrush(Colors.White);
                Header.PopupImage.Source = new BitmapImage(new Uri(imagePath));
            }
            else
            {
                string str = System.IO.Directory.GetCurrentDirectory();
                string imagePath = str + "\\icon\\fail.png";

                Header.PopupText.Text = "Başarısız: " + message;
                Header.PopupText.Foreground = new SolidColorBrush(Colors.White);
                Header.PopupImage.Source = new BitmapImage(new Uri(imagePath));


            }

            m_toolTipClosingTimer.Change(2500, Timeout.Infinite);
        }


        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {
            ClearField();
        }

        private void lvMGrup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(lvMGrup.SelectedItems is null) && lvMGrup.SelectedItems.Count > 0)  //seçilen item null değilse
            {
                _selectedfurnace = _furnaceService.GetById((lvMGrup.SelectedItem as FurnaceDto).Id).Data;
                SetUpdateField();
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            //Güncelle yada yeni kayıt
            if (_selectedfurnace is null)
            {
                //Yeni Kayıt
                Furnace furnace = new Furnace();
                furnace.Name = tbxFurnaceName.Text;
                furnace.Derece = Convert.ToInt32(tbxMaxTemp.Text);
                furnace.BolumId = (int)cbxBolum.SelectedValue;
                furnace.Aktive = 1;
                var result = _furnaceService.Add(furnace);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }
            }
            else
            {
                //Güncelleme
                ;
                _selectedfurnace.Name = tbxFurnaceName.Text;
                _selectedfurnace.Derece = Convert.ToInt32(tbxMaxTemp.Text);
                _selectedfurnace.BolumId= (int)cbxBolum.SelectedValue;
                var result = _furnaceService.Update(_selectedfurnace);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }

            }

        }


        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (!(_selectedfurnace is null))
            {
                var result = _furnaceService.Delete(_selectedfurnace);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
