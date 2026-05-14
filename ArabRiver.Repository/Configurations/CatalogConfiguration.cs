using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArabRiver.Repository.Configurations
{
    public class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
    {
        public void Configure(EntityTypeBuilder<Catalog> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.GoogleDriveLink)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(5000);

            builder.HasOne(x => x.Partner)
                .WithMany(x => x.Catalogs)
                .HasForeignKey(x => x.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
