using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId)
        {
            return await _context.Categories.Include(x => x.Products).Where(x => x.Id == categoryId).SingleOrDefaultAsync();
            //return await _context.Categories.Include(x => x.Products).Where(x => x.Id == categoryId).FirstOrDefaultAsync();
            //....FirstOrDefault() verilen koşulldaki id'ye uygun 4 5 tane varsa ilkini bulur ama singleordefault dersek bu id'den birden fazla bulursa hata döner. id'den birtane olması lazım primary key o halde single kullanmak daha mantıklı.
        }
    }
}
