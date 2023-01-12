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
    /// Win_Tnm_Islem.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Tnm_Islem : Window
    {
        public Win_Tnm_Islem()
        {
            InitializeComponent();
            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);
            _islemService = new IslemManager(new EfIslemDal());
            LoadIslem();
        }
        //mouse sürükle
        private MouseButton _button;

        //tooltip timer
        private Timer m_toolTipClosingTimer;

        readonly IIslemService _islemService;
        Islem _selectedIslem;
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
            List<Islem> islemler = _islemService.GetList().Data;
            lvMGrup.ItemsSource = islemler;
        }
        void SetUpdateField()
        {
            tbxIslemName.Text = _selectedIslem.Name;
            LabelKaydet.Content = "Güncelle";
        }
        void ClearField()
        {
            tbxIslemName.Text = string.Empty;
            _selectedIslem = null;
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
            popup_uc.Placement = PlacementMode.Right;
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
                _selectedIslem = lvMGrup.SelectedItem as Islem;
                SetUpdateField();
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            //Güncelle yada yeni kayıt
            if (_selectedIslem is null)
            {
                //Yeni Kayıt
                Islem islem = new Islem();
                islem.Name = tbxIslemName.Text;
                var result = _islemService.Add(islem);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }
            }
            else
            {
                //Güncelleme
                ;
                    _selectedIslem.Name = tbxIslemName.Text;
                var result = _islemService.Update(_selectedIslem);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }

            }

        }


        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (!(_selectedIslem is null))
            {
                var result = _islemService.Delete(_selectedIslem);
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
