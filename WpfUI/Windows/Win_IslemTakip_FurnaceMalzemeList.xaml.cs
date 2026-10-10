using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Dtos;
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
using System.Windows.Shapes;
using System.Xml;

namespace WpfUI.Windows
{
    /// <summary>
    /// Win_IslemTakip_FurnaceMalzemeList.xaml etkileşim mantığı
    /// </summary>
      
    public partial class Win_IslemTakip_FurnaceMalzemeList : Window
    {
        Win_IslemTakip _winislemtakip = new Win_IslemTakip();
        public Win_IslemTakip_FurnaceMalzemeList(Win_IslemTakip winislemtakip)
        {
            InitializeComponent();
            _winislemtakip = winislemtakip;
            _islemTakipService = new IslemTakipManager(new EfIslemTakipDal());
            loadTakipListe();
        }
        IIslemTakipService _islemTakipService;
        async void loadTakipListe()
        {
            
            var list = await _islemTakipService.GetListDtoByFurnaceAsync((_winislemtakip.lbfurnace.SelectedValue as FurnaceDetailDto).Id);
            lvTakipListe.ItemsSource = list.Data;
            TbFurnaceName.Text = (_winislemtakip.lbfurnace.SelectedValue as FurnaceDetailDto).FurnaceName;
            TbFurnaceName.Foreground = new SolidColorBrush(Colors.Green);

            TbIslemName.Text = (_winislemtakip.lbfurnace.SelectedValue as FurnaceDetailDto).IslemAdı;
            //KG
            //Sıcaklık
            //Zaman
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
            //MessageBox.Show((_winislemtakip.lbfurnace.SelectedValue as FurnaceDetailDto).FurnaceName.ToString());
            this.Close();
        }
    }
}
