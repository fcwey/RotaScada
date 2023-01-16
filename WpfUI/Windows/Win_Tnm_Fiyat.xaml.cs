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
    /// Win_Tnm_Fiyat.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Tnm_Fiyat : Window
    {
        public Win_Tnm_Fiyat()
        {

            InitializeComponent();
            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);


            _bolumIslemService = new BolumIslemManager(new EfBolumIslemDal());
            _bolumService = new BolumManager(new EfBolumDal());
            _fiyatService = new FiyatManager(new EfFiyatDal());
            _malzemeGrupService = new MalzemeGrupManager(new EfMalzemeGrupDal());
            _servisService = new ServisManager(new EfServisDal());
            CbxByBolumId(1);
            LoadIslem();
            lvMGrup.Items.Filter = VacumFilter;

        }
        //mouse sürükle

        //tooltip timer
        private Timer m_toolTipClosingTimer;

        readonly IBolumIslemService _bolumIslemService;
        readonly IBolumService _bolumService;
        readonly IFiyatService _fiyatService;
        readonly IMalzemeGrupService _malzemeGrupService;
        readonly IServisService _servisService;

        FiyatDto _selectedFiyatDto;
        Fiyat _selectedFiyat;
        BolumIslem _selectedBolumIslem;
        public MalzemeGrup _selectedMGrup;
        ToolTip tp = new ToolTip();

        public string filterchar;


        private void ToolTipClosingCallBack(object p_useless)
        {
            Dispatcher.Invoke(() =>
            {
                popup_uc.IsOpen = false;
            });
        }
        public void selectedMalzemeLoad()
        {
            txtMgrupName.Text = _selectedMGrup.GrupName;
        }
        private bool VacumFilter(object obj)
        {

            var FilterObj = obj as FiyatDto;
            //Listede kaç tane

            return FilterObj.BolumName.Contains("Vakum Fırın", StringComparison.OrdinalIgnoreCase);
        }
        private bool AthFilter(object obj)
        {

            var FilterObj = obj as FiyatDto;
            //Listede kaç tane

            return FilterObj.BolumName.Contains("Atmosfer", StringComparison.OrdinalIgnoreCase);
        }
        private bool TuzFilter(object obj)
        {

            var FilterObj = obj as FiyatDto;
            //Listede kaç tane

            return FilterObj.BolumName.Contains("Tuz", StringComparison.OrdinalIgnoreCase);
        }
        private bool IndFilter(object obj)
        {

            var FilterObj = obj as FiyatDto;
            //Listede kaç tane

            return FilterObj.BolumName.Contains("İnduksiyon", StringComparison.OrdinalIgnoreCase);
        }
        void LoadIslem()
        {
            List<FiyatDto> fiyat = _fiyatService.GetListFiyatDtos().Data;
            lvMGrup.ItemsSource = fiyat;
        }
        void SetUpdateField()
        {
            tbxFiyat.Text = _selectedFiyatDto.Fiyati.ToString();
            cbxBolumIslem.SelectedValue = _selectedFiyat.BolumIslemId;
            txtMgrupName.Text = _selectedFiyatDto.MGrupName;
            _selectedMGrup = _malzemeGrupService.GetById(_selectedFiyat.MGrupId).Data;


            LabelKaydet.Content = "Güncelle";
        }
        void ClearField()
        {
            tbxFiyat.Text = string.Empty;
            _selectedFiyat = null;
            _selectedFiyatDto = null;
            txtMgrupName.Text = string.Empty;
            cbxBolumIslem.SelectedIndex= -1;
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
                _selectedFiyatDto = lvMGrup.SelectedItem as FiyatDto;
                _selectedFiyat = _fiyatService.GetById(_selectedFiyatDto.Id).Data;
                SetUpdateField();
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            //Güncelle yada yeni kayıt
            if (_selectedFiyatDto is null)
            {
                //Yeni Kayıt
                Fiyat fiyat = new Fiyat();
                fiyat.Fiyati = Convert.ToDouble(tbxFiyat.Text);
                fiyat.MGrupId = _selectedMGrup.Id;
                fiyat.BolumIslemId = (int)cbxBolumIslem.SelectedValue;
                var result = _fiyatService.Add(fiyat);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }
            }
            else
            {
                //Güncelleme

                _selectedFiyat.Fiyati = Convert.ToDouble(tbxFiyat.Text);
                _selectedFiyat.MGrupId = _selectedMGrup.Id;
                _selectedFiyat.BolumIslemId =(int) cbxBolumIslem.SelectedValue;
                var result = _fiyatService.Update(_selectedFiyat);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadIslem();
                }

            }

        }


        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (!(_selectedFiyatDto is null))
            {
                var result = _fiyatService.Delete(_selectedFiyat);
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
            //Close();
            //       cbxBolumIslem.SelectedIndex= 13;
            MessageBox.Show(cbxBolumIslem.SelectedValue.ToString());
        }
        
        void CbxByBolumId(int xId)
        {
             List<BolumIslem> bolumler = _bolumIslemService.GetListByBolumId(xId).Data;
            cbxBolumIslem.SelectedValuePath = "Id";
            cbxBolumIslem.DisplayMemberPath = "BolumIslemi";

            cbxBolumIslem.ItemsSource = bolumler;


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
            //BAK
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_List_Malzeme_Fiyat window = new Win_Tnm_List_Malzeme_Fiyat(this);
            window.Effect = new BlurEffect();
            window.ShowDialog();
            window.Effect = null;
        }

   
    }
}
