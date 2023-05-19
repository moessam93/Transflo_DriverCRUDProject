using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Transflo_DriverCRUDProject.Repos;

namespace Transflo_DriverCRUDProject
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the DriverRepository
            services.AddScoped<DriverRepository>();

            // Add other services and dependencies here if needed

            // Add controllers and enable API endpoints
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // Configure production environment settings
            }

            // Enable routing and endpoints
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
