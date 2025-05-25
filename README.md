```markdown
# GestionOceanBijoux

GestionOceanBijoux est une application de bureau développée en C# avec WPF (.NET 8), reposant sur l’architecture MVVM. Elle permet de gérer les produits, catégories, matériaux, styles et fabrications d’un site e-commerce fictif de bijoux, via une API Laravel distante.

## Technologies utilisées

- WPF .NET 8  
- MVVM (Model - View - ViewModel)  
- C# / XAML  
- API REST (consommée via HttpClient)  
- JSON (avec System.Text.Json)  
- Git (hébergement GitHub)

## Fonctionnalités principales

- Affichage et gestion des produits, catégories, matériaux, styles et fabrications  
- Connexion à une API Laravel : récupération et envoi de données (GET, POST, PUT, DELETE)  
- Interface graphique en XAML  
- Architecture MVVM claire, facilitant la maintenance et les évolutions

## Architecture du projet

Le projet est structuré selon le modèle MVVM, avec séparation claire entre les modèles de données, la logique de présentation et l’interface utilisateur.

```

GestionOceanBijoux/
│
├── Models/         → Entités métiers (Produit, Categorie, etc.)
├── Views/          → Interfaces utilisateur (XAML)
├── ViewModels/     → Logique métier liée à l'affichage
├── Services/       → Communication avec l’API Laravel
├── Helpers/        → Méthodes utilitaires
└── MainWindow/     → Fenêtre principale avec navigation

````

## Exemples de code

### Appel API – `ApiService.cs`

```csharp
public async Task<List<Produit>> GetProduitsAsync()
{
    string url = apiUrl + "/produits";
    HttpResponseMessage response = await client.GetAsync(url);
    if (response.IsSuccessStatusCode)
    {
        string jsonString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Produit>>(jsonString) ?? new List<Produit>();
    }
    throw new Exception("Erreur API : " + response.ReasonPhrase);
}
````

### Exemple de ViewModel – `ProduitViewModel.cs`

```csharp
public class ProduitViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Produit> Produits { get; set; } = new();
    public Produit ProduitSelectionne { get; set; } = new();

    public ICommand AjouterCommand => new RelayCommand(AjouterProduit);
    public ICommand SupprimerCommand => new RelayCommand(SupprimerProduit);

    private async void AjouterProduit()
    {
        await apiService.AddProduitAsync(ProduitSelectionne);
        await ChargerProduits();
    }
}
```

## Pages et vues disponibles

| Vue             | Description                          |
| --------------- | ------------------------------------ |
| MainWindow      | Fenêtre principale et navigation     |
| LoginView       | Interface de connexion (optionnelle) |
| ProduitView     | Gestion des produits                 |
| CategorieView   | Gestion des catégories               |
| StyleView       | Gestion des styles                   |
| MateriauView    | Gestion des matériaux                |
| FabricationView | Gestion des fabrications             |

## Lancer le projet

1. Ouvrir le fichier `GestionOceanBijoux.sln` dans Visual Studio 2022 ou version supérieure
2. Vérifier que l’API Laravel est bien active (en local ou en ligne)
3. Adapter l’URL de base dans `ApiService.cs` si nécessaire
4. Lancer l’application avec F5

## À propos

Projet développé par Océane Bondon dans le cadre du BTS SIO SLAM, session 2025.
L’API utilisée est développée séparément avec Laravel, dans le projet associé *Océan de Bijoux*.

```
