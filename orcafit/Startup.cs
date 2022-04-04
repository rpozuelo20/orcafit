using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using orcafit.Helpers;
using orcafit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            //  Cookies para autenticacion:
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();
            //  Cadenas de conexion:
            string urlapi = this.Configuration.GetValue<string>("ApiUrls:orcafitApi");
            string azStorageKeys = this.Configuration.GetConnectionString("orcafitAzStorageKeys");
            //  Llamada del heperTokenCallApi e inyeccion de los servicios:
            HelperTokenCallApi helperTokenCallApi = new HelperTokenCallApi(urlapi);
            ServiceRutinas serviceRutinas = new ServiceRutinas(helperTokenCallApi, urlapi);
            ServiceUsuarios serviceUsuarios = new ServiceUsuarios(helperTokenCallApi, urlapi);
            //  -creacion del cliente Blob mediante nuestra Key:
            BlobServiceClient blobServiceClient = new BlobServiceClient(azStorageKeys);
            //  -inyeccion de los servicios para las llamadas de la api y sus respectivos metodos:
            services.AddTransient<HelperTokenCallApi>(x => helperTokenCallApi);
            services.AddTransient<ServiceRutinas>(x => serviceRutinas);
            services.AddTransient<ServiceUsuarios>(x => serviceUsuarios);
            //  -inyeccion de los servicios para el uso de storageBlobs:
            services.AddTransient<BlobServiceClient>(x => blobServiceClient);
            services.AddTransient<ServiceStorageBlobs>();
            //  Se añade la memoria cache y sesion para poder usarla con la seguridad y almacenar el token:
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.IsEssential = true;
            });


            services.AddControllersWithViews(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    //  Necesario realizar el uso de Authentication para la seguridad.

            app.UseAuthorization();

            app.UseSession();   //  Necesario realizar el uso de Session para la seguridad.

            //  Recordemos que esta parte esta modificada, ahora es UseMvc:
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
