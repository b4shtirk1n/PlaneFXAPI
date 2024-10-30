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
            Configuration["ConnectionStrings:PlaneFX"] = postgres.GetConnectionString();

            builder.ConfigureAppConfiguration(c => c.AddConfiguration(Configuration));
        }

        public Task InitializeAsync() => postgres.StartAsync();

        public new Task DisposeAsync() => postgres.StopAsync();
    }
}