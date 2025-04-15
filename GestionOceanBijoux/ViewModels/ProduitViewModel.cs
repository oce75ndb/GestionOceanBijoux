using GestionOceanBijoux.Services;
using GestionOceanBijoux.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace GestionOceanBijoux.ViewModels
{
    public class ProduitViewModel : INotifyPropertyChanged
    {
        private ApiService _apiService;
        private ObservableCollection<Produit> _produits;

        public ObservableCollection<Produit> Produits
        {
            get { return _produits; }
            set { _produits = value; OnPropertyChanged(nameof(Produits)); }
        }

        public ICommand SupprimerProduitCommand { get; set; }

        public ProduitViewModel()
        {
            _apiService = new ApiService();
            Produits = new ObservableCollection<Produit>();
            SupprimerProduitCommand = new RelayCommand(async (obj) =>
            {
                if (obj is Produit produit)
                {
                    var result = MessageBox.Show($"Es-tu sûre de vouloir supprimer le produit \"{produit.nom}\" ?",
                                                 "Confirmation",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        bool success = await _apiService.DeleteProduitAsync(produit.id);
                        if (success)
                        {
                            Produits.Remove(produit);
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la suppression du produit.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            });

            LoadProduits();
        }

        private async Task LoadProduits()
        {
            var produitlist = await _apiService.GetProduitsAsync();
            produitlist = produitlist.Distinct().ToList();
            Produits.Clear();
            foreach (var produit in produitlist)
            {
                Produits.Add(produit);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
