using GestionOceanBijoux.ViewModels;
using System.Windows.Controls;

namespace GestionOceanBijoux.Views
{
    public partial class ProduitView : Page
    {
        public ProduitView()
        {
            InitializeComponent();
            this.DataContext = new ProduitViewModel();
        }
    }
}
