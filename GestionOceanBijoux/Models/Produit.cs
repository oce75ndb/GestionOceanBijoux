﻿namespace GestionOceanBijoux.Models
{
    public class Produit
    {
        public int id { get; set; }
        public required string nom { get; set; }
        public string? slug { get; set; }
        public string? description { get; set; }
        public decimal prix { get; set; }
        public string? image { get; set; }
        public int stock { get; set; }

        public int categorie_id { get; set; }
        public int materiau_id { get; set; }
        public int style_id { get; set; }
        public int fabrication_id { get; set; }

        public string? dimensions { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }

}
