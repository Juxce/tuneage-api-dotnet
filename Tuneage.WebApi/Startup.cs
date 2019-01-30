using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tuneage.Data.Orm.EF.DataContexts;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Data.TestData;

namespace Tuneage.WebApi
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc();

            // Custom code to register the Data Context with ASP.NET Core's dependency injection IServiceCollection container
            ConfigureDatabase(services);

            // Custom code to register repositories with ASP.NET Core's dependency injection IServiceCollection container
            services.AddTransient<ILabelRepository, LabelRepository>();
            services.AddTransient<IArtistRepository, ArtistRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual async void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Uncomment the custom call below to allow a fresh, empty local database to be seeded on startup
            //await SeedThatThing(app);
        }

        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<TuneageDataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TuneageDataContext")));

            // Register the data seeder
            services.AddTransient<DataSeeder>();
        }

        protected async Task SeedThatThing(IApplicationBuilder app, bool isIntegrationTest = false)
        {
            // Seed the database
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var seeder = serviceScope.ServiceProvider.GetService<DataSeeder>();
                await seeder.Seed(isIntegrationTest);
            }
        }
    }
}
