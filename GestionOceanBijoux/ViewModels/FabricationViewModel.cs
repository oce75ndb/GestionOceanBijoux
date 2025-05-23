using GestionOceanBijoux.Models;
using GestionOceanBijoux.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Fabrication = GestionOceanBijoux.Models.Fabrication;

namespace GestionOceanBijoux.ViewModels
{
    public class FabricationViewModel : INotifyPropertyChanged
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
        private ObservableCollection<Fabrication> _fabricationx;

        public ObservableCollection<Fabrication> Fabrications
        {
            get { return _fabricationx; }
            set { _fabricationx = value; OnPropertyChanged(nameof(Fabrication)); }
        }

        // Pour ajouter
        private string _fabrication;
        public string Fabrication
        {
            get => _fabrication;
            set { _fabrication = value; OnPropertyChanged(nameof(Fabrication)); }
        }

        public ICommand SupprimerFabricationCommand { get; set; }
        public ICommand AjouterFabricationCommand { get; set; }
        public ICommand EnregistrerFabricationCommand { get; set; }

        public FabricationViewModel()
        {
            _apiService = new ApiService();
            Fabrications = new ObservableCollection<Fabrication>();

            SupprimerFabricationCommand = new RelayCommand(async (obj) =>
            {
                if (obj is Fabrication fabrication)
                {
                    var result = MessageBox.Show($"Es-tu sûre de vouloir supprimer la fabrication \"{fabrication.fabrication}\" ?",
                                                 "Confirmation",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        bool success = await _apiService.DeleteFabricationAsync(fabrication.id);
                        if (success)
                        {
                            Fabrications.Remove(fabrication);
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la suppression de la fabrication.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            });
            AjouterFabricationCommand = new RelayCommand(async (obj) =>
            {
                if (!string.IsNullOrWhiteSpace(Fabrication))
                {
                    var nouvelle = new Fabrication { fabrication = Fabrication };
                    Fabrication createdFabrication = await _apiService.AddFabricationAsync(nouvelle);

                    if (createdFabrication != null)
                    {
                        Fabrications.Add(createdFabrication);
                        MessageBox.Show($"Fabrication ajoutée avec ID {createdFabrication.id} !");
                        Fabrication = "";
                    }
                    else
                        MessageBox.Show("Erreur lors de l'ajout de la fabrication.");                   
                }
            });
            EnregistrerFabricationCommand = new RelayCommand(async (obj) =>
            {
                if (null != obj)
                {
                    var fabrication = (Fabrication)obj;
                    Fabrication updatedFabrication = await _apiService.UpdateFabricationAsync(fabrication);

                    if (updatedFabrication != null)
                    {
                        MessageBox.Show($"Fabrication modifiée avec ID {updatedFabrication.id} !");
                    }
                    else
                        MessageBox.Show("Erreur lors de la modification de la fabrication.");
                }
            });

            _ = LoadFabrications();
        }

        private async Task LoadFabrications()
        {
            IsLoading = true;

            var fabricationxList = await _apiService.GetFabricationsAsync();
            fabricationxList = fabricationxList.Distinct().ToList();
            Fabrications.Clear();
            foreach (var fabrication in fabricationxList)
            {
                Fabrications.Add(fabrication);
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
