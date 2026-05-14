using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArabRiver.Repository.Configurations
{
    public class LeadConfiguration : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.OrganizationName)
                .HasMaxLength(200);

            builder.Property(x => x.Country)
                .HasMaxLength(100);

            builder.Property(x => x.CountryCode)
                .HasMaxLength(10);

            builder.Property(x => x.CatalogNameSnapshot)
                .HasMaxLength(200);

            builder.HasOne(x => x.Catalog)
                .WithMany()
                .HasForeignKey(x => x.CatalogId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Country);

            builder.HasIndex(x => x.CreatedAt);

            builder.HasIndex(x => x.IsEgypt);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(100);
        }
    }
}
