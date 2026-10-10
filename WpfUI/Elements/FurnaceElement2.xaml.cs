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

namespace WpfUI.Elements
{
    /// <summary>
    /// FurnaceElement2.xaml etkileşim mantığı
    /// </summary>
    public partial class FurnaceElement2 : UserControl
    {
        public static readonly RoutedEvent ElementClickEvent =
        EventManager.RegisterRoutedEvent("ElementClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FurnaceElement2));

        public event RoutedEventHandler ElementClick
        {
            add { AddHandler(ElementClickEvent, value); }
            remove { RemoveHandler(ElementClickEvent, value); }
        }

        public FurnaceElement2()
        {
            InitializeComponent();
        }

        void Button_Click(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(ElementClickEvent));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(FurnaceElement2));
        public string IslemName
        {
            get { return (string)GetValue(IslemNameProperty); }
            set { SetValue(IslemNameProperty, value); }
        }
        public static readonly DependencyProperty IslemNameProperty = DependencyProperty.Register("IslemName", typeof(string), typeof(FurnaceElement2));

        public int Status
        {
            get { return (int)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(FurnaceElement2));

        public int Adet
        {
            get { return (int)GetValue(AdetProperty); }
            set { SetValue(AdetProperty, value); }
        }
        public static readonly DependencyProperty AdetProperty = DependencyProperty.Register("Adet", typeof(int), typeof(FurnaceElement2));

        public double Kg
        {
            get { return (double)GetValue(KgProperty); }
            set { SetValue(KgProperty, value); }
        }
        public static readonly DependencyProperty KgProperty = DependencyProperty.Register("Kg", typeof(double), typeof(FurnaceElement2));



        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(FurnaceElement2));

      
    }
}
