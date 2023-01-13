using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfUI.Windows;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            //Close();
        }

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Menu_MGrup_Click(object sender, RoutedEventArgs e)
        {

            //WindowMalzemeGrupTanimlari WinMgrup= new WindowMalzemeGrupTanimlari();
            WindowMGrup WinMgrup = new WindowMGrup();
            WinMgrup.ShowDialog();
        }

      
       

        private void Menu_MalzemeAdd_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_MalzemeTnm WinMgrup = new Win_Tnm_MalzemeTnm();
            WinMgrup.ShowDialog();
        }

        private void islemaAdd_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_Islem Winislem = new Win_Tnm_Islem();
            Winislem.ShowDialog();
        }

        private void bolumislemaAdd_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_BolumIslem WinBolumislem= new Win_Tnm_BolumIslem();
            WinBolumislem.ShowDialog();

        }

        private void servis_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_Servis WinServis = new Win_Tnm_Servis();
            WinServis.ShowDialog();
        }

        private void musteri_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_Musteri WinMusteri = new Win_Tnm_Musteri();
            WinMusteri.ShowDialog();
        }

        private void FiyatTanim_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_Fiyat WinFiyat = new Win_Tnm_Fiyat();
            WinFiyat.ShowDialog();
        }

        private void furnace_Click(object sender, RoutedEventArgs e)
        {
            Win_Tnm_AktifIslemler WinFurnace = new Win_Tnm_AktifIslemler();
            WinFurnace.ShowDialog();
        }

        private void malKabul_Click(object sender, RoutedEventArgs e)
        {
            Win_MalKabul WinFurnace = new Win_MalKabul();
            WinFurnace.ShowDialog();
        }
    }
}
