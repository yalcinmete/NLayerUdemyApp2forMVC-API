using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);

        //productRepository.GetAll(x=>x.id>5).ToList();
        IQueryable<T> GetAll();

        //IQueryable dönmemizin nedeni,IQueryable sorguları direkt veritabanına gitmez.ToList(),ToListAsync() gibi metotları çağırırsak veritabanına gider. IQueryable dan sonra where koşulları da eklemek istediğimiz için yani daha performanslı olması için IQueryable kullanıyoruz. Yani veritabınına şuan bir sorgu yapmıyoruz. Veritabanına yapılacak sorguyu oluşturuyoruz.
        //productRepository.GetAll(x=>x.id>5).OrderBy.ToListAsync();
        //TDelegete'ler metotları referans ederler.
        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task AddAsync(T entity);

        //List yerine IEnumerable(interface) kullandık.Yazılımda mümkün olduğunda soyut nesnelerle çalışmak önemli.Soyut nesneleri istediğim tipe dönüştürebilirim.Yani IEnumerable implemente etmiş istediğim classa dönüştürebilirim.
        Task AddRangeAsync(IEnumerable<T> entities);

        //Update veya remove bunların async metotları yok. Olmasına da gerek yok .Ef sadece takip ettiği varlığın stateni değiştiriyor. Bu uzun süren bir işlem olmadığı için async yapısı yok . Update yapabilmeniz için bir entity vermeniz lazım. Entity zaten ef tarafından takip ediliyor. Update dediğimizde sadece takip etmiş olduğu varlığın state'ni modify olarak değiştiriyor.Yani uzun süren bir işlem değil.  Ama Add() memory'e bir data ekliyor.Add()'de bir süreç var.
        void Update(T entity);
        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}
