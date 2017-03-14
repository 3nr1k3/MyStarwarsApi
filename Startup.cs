using System;
using AutoMapper;
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
using MyStarwarsApi.Models.ViewModel;
using MyStarwarsApi.Repo;
using MyStarwarsApi.Repo.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

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
        public void ConfigureServices(
            IServiceCollection services
            )
        {
            // Add framework services.
            services.AddMvc();
            services.AddMvcCore()
                    .AddApiExplorer();

            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", 
                new Info 
                { 
                    Title = "Starwars Api - V1", 
                    Version = "v1",
                    Description = "",
                    TermsOfService = "Only I can use it",
                    Contact = new Contact
                    {
                        Name = "Enrique Cardero",
                        Email = "enrique.cardero.ruiz@gmail.com"
                    },
                    License = new License
                    {
                        Name = "Will see..."
                    }
                });
            });

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

            services.AddAutoMapper();

            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();

            services.AddSingleton<IWebHostBuilder, WebHostBuilder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory
            )
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            /*app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"app/wwwroot")),
                    RequestPath = new PathString("/app/wwwroot")
            });*/

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Starwars Api");

                c.EnabledValidator();
                c.BooleanValues(new object[] { 0, 1 });
                c.DocExpansion("full");
                c.InjectOnCompleteJavaScript("/swagger-ui/on-complete.js");
                c.InjectOnFailureJavaScript("/swagger-ui/on-failure.js");
                c.SupportedSubmitMethods(new[] { "get", "post", "put", "patch" });
                c.ShowRequestHeaders();
                c.ShowJsonEditor();
            });

            app.UseOAuthValidation();
            app.UseOpenIddict();
            /*using (var context = app.ApplicationServices.GetRequiredService<SqliteDbContext>())
            {
                context.Database.EnsureCreated();
                CharacterRepository.FillCharacterRepository(context);
            }*/

            app.UseMvc();
            app.UseWelcomePage();

            Mapper.Initialize(c => {
                c.CreateMap<Character,CharacterCreateViewModel>()
                    .ReverseMap()
                    .ForMember(m => m.avatar, opt => opt.Ignore())
                    .ForMember(m => m.charactersKilled, opt => opt.Ignore());
            });
        }
    }
}
