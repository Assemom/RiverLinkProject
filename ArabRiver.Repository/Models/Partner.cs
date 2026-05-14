namespace ArabRiver.Repository.Models
{
    public class Partner
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<Catalog> Catalogs { get; set; } = new List<Catalog>();
    }
}
