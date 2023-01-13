using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _malGirisKayitService = new MalGirisKayitManager(new EfMalGirisKayitDal());
            _malGirisMalzemeService = new MalGirisMalzemeManager(new EfMalGirisMalzemeDal());
            _malGirisIslemService = new MalGirisIslemManager(new EfMalGirisIslemDal());

            malgirisKayit();
        }

        private Timer m_toolTipClosingTimer;

        //Dışardan listeler
        public Musteri _selectedMusteri;// gelen
        public Malzeme _selectedMalzeme;// gelen

        //iç piyasa
        MalGirisKayit _selectedMalGirisKayit;


        //Servisler
        IMalzemeGrupService _malzemeGrupService;

        IMalGirisKayitService _malGirisKayitService;
        IMalGirisMalzemeService _malGirisMalzemeService;
        IMalGirisIslemService _malGirisIslemService;
        void malgirisKayit()
        {
            MalGirisKayit girisKayit = new MalGirisKayit();
            girisKayit.GirisTarihi=DateTime.Now;
            girisKayit.TeslimTarihi=DateTime.Now;
            girisKayit.FirmaId = 1;
            girisKayit.UserId = 1;
            var result = _malGirisKayitService.Add(girisKayit);
            CommandMessage(result.IsSuccess, result.Message);
            if (result.IsSuccess)
            {
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
                txtMalStandartno.Clear();
            }
        }
    }
}
