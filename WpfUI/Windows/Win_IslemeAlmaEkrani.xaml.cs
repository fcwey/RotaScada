using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using static Entities.Constant.TakipStatus;

namespace WpfUI.Windows
{
    /// <summary>
    /// Win_IslemeAlmaEkrani.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_IslemeAlmaEkrani : Window
    {
        Win_IslemTakip _winislemtakip = new Win_IslemTakip();
        public Win_IslemeAlmaEkrani(Win_IslemTakip winislemtakip)
        {
            InitializeComponent();
            _winislemtakip = winislemtakip;
            _aktifIslemService = new AktifIslemManager(new EfAktifIslemDal());
            _islemTakipService = new IslemTakipManager(new EfIslemTakipDal());
            lvTakipList.ItemsSource = this._winislemtakip.lvTakipListe.Items.Cast<IslemTakipDto>().Where(p => p.IsSelected == true).ToList();
            LoadAkt();

        }
        IAktifIslemService _aktifIslemService;
        IIslemTakipService _islemTakipService;
       async void LoadAkt()
        {
            var list = await _aktifIslemService.GetListAktifIslemDetailDtosAsync((_winislemtakip.lbx.SelectedItem as Bolum).Id);
            lvAkt.ItemsSource = list.Data;
            //lvAkt.ItemsSource = akt;

            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvAkt.ItemsSource);
            //PropertyGroupDescription groupDescription = new PropertyGroupDescription("FurnaceName");
            //view.GroupDescriptions.Add(groupDescription);

            //this._winMalKabul.lvTakipListe
 
           

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvAkt.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("FurnaceName");
            view.GroupDescriptions.Add(groupDescription);

        }


        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private async void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            var list = lvTakipList.Items.Cast<IslemTakipDto>().ToList();
            int aktId = (lvAkt.SelectedItem as AktifIslemDetailDto).Id;
            await _islemTakipService.SendToFurnaceAsync(list, aktId);
            
            //lvAkt.SelectedItems
            //int x= (lvAkt.SelectedItem as AktifIslemDto).;
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
