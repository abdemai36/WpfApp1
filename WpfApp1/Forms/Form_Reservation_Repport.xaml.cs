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
using WpfApp1.Data;
using WpfApp1.Data.DataSet_ReservationTableAdapters;

namespace WpfApp1.Forms
{
    /// <summary>
    /// Logique d'interaction pour Form_Reservation_Repport.xaml
    /// </summary>
    public partial class Form_Reservation_Repport : Window
    {
        public Form_Reservation_Repport()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                DataSet_Reservation Ds = new DataSet_Reservation();
                List_ReservationsTableAdapter da = new List_ReservationsTableAdapter();
                da.Fill(Ds.List_Reservations);
                Rapportés.Repport_Reservation repport = new Rapportés.Repport_Reservation();
                repport.SetDataSource(Ds);
                CrystalViewer_Reservation.ViewerCore.ReportSource = repport;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
