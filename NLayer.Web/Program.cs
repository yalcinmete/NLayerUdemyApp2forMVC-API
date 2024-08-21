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

            //MVC Video46. API'den kopyala yapýstýr yaptýk.
            //Video29 AddAutoMapper iki overloadý var. Ya bana tip ver diyor ya da assembly ver diyor.Tip verince Profile sýnýfýný inherit eden classý bulup mapleme iþlemini yapar.
            builder.Services.AddAutoMapper(typeof(MapProfile));

            // MVC Video46.API program.cs'den kopya çektik.
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

            //MVC Video 53.Filterinin contstructorunda DI geçiyorsan program.cs'e bunu eklemelisin.
            builder.Services.AddScoped(typeof(NotFoundFilter<>));


            //MVC Video46. API program.cs'den kopya çektik.
            //Video41 AutoFac eklendi.
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
            //Birden fazla module olursa yukarýdaki satýrý aþagýya kopyala new RepoServiceModule yazan yere yeni module'ü ekle. 

            var app = builder.Build();

            //Developmentta ise hatalarý göster ?
            //app.UseExceptionHandler("/Home/Error");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //Developmentta deðilse hatalarý göster?
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