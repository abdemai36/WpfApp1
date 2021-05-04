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
using System.Data;
using System.Data.SqlClient;
using WpfApp1.Forms;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour Reservation.xaml
    /// </summary>
    public partial class Reservation : Page
    {
        public Reservation()
        {
            InitializeComponent();
            
        }
        DB_LocationVoituresEntities1 dbContext = new DB_LocationVoituresEntities1();
        SqlConnection cnx = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=DB_LocationVoitures;Integrated Security=True");
        SqlDataAdapter da;
        DataSet ds = new DataSet();
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            DTP_debut.SelectedDate = DateTime.Now;
            DTP_fin.SelectedDate = DateTime.Now;

            //combobox dyal voiture marque
            da = new SqlDataAdapter("select * from Marque order by Libelle_Marque asc", cnx);
            da.Fill(ds,"tb_voiture_Marque");
            comb_nom_voiture_marque.ItemsSource = ds.Tables["tb_voiture_Marque"].DefaultView;
            comb_nom_voiture_marque.DisplayMemberPath = "Libelle_Marque";
            comb_nom_voiture_marque.SelectedValuePath = "ID_Marque";

            //combobox dyal client
            da = new SqlDataAdapter("select * from Client order by Nom asc", cnx);
            da.Fill(ds, "tb_Client");
            ds.Tables["tb_Client"].Columns.Add(
                    "FullName",
                    typeof(string),
                    "Nom + ' ' + Prenom ");
            comb_client.ItemsSource = ds.Tables["tb_Client"].DefaultView;
            comb_client.DisplayMemberPath = "FullName";
            comb_client.SelectedValuePath = "ID_Client";

            SetDataToGrid();
            txt_id.Text = Convert.ToString(Convert.ToInt32(grid.Items.Count.ToString()) + 1);
        }

        private void SetDataToGrid()
        {
            grid.ItemsSource = dbContext.Resevations.Select(s => new {ID= s.ID_Reservation,Nom= s.Client.Nom,Prénom= s.Client.Prenom,Marque= s.Marque.Libelle_Marque  ,Model=s.Model.Libelle_Model ,Avance =s.Avance+" DH",Debut= s.Date_D,Fin= s.Date_F,Jours= s.Nomber_Jours }).ToList();
        }

        private void DTP_fin_CalendarClosed(object sender, RoutedEventArgs e)
        {

            try
            {

                if (DTP_debut.SelectedDate.Value > DTP_fin.SelectedDate.Value)
                {
                    MessageBox.Show("La date de Debut doit etre Inférieur à la date fin reservation", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    TimeSpan date = DTP_fin.SelectedDate.Value - DTP_debut.SelectedDate.Value;
                    txt_nbr_Jours.Text = (date.Days+1).ToString();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("saisir la date de reservation ", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        private void btn_ajouter_Click_1(object sender, RoutedEventArgs e)
        {
            //try
            //{
                if (comb_client.Text == string.Empty || comb_nom_voiture_marque.Text == string.Empty || txt_nbr_Jours.Text==string.Empty)
                {
                    MessageBox.Show("Saisir tout les information ", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var db = dbContext.Resevations;
                    foreach(var rese in db)
                    {
                        if (comb_client.SelectedIndex == -1)
                        {
                            MessageBox.Show("Ce Client n'existe pas ! ", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            comb_client.Focus();
                            return;
                        }
                        else if (comb_nom_voiture_marque.SelectedIndex==-1)
                        {
                            MessageBox.Show("Ce marque n'existe pas ! ", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            comb_nom_voiture_marque.Focus();
                            return;
                        }
                        else if (comb_nom_voiture_model.SelectedIndex == -1)
                        {
                            MessageBox.Show("Ce medel n'existe pas ! ", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            comb_nom_voiture_marque.Focus();
                            return;
                        }
                    }
                    Resevation r = new Resevation();
                    r.ID_Client =int.Parse(comb_client.SelectedValue.ToString());
                    r.ID_Marque = int.Parse(comb_nom_voiture_marque.SelectedValue.ToString());
                    r.ID_Model =int.Parse(comb_nom_voiture_model.SelectedValue.ToString());
                    if (txt_avance.Text == string.Empty)
                        txt_avance.Text = Convert.ToString(0);
                    r.Avance =Convert.ToDecimal(txt_avance.Text);
                    r.Date_D = Convert.ToDateTime(DTP_debut.SelectedDate.Value.ToString());
                    r.Date_F = Convert.ToDateTime(DTP_fin.SelectedDate.Value.ToString());
                    r.Nomber_Jours = int.Parse(txt_nbr_Jours.Text);
                    dbContext.Resevations.Add(r);
                    dbContext.SaveChanges();
                    MessageBox.Show("Ajouter avec succes", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    grid_SelectedCellsChanged(sender, null);
                    SetDataToGrid();
                    txt_id.Text = Convert.ToString(Convert.ToInt32(grid.Items.Count.ToString()) + 1);
                    ClearControls();
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void ClearControls()
        {
            comb_client.SelectedIndex = -1;
            comb_nom_voiture_marque.SelectedIndex = -1;
            comb_nom_voiture_model.SelectedIndex = -1;
            txt_avance.Text = string.Empty;
            txt_nbr_Jours.Text = string.Empty;
        }

     


        private void btn_modifier_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                
                
                if (comb_client.Text == string.Empty || comb_nom_voiture_marque.Text == string.Empty ||comb_nom_voiture_model.Text == string.Empty || comb_client.Text==string.Empty)
                {
                    MessageBox.Show("Saisir tout les information ", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var db = dbContext.Resevations;
                    foreach (var rese in db)
                    {
                        if (comb_client.SelectedIndex == -1)
                        {
                            MessageBox.Show("Ce Client n'existe pas ! ", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            comb_client.Focus();
                            return;
                        }
                        else if (comb_nom_voiture_marque.SelectedIndex == -1)
                        {
                            MessageBox.Show("cette marque n'existe pas ! ", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            comb_nom_voiture_marque.Focus();
                            return;
                        }else if (comb_nom_voiture_model.SelectedIndex == -1)
                        {
                            MessageBox.Show("cette model n'existe pas ! ", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            comb_nom_voiture_model.Focus();
                            return;
                        }
                    }
                    
                    int IDReserv = int.Parse(txt_id.Text);
                    Resevation rs = (from r in dbContext.Resevations where r.ID_Reservation == IDReserv select r).First();
                    rs.ID_Reservation = int.Parse(txt_id.Text);
                    rs.ID_Client = int.Parse(comb_client.SelectedValue.ToString());
                    rs.ID_Marque= int.Parse(comb_nom_voiture_marque.SelectedValue.ToString());
                    rs.ID_Model = int.Parse(comb_nom_voiture_model.SelectedValue.ToString());
                    if (txt_avance.Text == string.Empty)
                        txt_avance.Text = Convert.ToString(0);
                    rs.Avance = Convert.ToInt32(txt_avance.Text);
                    rs.Date_D = Convert.ToDateTime(DTP_debut.SelectedDate.Value.ToString());
                    rs.Date_F = Convert.ToDateTime(DTP_fin.SelectedDate.Value.ToString());
                    rs.Nomber_Jours = int.Parse(txt_nbr_Jours.Text);
                    dbContext.SaveChanges();
                    grid_SelectedCellsChanged(sender, null);
                    MessageBox.Show("Modifier avec succes ", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    SetDataToGrid();
     
                    ClearControls();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                    object item = grid.SelectedItem;
                    if (item != null)
                    {
                        string ColumnID = (grid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                        string ColumnClientNom = (grid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                        string ColumnClientPrenom = (grid.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
                        string ColumnMarque = (grid.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text;
                        string ColumnModel = (grid.SelectedCells[4].Column.GetCellContent(item) as TextBlock).Text;
                        string ColumnAvance = (grid.SelectedCells[5].Column.GetCellContent(item) as TextBlock).Text;
                        string ColumnDate_D = (grid.SelectedCells[6].Column.GetCellContent(item) as TextBlock).Text;
                        string ColumnDate_F = (grid.SelectedCells[7].Column.GetCellContent(item) as TextBlock).Text;
                        string ColumnNomberJour = (grid.SelectedCells[8].Column.GetCellContent(item) as TextBlock).Text;
                        txt_id.Text = ColumnID;
                        comb_client.Text = ColumnClientNom + " " + ColumnClientPrenom;
                        comb_nom_voiture_marque.Text = ColumnMarque;
                        comb_nom_voiture_model.Text = ColumnModel;
                        txt_avance.Text = ColumnAvance;
                        DTP_debut.Text = ColumnDate_D;
                        DTP_fin.Text = ColumnDate_F;
                        txt_nbr_Jours.Text = ColumnNomberJour;
                    }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        private void btn_nouveau_Click(object sender, RoutedEventArgs e)
        {
            grid.SelectedIndex = -1;
            txt_id.Text = Convert.ToString(Convert.ToInt32(grid.Items.Count.ToString()) + 1);
            ClearControls();
            comb_client.Focus();
            
        }

        private void btn_supprimer_Click(object sender, RoutedEventArgs e)
        {
            if (comb_client.Text == string.Empty || comb_nom_voiture_marque.Text == string.Empty)
            {
                MessageBox.Show("Saisir tout les information que vous Supprimer", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            }else
            {
                MessageBoxResult res = MessageBox.Show("Voulez vous vraiment supprimer La résérvation", "Confiramtion", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    int IDReserv = int.Parse(txt_id.Text);
                    Resevation rs = (from r in dbContext.Resevations where r.ID_Reservation == IDReserv select r).First();
                    dbContext.Resevations.Remove(rs);
                    dbContext.SaveChanges();
                    MessageBox.Show("La supprission avec succes ", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    grid_SelectedCellsChanged(sender, null);
                    SetDataToGrid();
                    ClearControls();
                }
            }
        }

        private void txt_avance_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_avance.Text, @"[^0-9]\.[^0-9]"))
            {
                MessageBox.Show("S'il vous plait saisir un nombre.");
                txt_avance.Text = txt_avance.Text.Remove(txt_avance.Text.Length - 1);
            }
        }

        private void txt_Rechercher_SelectionChanged(object sender, RoutedEventArgs e)
        {
            grid.ItemsSource = dbContext.Resevations.Where(
                x =>x.Client.Nom.Contains(txt_Rechercher.Text) || x.Client.Prenom.Contains(txt_Rechercher.Text) ||  x.Marque.Libelle_Marque.Contains(txt_Rechercher.Text) || x.Model.Libelle_Model.Contains(txt_Rechercher.Text))
                .Select(s => new { ID = s.ID_Reservation, Nom = s.Client.Nom, Prénom = s.Client.Prenom,
                    Marque = s.Marque.Libelle_Marque,
                    Model = s.Model.Libelle_Model, Avance = s.Avance + " DH", 
                    Debut = s.Date_D, Fin = s.Date_F, Jours = s.Nomber_Jours }).ToList();
        }

        private void btn_Rapport_Click(object sender, RoutedEventArgs e)
        {
            Form_Reservation_Repport frmRR = new Form_Reservation_Repport();
            frmRR.Show();
        }

    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show(comb_nom_voiture_model.SelectedValue.ToString());
        }

        private void comb_nom_voiture_marque_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                //combobox dyal voiture model
                DataSet dsV = new DataSet();
                
                comb_nom_voiture_model.IsEnabled = true;
                da = new SqlDataAdapter("select * from Model where ID_Marque='"+comb_nom_voiture_marque.SelectedValue+"'  order by Libelle_Model asc", cnx);
                da.Fill(dsV, "tb_voiture_Model");
                comb_nom_voiture_model.ItemsSource = dsV.Tables["tb_voiture_Model"].DefaultView;
                comb_nom_voiture_model.DisplayMemberPath = "Libelle_Model";
                comb_nom_voiture_model.SelectedValuePath = "ID_Model";
               
            }
            catch (Exception)
            {

            }
        }

     
    }
}
