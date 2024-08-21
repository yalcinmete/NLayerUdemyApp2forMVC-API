using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using System.Reflection;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        //Veritabanı yolunu startup dosyasından vermek istediğimizde DbContextOptions'dan yardım alırız.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //ProductFeature Product üzerinden de bu şekilde eklenebilir.
            //var p = new Product() { ProductFeature = new ProductFeature() { Color... } };
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; } //ProductFeature Product üzerinden de eklenebilir.


        //MVC Video54 Ortası.SaveChangesAsync yerine SaveChange'i de çağırdığımız durumlar olabilir. SaveChange'i de override edelim:
        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                entityReference.UploadDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }
            return base.SaveChanges();
        }


        //MVC Video54.Created Date / Update Date. MVC veya API'ye özgü değil Created Date / Update Date atamak için savechanged metodunu ezebiliriz.EF core SaveChanges metodu çağırana kadar veritabanına işlem yapmaz. Buraad veritabanına yansıtmadan hemen öncesinde entity'nin update mi edildiğini yeni insert mi edildiğini anlayacağız.Buna göre updated date create date güncelleyeceğiz.
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries()) 
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        //item.state = entitystate.Added ise;
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                //entityReference 'nin CreatedDate propertisindeki Modified'ını değiştirme.Createddate sıfırlanmasın
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                entityReference.UploadDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Category>().HasKey(x => x.Id); //Bu sayfa dolmasın diye configurasyonları Configurasyon klasörü altına aldık.

            //Diğer klasörlerdeki configurationları iki farklı şekilde apply edebiliyoruz.
            //1.Tek tek söylemek ama çok fazla konfigurasyonun varsa uzun yol olacaktır.
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());
            //2.yol:Reflection yardımıyla.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //Assembly.GetExecutingAssembly() = çalışmış olduğum katman(NLayer.Repository)daki konfigurasyonları bul.


            //ProductFeature örnek datasını da buradan yükleyelim :
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature()
            {
                Id = 1,
                Color = "Kırmızı",
                Height = 100,
                Width = 200,
                ProductId = 1,
            },
            new ProductFeature()
            {
                Id = 2,
                Color = "Mavi",
                Height = 300,
                Width = 500,
                ProductId = 2,
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
