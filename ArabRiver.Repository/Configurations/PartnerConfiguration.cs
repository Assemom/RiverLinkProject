using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArabRiver.Repository.Configurations
{
    public class PartnerConfiguration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            var seededAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Partner
                {
                    Id = new Guid("b65d0dcb-0d0d-4f63-9a7e-7d4b1ad6d8a1"),
                    Name = "insto",
                    CreatedAt = seededAt,
                    UpdatedAt = seededAt
                },
                new Partner
                {
                    Id = new Guid("ba3d6f3b-7a21-4f20-9f79-0a0dd1c6e5b8"),
                    Name = "nouvag",
                    CreatedAt = seededAt,
                    UpdatedAt = seededAt
                },
                new Partner
                {
                    Id = new Guid("9f5bfb8b-8f5a-48a3-97ab-19c2b4f4a8c5"),
                    Name = "frimed",
                    CreatedAt = seededAt,
                    UpdatedAt = seededAt
                },
                new Partner
                {
                    Id = new Guid("0c1c7c20-71de-4a0d-bb8e-2c1f7903f6a2"),
                    Name = "aygun",
                    CreatedAt = seededAt,
                    UpdatedAt = seededAt
                },
                new Partner
                {
                    Id = new Guid("4ed77bdb-2c1d-4d5a-97f2-6a8f1d35c6b4"),
                    Name = "DrFrigz",
                    CreatedAt = seededAt,
                    UpdatedAt = seededAt
                },
                new Partner
                {
                    Id = new Guid("8e1c1f52-6b54-4bf0-9f84-3b6c71b624af"),
                    Name = "Vicoris Health",
                    CreatedAt = seededAt,
                    UpdatedAt = seededAt
                },
                new Partner
                {
                    Id = new Guid("a12d67de-1bc1-48ce-b11d-8c0c5a9f6b77"),
                    Name = "Steristar",
                    CreatedAt = seededAt,
                    UpdatedAt = seededAt
                },
                new Partner
                {
                    Id = new Guid("e13a1c9a-62a5-4e1e-8d2f-4f5b3b2b2e11"),
                    Name = "Swantia",
                    CreatedAt = seededAt,
                    UpdatedAt = seededAt
                },
                new Partner
                {
                    Id = new Guid("f6a4e9f2-3bfa-4b08-8a5c-46f88f1b6a10"),
                    Name = "ABI surgical",
                    CreatedAt = seededAt,
                    UpdatedAt = seededAt
                });
        }
    }
}
