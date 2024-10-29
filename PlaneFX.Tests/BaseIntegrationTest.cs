namespace PlaneFX.Tests
{
    public abstract class BaseIntegrationTest : IClassFixture<ApiWebApplicationFactory>, IDisposable
    {
        private readonly IServiceScope scope;
        protected readonly IConfiguration configuration;
        protected readonly PlaneFXContext DbContext;
        protected readonly StartupService startup;
        protected readonly UserService userService;

        protected BaseIntegrationTest(ApiWebApplicationFactory factory)
        {
            scope = factory.Services.CreateScope();
            configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            DbContext = scope.ServiceProvider.GetRequiredService<PlaneFXContext>();
            startup = scope.ServiceProvider.GetRequiredService<StartupService>();
            userService = scope.ServiceProvider.GetRequiredService<UserService>();
        }

        public void Dispose()
        {
            scope?.Dispose();
            DbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}