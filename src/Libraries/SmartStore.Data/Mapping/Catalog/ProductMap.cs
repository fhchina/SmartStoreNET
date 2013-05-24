using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;

namespace SmartStore.Data.Mapping.Catalog
{
    public partial class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            this.ToTable("Product");
            this.HasKey(p => p.Id);
            this.Property(p => p.Name).IsRequired().HasMaxLength(400);
            this.Property(p => p.ShortDescription);
            this.Property(p => p.FullDescription);
            this.Property(p => p.AdminComment);
            this.Property(p => p.MetaKeywords).HasMaxLength(400);
            this.Property(p => p.MetaDescription);
            this.Property(p => p.MetaTitle).HasMaxLength(400);
        }
    }
}