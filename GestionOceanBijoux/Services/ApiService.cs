using System.Net.Http;
using System.Text.Json;
using GestionOceanBijoux.Models;

namespace GestionOceanBijoux.Services
{
    public class ApiService
    {
        private static readonly HttpClient client;

        static ApiService()
        {
            var handler = new HttpClientHandler()
            {
                MaxConnectionsPerServer = 10,
            };

            client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30),
            };
        }

        // Récupération des produits dans l'API
        public async Task<List<Produit>> GetProduitsAsync()
        {
            try
            {
                string apiUrl = "http://127.0.0.1:8000/api/produits";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    List<Produit>? produits = JsonSerializer.Deserialize<List<Produit>>(jsonString);
                    return produits ?? new List<Produit>();
                }
                else
                {
                    throw new Exception($"Erreur {response.StatusCode} : {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération ou de la désérialisation des produits : " + ex.Message);
            }
        }

        // Récupération des catégories dans l'API
        public async Task<List<Categorie>> GetCategoriesAsync()
        {
            try
            {
                string apiUrl = "http://127.0.0.1:8000/api/categories";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    List<Categorie>? categories = JsonSerializer.Deserialize<List<Categorie>>(jsonString);
                    return categories ?? new List<Categorie>();
                }
                else
                {
                    throw new Exception($"Erreur {response.StatusCode} : {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération ou de la désérialisation des catégories : " + ex.Message);
            }
        }

        // Suppression d'un produit
        public async Task<bool> DeleteProduitAsync(int id)
        {
            try
            {
                string apiUrl = $"http://127.0.0.1:8000/api/produits/{id}";
                HttpResponseMessage response = await client.DeleteAsync(apiUrl);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression du produit : " + ex.Message);
            }
        }

        // Suppression d'une catégorie
        public async Task<bool> DeleteCategorieAsync(int id)
        {
            try
            {
                string apiUrl = $"http://127.0.0.1:8000/api/categories/{id}";
                HttpResponseMessage response = await client.DeleteAsync(apiUrl);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression de la catégorie : " + ex.Message);
            }
        }

        // Ajout d'un produit
        public async Task<bool> AddProduitAsync(Produit produit)
        {
            try
            {
                string apiUrl = "http://127.0.0.1:8000/api/produits";
                var jsonContent = new StringContent(JsonSerializer.Serialize(produit), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'ajout du produit : " + ex.Message);
            }
        }

        // Ajout d'une catégorie
        public async Task<bool> AddCategorieAsync(Categorie categorie)
        {
            try
            {
                string apiUrl = "http://127.0.0.1:8000/api/categories";
                var jsonContent = new StringContent(JsonSerializer.Serialize(categorie), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'ajout de la catégorie : " + ex.Message);
            }
        }
    }
}