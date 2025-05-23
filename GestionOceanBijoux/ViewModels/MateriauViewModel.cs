using GestionOceanBijoux.Models;
using GestionOceanBijoux.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Materiau = GestionOceanBijoux.Models.Materiau;

namespace GestionOceanBijoux.ViewModels
{
    public class MateriauViewModel : INotifyPropertyChanged
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
        private ObservableCollection<Materiau> _materiaux;

        public ObservableCollection<Materiau> Materiaux
        {
            get { return _materiaux; }
            set { _materiaux = value; OnPropertyChanged(nameof(Materiau)); }
        }

        // Valeur pour ajouter un matériau
        private string _materiau;
        public string Materiau
        {
            get => _materiau;
            set { _materiau = value; OnPropertyChanged(nameof(Materiau)); }
        }

        public ICommand SupprimerMateriauCommand { get; set; }
        public ICommand AjouterMateriauCommand { get; set; }
        public ICommand EnregistrerMateriauCommand { get; set; }

        public MateriauViewModel()
        {
            _apiService = new ApiService();
            Materiaux = new ObservableCollection<Materiau>();

            SupprimerMateriauCommand = new RelayCommand(async (obj) =>
            {
                if (obj is Materiau materiau)
                {
                    var result = MessageBox.Show($"Es-tu sûre de vouloir supprimer le matériau \"{materiau.materiau}\" ?",
                                                 "Confirmation",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        bool success = await _apiService.DeleteMateriauAsync(materiau.id);
                        if (success)
                        {
                            Materiaux.Remove(materiau);
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la suppression du materiau.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            });
            AjouterMateriauCommand = new RelayCommand(async (obj) =>
            {
                if (!string.IsNullOrWhiteSpace(Materiau))
                {
                    var nouvelle = new Materiau { materiau = Materiau };
                    Materiau createdMateriau = await _apiService.AddMateriauAsync(nouvelle);

                    if (createdMateriau != null)
                    {
                        Materiaux.Add(createdMateriau);
                        MessageBox.Show($"Materiau ajouté avec ID {createdMateriau.id} !");
                        Materiau = "";
                    }
                    else
                        MessageBox.Show("Erreur lors de l'ajout du matériau.");                   
                }
            });
            EnregistrerMateriauCommand = new RelayCommand(async (obj) =>
            {
                if (null != obj)
                {
                    var materiau = (Materiau)obj;
                    Materiau updatedMateriau = await _apiService.UpdateMateriauAsync(materiau);

                    if (updatedMateriau != null)
                    {
                        MessageBox.Show($"Materiau modifié avec ID {updatedMateriau.id} !");
                    }
                    else
                        MessageBox.Show("Erreur lors de la modification du materiau.");
                }
            });

            _ = LoadMateriaux();
        }

        private async Task LoadMateriaux()
        {
            IsLoading = true;

            var materiauxList = await _apiService.GetMateriauxAsync();
            materiauxList = materiauxList.Distinct().ToList();
            Materiaux.Clear();
            foreach (var materiau in materiauxList)
            {
                Materiaux.Add(materiau);
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
