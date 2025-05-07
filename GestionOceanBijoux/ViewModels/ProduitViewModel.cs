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

        // Champs pour le formulaire d'ajout
        private string _nomProduit;
        public string NomProduit
        {
            get => _nomProduit;
            set { _nomProduit = value; OnPropertyChanged(nameof(NomProduit)); }
        }

        private string _prixProduit;
        public string PrixProduit
        {
            get => _prixProduit;
            set { _prixProduit = value; OnPropertyChanged(nameof(PrixProduit)); }
        }

        private int _stockProduit;
        public int StockProduit
        {
            get => _stockProduit;
            set { _stockProduit = value; OnPropertyChanged(nameof(StockProduit)); }
        }

        // Commandes
        public ICommand SupprimerProduitCommand { get; set; }
        public ICommand AjouterProduitCommand { get; set; }

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

            AjouterProduitCommand = new RelayCommand(async (obj) =>
            {
                Produit nouveau = new Produit
                {
                    nom = NomProduit,
                    prix = PrixProduit,
                    stock = StockProduit,
                    categorie_id = 1 // Change ça si tu veux sélectionner une catégorie
                };

                bool success = await _apiService.AddProduitAsync(nouveau);
                if (success)
                {
                    Produits.Add(nouveau);
                    MessageBox.Show("Produit ajouté avec succès !");
                    NomProduit = "";
                    PrixProduit = "";
                    StockProduit = 0;
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du produit.");
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
