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
    /// Win_MalKabul_List_Malzeme.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_MalKabul_List_Malzeme : Window
    {
        Win_MalKabul _winMalKabul = new Win_MalKabul();
        public Win_MalKabul_List_Malzeme(Win_MalKabul winMalKabul)
        {
            InitializeComponent();
            _winMalKabul = winMalKabul;
            _malzemeService = new MalzemeManager(new EfMalzemeDal());
            LoadMGrup();
            txtFilter.Text = _winMalKabul.filterchar;
            txtFilter.SelectionStart = txtFilter.Text.Length;
            txtFilter.SelectionLength = 0;
            txtFilter.Focus();

        }
        readonly IMalzemeService _malzemeService;

        void LoadMGrup()
        {
            List<Malzeme> malzemes = _malzemeService.GetList().Data;
            lvMGrup.ItemsSource = malzemes;
        }

        private bool NameFilter(object obj)
        {

            var FilterObj = obj as Malzeme;

            return FilterObj.MalzemeName.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase);
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvMGrup_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(lvMGrup.SelectedItems is null) && lvMGrup.SelectedItems.Count > 0)  //seçilen item null değilse
            {
                if (!((lvMGrup.SelectedItem as Malzeme) is null))
                {
                    this._winMalKabul._selectedMalzeme = lvMGrup.SelectedItem as Malzeme;
                    this._winMalKabul.selectedMalzemeLoad();
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
