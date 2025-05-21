using GestionOceanBijoux.ViewModels;
using System.Windows.Controls;

namespace GestionOceanBijoux.Views
{
    public partial class MateriauView : Page
    {
        public MateriauView()
        {
            InitializeComponent();
            this.DataContext = new MateriauViewModel();
        }
    }
}
