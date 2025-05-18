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
            //MainFrame.NavigationService.RemoveBackEntry();
            //ResetButtonState();
            //((Button)sender).IsEnabled = false;
        }

        public void Categorie_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CategorieView());
            //MainFrame.NavigationService.RemoveBackEntry();
            //ResetButtonState();
            //((Button)sender).IsEnabled = false;
        }

        private void ResetButtonState()
        {
            ProduitBouton.IsEnabled = true;
        }
    }
}