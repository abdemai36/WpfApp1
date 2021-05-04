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

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
 
        DB_LocationVoituresEntities1 dbContext = new DB_LocationVoituresEntities1();
        public Dashboard()
        {
            InitializeComponent();
            louer.Content = dbContext.Voitures.Where(r => r.Etat == "louer").Count();
            dispo.Content = dbContext.Voitures.Where(r => r.Etat == "disponible").Count();
            reserv.Content = dbContext.Resevations.Count();
        }
        
    }
}
