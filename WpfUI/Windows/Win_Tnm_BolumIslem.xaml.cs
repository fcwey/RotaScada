using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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
    /// Win_Tnm_BolumIslem.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Tnm_BolumIslem : Window
    {
        public Win_Tnm_BolumIslem()
        {
            InitializeComponent();
            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);

            _bolumIslemService = new BolumIslemManager(new EfBolumIslemDal());
            _bolumService = new BolumManager(new EfBolumDal());
            loadBolum();
           // LoadIslem();
        }
        //mouse sürükle
        private MouseButton _button;

        //tooltip timer
        private Timer m_toolTipClosingTimer;

        readonly IBolumIslemService _bolumIslemService;
        readonly IBolumService _bolumService;

        BolumIslemDto _selectedBolumIslemDto;
        BolumIslem _selectedBolumIslem;
        List<Islem> islemList;
        ToolTip tp = new ToolTip();

        private void ToolTipClosingCallBack(object p_useless)
        {
            Dispatcher.Invoke(() =>
            {
                popup_uc.IsOpen = false;
            });
        }
        //private bool VacumFilter(object obj)
        //{

        //    var FilterObj = obj as BolumIslemDto;
        //    //Listede kaç tane

        //    return FilterObj.BolumName.Contains("Vakum Fırın", StringComparison.OrdinalIgnoreCase);
        //}
        //void LoadIslem()
        //{
        //    List<BolumIslemDto> islemler = _bolumIslemService.GetListBolumIslemDtos().Data;
        //    lvMGrup.ItemsSource = islemler;
        //}
        void loadBolum()
        {
            List<Bolum> bolums = _bolumService.GetList().Data;
            lbxBolum.ItemsSource = bolums;
            if (lbxBolum.Items.Count > 0) lbxBolum.SelectedIndex = 0;
        }
        async void LoadIslem(int bolumId)
        {
            var list = await _bolumIslemService.GetListBolumIslemDtosAsync(bolumId);
            lvMGrup.ItemsSource = list.Data;
        }
        void SetUpdateField()
        {
            tbxBolumIslemName.Text = _selectedBolumIslemDto.BolumIslemi;
           
            LabelKaydet.Content = "Güncelle";
        }
        void ClearField()
        {
            tbxBolumIslemName.Text = string.Empty;
            _selectedBolumIslemDto = null;
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

                //Header.PopupText.Text = message;
                //Header.PopupText.Foreground = new SolidColorBrush(Colors.White);
                //Header.PopupImage.Source = new BitmapImage(new Uri(imagePath));
            }
            else
            {
                string str = System.IO.Directory.GetCurrentDirectory();
                string imagePath = str + "\\icon\\fail.png";

                //Header.PopupText.Text = "Başarısız: " + message;
                //Header.PopupText.Foreground = new SolidColorBrush(Colors.White);
                //Header.PopupImage.Source = new BitmapImage(new Uri(imagePath));


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
                _selectedBolumIslemDto = lvMGrup.SelectedItem as BolumIslemDto;
                SetUpdateField();
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            //Güncelle yada yeni kayıt
            if (_selectedBolumIslemDto is null)
            {
                //Yeni Kayıt
                BolumIslem bolümIslem = new BolumIslem();
                bolümIslem.BolumIslemi = tbxBolumIslemName.Text;
                //if (vacum.IsActive)
                //{
                //    bolümIslem.BolumId = 1;
                //}
                //else if (tuz.IsActive)
                //{
                //    bolümIslem.BolumId = 2;
                //}
                //else if (ind.IsActive)
                //{
                //    bolümIslem.BolumId = 3;
                //}
                //else if (ath.IsActive)
                //{
                //    bolümIslem.BolumId = 4;
                //}
                bolümIslem.BolumId = (lbxBolum.SelectedItem as Bolum).Id;
                var result = _bolumIslemService.Add(bolümIslem);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem((lbxBolum.SelectedItem as Bolum).Id);
                }
            }
            else
            {
                //Güncelleme
                ;
                _selectedBolumIslem = _bolumIslemService.GetById(_selectedBolumIslemDto.Id).Data;
                _selectedBolumIslem.BolumIslemi = tbxBolumIslemName.Text;
                var result = _bolumIslemService.Update(_selectedBolumIslem);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem((lbxBolum.SelectedItem as Bolum).Id);
                }

            }

        }


        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (!(_selectedBolumIslemDto is null))
            {
                _selectedBolumIslem = _bolumIslemService.GetById(_selectedBolumIslemDto.Id).Data;
                var result = _bolumIslemService.Delete(_selectedBolumIslem);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem((lbxBolum.SelectedItem as Bolum).Id);
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //private void lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (!(lbx.SelectedItems is null) && lbx.SelectedItems.Count > 0)  //seçilen item null değilse
        //    {
        //        ;
        //        //MessageBox.Show(_servis.Name.ToString());
        //       // loadFurnaceListe((lbx.SelectedItem as Bolum).Id);
        //    }
        //}
        private void AGotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = sender as FrameworkElement;
                Bolum item = element.DataContext as Bolum;
                if (item != null)
                {
                    lbxBolum.SelectedValue = item;
                }
            }
            catch { }
        }

        private void lbxBolum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(lbxBolum.SelectedItems is null) && lbxBolum.SelectedItems.Count > 0)  //seçilen item null değilse
            {
                LoadIslem((lbxBolum.SelectedItem as Bolum).Id);
            }
        }
        //private void Vacum_ElementClick(object sender, RoutedEventArgs e)
        //{
        //    if (!vacum.IsActive)
        //    {
        //        vacum.IsActive = true;
        //        tuz.IsActive = false;
        //        ath.IsActive = false;
        //        ind.IsActive = false;

        //    }
        //}


    }
}
