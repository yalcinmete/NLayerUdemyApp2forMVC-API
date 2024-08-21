using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;

namespace NLayer.Repository.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Stock).IsRequired();

            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.ToTable("Products");


            //Bunu yazmasan da olur entitylerde ef'nin istediği yapıda propertyleri olusturdugumuz için otomatik yapı kurulacaktır.Sadece göstermek için yazdık.
            //Bir productın bir categorisi olucak(hasone).Categorynin birden çok productsı olabilir.ForeignKey de producttaki categoryıd olacak.
            //Şimdilik dursun zaten önce fluentapi buraya bakar burayı dikkate alır ef.
            builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
        }
    }
}
