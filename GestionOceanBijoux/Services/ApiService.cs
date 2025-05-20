using System.Net.Http;
using System.Security.Policy;
using System.Text;
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
                string url = apiUrl + "/produits";
                HttpResponseMessage response = await client.GetAsync(url);

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
                string url = apiUrl + "/categories";
                HttpResponseMessage response = await client.GetAsync(url);

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

        public async Task<List<Style>> GetStylesAsync()
        {
            try
            {
                string url = apiUrl + "/styles";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    List<Style>? styles = JsonSerializer.Deserialize<List<Style>>(jsonString);
                    return styles ?? new List<Style>();
                }
                else
                {
                    throw new Exception($"Erreur {response.StatusCode} : {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération ou de la désérialisation des styles : " + ex.Message);
            }
        }

        public async Task<List<Materiau>> GetMateriauxAsync()
        {
            try
            {
                string url = apiUrl + "/materiaux";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    List<Materiau>? materiaux = JsonSerializer.Deserialize<List<Materiau>>(jsonString);
                    return materiaux ?? new List<Materiau>();
                }
                else
                {
                    throw new Exception($"Erreur {response.StatusCode} : {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération ou de la désérialisation des matériaux : " + ex.Message);
            }
        }

        public async Task<List<Fabrication>> GetFabricationsAsync()
        {
            try
            {
                string url = apiUrl + "/fabrications";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    List<Fabrication>? fabrications = JsonSerializer.Deserialize<List<Fabrication>>(jsonString);
                    return fabrications ?? new List<Fabrication>();
                }
                else
                {
                    throw new Exception($"Erreur {response.StatusCode} : {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération ou de la désérialisation des fabrications : " + ex.Message);
            }
        }

        public async Task<bool> DeleteProduitAsync(int id)
        {
            try
            {
                string url = apiUrl + $"/produits/{id}";
                HttpResponseMessage response = await client.DeleteAsync(url);
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
                string url = apiUrl + $"/categories/{id}";
                HttpResponseMessage response = await client.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression de la catégorie : " + ex.Message);
            }
        }

        public async Task<Produit> AddProduitAsync(Produit produit)
        {
            string url = apiUrl + "/produits";
            var jsonContent = new StringContent(JsonSerializer.Serialize(produit), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, jsonContent);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var createdProduit = JsonSerializer.Deserialize<Produit>(result);

                return createdProduit;
            }
            else
                return null;
        }

        public async Task<Categorie> AddCategorieAsync(Categorie categorie)
        {

            string url = apiUrl + "/categories";
            var jsonContent = new StringContent(JsonSerializer.Serialize(categorie), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var createdCategorie = JsonSerializer.Deserialize<Categorie>(result);

                return createdCategorie;
            }
            else
                return null;
        }

        public async Task<bool> UpdateProduitAsync(Produit produit)
        {
            try
            {
                string url = apiUrl + $"/produits{produit.id}";
                var jsonContent = new StringContent(JsonSerializer.Serialize(produit), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la modification du produit : " + ex.Message);
            }
        }

        public async Task<bool> UpdateCategoriesAsync(Produit categorie)
        {
            try
            {
                string url = apiUrl + $"/categories{categorie.id}";
                var jsonContent = new StringContent(JsonSerializer.Serialize(categorie), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la modification de la catégorie : " + ex.Message);
            }
        }

    }
}
