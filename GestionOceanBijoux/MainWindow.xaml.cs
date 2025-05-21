using GestionOceanBijoux.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace GestionOceanBijoux
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (MainFrame.Content is Page currentPage)
            {
                this.Title = currentPage.Title;
            }
        }
        public void Produit_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProduitView());
        }

        public void Categorie_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CategorieView());
        }

        public void Style_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StyleView());
        }

        public void Materiau_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MateriauView());
        }

        public void Fabrication_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FabricationView());
        }

        private void ResetButtonState()
        {
            ProduitBouton.IsEnabled = true;
        }
    }
}