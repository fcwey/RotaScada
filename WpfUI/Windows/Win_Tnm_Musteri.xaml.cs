using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Win_Tnm_Musteri.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Tnm_Musteri : Window
    {
        private Timer m_toolTipClosingTimer;
        public Win_Tnm_Musteri()
        {
            InitializeComponent();

            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);

            _musteriService = new MusteriManager(new EfMusteriDal());
            _musteriFiyatService = new MusteriFiyatManager(new EfMusteriFiyatDal());
            _servisService = new ServisManager(new EfServisDal());
            //txtMgrupName.Focus();
            List<ServisDto> servisler = _servisService.GetListDto().Data;
            cbxServisler.ItemsSource = servisler;
            cbxServisler.DisplayMemberPath = "Name";
            cbxServisler.SelectedValuePath = "Id";

            LoadMFiyat();
            cbxServisler.Visibility = Visibility.Hidden;
        }
        readonly IMusteriService _musteriService;
        readonly IMusteriFiyatService _musteriFiyatService;
        readonly IServisService _servisService;

        public string filterchar;
        public Musteri _selectedMusteri;// gelen
        MusteriDto _selectedMusteriDto;

        List<MusteriFiyatDto> musteriFiyatDtos;

        ToolTip tp = new ToolTip();

        private void ToolTipClosingCallBack(object p_useless)
        {
            Dispatcher.Invoke(() =>
            {
                popup_uc.IsOpen = false;
            });
        }
        enum RadioOptions { Option1, Option2 }
        public class EnumBooleanConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value.Equals(parameter);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return ((bool)value) ? parameter : Binding.DoNothing;
            }
        }
        public void selectedMusteriLoad()
        {

            txtSearch.Text = _selectedMusteri.FirmaAd;
            SetUpdateField();
            LoadMFiyat();


        }

        void LoadMFiyat()
        {
            if (_selectedMusteri != null)
            {
                musteriFiyatDtos = _musteriFiyatService.GetListDto(_selectedMusteri.Id).Data;//  _malzemeService.GetListMalzemeDtos().Data;
                lv.ItemsSource = musteriFiyatDtos;
            }
            else
            {
                musteriFiyatDtos = _musteriFiyatService.GetListDto(0).Data;//  _malzemeService.GetListMalzemeDtos().Data;
                lv.ItemsSource = musteriFiyatDtos;
            }
           

        }
        void SetUpdateField()
        {
            txtAdress.Text = _selectedMusteri.Adres;
            txtAdress1.Text = _selectedMusteri.Adres2;
            txtWebAdress.Text = _selectedMusteri.Web;
            txtEmail.Text = _selectedMusteri.Email;
            txtFirmaAdi.Text = _selectedMusteri.FirmaAd;
            txtNot.Text = _selectedMusteri.Note;
            txtTelefon.Text = _selectedMusteri.Telefon;
            txtYetkili.Text = _selectedMusteri.Yetkili;

            //Ödeme Türü
            if (_selectedMusteri.OdemeTuru == 0)
            {
                rbtnNakit.IsChecked = true;
            }
            else if (_selectedMusteri.OdemeTuru == 1)
            {
                rbtnHesap.IsChecked = true;
            }
            else if (_selectedMusteri.OdemeTuru == 2)
            {
                rbtnMuhasebe.IsChecked = true;
            }


            //Nakliye Durumu
            if (_selectedMusteri.Nakliye == 0)
            {
                rbtnMusteri.IsChecked = true;
            }
            else if (_selectedMusteri.Nakliye == 1)
            {
                rbtnServis.IsChecked = true;
                cbxServisler.SelectedValue =(int) _selectedMusteri.ServisId;
            }
            LabelKaydet.Content = "Güncelle";
            
        }
        void ClearField()
        {
            txtAdress.Text = string.Empty;
            txtAdress1.Text = string.Empty;
            txtWebAdress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtFirmaAdi.Text = string.Empty;
            txtNot.Text = string.Empty;
            txtTelefon.Text = string.Empty;
            txtYetkili.Text = string.Empty;

            rbtnMusteri.IsChecked = true;
            rbtnNakit.IsChecked = true;
            txtSearch.Text = string.Empty;
            _selectedMusteri = null;
            //cbxServisler.SelectedIndex = -1;
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

        private void lvMalzemeDto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (!(lvMalzemeDto.SelectedItems is null) && lvMalzemeDto.SelectedItems.Count > 0)  //seçilen item null değilse
            //{
            //    _selectedMalzeme = _malzemeService.GetById((lvMalzemeDto.SelectedItem as MalzemeDto).Id).Data;
            //    _selectedMalzemeDto = lvMalzemeDto.SelectedItem as MalzemeDto;
            //    _selectedMGrup = null;
            //    SetUpdateField();
            //    //_carService.Delete(cars.FirstOrDefault(x => x.Id == (lvCars.SelectedItem as CarDetailDto).Id));
            //}
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            //Güncelle yada yeni kayıt
            if (_selectedMusteri is null)
            {
                //Yeni Kayıt
                Musteri musteri = new Musteri();
                musteri.FirmaAd = txtFirmaAdi.Text;
                musteri.Yetkili = txtYetkili.Text;
                musteri.Adres = txtAdress.Text;
                musteri.Adres2 = txtAdress1.Text;
                musteri.Email = txtEmail.Text;
                musteri.Telefon = txtTelefon.Text;
                musteri.Web = txtWebAdress.Text;
                musteri.Note = txtNot.Text;

                //ödeme türü
                if (rbtnNakit.IsChecked==true)
                {
                    musteri.OdemeTuru = 0;
                }
                else if (rbtnHesap.IsChecked == true)
                {
                    musteri.OdemeTuru = 1;
                }
                else if (rbtnMuhasebe.IsChecked == true)
                {
                    musteri.OdemeTuru = 2;
                }

                //nakliye türü
                if (rbtnMusteri.IsChecked == true)
                {
                    musteri.OdemeTuru = 0;
                }
                else if (rbtnServis.IsChecked == true)
                {
                    musteri.OdemeTuru = 1;
                    musteri.ServisId =(int) cbxServisler.SelectedValue;
                }


                var result = _musteriService.Add(musteri);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                   // LoadMalzemes();
                }
            }
            else
            {
                //update
                _selectedMusteri.FirmaAd = txtFirmaAdi.Text;
                _selectedMusteri.Yetkili = txtYetkili.Text;
                _selectedMusteri.Adres = txtAdress.Text;
                _selectedMusteri.Adres2 = txtAdress1.Text;
                _selectedMusteri.Email = txtEmail.Text;
                _selectedMusteri.Telefon = txtTelefon.Text;
                _selectedMusteri.Web = txtWebAdress.Text;
                _selectedMusteri.Note = txtNot.Text;

                //ödeme türü
                if (rbtnNakit.IsChecked == true)
                {
                    _selectedMusteri.OdemeTuru = 0;
                }
                else if (rbtnHesap.IsChecked == true)
                {
                    _selectedMusteri.OdemeTuru = 1;
                }
                else if (rbtnMuhasebe.IsChecked == true)
                {
                    _selectedMusteri.OdemeTuru = 2;
                }

                //nakliye türü
                if (rbtnMusteri.IsChecked == true)
                {
                    _selectedMusteri.Nakliye = 0;
                }
                else if (rbtnServis.IsChecked == true)
                {
                    _selectedMusteri.Nakliye = 1;
                    _selectedMusteri.ServisId = (int)cbxServisler.SelectedValue;
                }



                var result = _musteriService.Update(_selectedMusteri);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                  //  LoadMalzemes();
                }

            }

        }


        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (!(_selectedMusteri is null))
            {
                var result = _musteriService.Delete(_selectedMusteri);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                  //  LoadMalzemes();
                    ClearField();
                }
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_List_Musteri window = new Win_Tnm_List_Musteri(this);
            winGrid.Effect = new BlurEffect();
            window.ShowDialog();
            winGrid.Effect = null;
        }




        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            filterchar = e.Key.ToString();
            Win_Tnm_List_Musteri window = new Win_Tnm_List_Musteri(this);
            winGrid.Effect = new BlurEffect();
            window.ShowDialog();
            winGrid.Effect = null;
        }

        private void rbtnServis_Checked(object sender, RoutedEventArgs e)
        {

            cbxServisler.Visibility = Visibility.Visible;
            
        }

        private void rbtnMusteri_Checked(object sender, RoutedEventArgs e)
        {
            if (cbxServisler!=null)
            {
                cbxServisler.Visibility = Visibility.Hidden;
            }
            
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show(cbxServisler.SelectedValue.ToString());
        }
    }
}
