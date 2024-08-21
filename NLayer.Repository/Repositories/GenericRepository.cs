using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using System.Linq.Expressions;

namespace NLayer.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //Protected yapmamızın nedeni repositorylere özel metotlar yazmak istediğimizde GenericRepository'i miras alan özel repositorylerde _context nesnesini kullanabilmek istiyoruz.
        protected readonly AppDbContext _context;
        //Private readonly seçmemizin nedeni metotlar içinden ya da başka bir yerden _dbset nesnesine atama yapılmasını önlüyoruz.Ya tanımlandığı ilk yerde (yani burada) ya da ctor'da atama yapılabilir. 
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public IQueryable<T> GetAll()
        {
            //Sen 10000tane veri çekersen EF core belleğe 10000veriyi alıp izler. Performans kaybına neden olur.AsNoTracking() diyerek veriyi izleme diyoruz. Çünkü biz burada update,insert,delete işlemleri yapmayacağım için izleme yapmasına gerek yok sadece datayı alacağız.
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //FindAsync(id,id2,..) birden çok id alabilir.Çünkü bir tabloda birden çok id tanımlaması yapılabilir 
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {

            //_context.Entry(entity).State = EntityState.Deleted aşağıdaki kod ile aynı. Bu yüzden Async metodu yok cünkü async ihtiyaç yok sadece ef izlenen varlıkların entitystate.deleted bayrağına işaret koyuyor.Yüklü bir iş yapmıyoruz.Memorydeki entityinin state değerini değiştiriyoruz sadece.Ne zaman savechanges() yaparsak entitystate'i değişmiş entityleri database'e işler. Aynı şey RemobeRange,Update için de geçerli.
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
