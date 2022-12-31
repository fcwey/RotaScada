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
    /// Win_Tnm_MalzemeTnm.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Tnm_MalzemeTnm : Window
    {

        private Timer m_toolTipClosingTimer;
        public Win_Tnm_MalzemeTnm()
        {
            InitializeComponent();

            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);


            _malzemeGrupService = new MalzemeGrupManager(new EfMalzemeGrupDal());
            _malzemeService = new MalzemeManager(new EfMalzemeDal());
            txtMgrupName.Focus();
            LoadMalzemes();
        }
        readonly IMalzemeGrupService _malzemeGrupService;
        readonly IMalzemeService _malzemeService;
       
        public string filterchar;
        public MalzemeGrup _selectedMGrup;
        Malzeme _selectedMalzeme;
        MalzemeDto _selectedMalzemeDto;

        List<MalzemeDto> malzemeDtoList;
        List<Malzeme> malzemeList;

        ToolTip tp = new ToolTip();

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
        void LoadMalzemes()
        {
            malzemeDtoList = _malzemeService.GetListMalzemeDtos().Data;

            lvMalzemeDto.ItemsSource = malzemeDtoList;
        }
        void SetUpdateField()
        {
            // tbxMgrupName.Text = _selectedMGrup.GrupName;
            txtMgrupName.Text = _selectedMalzemeDto.MGrupName;
            txtMStandartNo.Text = _selectedMalzemeDto.MalzemeName;
            txtMaxSertlik.Text = _selectedMalzemeDto.MaxSertlik;
            LabelKaydet.Content = "Güncelle";
        }
        void ClearField()
        {
            // tbxMgrupName.Text = string.Empty;
            _selectedMGrup = null;
            _selectedMalzeme = null;
            _selectedMalzemeDto = null;
            txtMaxSertlik.Text = string.Empty;
            txtMgrupName.Text = string.Empty;
            txtMStandartNo.Text = string.Empty;
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

        private void lvMalzemeDto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(lvMalzemeDto.SelectedItems is null) && lvMalzemeDto.SelectedItems.Count > 0)  //seçilen item null değilse
            {
                _selectedMalzeme = _malzemeService.GetById((lvMalzemeDto.SelectedItem as MalzemeDto).Id).Data;
                _selectedMalzemeDto = lvMalzemeDto.SelectedItem as MalzemeDto;
                _selectedMGrup = null;
                SetUpdateField();
                //_carService.Delete(cars.FirstOrDefault(x => x.Id == (lvCars.SelectedItem as CarDetailDto).Id));
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            //Güncelle yada yeni kayıt
            if (_selectedMalzemeDto is null)
            {
                //Yeni Kayıt
                Malzeme malzeme = new Malzeme();
                malzeme.MGrupId = _selectedMGrup.Id;
                malzeme.MalzemeName = txtMStandartNo.Text;
                malzeme.MaxSertlik = txtMaxSertlik.Text;

                var result = _malzemeService.Add(malzeme);
                if (result.IsSuccess)
                {
                    LoadMalzemes();
                }
            }
            else
            {
                //update

                _selectedMalzeme.MaxSertlik = txtMaxSertlik.Text;
                _selectedMalzeme.MalzemeName= txtMStandartNo.Text;
                if (_selectedMGrup==null)
                {
                    _selectedMalzeme.MGrupId = _selectedMalzeme.MGrupId;
                }
                else
                {
                    _selectedMalzeme.MGrupId = _selectedMGrup.Id;
                }
                
                var result = _malzemeService.Update(_selectedMalzeme);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadMalzemes();
                }

            }

        }


        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (!(_selectedMalzeme is null))
            {
                var result = _malzemeService.Delete(_selectedMalzeme);
                CommandMessage(result.IsSuccess, result.Message);
                if (result.IsSuccess)
                {
                    LoadMalzemes();
                    ClearField();
                }
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_List_Malzeme window = new Win_Tnm_List_Malzeme(this);
            winGrid.Effect = new BlurEffect();
            window.ShowDialog();
            winGrid.Effect = null;
        }

      

        public void txtMgrupName_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            filterchar = e.Key.ToString();
            Win_Tnm_List_Malzeme window = new Win_Tnm_List_Malzeme(this);
            winGrid.Effect = new BlurEffect();
            window.ShowDialog();
            winGrid.Effect = null;
        }

       
    }
}
