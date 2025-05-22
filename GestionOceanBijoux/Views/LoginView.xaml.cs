using GestionOceanBijoux.Services;
using GestionOceanBijoux;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GestionOceanBijoux.Views
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private readonly ApiService _apiService = new ApiService();
        public LoginView()
        {
            InitializeComponent();
        }
        private readonly HttpClient _httpClient = new HttpClient();

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text;
            var password = PasswordBox.Password;

            // Appel de la méthode LoginAsync de ApiService
            var token = await _apiService.LoginAsync(email, password);

            if (token != null)
            {
                Settings.Default.UserToken = token;
                Settings.Default.Save(); // Sauvegarder les paramètres
                Console.WriteLine("token", token);
                MainWindow mainWindow = new MainWindow();

                this.Close();

                mainWindow.Show();

            }
            else
            {
                MessageBox.Show("Échec de la connexion. Vérifiez vos identifiants.");
            }
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
