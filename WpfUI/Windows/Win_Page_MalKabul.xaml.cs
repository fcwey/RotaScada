using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Brush = System.Windows.Media.Brush;
using Color = System.Drawing.Color;

namespace WpfUI.Windows
{
    /// <summary>
    /// Win_Page_MalKabul.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Page_MalKabul : Window
    {
        public Win_Page_MalKabul()
        {
            InitializeComponent();
            m_toolTipClosingTimer = new Timer(ToolTipClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);
        }
        private Timer m_toolTipClosingTimer;
        public Musteri _selectedMusteri;// gelen
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
    }
}
