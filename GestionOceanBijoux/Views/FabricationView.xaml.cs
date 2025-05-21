using GestionOceanBijoux.ViewModels;
using System.Windows.Controls;

namespace GestionOceanBijoux.Views
{
    public partial class FabricationView : Page
    {
        public FabricationView()
        {
            InitializeComponent();
            this.DataContext = new FabricationViewModel();
        }
    }
}
