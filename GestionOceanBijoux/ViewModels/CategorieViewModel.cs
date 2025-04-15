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
        private ApiService _apiService;
        private ObservableCollection<Categorie> _categories;

        public ObservableCollection<Categorie> Categories
        {
            get { return _categories; }
            set { _categories = value; OnPropertyChanged(nameof(Categories)); }
        }

        public ICommand SupprimerCategorieCommand { get; set; }

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

            LoadCategories();
        }

        private async Task LoadCategories()
        {
            var categorieList = await _apiService.GetCategoriesAsync();
            categorieList = categorieList.Distinct().ToList();
            Categories.Clear();
            foreach (var categorie in categorieList)
            {
                Categories.Add(categorie);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
