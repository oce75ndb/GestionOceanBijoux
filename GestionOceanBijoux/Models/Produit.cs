namespace GestionOceanBijoux.Models
{
    public class Produit
    {
        public int id { get; set; }
        public string nom { get; set; }
        public string? slug { get; set; }
        public string? description { get; set; }
        public string prix { get; set; }
        public string? image { get; set; }
        public int stock { get; set; }
        public int categorie_id { get; set; }
        public string? materiau { get; set; }
        public string? style { get; set; }
        public string? dimensions { get; set; }
        public string? fabrication { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }

}
