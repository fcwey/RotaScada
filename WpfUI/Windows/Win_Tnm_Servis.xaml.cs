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
    /// Win_Tnm_Servis.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Tnm_Servis : Window
    {
        public Win_Tnm_Servis()
        {
            InitializeComponent();
            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);
          
            _servisService = new ServisManager(new EfServisDal());
            LoadIslem();
        }
        //mouse sürükle
        private MouseButton _button;

        //tooltip timer
        private Timer m_toolTipClosingTimer;

        readonly IServisService _servisService;
        Servis _selectedServis;
        List<Islem> islemList;
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
            List<Servis> islemler = _servisService.GetList().Data;
            lvMGrup.ItemsSource = islemler;
        }
        void SetUpdateField()
        {
            tbxServisName.Text = _selectedServis.Name;
            LabelKaydet.Content = "Güncelle";
        }
        void ClearField()
        {
            tbxServisName.Text = string.Empty;
            _selectedServis = null;
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
                string str = AppContext.BaseDirectory;
                string imagePath = str + "\\Assets\\success.png";

                Header.PopupText.Text = message;
                Header.PopupText.Foreground = new SolidColorBrush(Colors.White);
               // Header.PopupImage.Source = new BitmapImage(new Uri(imagePath));
            }
            else
            {
                string str = AppContext.BaseDirectory;
                string imagePath = str + "\\Assets\\fail.png";

                Header.PopupText.Text = "Başarısız: " + message;
                Header.PopupText.Foreground = new SolidColorBrush(Colors.White);
               // Header.PopupImage.Source = new BitmapImage(new Uri(imagePath));


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
                _selectedServis = lvMGrup.SelectedItem as Servis;
                SetUpdateField();
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            //Güncelle yada yeni kayıt
            if (_selectedServis is null)
            {
                //Yeni Kayıt
                Servis servis = new Servis();
                servis.Name = tbxServisName.Text;
                var result = _servisService.Add(servis);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }
            }
            else
            {
                //Güncelleme
                ;
                _selectedServis.Name = tbxServisName.Text;
                var result = _servisService.Update(_selectedServis);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }

            }

        }


        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (!(_selectedServis is null))
            {
                var result = _servisService.Delete(_selectedServis);
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
