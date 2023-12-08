using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Schema;

namespace WpfAppSnooker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Versenyzo> Versenyzok = File.ReadAllLines("snooker.txt").Select(x => new Versenyzo(x)).Skip(1).ToList();
        public MainWindow()
        {
            InitializeComponent();
            DatagridSetup();
            OrszagValasztoSetup();
        }

        private void btnF3_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Ennyi versenyzo szerepel a vilagranglistan: {Versenyzok.Count}");
        }

        private void btnF4_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"A versenyzok ennyi bevetelre tettek szert atlagosan: {Versenyzok.Average(x => x.Nyeremeny):f2}"); 
        }

        private void btnF5_Click(object sender, RoutedEventArgs e)
        {
            var aktVersenyzo = Versenyzok.Where(x => x.Orszag == cbOrszagValaszto.SelectedItem.ToString()).MaxBy(x => x.Nyeremeny);
            lblHelyezes.Content = aktVersenyzo.Helyezes;
            lblNev.Content = aktVersenyzo.Nev;
            lblOrszag.Content = aktVersenyzo.Orszag;
            lblNyeremeny.Content = aktVersenyzo.Nyeremeny * int.Parse(txtArfolyam.Text);
        }

        private void btnF6_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Versenyzok.Any(x => x.Orszag == txtVanIlyenOrszag.Text) ? $"Van \"{txtVanIlyenOrszag.Text}\" nevu orszag a tablaban!" : $"Nincs \"{txtVanIlyenOrszag.Text}\" nevu orszag a tablaban!");
        }

        private void btnF7_Click(object sender, RoutedEventArgs e)
        {
            int minfo = Convert.ToInt32(sliMinLetszam.Value);
            lbStatisztika.Items.Clear();
            lbStatisztika.Items.Add("7.feladat: Statisztika:");
            Versenyzok.GroupBy(x => x.Orszag).Where(x => x.Count() >= minfo).ToList().ForEach(x => lbStatisztika.Items.Add($"{x.Key} - {x.Count()}"));

        }

        private void OrszagValasztoSetup()
        {
            cbOrszagValaszto.ItemsSource = Versenyzok.Select(x => x.Orszag).Distinct();
            cbOrszagValaszto.SelectedIndex = 0;
        }

        private void DatagridSetup()
        {
            dgTablazat.ItemsSource = Versenyzok;
        }

        private void lbStatisztika_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgTablazat.ItemsSource = Versenyzok.Where(x => x.Orszag == lbStatisztika.SelectedItem.ToString().Split(" - ")[0]).ToList();
        }
    }
}
