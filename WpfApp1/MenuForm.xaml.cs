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

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            username.Content = App.Current.Properties["username"];
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            logo.Visibility = Visibility.Hidden;
            parent.Content = new Dashboard();
        }

        private void label_nme_IsMouseCapturedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            logo.Visibility = Visibility.Hidden;
            parent.Content = new Reservation();
        }
    }
}
