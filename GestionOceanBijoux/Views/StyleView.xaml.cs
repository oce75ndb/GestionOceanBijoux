using GestionOceanBijoux.ViewModels;
using System.Windows.Controls;

namespace GestionOceanBijoux.Views
{
    public partial class StyleView : Page
    {
        public StyleView()
        {
            InitializeComponent();
            this.DataContext = new StyleViewModel();
        }
    }
}
