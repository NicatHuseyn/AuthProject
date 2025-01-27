using AuthProject.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthProject.Data.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p=>p.Name).HasMaxLength(20).IsRequired();
        builder.Property(p=>p.Stock).IsRequired();
        builder.Property(p=>p.Price).IsRequired().HasColumnType("decimal(18,2)");

        builder.Property(p=>p.AppUserId).IsRequired();
    }
}
