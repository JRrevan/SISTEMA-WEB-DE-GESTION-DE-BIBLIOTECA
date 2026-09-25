using Biblioteca.Servicios;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Biblioteca
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Agregar el soporte para Controladores y Vistas (MVC)
            builder.Services.AddControllersWithViews()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Add("/Presentacion/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Presentacion/Views/Shared/{0}.cshtml");
                });

            // 2. REGISTRAR TU SERVICIO COMO SINGLETON (Crucial para mantener los datos)
            builder.Services.AddSingleton<CatalogoService>();
            builder.Services.AddSingleton<CatalogoService>();
            builder.Services.AddScoped<CargaXmlService>();

            var app = builder.Build();

            // 3. Configurar el pipeline HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // 4. Configurar la ruta por defecto (apunta a HomeController)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}