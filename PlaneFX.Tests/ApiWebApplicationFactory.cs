namespace PlaneFX.Tests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        private readonly PostgreSqlContainer postgres = new PostgreSqlBuilder()
            .WithImage("postgres:alpine")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            configuration["ConnectionStrings:PlaneFX"] = postgres.GetConnectionString();
            configuration[StartupService.TG_ID] = "123456789";
            configuration[StartupService.TG_USERNAME] = "cherkashh";
            configuration[StartupService.TIME_ZONE] = "5";
            configuration[StartupService.TG_API_TOKEN] = "ghfjkdy57494hf";

            builder.ConfigureAppConfiguration(c => c.AddConfiguration(configuration));
        }

        public Task InitializeAsync() => postgres.StartAsync();

        public new Task DisposeAsync() => postgres.StopAsync();
    }
}