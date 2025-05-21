using GestionOceanBijoux.Models;
using GestionOceanBijoux.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Style = GestionOceanBijoux.Models.Style;

namespace GestionOceanBijoux.ViewModels
{
    public class StyleViewModel : INotifyPropertyChanged
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
        private ObservableCollection<Style> _styles;

        public ObservableCollection<Style> Styles
        {
            get { return _styles; }
            set { _styles = value; OnPropertyChanged(nameof(Style)); }
        }

        private string _style;
        public string Style
        {
            get => _style;
            set { _style = value; OnPropertyChanged(nameof(Style)); }
        }

        public ICommand SupprimerStyleCommand { get; set; }
        public ICommand AjouterStyleCommand { get; set; }

        public StyleViewModel()
        {
            _apiService = new ApiService();
            Styles = new ObservableCollection<Style>();

            SupprimerStyleCommand = new RelayCommand(async (obj) =>
            {
                if (obj is Style style)
                {
                    var result = MessageBox.Show($"Es-tu sûre de vouloir supprimer le style \"{style.style}\" ?",
                                                 "Confirmation",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        bool success = await _apiService.DeleteStyleAsync(style.id);
                        if (success)
                        {
                            Styles.Remove(style);
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la suppression du style.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            });

            AjouterStyleCommand = new RelayCommand(async (obj) =>
            {
                if (!string.IsNullOrWhiteSpace(Style))
                {
                    var nouvelle = new Style { style = Style };
                    Style createdStyle = await _apiService.AddStyleAsync(nouvelle);

                    if (createdStyle != null)
                    {
                        Styles.Add(createdStyle);
                        MessageBox.Show($"Style ajouté avec ID {createdStyle.id} !");
                        Style = "";
                    }
                    else
                        MessageBox.Show("Erreur lors de l'ajout du style.");                   
                }
            });

            LoadStyles();
        }

        private async Task LoadStyles()
        {
            IsLoading = true;
            var stylesList = await _apiService.GetStylesAsync();
            stylesList = stylesList.Distinct().ToList();
            Styles.Clear();
            foreach (var style in stylesList)
            {
                Styles.Add(style);
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
