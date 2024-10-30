using System.Data.Common;

namespace PlaneFX.Tests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        private readonly PostgreSqlContainer postgres = new PostgreSqlBuilder()
            .WithImage("postgres:alpine")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(c => c.AddConfiguration(Configuration));
            builder.ConfigureServices(services =>
            {
                services.Remove(services.SingleOrDefault(s =>
                    typeof(DbContextOptions<PlaneFXContext>) == s.ServiceType)!);

                services.Remove(services.SingleOrDefault(s => typeof(PlaneFXContext) == s.ServiceType)!);
                services.Remove(services.SingleOrDefault(s => typeof(DbConnection) == s.ServiceType)!);
                services.AddDbContext<PlaneFXContext>(o => o.UseNpgsql(postgres.GetConnectionString()));
            });
        }

        public Task InitializeAsync() => postgres.StartAsync();

        public new Task DisposeAsync() => postgres.StopAsync();
    }
}