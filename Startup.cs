using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyStarwarsApi.Context;
using MyStarwarsApi.Models;
using MyStarwarsApi.Repo;

namespace MyStarwarsApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddEntityFrameworkSqlite()
                    .AddDbContext<SqliteDbContext>(options => 
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "MySwApi.db" };
                var connectionString = connectionStringBuilder.ToString();
                var connection = new SqliteConnection(connectionString);
                options.UseSqlite(connection);

                options.UseOpenIddict<Guid>();
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SqliteDbContext>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict<Guid>()
                .AddEntityFrameworkCoreStores<SqliteDbContext>()
                .AddMvcBinders()
                .EnableTokenEndpoint("/connect/token")
                .AllowPasswordFlow()
                
                //For development only!
                .DisableHttpsRequirement();

            services.AddScoped<ICharacterRepository, CharacterRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();

            app.UseOAuthValidation();
            app.UseOpenIddict();
            using (var context = app.ApplicationServices.GetRequiredService<SqliteDbContext>())
            {
                context.Database.EnsureCreated();
            }

            app.UseMvc();
            app.UseWelcomePage();
        }
    }
}
