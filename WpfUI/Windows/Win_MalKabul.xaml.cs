using Business.Abstract;
using Business.Concrete;
using Castle.Core.Internal;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.PortableExecutable;
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
    /// Win_MalKabul.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_MalKabul : Window
    {
        public Win_MalKabul()
        {
            InitializeComponent();
            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);
            _malzemeGrupService = new MalzemeGrupManager(new EfMalzemeGrupDal());

            //mal giriş
            _malGirisIslemService = new MalGirisIslemManager(new EfMalGirisIslemDal());
            _malGirisMalzemeService = new MalGirisMalzemeManager(new EfMalGirisMalzemeDal(), _malGirisIslemService);
            _malGirisKayitService = new MalGirisKayitManager(new EfMalGirisKayitDal(), _malGirisMalzemeService);

           
           

            _bolumIslemService = new BolumIslemManager(new EfBolumIslemDal());
            _fiyatService = new FiyatManager(new EfFiyatDal());

            _bolumService = new BolumManager(new EfBolumDal());
            loadServis();
             
        }

        private Timer m_toolTipClosingTimer;

        //Dışardan listeler
        public Musteri _selectedMusteri;// gelen
        public Malzeme _selectedMalzeme;// gelen

        //iç piyasa
        MalGirisKayit _selectedMalGirisKayit;
        MalGirisMalzeme _malGirisMalzeme;

        int selectedBolumId;
        //Servisler
        IMalzemeGrupService _malzemeGrupService;
        IBolumService _bolumService;
        IMalGirisKayitService _malGirisKayitService;
        IMalGirisMalzemeService _malGirisMalzemeService;
        IMalGirisIslemService _malGirisIslemService;

        IBolumIslemService _bolumIslemService;
        IFiyatService _fiyatService;
        void malgirisKayit()
        {
            MalGirisKayit girisKayit = new MalGirisKayit();
            girisKayit.UserId = 1;
            var result = _malGirisKayitService.Add(girisKayit);
            CommandMessage(result.IsSuccess, result.Message);
            if (result.IsSuccess)
            {
                _selectedMalGirisKayit = girisKayit;
                 MessageBox.Show(girisKayit.Id.ToString());
            }

        }
        private void ToolTipClosingCallBack(object p_useless)
        {
            Dispatcher.Invoke(() =>
            {
                popup_uc.IsOpen = false;
            });
        }
        public void selectedMusteriLoad()
        {
            txtFirmaName.Text = _selectedMusteri.FirmaAd.ToUpper();
            txtFirmaNot.Text = _selectedMusteri.Note;
            BrushConverter bc = new BrushConverter();
            FirmaExpander.Background = (Brush)new BrushConverter().ConvertFrom("#68B984");
        }
        public void selectedMalzemeLoad()
        {
            txtMalStandartno.Text = _selectedMalzeme.MalzemeName;
            txtSertlik.Text = _selectedMalzeme.MaxSertlik;
            txtMalzemeGrubu.Text = _malzemeGrupService.GetById(_selectedMalzeme.MGrupId).Data.GrupName;
        }
        void clearFieldMalzeme()
        {

            txtMalStandartno.Clear();
            txtMalzemeGrubu.Clear();
            txtSertlik.Clear();
            txtSertDerin.Clear();
            txtAdet.Clear();
            txtAgirlik.Clear();
            _selectedMalzeme = null;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
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

        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMalGirisKayit != null)
            {
                MalGirisKayit kayit = new MalGirisKayit();
                kayit = _selectedMalGirisKayit;
                kayit.FirmaId = _selectedMusteri.Id;
                // kayit.GirisTarihi
                //kayit.TeslimTarihi
                // kayit.UserId
                var result = _malGirisKayitService.Update(kayit);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message);

                }
            }
            else
            {
                MessageBox.Show("Firma giriniz.");
            }

            //update yap ve bir çıkış bekle ; işlem tanımlanmamış yada bir nedenle yapılamıyor vs

        }
        public string filterchar;
        private void txtFirmaName_KeyDown(object sender, KeyEventArgs e)
        {
            
            e.Handled = true;
            filterchar = e.Key.ToString();
            Win_MalKabul_List_Musteri window = new Win_MalKabul_List_Musteri(this);
            //winGrid.Effect = new BlurEffect();
            window.ShowDialog();
            //winGrid.Effect = null;


        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMalGirisKayit==null)
            {
                malgirisKayit();
            }
           
            Win_MalKabul_List_Musteri window = new Win_MalKabul_List_Musteri(this);
            //winGrid.Effect = new BlurEffect();
            window.ShowDialog();


        }

        private void txtMalStandartno_KeyDown(object sender, KeyEventArgs e)
        {
            if (_selectedMusteri != null)
            {
                e.Handled = true;
                filterchar = e.Key.ToString();
                Win_MalKabul_List_Malzeme window = new Win_MalKabul_List_Malzeme(this);
                //winGrid.Effect = new BlurEffect();
                window.ShowDialog();
                //winGrid.Effect = null;
            }
            else
            {

                MessageBox.Show("Firma Seçiniz", "UYARI", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMalStandartno.Clear();
            }

        }

        private void btnSelectMalzeme_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMusteri != null)
            {
                Win_MalKabul_List_Malzeme window = new Win_MalKabul_List_Malzeme(this);
                //winGrid.Effect = new BlurEffect();
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Firma Seçiniz", "UYARI", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        bool malzemeControl()
        {
            if (_selectedMusteri == null)
            {
                MessageBox.Show("Firma Seçiniz", "UYARI", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtAdet.Text))
            {
                MessageBox.Show("Adet değeri girilmedi", "UYARI", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtAgirlik.Text))
            {
                MessageBox.Show("Ağırlık giriniz", "UYARI", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (_selectedMalzeme == null)
            {
                MessageBox.Show("Malzeme Seçiniz", "UYARI", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        bool islemControl()
        {
            if (lvMalzeme.SelectedIndex == -1)
            {
                MessageBox.Show("Malzeme Seçilmedi", "UYARI", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (cbxBolumIslem.SelectedIndex == -1)
            {
                MessageBox.Show("İşlem seçilmedi!", "UYARI", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtBirimFiyat.Text))
            {
                MessageBox.Show("Birim Fiyat giriniz", "UYARI", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        void loadMalzeme()
        {
            if (_selectedMalGirisKayit != null)
            {
                List<MalGirisMalzemeDto> malzemedtos = _malGirisMalzemeService.GetListByDto(selectedBolumId, _selectedMalGirisKayit.Id).Data;
                lvMalzeme.ItemsSource = malzemedtos;

                clearFieldMalzeme();
            }
        }
        private void btnMalzemeEkle_Click(object sender, RoutedEventArgs e)
        {
            // Kayıtlı malzemeyi ekler
            //  malzemeEkle();
            // _malGirisMalzemeService
            if (malzemeControl())
            {
                MalGirisMalzeme malGirisMalzeme = new MalGirisMalzeme();
                malGirisMalzeme.MalGirisId = _selectedMalGirisKayit.Id;
                malGirisMalzeme.Adet = Convert.ToInt16(txtAdet.Text);
                malGirisMalzeme.Agirlik = Convert.ToDouble(txtAgirlik.Text);
                malGirisMalzeme.Sertlik = txtSertlik.Text;
                malGirisMalzeme.SertlikDerinlik = txtSertDerin.Text;
                malGirisMalzeme.BolumId = selectedBolumId;
                malGirisMalzeme.MalzemeId = _selectedMalzeme.Id;
                var result = _malGirisMalzemeService.Add(malGirisMalzeme);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    loadMalzeme();

                }
            }
        }
        void CbxByBolumId(int xId)
        {
            List<BolumIslem> bolumler = _bolumIslemService.GetListByBolumId(xId).Data;
            cbxBolumIslem.SelectedValuePath = "Id";
            cbxBolumIslem.DisplayMemberPath = "BolumIslemi";

            cbxBolumIslem.ItemsSource = bolumler;
            selectedBolumId = xId;
            if (_selectedMalGirisKayit != null)
            {
                loadMalzeme();
                loadIslem();
                clearFieldMalzeme();
                clearFieldIslem();
                if (lvMalzeme.Items.Count > 0)
                {
                    lvMalzeme.SelectedIndex = 0;
                    lvMalzeme.Focus();
                }
            }

            //cbxBolumIslem.ItemsSource
            // malzemeleri Listele



        }

        void clearFieldIslem()
        {
            txtBirimFiyat.Clear();
            txtNotIslem.Clear();
            cbxBolumIslem.SelectedIndex = -1;
        }
        void loadIslem()
        {
            if (_malGirisMalzeme != null)
            {
                List<MalGirisIslemDto> list = _malGirisIslemService.GetListByDto(selectedBolumId, _malGirisMalzeme.Id).Data;
                lvIslem.ItemsSource = list;
            }

        }
        private void btnAddIslem_Click(object sender, RoutedEventArgs e)
        {
            if (islemControl())
            {
                MalGirisIslem malGirisislem = new MalGirisIslem();
                malGirisislem.BolumIslemId = (int)cbxBolumIslem.SelectedValue;
                malGirisislem.BirimFiyat = Convert.ToDouble(txtBirimFiyat.Text);
                malGirisislem.Not = txtNotIslem.Text;
                malGirisislem.MalGirisMalzemeId = _malGirisMalzeme.Id;
                if (true)
                {
                    //fiyat değiştimi???
                }
                var result = _malGirisIslemService.Add(malGirisislem);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    loadIslem();
                    clearFieldIslem();

                }
            }
        }

        private void cbxBolumIslem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // txtBirimFiyat.Text=
        }

        private void lvMalzeme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(lvMalzeme.SelectedItems is null) && lvMalzeme.SelectedItems.Count > 0)  //seçilen item null değilse
            {

                _malGirisMalzeme = _malGirisMalzemeService.GetById((lvMalzeme.SelectedItem as MalGirisMalzemeDto).Id).Data;
                loadIslem();
            }
        }
        void loadServis()
        {
            List<Bolum> bolums = _bolumService.GetList().Data;
            lbx.ItemsSource = bolums;
            if (lbx.Items.Count > 0) lbx.SelectedIndex = 0;
        }


        private void lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(lbx.SelectedItems is null) && lbx.SelectedItems.Count > 0)  //seçilen item null değilse
            {
                CbxByBolumId((lbx.SelectedItem as Bolum).Id);
            }
        }
        private void AGotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = sender as FrameworkElement;
                Bolum item = element.DataContext as Bolum;
                if (item != null)
                {
                    lbx.SelectedValue = item;
                }
            }
            catch { }
        }
    }
}
