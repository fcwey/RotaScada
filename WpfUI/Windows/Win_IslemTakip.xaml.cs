using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Constant;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using WpfUI.Models;
using static Entities.Constant.TakipStatus;

namespace WpfUI.Windows
{
    /// <summary>
    /// Win_IslemTakip.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_IslemTakip : Window
    {
        public Win_IslemTakip()
        {
            InitializeComponent();
            _servisService = new ServisManager(new EfServisDal());
            _bolumService = new BolumManager(new EfBolumDal());
            _islemTakipService = new IslemTakipManager(new EfIslemTakipDal());
            _furnaceService = new FurnaceManager(new EfFurnaceDal());
            loadBolum();
            //  loadServis();


        }
        IServisService _servisService;
        IFurnaceService _furnaceService;
        IBolumService _bolumService;
        IIslemTakipService _islemTakipService;
        public List<IslemTakipDto> SelectedIslemTakipDtos;

        //kullanılmayabilir

        List<Unit> units = new List<Unit>
            {
                new Unit { ID=(int)UnitType.stokGiris, Name="STOK-GİRİŞ" },
                new Unit { ID=(int)UnitType.islemde,Name="İŞLEMDE" },
                new Unit { ID=(int)UnitType.kalitekontrol,Name="KALİTE-KONTROL"},
                 new Unit { ID=(int)UnitType.stokCikis,Name="STOK-ÇIKIŞ"},
                  new Unit { ID=(int)UnitType.hepsi,Name="HEPSİ"}
            };

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        void loadBolum()
        {
            List<Bolum> bolums = _bolumService.GetList().Data;
            lbx.ItemsSource = bolums;
            if (lbx.Items.Count > 0)
            {
                lbx.SelectedIndex = 0;
                if (!(lbx.SelectedItems is null) && lbx.SelectedItems.Count > 0)  //seçilen item null değilse
                {
                    ;
                    //MessageBox.Show(_servis.Name.ToString());
                    loadTakipListe((lbx.SelectedItem as Bolum).Id, TakipStatus.Nerede.StokGiris);
                }

            }
            lbxUnits.ItemsSource = units;
            if (lbxUnits.Items.Count > 0) lbxUnits.SelectedIndex = 0;
        }
        void loadServis()
        {
            List<Bolum> servis = _bolumService.GetList().Data;
            lbfurnace.ItemsSource = servis;
        }
        async void loadTakipListe(int bolumId, string nerede)
        {
            var list = await _islemTakipService.GetListDtoAsync(bolumId, nerede);
            lvTakipListe.ItemsSource = list.Data;
        }
        async void loadFurnaceListe(int bolumId)
        {
            var list = await _furnaceService.GetListFurnaceDetailDtosAsync(bolumId);
            lbfurnace.ItemsSource = list.Data;
        }


        private void lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(lbx.SelectedItems is null) && lbx.SelectedItems.Count > 0)  //seçilen item null değilse
            {
                ;
                //MessageBox.Show(_servis.Name.ToString());
                loadFurnaceListe((lbx.SelectedItem as Bolum).Id);
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

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Win_IslemTakip_FurnaceMalzemeList WinIslemAlmaDialog = new Win_IslemTakip_FurnaceMalzemeList(this);
            WinIslemAlmaDialog.ShowDialog();
        }
        private void frnAGotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = sender as FrameworkElement;
                FurnaceDetailDto item = element.DataContext as FurnaceDetailDto;
                if (item != null)
                {
                    lbfurnace.SelectedValue = item;
                }
            }
            catch { }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            //var filterResult = lvTakipListe.ItemsSource as List<IslemTakipDto>;
            //List<IslemTakipDto> liste = (List<IslemTakipDto>)filterResult.Where(x => x.IsSelected==true);
            var list = lvTakipListe.Items.Cast<IslemTakipDto>().Where(p => p.IsSelected == true).ToList().Count();
            if (lvTakipListe.Items.Cast<IslemTakipDto>().Where(p => p.IsSelected == true).ToList().Count() > 0)
            {
                Win_IslemeAlmaEkrani WinIslemAlmaDialog = new Win_IslemeAlmaEkrani(this);
                WinIslemAlmaDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Listeden malzeme seçilmedi. ");
            }

        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void lbxUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Select type ==1 ise
            // durumu değiştirir.
            //Stok kontrol 
            if (!(lbxUnits.SelectedItems is null) && lbxUnits.SelectedItems.Count > 0)  //seçilen item null değilse
            {
                ;
                //MessageBox.Show(_servis.Name.ToString());
                UnitType unite = (UnitType)(lbxUnits.SelectedItem as Unit).ID;
                switch (unite)
                {
                    case UnitType.stokGiris:

                        loadTakipListe((lbx.SelectedItem as Bolum).Id, TakipStatus.Nerede.StokGiris);
                        break;
                    case UnitType.islemde:
                        loadTakipListe((lbx.SelectedItem as Bolum).Id, TakipStatus.Nerede.Islem);
                        break;
                    case UnitType.kalitekontrol:
                        loadTakipListe((lbx.SelectedItem as Bolum).Id, TakipStatus.Nerede.KaliteKontrol);
                        break;
                    case UnitType.stokCikis:
                        loadTakipListe((lbx.SelectedItem as Bolum).Id, TakipStatus.Nerede.StokCikis);
                        break;
                    case UnitType.hepsi:
                        var list = await _islemTakipService.GetListDtoAsync();
                        lvTakipListe.ItemsSource = list.Data;
                        break;
                    default:
                        break;
                }
            }

        }
        private void AGotFocusUnit(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = sender as FrameworkElement;
                Unit item = element.DataContext as Unit;
                if (item != null)
                {
                    lbxUnits.SelectedValue = item;
                }
            }
            catch { }
        }

    }
}
