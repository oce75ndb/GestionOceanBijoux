using GestionOceanBijoux.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GestionOceanBijoux
{

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoginView login = new LoginView();
            login.Show();
        }
    }


}
