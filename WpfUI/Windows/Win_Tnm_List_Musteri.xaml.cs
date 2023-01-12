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
    /// Win_Tnm_List_Musteri.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Tnm_List_Musteri : Window
    {
        Win_Tnm_Musteri _winMusteri = new Win_Tnm_Musteri();
        public Win_Tnm_List_Musteri(Win_Tnm_Musteri winMusteri)
        {

            InitializeComponent();
            _winMusteri = winMusteri;
            _musteriService = new MusteriManager(new EfMusteriDal());
            LoadMGrup();
            txtFilter.Text = _winMusteri.filterchar;
            txtFilter.SelectionStart = txtFilter.Text.Length;
            txtFilter.SelectionLength = 0;
            txtFilter.Focus();

        }
        readonly IMusteriService _musteriService;
       
        Musteri _selectedMusteri;
       // List<Musteri> musteriList;

        void LoadMGrup()
        {
            List<Musteri> musteriler = _musteriService.GetList().Data;
            lvMGrup.ItemsSource = musteriler;
        }

        private bool NameFilter(object obj)
        {

            var FilterObj = obj as Musteri;

            return FilterObj.FirmaAd.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase);
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvMGrup_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(lvMGrup.SelectedItems is null) && lvMGrup.SelectedItems.Count > 0)  //seçilen item null değilse
            {
                if (!((lvMGrup.SelectedItem as Musteri) is null))
                {
                    this._winMusteri._selectedMusteri = lvMGrup.SelectedItem as Musteri;
                    this._winMusteri.selectedMusteriLoad();
                    Close();
                }

            }
        }


        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (txtFilter == null)
            {
                lvMGrup.Items.Filter = null;
            }
            else
            {
                lvMGrup.Items.Filter = NameFilter;
            }
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {

            this.MouseDown += delegate { DragMove(); };

        }
    }
}
