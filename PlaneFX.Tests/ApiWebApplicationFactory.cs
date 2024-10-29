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
            builder.ConfigureTestServices(services =>
            {
                var descriptorType = typeof(DbContextOptions<PlaneFXContext>);
                var descriptor = services.SingleOrDefault(s => s.ServiceType == descriptorType);

                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddDbContext<PlaneFXContext>(o => o.UseNpgsql(postgres.GetConnectionString()));
            });
        }

        public Task InitializeAsync() => postgres.StartAsync();

        public new Task DisposeAsync() => postgres.StopAsync();
    }
}