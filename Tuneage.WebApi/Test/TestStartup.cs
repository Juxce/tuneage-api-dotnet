using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tuneage.Data.Orm.EF.DataContexts;
using Tuneage.Data.TestData;

namespace Tuneage.WebApi.Test
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureDatabase(IServiceCollection services)
        {
            // Register default database connection with in-memory database
            services.AddDbContext<TuneageDataContext>(options =>
                options.UseInMemoryDatabase("tuneage_test_db"));

            // Register the data seeder
            services.AddTransient<DataSeeder>();
        }

        public override async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Perform all configuration in the base class
            base.Configure(app, env);

            // Now seed the database
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var seeder = serviceScope.ServiceProvider.GetService<DataSeeder>();
                await seeder.Seed();
            }
        }
    }
}
