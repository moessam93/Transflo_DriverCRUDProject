using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Transflo_DriverCRUDProject.Repos;

namespace Transflo_DriverCRUDProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            var host = builder.Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();

                // Get the connection string from the configuration
                string connectionString = configuration.GetConnectionString("DefaultConnection");

                // Initialize the database
                using (var driverRepository = new DriverRepository(configuration))
                {
                    driverRepository.InitializeDatabase();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
