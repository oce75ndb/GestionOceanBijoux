namespace GestionOceanBijoux.Models
{
    public class Categorie
    {
        public int id { get; set; }
        public int parent_id { get; set; }
        public required string categorie { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
