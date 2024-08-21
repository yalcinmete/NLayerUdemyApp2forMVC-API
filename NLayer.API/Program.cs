using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;
using System.Reflection;

namespace NLayer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Video36.FluentValidation
            //builder.Services.AddControllers();
            builder.Services.AddControllers(option => { option.Filters.Add(new ValidateFilterAttribute()); }).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
            //RegisterValidatorsFromAssemblyContaining ; bana class� ver ben assemblysini(katman�) alay�m.
            //Video 37.AddControllers(options=>{options.Filter.Add(new ValidateFilterAttribute())}) Proje seviyesinde FilterAttribute eklemi� olduk. Tek sat�r option verdi�in zaman s�sl� paranteze de gerek yok.

            //Video37.Kendi Filterimizdan d�nen responselar� kullan�c�ya d�nebilmek i�in API'n�n hata d�n�� responsenu pasif hale getirmemiz gerekiyor. Bu sadece API taraf�nda aktif. MVC taraf�nda bu kod aktif olmad��� i�in pasife �ekmene gerek kalm�yor. ��nk� MVC taraf�nda bir sayfa d�n�yorsun hangi sayfay� d�nece�ini bilmedi�i i�in sistem orada kendi filterini aktif ayarlam�yor.
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Video42.MemoryCache'i aktif et.
            builder.Services.AddMemoryCache();

            //Video40.E�er bir filteriniz(NotFoundFilter) constructorunda herhangi bir servisi veya herhangibir class� DI(DepencyInjection) olarak ge�iyorsa bunu program.cs'de yazmam�z gerekiyor. 
            builder.Services.AddScoped(typeof(NotFoundFilter<>));

            //Video41 AutoFac ile yorum sat�r�na al�nd�.
            //builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            //builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            //builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

            //Video29 AddAutoMapper iki overload� var. Ya bana tip ver diyor ya da assembly ver diyor.Tip verince Profile s�n�f�n� inherit eden class� bulup mapleme i�lemini yapar.
            builder.Services.AddAutoMapper(typeof(MapProfile));

            //Video41 AutoFac ile yorum sat�r�na al�nd�.
            //Video33
            //builder.Services.AddScoped<IProductRepository, ProductRepository>();
            //builder.Services.AddScoped<IProductService, ProductService>();

            //Video41 AutoFac ile yorum sat�r�na al�nd�.
            //Video34
            //builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            //builder.Services.AddScoped<ICategoryService, CategoryService>();


            builder.Services.AddDbContext<AppDbContext>(x =>
            {
                // appsettingten alaca�� connection adresini nereden alaca�� bilgisini veriyoruz. di�er option parametresi ile dbcontext'in NLayerAPI i�erisinde de�il ba�ka bir katmanda oldu�unu o katman�n yerini veriyoruz.
                x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
                {
                    //A��k a��k "NLayer.Repository" �eklinde verebiliyoruz ama tip g�venli�i i�in (NLayer.Repository ismi ileride de�i�ebilir) GetAssembly veriyoruz.
                    //option.MigrationsAssembly("NLayer.Repository") 
                    option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);

                });
            });

            //Video41 AutoFac eklendi.
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
            //Birden fazla module olursa yukar�daki sat�r� a�ag�ya kopyala new RepoServiceModule yazan yere yeni module'� ekle. 
            


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //Video38 Kendi Global Exception Middlewaremizi ekledik.
            app.UseCustomException();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
