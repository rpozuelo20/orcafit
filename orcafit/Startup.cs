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
            //  Añado las Cookies para el Authentication.
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();
            //  Almaceno la url de la API y la Key del Storage.
            string urlapi = this.Configuration.GetValue<string>("ApiUrls:orcafitApi");
            string azStorageKeys = this.Configuration.GetConnectionString("orcafitAzStorageKeys");
            //  Realizo la inyeccion de los servicios que se van a utilizar.
            HelperTokenCallApi helperTokenCallApi = new HelperTokenCallApi(urlapi);
            ServiceRutinas serviceRutinas = new ServiceRutinas(helperTokenCallApi, urlapi);
            ServiceUsuarios serviceUsuarios = new ServiceUsuarios(helperTokenCallApi, urlapi);
            BlobServiceClient blobServiceClient = new BlobServiceClient(azStorageKeys);
            //  Realizo la inyeccion de los servicios que se van a utilizar.
            services.AddTransient<HelperTokenCallApi>(x => helperTokenCallApi);
            services.AddTransient<ServiceRutinas>(x => serviceRutinas);
            services.AddTransient<ServiceUsuarios>(x => serviceUsuarios);
            services.AddTransient<BlobServiceClient>(x => blobServiceClient);
            services.AddTransient<ServiceStorageBlobs>();
            //  Añado la MemoryCache y Session (es parte del tema de autenticacion y almacenar datos en sesion).
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.IsEssential = true;
            });
            //  Añado SignalR para poder usar el chat.
            services.AddSignalR();

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
            //  Necesario realizar el uso de Authentication para la seguridad.
            app.UseAuthentication();

            app.UseAuthorization();
            //  Necesario realizar el uso de Session para la seguridad.
            app.UseSession();

            //  UseEndpoints para poder usar SignalR.
            app.UseEndpoints(endpoints =>
            {
                //  Añado el endpoint de chat.
                endpoints.MapHub<ChatHub>("/Chat");
            });
            //  UseMvc para poder mapear las rutas.
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
