using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
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

namespace WpfUI.Windows
{
    /// <summary>
    /// win_Deneme.xaml etkileşim mantığı
    /// </summary>
    public partial class win_Deneme : Window
    {
        public List<Servis> servisler { get; set; }
        public Servis selected { get; set; }
        public win_Deneme()
        {
            InitializeComponent();
            _servisService = new ServisManager(new EfServisDal());
            _bolumService = new BolumManager(new EfBolumDal());
            loadServis();
        }
        IServisService _servisService;
        IBolumService _bolumService;
        Servis _servis;
        void loadServis()
        {
            List<Bolum> servis = _bolumService.GetList().Data;
            lbx.ItemsSource= servis;
        }

        private void lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(lbx.SelectedItems is null) && lbx.SelectedItems.Count > 0)  //seçilen item null değilse
            {
                //_servis = lbx.SelectedItem as Servis;
                //MessageBox.Show(_servis.Name.ToString());
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
