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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfUI.Windows
{
    /// <summary>
    /// Win_Tnm_List_Malzeme.xaml etkileşim mantığı
    /// </summary>
    public partial class Win_Tnm_List_Malzeme : Window
    {
        // Form_MalKabul _form = new Form_MalKabul();
        Win_Tnm_MalzemeTnm _winMalzeme = new Win_Tnm_MalzemeTnm();
        public Win_Tnm_List_Malzeme(Win_Tnm_MalzemeTnm winMalzeme)
        {
            InitializeComponent();
            _winMalzeme = winMalzeme;
            _malzemeGrupService = new MalzemeGrupManager(new EfMalzemeGrupDal());
            LoadMGrup();
            txtFilter.Text = _winMalzeme.filterchar;
            txtFilter.SelectionStart = txtFilter.Text.Length;
            txtFilter.SelectionLength = 0;
            txtFilter.Focus();

        }
        readonly IMalzemeGrupService _malzemeGrupService;
        MalzemeGrup _selectedMGrup;
        List<MalzemeGrup> mGrupList;

        void LoadMGrup()
        {
            List<MalzemeGrup> malzemeGrups = _malzemeGrupService.GetList().Data;
            lvMGrup.ItemsSource = malzemeGrups;
        }

        private bool NameFilter(object obj)
        {

            var FilterObj = obj as MalzemeGrup;

            return FilterObj.GrupName.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase);
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvMGrup_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(lvMGrup.SelectedItems is null) && lvMGrup.SelectedItems.Count > 0)  //seçilen item null değilse
            {
                if (!((lvMGrup.SelectedItem as MalzemeGrup) is null))
                {
                    this._winMalzeme._selectedMGrup = lvMGrup.SelectedItem as MalzemeGrup;
                    this._winMalzeme.selectedMalzemeLoad();
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
