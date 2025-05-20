using GestionOceanBijoux.Services;
using GestionOceanBijoux.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Style = GestionOceanBijoux.Models.Style;

namespace GestionOceanBijoux.ViewModels
{
    public class ProduitViewModel : INotifyPropertyChanged
    {

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private readonly ApiService _apiService = new();

        public ObservableCollection<Produit> Produits { get; set; } = new();
        public List<Categorie> Categories { get; set; } = new();
        public List<Style> Styles { get; set; } = new();
        public List<Materiau> Materiaux { get; set; } = new();
        public List<Fabrication> Fabrications { get; set; } = new();

        // Champs pour l'ajout
        public string NomProduit { get; set; } = string.Empty;
        public string PrixProduit { get; set; } = string.Empty;
        public int StockProduit { get; set; }
        public int CategorieId { get; set; }
        public int StyleId { get; set; }
        public int MateriauId { get; set; }
        public int FabricationId { get; set; }


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
                    categorie_id = CategorieId,
                    style_id = StyleId,
                    materiau_id = MateriauId,
                    fabrication_id = FabricationId
                };

                Produit createdProduit = await _apiService.AddProduitAsync(nouveau);
                if (createdProduit!=null)
                {
                    Produits.Add(createdProduit);
                    MessageBox.Show($"Produit ajouté avec ID {createdProduit.id} !");
                    
                    // Remise à 0 des champs
                    NomProduit = string.Empty;
                    PrixProduit = string.Empty;
                    StockProduit = 0;
                    CategorieId = 0;
                    StyleId = 0;
                    MateriauId = 0;
                    FabricationId = 0;
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


            _ = LoadData();

        }

        private async Task LoadData()
        {
            IsLoading = true;
            var produitlist = await _apiService.GetProduitsAsync();
            produitlist = produitlist.Distinct().ToList();
            Produits.Clear();
            foreach (var produit in produitlist)
            {
                Produits.Add(produit);
            }

            var categorieslist = await _apiService.GetCategoriesAsync();
            categorieslist = categorieslist.Distinct().ToList();
            Categories.Clear();
            foreach (var categorie in categorieslist)
            {
                Categories.Add(categorie);
            }

            var styleslist = await _apiService.GetStylesAsync();
            styleslist = styleslist.Distinct().ToList();
            Styles.Clear();
            foreach (var style in styleslist)
            {
                Styles.Add(style);
            }

            var materiauxlist = await _apiService.GetMateriauxAsync();
            materiauxlist = materiauxlist.Distinct().ToList();
            Materiaux.Clear();
            foreach (var materiau in materiauxlist)
            {
                Materiaux.Add(materiau);
            }

            var fabricationslist = await _apiService.GetFabricationsAsync();
            fabricationslist = fabricationslist.Distinct().ToList();
            Fabrications.Clear();
            foreach (var fabrication in fabricationslist)
            {
                Fabrications.Add(fabrication);
            }
            IsLoading = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}