using GestionOceanBijoux.ViewModels;
using System.Windows.Controls;

namespace GestionOceanBijoux.Views
{
    public partial class CategorieView : Page
    {
        public CategorieView()
        {
            InitializeComponent();
            this.DataContext = new CategorieViewModel();
        }
    }
}
