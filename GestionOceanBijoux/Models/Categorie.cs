namespace GestionOceanBijoux.Models
{
    public class Categorie
    {
        public int id { get; set; }
        public string? parent_id { get; set; }
        public string categorie { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

    }
}
