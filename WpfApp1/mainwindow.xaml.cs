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
using WpfApp1.Properties;

 



namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            label_Check.Visibility=Visibility.Hidden;
            txt_user_name.Focus();
            
        }
        DB_LocationVoituresEntities1 dbContext = new DB_LocationVoituresEntities1();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txt_user_name.Text!=string.Empty && txt_password.Password != string.Empty)
                {
                    if (dbContext.Logins.Where(r => r.Nom == txt_user_name.Text && r.Mot_de_passe == txt_password.Password).Count() > 0)
                    {
                        App.Current.Properties["username"] = "Bonjour " + txt_user_name.Text;
                        this.Hide();
                        Window1 w = new Window1();
                        w.Show();
                        Detail d = new Detail();
                        d.Date_Entree = DateTime.Now.Date;
                        d.time_Entree = DateTime.Now.ToString("HH:mm");
                        d.Nom = txt_user_name.Text;
                        d.pass = txt_password.Password;
                        dbContext.Details.Add(d);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        label_Check.Visibility = Visibility.Visible;
                        txt_user_name.Focus();
                        txt_password.Clear();
                    }
                }
                else
                {
                    label_Check.Visibility = Visibility.Visible;
                    txt_user_name.Focus();
                    label_Check.Content = "Saisir Le nom et mot de passe";
                }

                
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Erreur en niveau de base de donnée", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(ex.Message);
            }           
        }
    }
}
