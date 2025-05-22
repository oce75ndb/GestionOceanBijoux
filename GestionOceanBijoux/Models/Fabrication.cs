namespace GestionOceanBijoux.Models
{
    public class Fabrication
    {
        public int id { get; set; }
        public required string fabrication { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

    }
}
