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
    /// Win_MalKabul_List_Musteri.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_MalKabul_List_Musteri : Window
    {
        Win_MalKabul _winMalKabul = new Win_MalKabul();
        public Win_MalKabul_List_Musteri(Win_MalKabul winMalKabul)
        {

            InitializeComponent();
            _winMalKabul = winMalKabul;
            _musteriService = new MusteriManager(new EfMusteriDal());
            LoadMGrup();
            txtFilter.Text = _winMalKabul.filterchar;
            txtFilter.SelectionStart = txtFilter.Text.Length;
            txtFilter.SelectionLength = 0;
            txtFilter.Focus();
            _winMalKabul.filterchar = string.Empty;
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
                    this._winMalKabul._selectedMusteri = lvMGrup.SelectedItem as Musteri;
                    this._winMalKabul.selectedMusteriLoad();
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
