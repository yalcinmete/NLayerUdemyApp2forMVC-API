using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsWithCategory()
        {
            //Eager Loading yaptık yani daha datayı çekerken categorilerin de alınmasını istedik. Bir de LazyLoading var . LazyLoading de Producta bağlı kategorileri ihtiyaç olduğunda daha sonra çekersen LazyLoading olur.
            //miras alınan classta _context protected olarak tanımlandığı için miras alan bu sınıfta kullanabliyoruz.Aksi halde   protected readonly AppDbContext _context; tanımlaması yapmak zorunda kalıcaktın.
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }
    }
}
