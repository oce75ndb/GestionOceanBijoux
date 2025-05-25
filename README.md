
# GestionOceanBijoux

**GestionOceanBijoux** est une application de bureau développée en C# avec WPF (.NET 8), suivant le modèle d'architecture MVVM. Ellae permet de gérer les données d’un site e-commerce fictif spécialisé dans les bijoux, via une API Laravel.

## Présentation

Ce projet a été réalisé dans le cadre du BTS SIO SLAM. Il permet de consulter, ajouter, modifier et supprimer des produits, catégories, matériaux, styles et fabrications. L'application se connecte à une API distante pour récupérer et envoyer les données.

## Technologies utilisées

- .NET 8 (WPF)
- C# / XAML
- MVVM (Model - View - ViewModel)
- API RESTful (via HttpClient)
- JSON (System.Text.Json)
- Git / GitHub

## Fonctionnalités

- Connexion à une API Laravel distante
- Affichage dynamique des données (produits, catégories, etc.)
- Formulaires d’ajout et de modification
- Suppression d’éléments avec confirmation
- Navigation entre les différentes vues
- Rafraîchissement automatique après chaque opération

## Architecture du projet

Le projet est structuré de façon modulaire selon le modèle MVVM :

```
GestionOceanBijoux/
│
├── Models/         → Représentation des entités (Produit, Categorie, etc.)
├── Views/          → Interfaces graphiques (XAML)
├── ViewModels/     → Logique métier et gestion de l’état des vues
├── Services/       → Requêtes vers l’API Laravel
├── Helpers/        → Méthodes utilitaires
└── MainWindow/     → Fenêtre principale et gestion de la navigation
```

## Exemple de logique

**Extrait de `ApiService.cs`** – Récupération des produits depuis l’API :

```csharp
public async Task<List<Produit>> GetProduitsAsync()
{
    string url = apiUrl + "/produits";
    HttpResponseMessage response = await client.GetAsync(url);
    if (response.IsSuccessStatusCode)
    {
        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Produit>>(json) ?? new List<Produit>();
    }
    throw new Exception("Erreur API : " + response.ReasonPhrase);
}
```

**Extrait de `ProduitViewModel.cs`** – Ajout d’un produit :

```csharp
private async void AjouterProduit()
{
    await apiService.AddProduitAsync(ProduitSelectionne);
    await ChargerProduits();
}
```

## Vues disponibles

| Vue              | Description                             |
|------------------|-----------------------------------------|
| `MainWindow`      | Fenêtre principale et navigation        |
| `LoginView`       | Connexion utilisateur (si activée)      |
| `ProduitView`     | Gestion des produits                    |
| `CategorieView`   | Gestion des catégories                  |
| `StyleView`       | Gestion des styles                      |
| `MateriauView`    | Gestion des matériaux                   |
| `FabricationView` | Gestion des fabrications                |

## Lancement du projet

1. Ouvrir `GestionOceanBijoux.sln` dans Visual Studio 2022 (ou supérieur)
2. Vérifier que l’API Laravel est bien en ligne
3. Adapter l’URL dans `ApiService.cs` si nécessaire
4. Lancer le projet avec F5

## Pistes d’amélioration
Certaines optimisations peuvent encore être envisagées pour aller plus loin :

- Intégrer un système de recherche ou de filtre dans les listes
- Améliorer l'affichage des messages d’erreur ou de confirmation
- Prévoir une pagination en cas de grand nombre de résultats
- Amélioration du design graphique et de l’ergonomie générale

## À propos

Projet développé par **Océane Bondon** (BTS SIO SLAM 2025).  
Ce projet interagit avec une API Laravel développée séparément dans le cadre du site "Océan de Bijoux".
