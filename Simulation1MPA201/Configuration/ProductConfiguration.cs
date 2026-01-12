using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simulation1MPA201.Models;

namespace Simulation1MPA201.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Title).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(512);
        builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(1024);
        builder.Property(x => x.Price).HasPrecision(10, 2);

        builder.ToTable(opt =>
        {
            opt.HasCheckConstraint("CK_Product_Price", "[Price]>0");
            opt.HasCheckConstraint("CK_Product_Rating", "[Rating] between 0 and 5");
        });

        builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);

    }
}
