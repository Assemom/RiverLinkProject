using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace ArabRiver.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Catalog> Catalogs { get; set; }

        public DbSet<Lead> Leads { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<OutsideEgyptVisitor> OutsideEgyptVisitors { get; set; }

        public DbSet<Partner> Partners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }
    }
}
