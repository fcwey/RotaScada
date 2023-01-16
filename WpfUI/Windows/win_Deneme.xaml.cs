using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
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
            loadServis();
        }
        IServisService _servisService;
        void loadServis()
        {
            List<Servis> servis = _servisService.GetList().Data;
            lbx.ItemsSource= servis;
            //Collection= servis;
        }

        private void lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
