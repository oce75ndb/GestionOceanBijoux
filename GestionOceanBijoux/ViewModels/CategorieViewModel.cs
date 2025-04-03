using GestionOceanBijoux.Models;
using GestionOceanBijoux.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

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

        public CategorieViewModel()
        {
            _apiService = new ApiService();
            Categories = new ObservableCollection<Categorie>();
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
