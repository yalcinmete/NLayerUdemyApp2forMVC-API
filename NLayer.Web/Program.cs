using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NLayer.Repository;
using NLayer.Service.Mapping;
using NLayer.Service.Validations;
using NLayer.Web.Modules;
using System.Reflection;

namespace NLayer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>()); ;

            //MVC Video46. API'den kopyala yap�st�r yapt�k.
            //Video29 AddAutoMapper iki overload� var. Ya bana tip ver diyor ya da assembly ver diyor.Tip verince Profile s�n�f�n� inherit eden class� bulup mapleme i�lemini yapar.
            builder.Services.AddAutoMapper(typeof(MapProfile));

            // MVC Video46.API program.cs'den kopya �ektik.
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

            //MVC Video 53.Filterinin contstructorunda DI ge�iyorsan program.cs'e bunu eklemelisin.
            builder.Services.AddScoped(typeof(NotFoundFilter<>));


            //MVC Video46. API program.cs'den kopya �ektik.
            //Video41 AutoFac eklendi.
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
            //Birden fazla module olursa yukar�daki sat�r� a�ag�ya kopyala new RepoServiceModule yazan yere yeni module'� ekle. 

            var app = builder.Build();

            //Developmentta ise hatalar� g�ster ?
            //app.UseExceptionHandler("/Home/Error");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //Developmentta de�ilse hatalar� g�ster?
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}