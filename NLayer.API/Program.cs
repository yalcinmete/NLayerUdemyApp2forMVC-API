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
            //RegisterValidatorsFromAssemblyContaining ; bana classý ver ben assemblysini(katmaný) alayým.
            //Video 37.AddControllers(options=>{options.Filter.Add(new ValidateFilterAttribute())}) Proje seviyesinde FilterAttribute eklemiþ olduk. Tek satýr option verdiðin zaman süslü paranteze de gerek yok.

            //Video37.Kendi Filterimizdan dönen responselarý kullanýcýya dönebilmek için API'nýn hata dönüþ responsenu pasif hale getirmemiz gerekiyor. Bu sadece API tarafýnda aktif. MVC tarafýnda bu kod aktif olmadýðý için pasife çekmene gerek kalmýyor. Çünkü MVC tarafýnda bir sayfa dönüyorsun hangi sayfayý döneceðini bilmediði için sistem orada kendi filterini aktif ayarlamýyor.
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Video42.MemoryCache'i aktif et.
            builder.Services.AddMemoryCache();

            //Video40.Eðer bir filteriniz(NotFoundFilter) constructorunda herhangi bir servisi veya herhangibir classý DI(DepencyInjection) olarak geçiyorsa bunu program.cs'de yazmamýz gerekiyor. 
            builder.Services.AddScoped(typeof(NotFoundFilter<>));

            //Video41 AutoFac ile yorum satýrýna alýndý.
            //builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            //builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            //builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

            //Video29 AddAutoMapper iki overloadý var. Ya bana tip ver diyor ya da assembly ver diyor.Tip verince Profile sýnýfýný inherit eden classý bulup mapleme iþlemini yapar.
            builder.Services.AddAutoMapper(typeof(MapProfile));

            //Video41 AutoFac ile yorum satýrýna alýndý.
            //Video33
            //builder.Services.AddScoped<IProductRepository, ProductRepository>();
            //builder.Services.AddScoped<IProductService, ProductService>();

            //Video41 AutoFac ile yorum satýrýna alýndý.
            //Video34
            //builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            //builder.Services.AddScoped<ICategoryService, CategoryService>();


            builder.Services.AddDbContext<AppDbContext>(x =>
            {
                // appsettingten alacaðý connection adresini nereden alacaðý bilgisini veriyoruz. diðer option parametresi ile dbcontext'in NLayerAPI içerisinde deðil baþka bir katmanda olduðunu o katmanýn yerini veriyoruz.
                x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
                {
                    //Açýk açýk "NLayer.Repository" þeklinde verebiliyoruz ama tip güvenliði için (NLayer.Repository ismi ileride deðiþebilir) GetAssembly veriyoruz.
                    //option.MigrationsAssembly("NLayer.Repository") 
                    option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);

                });
            });

            //Video41 AutoFac eklendi.
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
            //Birden fazla module olursa yukarýdaki satýrý aþagýya kopyala new RepoServiceModule yazan yere yeni module'ü ekle. 
            


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
