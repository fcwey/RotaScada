using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Reflection.PortableExecutable;
using System.ComponentModel;
using System.IO;
using Core.Utilities.Results;

namespace WpfUI.Windows
{
    /// <summary>
    /// WindowMGrup.xaml etkileşim mantığı
    /// </summary>
    public partial class WindowMGrup : Window
    {
        private Timer m_toolTipClosingTimer;
        public WindowMGrup()
        {
            InitializeComponent();
            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);
            _malzemeGrupService = new MalzemeGrupManager(new EfMalzemeGrupDal());
            LoadMGrup();
        }
        readonly IMalzemeGrupService _malzemeGrupService;
        MalzemeGrup _selectedMGrup;
        List<MalzemeGrup> mGrupList;
        ToolTip tp = new ToolTip();

        private void ToolTipClosingCallBack(object p_useless)
        {
            Dispatcher.Invoke(() =>
            {
                popup_uc.IsOpen = false;
            });
        }
        void LoadMGrup()
        {
            List<MalzemeGrup> malzemeGrups = _malzemeGrupService.GetList().Data;
            lvMGrup.ItemsSource = malzemeGrups;
        }
        void SetUpdateField()
        {
            tbxMgrupName.Text = _selectedMGrup.GrupName;
            LabelKaydet.Content = "Güncelle";
        }
        void ClearField()
        {
            tbxMgrupName.Text = string.Empty;
            _selectedMGrup = null;
            LabelKaydet.Content = "Ekle";
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseDown += delegate { DragMove(); };
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        void CommandMessage(bool success,string message)
        {

            popup_uc.PlacementTarget = Message;
            popup_uc.Placement = PlacementMode.Right;
            popup_uc.IsOpen = true;
            if (success)
            {
                string str=System.IO.Directory.GetCurrentDirectory();
                string imagePath = str + "\\icon\\success.png";

                Header.PopupText.Text = message;
                Header.PopupText.Foreground = new SolidColorBrush(Colors.White);
                Header.PopupImage.Source = new BitmapImage(new Uri(imagePath));
            }
            else
            {
                string str = System.IO.Directory.GetCurrentDirectory();
                string imagePath = str + "\\icon\\fail.png";

                Header.PopupText.Text = "Başarısız: "+message;
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
            if (!(lvMGrup.SelectedItems is null) && lvMGrup.SelectedItems.Count> 0)  //seçilen item null değilse
            {
                _selectedMGrup = lvMGrup.SelectedItem as MalzemeGrup;
                SetUpdateField();
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            //Güncelle yada yeni kayıt
            if (_selectedMGrup is null)
            {
                //Yeni Kayıt
                MalzemeGrup mgrup = new MalzemeGrup();
                mgrup.GrupName = tbxMgrupName.Text;
                var result = _malzemeGrupService.Add(mgrup);
                if (result.IsSuccess)
                {
                    LoadMGrup();
                }
            }
            else
            {
                //Güncelleme
                ;
                _selectedMGrup.GrupName = tbxMgrupName.Text;
                var result = _malzemeGrupService.Update(_selectedMGrup);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadMGrup();
                }
                
            }
            
        }


        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (!(_selectedMGrup is null))
            {
                var result = _malzemeGrupService.Delete(_selectedMGrup);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadMGrup();
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
