using GestionOceanBijoux.Services;
using GestionOceanBijoux.Models;
using GestionOceanBijoux.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GestionOceanBijoux.ViewModels
{
    public class ProduitViewModel
    {
        private ApiService _apiService;
        private ObservableCollection<Produit> _produits;
        public ObservableCollection<Produit> Produits
        {
            get { return _produits; }
            set { _produits = value; OnPropertyChanged(nameof(Produits)); }
        }

        public ProduitViewModel()
        {
            _apiService = new ApiService();
            Produits = new ObservableCollection<Produit>();
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
