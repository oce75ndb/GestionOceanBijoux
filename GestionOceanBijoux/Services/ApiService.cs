using System.Net.Http;
using System.Text.Json;
using GestionOceanBijoux.Models;

namespace GestionOceanBijoux.Services
{
    public class ApiService
    {
        private static readonly HttpClient client;
        //private string apiUrl = "http://oceandebijoux.fr/api";
        private string apiUrl = "http://localhost:8000/api";

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

        public async Task<List<Produit>> GetProduitsAsync()
        {
            try
            {
                string apiUrl = "http://oceandebijoux.fr/api/produits";
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

        public async Task<List<Categorie>> GetCategoriesAsync()
        {
            try
            {
                string apiUrl = "http://oceandebijoux.fr/api/categories";
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

        public async Task<bool> DeleteProduitAsync(int id)
        {
            try
            {
                string apiUrl = $"http://oceandebijoux.fr/api/produits/{id}";
                HttpResponseMessage response = await client.DeleteAsync(apiUrl);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression du produit : " + ex.Message);
            }
        }

        public async Task<bool> DeleteCategorieAsync(int id)
        {
            try
            {
                string apiUrl = $"http://oceandebijoux.fr/api/categories/{id}";
                HttpResponseMessage response = await client.DeleteAsync(apiUrl);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression de la catégorie : " + ex.Message);
            }
        }

        public async Task<bool> AddProduitAsync(Produit produit)
        {
            try
            {
                string apiUrl = "http://oceandebijoux.fr/api/produits";
                var jsonContent = new StringContent(JsonSerializer.Serialize(produit), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'ajout du produit : " + ex.Message);
            }
        }

        public async Task<bool> AddCategorieAsync(Categorie categorie)
        {
            try
            {
                string url = apiUrl + "/categories";
                var jsonContent = new StringContent(JsonSerializer.Serialize(categorie), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'ajout de la catégorie : " + ex.Message);
            }
        }

        public async Task<bool> UpdateProduitAsync(Produit produit)
        {
            try
            {
                string apiUrl = $"https://oceandebijoux.fr/api/produits{produit.id}";
                var jsonContent = new StringContent(JsonSerializer.Serialize(produit), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(apiUrl, jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la modification du produit : " + ex.Message);
            }
        }
    }
}
