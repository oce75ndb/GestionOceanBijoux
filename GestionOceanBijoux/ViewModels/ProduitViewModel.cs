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
        private readonly ApiService _apiService = new();

        public ObservableCollection<Produit> Produits { get; set; } = new();

        // Champs pour l'ajout
        public string NomProduit { get; set; } = string.Empty;
        public string PrixProduit { get; set; } = string.Empty;
        public int StockProduit { get; set; }

        // Commandes
        public ICommand SupprimerProduitCommand { get; set; }
        public ICommand AjouterProduitCommand { get; set; }
        public ICommand EnregistrerModificationsProduitCommand { get; set; }

        public ProduitViewModel()
        {
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
                Produit nouveau = new()
                {
                    nom = NomProduit,
                    prix = PrixProduit,
                    stock = StockProduit,
                    categorie_id = 1 // à adapter si tu veux sélectionner une catégorie
                };

                Produit createdProduit = await _apiService.AddProduitAsync(nouveau);
                if (createdProduit!=null)
                {
                    Produits.Add(createdProduit);
                    MessageBox.Show($"Produit ajouté avec ID {createdProduit.id} !");
                    NomProduit = string.Empty;
                    PrixProduit = string.Empty;
                    StockProduit = 0;
                    //OnPropertyChanged(nameof(NomProduit));
                    //OnPropertyChanged(nameof(PrixProduit));
                    //OnPropertyChanged(nameof(StockProduit));
                }
                else
                    MessageBox.Show("Erreur lors de l'ajout du produit.");
            });

            EnregistrerModificationsProduitCommand = new RelayCommand(async (obj) =>
            {
                if (obj is Produit produit)
                {
                    bool success = await _apiService.UpdateProduitAsync(produit);
                    if (success)
                    {
                        MessageBox.Show("Modifications enregistrées avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification du produit.");
                    }
                }
            });

            _ = LoadProduits();
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}