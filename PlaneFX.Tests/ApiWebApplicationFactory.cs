using Testcontainers.Redis;

namespace PlaneFX.Tests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        private readonly PostgreSqlContainer postgres;

        private readonly RedisContainer redis;

        public ApiWebApplicationFactory()
        {
            postgres = new PostgreSqlBuilder()
                .WithImage("postgres:alpine")
                .Build();

            redis = new RedisBuilder()
                .WithImage("redis:alpine")
                .DependsOn(postgres)
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            configuration["ConnectionStrings:PlaneFX"] = postgres.GetConnectionString();
            configuration["ConnectionStrings:Redis"] = redis.GetConnectionString();
            configuration[StartupService.TG_ID] = "123456789";
            configuration[StartupService.TG_USERNAME] = "cherkashh";
            configuration[StartupService.TIME_ZONE] = "5";
            configuration[StartupService.TG_API_TOKEN] = "ghfjkdy57494hf";

            builder.ConfigureAppConfiguration(c => c.AddConfiguration(configuration));
        }

        public async Task InitializeAsync()
        {
            await postgres.StartAsync();
            await redis.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await postgres.StopAsync();
            await redis.StopAsync();
        }
    }
}