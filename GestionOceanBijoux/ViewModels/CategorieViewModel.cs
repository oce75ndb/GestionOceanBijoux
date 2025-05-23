using GestionOceanBijoux.Models;
using GestionOceanBijoux.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace GestionOceanBijoux.ViewModels
{
    public class CategorieViewModel : INotifyPropertyChanged
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


        private ApiService _apiService;
        private ObservableCollection<Categorie> _categories;

        public ObservableCollection<Categorie> Categories
        {
            get { return _categories; }
            set { _categories = value; OnPropertyChanged(nameof(Categories)); }
        }

        private string _nomCategorie;
        public string NomCategorie
        {
            get => _nomCategorie;
            set { _nomCategorie = value; OnPropertyChanged(nameof(NomCategorie)); }
        }

        public ICommand SupprimerCategorieCommand { get; set; }
        public ICommand AjouterCategorieCommand { get; set; }
        public ICommand EnregistrerCategorieCommand { get; set; }

        public CategorieViewModel()
        {
            _apiService = new ApiService();
            Categories = new ObservableCollection<Categorie>();

            SupprimerCategorieCommand = new RelayCommand(async (obj) =>
            {
                if (obj is Categorie categorie)
                {
                    var result = MessageBox.Show($"Es-tu sûre de vouloir supprimer la catégorie \"{categorie.categorie}\" ?",
                                                 "Confirmation",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        bool success = await _apiService.DeleteCategorieAsync(categorie.id);
                        if (success)
                        {
                            Categories.Remove(categorie);
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la suppression de la catégorie.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            });
            AjouterCategorieCommand = new RelayCommand(async (obj) =>
            {
                if (!string.IsNullOrWhiteSpace(NomCategorie))
                {
                    var nouvelle = new Categorie { categorie = NomCategorie };
                    Categorie createdCategorie = await _apiService.AddCategorieAsync(nouvelle);

                    if (createdCategorie != null)
                    {
                        Categories.Add(createdCategorie);
                        MessageBox.Show($"Catégorie ajoutée avec ID {createdCategorie.id} !");
                        NomCategorie = "";
                    }
                    else
                        MessageBox.Show("Erreur lors de l'ajout de la catégorie.");                   
                }
            });
            EnregistrerCategorieCommand = new RelayCommand(async (obj) =>
            {
                if (null != obj)
                {
                    var categorie = (Categorie)obj;
                    Categorie updatedCategorie = await _apiService.UpdateCategorieAsync(categorie);

                    if (updatedCategorie != null)
                    {
                        MessageBox.Show($"Catégorie modifié avec ID {updatedCategorie.id} !");
                    }
                    else
                        MessageBox.Show("Erreur lors de la modification de la categorie.");
                }
            });

            LoadCategories();
        }

        private async Task LoadCategories()
        {
            IsLoading = true;
            var categorieList = await _apiService.GetCategoriesAsync();
            categorieList = categorieList.Distinct().ToList();
            Categories.Clear();
            foreach (var categorie in categorieList)
            {
                Categories.Add(categorie);
            }
            IsLoading = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
