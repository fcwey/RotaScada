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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfUI.Windows
{
    /// <summary>
    /// Win_Tnm_AktifIslemler.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Tnm_AktifIslemler : Window
    {
        public Win_Tnm_AktifIslemler()
        {
            InitializeComponent();
            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);


            _bolumService = new BolumManager(new EfBolumDal());
            _islemService = new IslemManager(new EfIslemDal());
            _furnaceService = new FurnaceManager(new EfFurnaceDal());
            _aktifIslemService = new AktifIslemManager(new EfAktifIslemDal());

            CbxByBolumId(1);
            LoadIslem();
            islemLoad();
            lvMGrup.Items.Filter = VacumFilter;

        }
        //mouse sürükle
        private MouseButton _button;

        //tooltip timer
        private Timer m_toolTipClosingTimer;

        readonly IBolumService _bolumService;
        readonly IIslemService _islemService;
        readonly IFurnaceService _furnaceService;
        readonly IAktifIslemService _aktifIslemService;

        int selectedBolumId = 1;
        AktifIslem _selectedAktifFurnace;
        ToolTip tp = new ToolTip();

        public string filterchar;

        void islemLoad()
        {
            List<Islem> islems=_islemService.GetList().Data;
            cbxIslem.ItemsSource = islems;
            cbxIslem.DisplayMemberPath = "Name";
            cbxIslem.SelectedValuePath = "Id";
        }
        private void ToolTipClosingCallBack(object p_useless)
        {
            Dispatcher.Invoke(() =>
            {
                popup_uc.IsOpen = false;
            });
        }

        private bool VacumFilter(object obj)
        {

            var FilterObj = obj as AktifIslemDto;
            //Listede kaç tane

            return FilterObj.BolumName.Contains("Vakum Fırın", StringComparison.OrdinalIgnoreCase);
        }
        private bool AthFilter(object obj)
        {

            var FilterObj = obj as AktifIslemDto;
            //Listede kaç tane

            return FilterObj.BolumName.Contains("Atmosfer", StringComparison.OrdinalIgnoreCase);
        }
        private bool TuzFilter(object obj)
        {

            var FilterObj = obj as AktifIslemDto;
            //Listede kaç tane

            return FilterObj.BolumName.Contains("Tuz", StringComparison.OrdinalIgnoreCase);
        }
        private bool IndFilter(object obj)
        {

            var FilterObj = obj as AktifIslemDto;
            //Listede kaç tane

            return FilterObj.BolumName.Contains("İnduksiyon", StringComparison.OrdinalIgnoreCase);
        }
        void LoadIslem()
        {
            List<AktifIslemDto> list = _aktifIslemService.GetListDto().Data;
            lvMGrup.ItemsSource = list;
        }
        void SetUpdateField()
        {
            cbxFurnace.SelectedValue = _selectedAktifFurnace.FurnaceId;
            cbxIslem.SelectedValue = _selectedAktifFurnace.IslemId;

            LabelKaydet.Content = "Güncelle";
        }
        void ClearField()
        {
            _selectedAktifFurnace = null;
            cbxIslem.SelectedIndex = -1;
            cbxFurnace.SelectedIndex = -1;
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
                _selectedAktifFurnace = _aktifIslemService.GetById((lvMGrup.SelectedItem as AktifIslemDto).Id).Data;
                SetUpdateField();
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            //Güncelle yada yeni kayıt
            if (_selectedAktifFurnace is null)
            {
                //Yeni Kayıt
                AktifIslem aktifIslem = new AktifIslem();
                aktifIslem.FurnaceId =(int) cbxFurnace.SelectedValue;
                aktifIslem.IslemId=(int)cbxIslem.SelectedValue;
                aktifIslem.BolumId = selectedBolumId;
                aktifIslem.Active = 1;
                var result = _aktifIslemService.Add(aktifIslem);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }
            }
            else
            {
                //Güncelleme

                _selectedAktifFurnace.FurnaceId = (int)cbxFurnace.SelectedValue;
                _selectedAktifFurnace.IslemId = (int)cbxIslem.SelectedValue;
                _selectedAktifFurnace.BolumId = selectedBolumId;
                _selectedAktifFurnace.Active = 1;
                var result = _aktifIslemService.Update(_selectedAktifFurnace);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }

            }

        }


        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (!(_selectedAktifFurnace is null))
            {
                var result = _aktifIslemService.Delete(_selectedAktifFurnace);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    ClearField();
                    LoadIslem();
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void CbxByBolumId(int xId)
        {
            List<FurnaceDto> furnaces = _furnaceService.GetListByBolumId(xId).Data;
            cbxFurnace.SelectedValuePath = "Id";
            cbxFurnace.DisplayMemberPath = "Name";
            cbxFurnace.ItemsSource = furnaces;
            selectedBolumId = xId;

        }
        private void Vacum_ElementClick(object sender, RoutedEventArgs e)
        {
            if (!vacum.IsActive)
            {
                vacum.IsActive = true;
                tuz.IsActive = false;
                ath.IsActive = false;
                ind.IsActive = false;
                lvMGrup.Items.Filter = VacumFilter;
                CbxByBolumId(1);
                ClearField();
            }
        }

        private void Tuz_ElementClick(object sender, RoutedEventArgs e)
        {
            if (!tuz.IsActive)
            {
                vacum.IsActive = false;
                tuz.IsActive = true;
                ath.IsActive = false;
                ind.IsActive = false;
                lvMGrup.Items.Filter = TuzFilter;
                CbxByBolumId(2);
                ClearField();

            }
        }
        private void Ind_ElementClick(object sender, RoutedEventArgs e)
        {
            if (!ind.IsActive)
            {
                vacum.IsActive = false;
                tuz.IsActive = false;
                ath.IsActive = false;
                ind.IsActive = true;
                lvMGrup.Items.Filter = IndFilter;
                CbxByBolumId(3);
                ClearField();
            }
        }
        private void Ath_ElementClick(object sender, RoutedEventArgs e)
        {
            if (!ath.IsActive)
            {
                vacum.IsActive = false;
                tuz.IsActive = false;
                ath.IsActive = true;
                ind.IsActive = false;
                lvMGrup.Items.Filter = AthFilter;
                CbxByBolumId(4);
                ClearField();
            }
        }

        private void txtMgrupName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            //Win_Tnm_List_Malzeme_Fiyat window = new Win_Tnm_List_Malzeme_Fiyat(this);
            //window.Effect = new BlurEffect();
            //window.ShowDialog();
            //window.Effect = null;
        }

        private void btnFurnaceAdd_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_Furnace window = new Win_Tnm_Furnace();
            this.Visibility= Visibility.Hidden;
            window.ShowDialog();
            this.Visibility = Visibility.Visible;
        }

        private void btnIslemAdd_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_Islem window = new Win_Tnm_Islem();
            this.Visibility = Visibility.Hidden;
            window.ShowDialog();
            this.Visibility = Visibility.Visible;
            islemLoad();
            ClearField();
            
        }
    }
}
