using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Museum.Data;
using Museum.Data.Context;
using Museum.API.TokenService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Museum.Repositories;
using Museum.Domain.Interface;
using Museum.Domain.Service;

namespace Museum.API
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
            services.AddDbContext<MuseumContext>(options =>
            {
                options
                .UseSqlServer(Configuration.GetConnectionString("MuseumConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // JWT token
            services.AddJwtBearerAuthentication(Configuration);

            services.AddControllers();

            services.AddOpenApi();
            // Repositories
            services.AddTransient<IExhabitRepository, ExhabitsRepository>();
            services.AddTransient<IExhibitionRepository, ExhibitionsRepository>();
            services.AddTransient<IAuditoriumsRepository, AuditoriumsRepository>();
            services.AddTransient<IMuseumsRepository, MuseumsRepository>();


            // Business Logic
            services.AddTransient<IExhabitService, ExhabitService>();
            services.AddTransient<IExhibitionService, ExhibitionService>();
            services.AddTransient<IAuditoriumService, AuditoriumService>();
            services.AddTransient<IMuseumService, MuseumService>();

            // Allow Cors for client app
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    corsBuilder => corsBuilder.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
