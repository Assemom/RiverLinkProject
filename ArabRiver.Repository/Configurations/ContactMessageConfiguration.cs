using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArabRiver.Repository.Configurations
{
    public class ContactMessageConfiguration
    : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(3000);

            builder.Property(x => x.Status)
                .HasDefaultValue("New");

            builder.HasIndex(x => x.CreatedAt);
        }
    }
}
