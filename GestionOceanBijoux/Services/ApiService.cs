using GestionOceanBijoux.Models;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace GestionOceanBijoux.Services
{
    public class ApiService
    {
        private static readonly HttpClient client;

        public static readonly string apiUrl = ConfigurationManager.AppSettings["api_url"]
            ?? throw new ArgumentNullException(nameof(apiUrl), "L'URL de l'API est introuvable dans app.config.");


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


        public async Task<string> LoginAsync(string email, string password)
        {
            var loginData = new
            {
                email = email,
                password = password
            };


            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{apiUrl}/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                using (JsonDocument document = JsonDocument.Parse(responseData))
                {
                    JsonElement root = document.RootElement;

                    if (root.TryGetProperty("token", out JsonElement tokenElement))
                    {
                        string token = tokenElement.GetString(); // GetString() converts the JSON string value to a C# string
                        Console.WriteLine($"Token (JsonDocument): {token}");
                        return token; // You would return the token here
                    }
                    else
                    {
                        Console.WriteLine("Token property not found in the JSON response.");
                        // Handle error: token not found
                    }
                }
            }

            return null;
        }


        // Produits
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
        public async Task<Produit> AddProduitAsync(Produit produit)
        {
            string url = apiUrl + "/produits";

            // Gestion du token
            string token = Settings.Default.UserToken;
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token non disponible. Veuillez vous reconnecter.");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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
        public async Task<bool> UpdateProduitAsync(Produit produit)
        {
            try
            {
                string url = apiUrl + $"/produits/{produit.id}";

                // Gestion du token
                string token = Settings.Default.UserToken;
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token non disponible. Veuillez vous reconnecter.");
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonContent = new StringContent(JsonSerializer.Serialize(produit), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la modification du produit : " + ex.Message);
            }
        }
        public async Task<bool> DeleteProduitAsync(int id)
        {
            try
            {
                string url = apiUrl + $"/produits/{id}";

                // Gestion du token
                string token = Settings.Default.UserToken;
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token non disponible. Veuillez vous reconnecter.");
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression du produit : " + ex.Message);
            }
        }

        // Catégories
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
        public async Task<Categorie> AddCategorieAsync(Categorie categorie)
        {
            string url = apiUrl + "/categories";

            // Gestion du token
            string token = Settings.Default.UserToken;
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token non disponible. Veuillez vous reconnecter.");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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
        public async Task<bool> UpdateCategoriesAsync(Produit categorie)
        {
            try
            {
                string url = apiUrl + $"/categories/{categorie.id}";

                // Gestion du token
                string token = Settings.Default.UserToken;
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token non disponible. Veuillez vous reconnecter.");
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonContent = new StringContent(JsonSerializer.Serialize(categorie), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la modification de la catégorie : " + ex.Message);
            }
        }
        public async Task<bool> DeleteCategorieAsync(int id)
        {
            try
            {
                string url = apiUrl + $"/categories/{id}";

                // Gestion du token
                string token = Settings.Default.UserToken;
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token non disponible. Veuillez vous reconnecter.");
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression de la catégorie : " + ex.Message);
            }
        }

        // Styles
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
        public async Task<Style> AddStyleAsync(Style style)
        {

            string url = apiUrl + "/styles";

            //Gestion du token
            string token = Settings.Default.UserToken;
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token non disponible. Veuillez vous reconnecter.");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonSerializer.Serialize(style), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var createdStyle = JsonSerializer.Deserialize<Style>(result);

                return createdStyle;
            }
            else
                return null;
        }
        public async Task<Style> UpdateStyleAsync(Style style)
        {

            string url = apiUrl + $"/styles/{style.id}";

            //Gestion du token
            string token = Settings.Default.UserToken;
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token non disponible. Veuillez vous reconnecter.");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonSerializer.Serialize(style), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var modifiedStyle = JsonSerializer.Deserialize<Style>(result);

                return modifiedStyle;
            }
            else
                return null;
        }
        public async Task<bool> DeleteStyleAsync(int id)
        {
            try
            {
                string url = apiUrl + $"/styles/{id}";

                //Gestion du token
                string token = Settings.Default.UserToken;
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token non disponible. Veuillez vous reconnecter.");
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression du style : " + ex.Message);
            }
        }

        // Matériaux
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
        public async Task<Materiau> AddMateriauAsync(Materiau materiau)
        {
            string url = apiUrl + "/materiaux";

            // Gestion du token
            string token = Settings.Default.UserToken;
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token non disponible. Veuillez vous reconnecter.");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonSerializer.Serialize(materiau), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var createdMateriau = JsonSerializer.Deserialize<Materiau>(result);
                return createdMateriau;
            }
            else
                return null;
        }
        public async Task<bool> DeleteMateriauAsync(int id)
        {
            try
            {
                string url = apiUrl + $"/materiaux/{id}";

                // Gestion du token
                string token = Settings.Default.UserToken;
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token non disponible. Veuillez vous reconnecter.");
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression du matériau : " + ex.Message);
            }
        }

        // Fabrications
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
        public async Task<Fabrication> AddFabricationAsync(Fabrication fabrication)
        {
            string url = apiUrl + "/fabrications";

            // Gestion du token
            string token = Settings.Default.UserToken;
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token non disponible. Veuillez vous reconnecter.");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonSerializer.Serialize(fabrication), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var createdFabrication = JsonSerializer.Deserialize<Fabrication>(result);
                return createdFabrication;
            }
            else
                return null;
        }
        public async Task<bool> DeleteFabricationAsync(int id)
        {
            try
            {
                string url = apiUrl + $"/fabrications/{id}";

                // Gestion du token
                string token = Settings.Default.UserToken;
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token non disponible. Veuillez vous reconnecter.");
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression de la fabrication : " + ex.Message);
            }
        }

    }
}
